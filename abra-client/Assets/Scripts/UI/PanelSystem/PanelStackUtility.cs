﻿using System.Collections.Generic;

namespace TalofaGames.UI.PanelSystem
{
  public static class PanelStackUtility
  {
    public static void GetVisiblePanelViewControllers(IPanelStackSystem stack, IList<IPanelViewController> output)
    {
      output.Clear();

      for (int i = stack.Count - 1; i >= 0; i--)
      {
        var currentController = stack[i];

        output.Add(currentController);

        //Stop as soon as we hit a panel that is fully opaque
        if (currentController.PanelType.VisibilityType == PanelType.Visibility.Opaque)
        {
          return;
        }
      }
    }
  }
}