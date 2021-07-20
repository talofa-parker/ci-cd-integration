using TalofaGames.UI.PanelSystem;

namespace TalofaGames.Bootstrapping.UI
{
  public class BootstrapperPVC : PanelViewControllerBehaviour<BootstrapperPV>
  {
    public override void ViewWillAppear()
    {
      base.ViewWillAppear();
      View.Title.text = $"{View.Title.text}\nDevice UID:\n{UnityEngine.SystemInfo.deviceUniqueIdentifier}";
    }
  }
}