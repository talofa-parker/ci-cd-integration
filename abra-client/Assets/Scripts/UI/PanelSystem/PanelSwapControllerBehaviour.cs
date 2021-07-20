using System.Threading.Tasks;
using TalofaGames.UI.Utility;
using UnityEngine;

namespace TalofaGames.UI.PanelSystem
{
  public class PanelSwapControllerBehaviour : MonoBehaviour, IPanelSwapController, IPanelViewContainer
  {
    [SerializeField]
    private UIEventManager eventManager;

    [SerializeField]
    private PanelSwapSystem panelSwapSystem;

    public PanelSwapSystem System => panelSwapSystem;

    private PanelSwapController baseController;

    public RectTransform ParentTransform => (RectTransform)this.transform;

    private PanelSwapController BaseController =>
        baseController ??
        (baseController = new PanelSwapController(panelSwapSystem, this, eventManager));

    protected virtual void OnEnable()
    {
      panelSwapSystem.AddController(this);
    }

    protected virtual void OnDisable()
    {
      panelSwapSystem.RemoveController(this);
    }

    public virtual async Task TransitionAsync()
    {
      await BaseController.TransitionAsync();
    }

    public void ViewDidLoad()
    {
    }

    public void ViewWillAppear()
    {
    }

    public void ViewDidAppear()
    {
    }

    public void ViewWillDisappear()
    {
    }

    public void ViewDidDisappear()
    {
    }
  }

}

