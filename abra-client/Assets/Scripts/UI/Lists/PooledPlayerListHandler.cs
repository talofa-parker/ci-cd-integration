using System;
using System.Collections;
using System.Collections.Generic;
using TalofaGames.UI.Pooling;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TalofaGames.UI.Lists
{
  public class PooledPlayerListHandler : MonoBehaviour
  {
    private enum SpawningMode
    {
      Default,
      Top,
      Bottom,
    }

    public enum ScrollStyle
    {
      Smooth,
      Snap,
    }

    [ShowInInspector, ReadOnly]
    public Pool Pool { get; private set; }
    [Required]
    public RectTransform ContentRoot;
    [Required]
    public PlayerListItem Prefab;
    [ShowInInspector, ReadOnly]
    public List<PlayerListItem> Entries { get; private set; } = new List<PlayerListItem>();
    [HideInInspector]
    public List<RectTransform> RectEntries { get; private set; } = new List<RectTransform>();
    [InfoBox("If used in a scrolling list, in order to execute auto-snapping/scrolling-too methods, assign this. If nothing is assigned, we will look in the parent", InfoMessageType.None)]
    public ScrollRect Scroll;
    public Action Scrolling, Scrolled, ScrolledLeftMost, ScrolledRightMost;

    [SerializeField, InfoBox("Change how entries get ordered when being added as a child", InfoMessageType.None)]
    private SpawningMode spawningMode;
    [ReadOnly]
    private int desiredScrollIndex = 0;
    private Coroutine lerpToCoroutine;

    private int maxScrollIndex => (RectEntries == null ? 0 : RectEntries.Count - 1);

    /// <summary>
    /// Creates entires from the existing objects and resets there data with the
    /// new.
    /// </summary>
    /// <param name="playerName">The user displayed name of the player</param>
    /// <param name="playerId">The players id</param>
    /// <returns>PlayerListItem</returns>
    public virtual PlayerListItem CreateOrUpdateFromPool(string playerName, string playerId)
    {
      if (Pool == null)
        Pool = new Pool(Prefab);

      PlayerListItem playerItem = Pool.Spawn<PlayerListItem>(ContentRoot);
      playerItem.ListHandler = this;
      playerItem.PlayerName = playerName;
      playerItem.PlayerId = playerId;

      Entries.Add(playerItem);
      RectTransform rect = playerItem.GetComponent<RectTransform>();
      RectEntries.Add(rect);
      RespectSpawningMode(rect);
      return playerItem;
    }

    /// <summary>
    /// Despawns all the entires in this list and pool
    /// </summary>
    public void DespawnAll()
    {
      for (int i = Entries.Count - 1; i >= 0; i--)
      {
        PlayerListItem item = Entries[i];
        Pool.Despawn(item);
        Entries.Remove(item);
        RectEntries.Remove(item.GetComponent<RectTransform>());
      }
    }

    public void Despawn(PlayerListItem entity)
    {
      foreach (PlayerListItem item in Entries)
        if (item == entity)
        {
          Pool.Despawn(item);
          Entries.Remove(item);
          RectEntries.Remove(item.GetComponent<RectTransform>());
          return;
        }

      Debug.LogError($"[<b>{nameof(PooledPlayerListHandler)}</b>] Trying to despawn {entity.name} but its not in the pool!", entity);
    }

    public void ScrollLeft(ScrollStyle style, float lerpTime = 0)
    {
      desiredScrollIndex--;
      if (desiredScrollIndex <= 0)
      {
        desiredScrollIndex = 0;
        ScrolledLeftMost?.Invoke();
      }

      switch (style)
      {
        case ScrollStyle.Smooth:
          LerpTo(lerpTime, RectEntries[desiredScrollIndex]);
          break;
        case ScrollStyle.Snap:
          SnapTo(RectEntries[desiredScrollIndex]);
          break;
      }
    }

    public void ScrollRight(ScrollStyle style, float lerpTime = 0)
    {
      desiredScrollIndex++;
      if (desiredScrollIndex >= maxScrollIndex)
      {
        desiredScrollIndex = maxScrollIndex;
        ScrolledRightMost?.Invoke();
      }

      switch (style)
      {
        case ScrollStyle.Smooth:
          LerpTo(lerpTime, RectEntries[desiredScrollIndex]);
          break;
        case ScrollStyle.Snap:
          SnapTo(RectEntries[desiredScrollIndex]);
          break;
      }
    }

    /// <summary>
    /// Snaps to an element on the scrollable list
    /// </summary>
    /// <param name="target">The element to snap too</param>
    public void SnapTo(RectTransform target)
    {
      ScrollRect scrollRect = GetScrollRect();
      Canvas.ForceUpdateCanvases();

      ContentRoot.anchoredPosition =
          (Vector2)scrollRect.transform.InverseTransformPoint(ContentRoot.position)
          - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    /// <summary>
    /// Smoothly moves to an element on the scrollable list
    /// </summary>
    /// <param name="lerpTime">The speed to lerp</param>
    /// <param name="target">The element to lerp too</param>
    public void LerpTo(float lerpTime, RectTransform target)
    {
      ScrollRect scrollRect = GetScrollRect();
      Canvas.ForceUpdateCanvases();

      Scrolling?.Invoke();
      if (lerpToCoroutine != null)
        StopCoroutine(lerpToCoroutine);
      lerpToCoroutine = StartCoroutine(Lerp(lerpTime));

      IEnumerator Lerp(float time)
      {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
          ContentRoot.anchoredPosition = Vector2.Lerp(
            (Vector2)scrollRect.transform.InverseTransformPoint(ContentRoot.position),
            (Vector2)scrollRect.transform.InverseTransformPoint(ContentRoot.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position),
            (elapsedTime / time));

          elapsedTime += Time.deltaTime;
          yield return new WaitForEndOfFrame();
        }
        Scrolled?.Invoke();
      }
    }

    public void RemoveLocalPlayer()
    {
      for (int i = 0; i < Entries.Count; i++)
      {
        if (Entries[i] == null)
          continue;
        if (Entries[i].IsMe)
        {
          Pool.Despawn(Entries[i]);
          Entries.Remove(Entries[i]);
        }
      }
    }

    private ScrollRect GetScrollRect()
    {
      if (Scroll != null)
        return Scroll;
      else if (transform.parent.TryGetComponent(out ScrollRect rect))
        return rect;
      else
      {
        Debug.LogError($"[<b>{nameof(PooledPlayerListHandler)}</b>] Trying to auto scroll but this listhandler can't find a scroll rect!");
        return null;
      }
    }

    private void RespectSpawningMode(RectTransform rect)
    {
      switch (spawningMode)
      {
        case SpawningMode.Default:
        default:
          break;
        case SpawningMode.Top:
          rect.SetAsFirstSibling();
          break;
        case SpawningMode.Bottom:
          rect.SetAsLastSibling();
          break;
      }
    }

    [Button("Spawn Temp List Item ")]
    private void SpawnDevListItem()
    {
#if UNITY_EDITOR
      // Spawns a prefab but maintains is reference to its prefab (basically like dragging a prefab in)
      var go = PrefabUtility.InstantiatePrefab(Prefab.gameObject, ContentRoot) as GameObject;
      // Select it
      Selection.activeGameObject = go;
      RespectSpawningMode(go.GetComponent<RectTransform>());
#endif
    }
  }
}
