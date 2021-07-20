using System.Collections.Generic;

namespace TalofaGames.UI.PanelSystem
{
  public interface IPanelStackSystem : IEnumerable<IPanelViewController>
  {
    int Count { get; }
    IPanelViewController this[int index] { get; }
    void AddController(IPanelStackController controller);
    void RemoveController(IPanelStackController controller);
    void Push(IPanelViewController controller);
    void Pop();
  }
}
