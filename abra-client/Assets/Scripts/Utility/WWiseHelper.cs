using System;
using AK.Wwise;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace TalofaGames.Utility
{
  /// <summary>
  /// Wrapper for help functions for posting values/events to wwise
  /// </summary>
  public static class WWiseHelper
  {
    private static GameObject wwiseGameObject;
    /// <summary>
    /// The object we send all wwise events too
    /// </summary>
    /// <value>Finds the default wwise object via its tag: "WwiseGlobal"</value>
    public static GameObject WWiseGameObject
    {
      get
      {
        if (wwiseGameObject == null)
          wwiseGameObject = GameObject.FindGameObjectWithTag("WwiseGlobal");
        return wwiseGameObject;
      }
    }

    public static void PostEvent(Event eventToPost, AkCallbackManager.EventCallback callback = null, AkCallbackType callbackType = AkCallbackType.AK_Marker)
    {
      if (eventToPost == null)
        Debug.LogWarning($"[<b>{nameof(WWiseHelper)}</b>] Trying to post a null event!");

      eventToPost?.Post(
        WWiseGameObject,
        (uint)callbackType,
        callback != null ? callback : null/*ServiceLocator.Get<ActiveRunController>().CurrentRun.ContentRunDefinition.WwiseEvent?*/);
    }

    public static void PostSwitch(Switch switchToPost)
    {
      if (switchToPost == null)
        Debug.LogWarning($"[<b>{nameof(WWiseHelper)}</b>] Trying to post a null switch!");

      switchToPost.SetValue(WWiseGameObject);
    }

    public static void PostState(State stateToPost)
    {
      if (stateToPost == null)
        Debug.LogWarning($"[<b>{nameof(WWiseHelper)}</b>] Trying to post a null state!");

      stateToPost.SetValue();
    }

    public static void PostRTPC(RTPC rtpcToPost, float value)
    {
      if (rtpcToPost == null)
        Debug.LogWarning($"[<b>{nameof(WWiseHelper)}</b>] Trying to post a null RTPC!");

      if (value > 10000 || value < -10000)
      {
        Debug.LogError($"[<b>{nameof(WWiseHelper)}</b>] Invalid WWise RTPC Value: {value}");
        Debug.LogError($"[<b>{nameof(WWiseHelper)}</b>] Cannot accurately post RTPC! WWise limits RTPCs values to 10,000. Please restructure your data so the value > 10000 || value < -10000. Clamping and posting!");
        value = Mathf.Clamp(value, -10000, 10000);
      }

      rtpcToPost?.SetValue(WWiseGameObject, value);
    }

    public static float GetRTPC(RTPC rtpcToGet)
    {
      if (rtpcToGet == null)
      {
        Debug.LogError($"[<b>{nameof(WWiseHelper)}</b>] Trying to get a null RTPC!");
        return 0;
      }

      return rtpcToGet.GetValue(WWiseGameObject);
    }

    public static void PostRTPC(RTPC rtpcToPost, double value) => PostRTPC(rtpcToPost, (float)value);
    public static void PostRTPCAdd(RTPC rtpcToPost, float valueToAdd) => PostRTPC(rtpcToPost, rtpcToPost.GetValue(WWiseGameObject) + valueToAdd);
    public static void PostRTPCSubtract(RTPC rtpcToPost, float valueToAdd) => PostRTPC(rtpcToPost, rtpcToPost.GetValue(WWiseGameObject) - valueToAdd);
  }
}