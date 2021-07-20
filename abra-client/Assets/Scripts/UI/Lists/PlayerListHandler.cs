using System.Collections.Generic;
using TalofaGames.UI.Pooling;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace TalofaGames.UI.Lists
{
  public class PlayerListHandler : MonoBehaviour
  {
    [Required]
    public RectTransform ContentRoot;
    [Required]
    public PlayerListItem Prefab;
    [ReadOnly]
    public List<PlayerListItem> Entries = new List<PlayerListItem>();

    private Pool pool;

    public virtual PlayerListItem CreateOrUpdate(string playerName, string playerId)
    {
      foreach (PlayerListItem item in Entries)
      {
        if (item == null)
          continue;
        if (playerId != item.PlayerId)
          continue;

        return item;
      }

      PlayerListItem playerItem = Instantiate(Prefab, ContentRoot) as PlayerListItem;
      playerItem.PlayerName = playerName;
      playerItem.PlayerId = playerId;
      Entries.Add(playerItem);
      return playerItem;
    }

    public void RemoveLocalPlayer()
    {
      for (int i = 0; i < Entries.Count; i++)
      {
        if (Entries[i] == null)
          continue;
        if (Entries[i].IsMe)
        {
          GameObject playerToRemove = Entries[i].gameObject;
          Entries.Remove(Entries[i]);
          Destroy(playerToRemove.gameObject);
        }
      }
    }

    public virtual void Destroy()
    {
      for (int i = 0; i < ContentRoot.childCount; i++)
        Destroy(ContentRoot.GetChild(i).gameObject);
    }

    [Button("Spawn Temp List Item ")]
    private void SpawnDevListItem()
    {
#if UNITY_EDITOR
      // Spawns a prefab but maintains is reference to its prefab (basically like dragging a prefab in)
      var go = PrefabUtility.InstantiatePrefab(Prefab.gameObject, ContentRoot) as GameObject;
      // Select it
      Selection.activeGameObject = go;
#endif
    }
  }
}
