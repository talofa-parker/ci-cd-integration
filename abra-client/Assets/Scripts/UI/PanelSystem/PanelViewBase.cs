﻿using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TalofaGames.UI.PanelSystem
{
  public abstract class PanelViewBase : MonoBehaviour
  {
    public abstract Task ShowAsync(CancellationToken cancellationToken);
    public abstract Task HideAsync(CancellationToken cancellationToken);
    public abstract void ShowImmediate();
    public abstract void HideImmediate();

    public Task ShowAsync()
    {
      return ShowAsync(CancellationToken.None);
    }

    public Task HideAsync()
    {
      return HideAsync(CancellationToken.None);
    }

  }
}


