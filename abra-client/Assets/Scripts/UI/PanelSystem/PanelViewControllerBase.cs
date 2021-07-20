using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TalofaGames.UI.PanelSystem
{
  public enum PanelViewControllerState
  {
    Disappeared,
    Appearing,
    Appeared,
    Disappearing
  }

  internal sealed class PanelViewControllerBase : IPanelViewController, IDisposable
  {
    private readonly PanelType panelType;

    private PanelViewBase panelView;

    private PanelViewControllerState state = PanelViewControllerState.Disappeared;
    public PanelViewControllerState State => state;

    private CancellationTokenSource cancellationTokenSource;

    private IPanelViewContainer parentPanelViewContainer;

    public PanelType PanelType => panelType;
    public PanelViewBase View => panelView;
    public bool IsViewLoaded => panelView != null;

    public IPanelViewContainer ParentViewContainer => parentPanelViewContainer;

    private readonly Action didLoad;
    private readonly Action willAppear;
    private readonly Action didAppear;
    private readonly Action willDisappear;
    private readonly Action didDisappear;
    private bool firstViewAppeared = false;

    public PanelViewControllerBase(PanelType type, Action didLoad, Action willAppear, Action didAppear, Action willDisappear, Action didDisappear)
    {
      panelType = type;
      this.didLoad = didLoad;
      this.willAppear = willAppear;
      this.didAppear = didAppear;
      this.willDisappear = willDisappear;
      this.didDisappear = didDisappear;
    }

    public PanelViewControllerBase(PanelType type, PanelViewBase view, Action didLoad, Action willAppear, Action didAppear, Action willDisappear, Action didDisappear)
    {
      panelType = type;
      panelView = view;
      this.didLoad = didLoad;
      this.willAppear = willAppear;
      this.didAppear = didAppear;
      this.willDisappear = willDisappear;
      this.didDisappear = didDisappear;
    }

    public void SetParentViewContainer(IPanelViewContainer parent)
    {
      parentPanelViewContainer = parent;
      if (panelView != null)
        panelView.transform.SetParent(parentPanelViewContainer.ParentTransform);
    }

    public async Task LoadViewAsync()
    {
      if (IsViewLoaded)
        return;

      var prefab = await panelType.GetPrefabAsync();

      if (prefab == null)
      {
        Debug.LogError($"[<b>{nameof(PanelViewControllerBase)}</b>] PanelType {panelType.name} returned null panel prefab.");
        throw new ArgumentNullException();
      }

      bool cachedState = prefab.gameObject.activeSelf;
      prefab.gameObject.SetActive(false);
      panelView = Object.Instantiate(prefab, ParentViewContainer?.ParentTransform);
      prefab.gameObject.SetActive(cachedState);
      didLoad?.Invoke();
      parentPanelViewContainer.ViewDidLoad();
    }

    public async Task ShowAsync(bool immediate = false)
    {
      //If we're currently appeared or appearing we're already doing the thing we wanna be doing so just return
      if (state == PanelViewControllerState.Appeared || state == PanelViewControllerState.Appearing)
        return;

      //If we're in the middle of disappearing we need to cancel the disappearing action
      if (state == PanelViewControllerState.Disappearing)
      {
        //Cancel Current Transition
        cancellationTokenSource.Cancel();
        cancellationTokenSource = null;
      }

      state = PanelViewControllerState.Appearing;

      if (cancellationTokenSource == null)
        cancellationTokenSource = new CancellationTokenSource();

      var currentToken = cancellationTokenSource.Token;

      if (!IsViewLoaded)
      {
        await LoadViewAsync().ConfigureAwait(true);

        if (currentToken.IsCancellationRequested)
        {
          return;
        }
      }

      willAppear?.Invoke();
      parentPanelViewContainer.ViewWillAppear();
      if (firstViewAppeared == true)
      {
        var tabbedChildren = panelView.GetComponentsInChildren<PanelSwapControllerBehaviour>();
        if (tabbedChildren != null && tabbedChildren.Length > 1)
          Debug.LogWarning($"[<b>{nameof(PanelViewControllerBase)}</b>] It seems this UI has 2 tab systems within the same panel, this is not supported and may act weird");
        else if (tabbedChildren != null && tabbedChildren.Length > 0)
        {
          /* Bit of a hack, but tabs only "Disappears" when shifting to another tab. 
            So with this, we check if they are any active displayed tabs, and when 
            the parent panel disapears, we force that event */
          var viewedTab = (IPanelViewContainer)tabbedChildren[0].System.CurrentViewController;
          viewedTab.ViewWillAppear();
        }
      }
      firstViewAppeared = true;

      if (immediate)
        panelView.ShowImmediate();
      else
        await panelView.ShowAsync(currentToken);

      if (cancellationTokenSource.IsCancellationRequested)
        return;

      state = PanelViewControllerState.Appeared;

      didAppear?.Invoke();
      parentPanelViewContainer.ViewDidAppear();
    }

    public async Task HideAsync(bool immediate = false)
    {
      //If we're already disappeared or disappearing we're already doing the right thing so just return
      if (state == PanelViewControllerState.Disappeared || state == PanelViewControllerState.Disappearing)
        return;

      //If we're currently appearing we should cancel the appear using our cancellation token
      if (state == PanelViewControllerState.Appearing && cancellationTokenSource != null)
      {
        //Cancel Current Transition
        cancellationTokenSource.Cancel();
        cancellationTokenSource = null;
      }

      state = PanelViewControllerState.Disappearing;

      if (cancellationTokenSource == null)
        cancellationTokenSource = new CancellationTokenSource();

      var currentToken = cancellationTokenSource.Token;

      willDisappear?.Invoke();
      parentPanelViewContainer.ViewWillDisappear();
      var tabbedChildren = panelView.GetComponentsInChildren<PanelSwapControllerBehaviour>();
      if (tabbedChildren != null && tabbedChildren.Length > 1)
        Debug.LogWarning($"[<b>{nameof(PanelViewControllerBase)}</b>] It seems this UI has 2 tab systems within a since panel, this is not supported and may act weird");
      else if (tabbedChildren != null && tabbedChildren.Length > 0)
      {
        /* Bit of a hack, but tabs only "Disappears" when shifting to another tab. 
          So with this, we check if they are any active displayed tabs, and when 
          the parent panel disapears, we force that event */
        var viewedTab = (IPanelViewContainer)tabbedChildren[0].System.CurrentViewController;
        viewedTab.ViewDidDisappear();
      }

      if (immediate)
        panelView.HideImmediate();
      else
        await panelView.HideAsync(currentToken);

      if (currentToken.IsCancellationRequested)
        return;

      didDisappear?.Invoke();
      parentPanelViewContainer.ViewDidDisappear();

      state = PanelViewControllerState.Disappeared;
    }

    public void Dispose()
    {
      cancellationTokenSource?.Dispose();
    }
  }
}