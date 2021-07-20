﻿namespace TalofaGames.UI.PanelSystem
{
  public interface IPanelViewControllerProvider
  {
    T Get<T>(PanelType type) where T : class, IPanelViewController;
    IPanelViewController Get(PanelType type);
  }
}

