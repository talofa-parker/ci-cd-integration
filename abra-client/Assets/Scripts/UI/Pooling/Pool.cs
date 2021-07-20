using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TalofaGames.UI.Pooling
{
  [Serializable]
  public class Pool
  {
    [ShowInInspector, ReadOnly]
    public PoolableGameObject Prefab => prefab;

    public bool SpawnInOrder = true;

    [ShowInInspector, ReadOnly]
    private readonly List<PoolableGameObject> instanceQueue = new List<PoolableGameObject>();
    private readonly PoolableGameObject prefab;

    public Pool(PoolableGameObject prefab)
    {
      this.prefab = prefab;
    }

    public T Spawn<T>(Transform parent = null) where T : PoolableGameObject
    {
      Debug.Assert(prefab is T, "pool prefab is not the correct type");
      return Spawn(parent) as T;
    }

    public PoolableGameObject Spawn(Transform parent = null)
    {
      PoolableGameObject spawnedObject = null;

      if (instanceQueue.Count > 0)
      {
        spawnedObject = instanceQueue[0];
        instanceQueue.RemoveAt(0);
      }
      else
      {
        spawnedObject = parent != null ? Object.Instantiate(prefab, parent) : Object.Instantiate(prefab);
        spawnedObject.SourcePool = this;
      }

      //If prefab is active, then also activate the spawned game object
      if (Prefab.gameObject.activeSelf)
      {
        spawnedObject.gameObject.SetActive(true);
      }

      spawnedObject.OnPoolableSpawned();

      return spawnedObject;
    }

    public void Despawn(PoolableGameObject spawnedObject)
    {
      Debug.Assert(spawnedObject != null, "Attempting to despawn a null object");
      Debug.Assert(spawnedObject.SourcePool == this, "Attempting to despawn an object to a pool that is not its spawn source");

      spawnedObject.OnPoolableDespawn();
      spawnedObject.gameObject.SetActive(false);
      instanceQueue.Add(spawnedObject);
    }

    public void RemoveInstance(PoolableGameObject instance)
    {
      instanceQueue.Remove(instance);
    }
  }
}