using System.Threading.Tasks;

namespace TalofaGames.UI.PanelSystem
{
  public interface IPanelAnimator
  {
    Task TransitionShowAsync();
    Task TransitionHideAsync();
  }
}