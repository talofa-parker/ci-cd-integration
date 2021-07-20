using System.Threading.Tasks;
using UnityEngine;

namespace TalofaGames.UI.PanelSystem
{
  using Task = System.Threading.Tasks.Task;

  public interface IPanelViewContainer
  {
    RectTransform ParentTransform { get; }

    void ViewDidLoad();
    void ViewWillAppear();
    void ViewDidAppear();
    void ViewWillDisappear();
    void ViewDidDisappear();
  }

  public interface IPanelViewController
  {
    PanelViewControllerState State { get; }
    PanelType PanelType { get; }
    PanelViewBase View { get; }
    IPanelViewContainer ParentViewContainer { get; }
    bool IsViewLoaded { get; }
    Task LoadViewAsync();
    Task HideAsync(bool immediate = false);
    Task ShowAsync(bool immediate = false);
    void SetParentViewContainer(IPanelViewContainer parent);
  }
}
