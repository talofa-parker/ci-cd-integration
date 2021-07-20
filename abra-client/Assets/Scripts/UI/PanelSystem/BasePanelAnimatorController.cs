﻿using System.Threading.Tasks;
using UnityEngine;

namespace TalofaGames.UI.PanelSystem
{
  public abstract class BasePanelAnimatorController : MonoBehaviour, IPanelAnimator
  {
    [SerializeField]
    protected Animator animator;

    public abstract Task TransitionShowAsync();

    public abstract Task TransitionHideAsync();

    protected async Task TransitionAsync(int stateHash, int layer)
    {
      //Ensure Animator is Initialized
      while (Application.isPlaying && (!animator.isInitialized || !animator.gameObject.activeInHierarchy))
      {
        await Task.Yield();
      }

      if (!Application.isPlaying || animator == null)
      {
        return;
      }

      animator.Play(stateHash);

      //Wait until we're in the target state
      while (Application.isPlaying && animator.GetCurrentAnimatorStateInfo(layer).shortNameHash != stateHash)
      {
        await Task.Yield();
      }

      float normalizedTime = 0;
      while (Application.isPlaying && normalizedTime < 1 && !Mathf.Approximately(normalizedTime, 1f))
      {
        var info = animator.GetCurrentAnimatorStateInfo(0);

        if (normalizedTime <= info.normalizedTime)
        {
          normalizedTime = info.normalizedTime;
        }
        else
        {
          //If suddenly normalized time is less than 1 we've looped back around or something.
          normalizedTime = 1f;
        }

        await Task.Yield();
      }
    }
  }
}
