using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TalofaGames.UI.PanelSystem
{
  /// <summary>
  /// PanelStackSystem maintains a stack of panel options
  /// Use together with a PanelStackController to create UI system with a history and a back button
  /// The queue manages pushing panels onto a queue. When a panel is popped, the next queued one with be pushed.
  /// </summary>
  [CreateAssetMenu(menuName = "UI/PanelSystem/PanelStackQueueSystem")]
  public class PanelStackQueueSystem : PanelStackSystem
  {
    [ShowInInspector, ReadOnly]
    public Queue<IPanelViewController> PanelQueue = new Queue<IPanelViewController>();

    public void PushToQueue(IPanelViewController controller)
    {
      if (Count == 0)
        Push(controller);
      else
        PanelQueue.Enqueue(controller);
    }

    public override void ViewDidDisappear(PanelStackControllerBehaviour panelStackControllerBehaviour)
    {
      base.ViewDidDisappear(panelStackControllerBehaviour);
      if (PanelQueue.Count != 0)
        Push(PanelQueue.Dequeue());
    }
  }
}