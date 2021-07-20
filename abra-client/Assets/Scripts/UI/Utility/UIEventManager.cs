using TalofaGames.UI.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TalofaGames.UI.Utility
{

  public interface IUIEventManager
  {
    void Lock();
    void Unlock();
  }

  public class UIEventManager : MonoBehaviour, IUIEventManager
  {

    static UIEventManager _current;
    public static UIEventManager Current
    {
      get
      {
        return _current;
      }
    }

    public static void LockCurrent()
    {
      if (_current != null)
      {
        _current.Lock();
      }
    }

    public static void UnlockCurrent()
    {
      if (_current != null)
      {
        _current.Unlock();
      }
    }

    public static bool IsPointerOverUI()
    {
      return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    EventSystem eventSystem;

    [SerializeField]
    CountLock inputLock = new CountLock();

    protected virtual void Awake()
    {
      eventSystem = GetComponent<EventSystem>();
      _current = this;
      inputLock.OnUnlocked.AddListener(this.OnUnlock);
      inputLock.OnLocked.AddListener(this.OnLock);
    }

    public void Lock()
    {
      inputLock.Lock();
    }

    public void Unlock()
    {
      inputLock.Unlock();
    }

    void OnLock()
    {
      if (eventSystem != null)
      {
        eventSystem.enabled = false;
      }
    }

    void OnUnlock()
    {
      if (eventSystem != null)
      {
        eventSystem.enabled = true;
      }
    }

  }

}