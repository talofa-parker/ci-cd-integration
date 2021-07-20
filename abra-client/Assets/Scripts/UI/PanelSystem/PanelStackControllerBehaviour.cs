using System.Threading.Tasks;
using TalofaGames.UI.Utility;
using UnityEngine;

namespace TalofaGames.UI.PanelSystem
{

  /// <summary>
  /// PanelStackControllerBehaviour is a MonoBehaviour wrapper around a PanelStackController instance
  /// </summary>
  [RequireComponent(typeof(RectTransform))]
  public class PanelStackControllerBehaviour : MonoBehaviour, IPanelStackController, IPanelViewContainer
  {
    [SerializeField]
    private UIEventManager eventManager;

    [SerializeField]
    private PanelStackSystem panelStackSystem;

    public PanelStackSystem System => panelStackSystem;

    public RectTransform ParentTransform => (RectTransform)transform;

    private PanelStackController baseController;

    [SerializeField, Tooltip("If true the panel stack will be cleared when this object is destroyed. This may be desired when reloading the game for example.")]
    protected bool clearSystemStackOnDestroy = true;

    private PanelStackController BaseController =>
        baseController ??
        (baseController = new PanelStackController(panelStackSystem, this, eventManager));

    private void OnEnable()
    {
      panelStackSystem.AddController(this);
    }

    private void OnDisable()
    {
      panelStackSystem.RemoveController(this);
    }

    private void OnDestroy()
    {
      if (clearSystemStackOnDestroy)
      {
        panelStackSystem.Clear();
      }
    }

    public async Task TransitionAsync()
    {
      await BaseController.TransitionAsync();
    }

    public void ViewDidLoad() => panelStackSystem.ViewDidLoad(this);
    public void ViewWillAppear() => panelStackSystem.ViewWillAppear(this);
    public void ViewDidAppear() => panelStackSystem.ViewDidAppear(this);
    public void ViewWillDisappear() => panelStackSystem.ViewWillDisappear(this);
    public void ViewDidDisappear() => panelStackSystem.ViewDidDisappear(this);
  }

}

