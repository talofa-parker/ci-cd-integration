using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TalofaGames.UI.PanelSystem
{
  public class PanelPusher : MonoBehaviour
  {
    [SerializeField]
    private PanelStackSystem stack;

    [SerializeField]
    protected PanelViewControllerProvider provider;

    [SerializeField]
    protected PanelType panelType;

    [SerializeField, FormerlySerializedAs("pushEvent")]
    private PushEvent pushOnEvent = PushEvent.Manual;

    [SerializeField]
    private bool clearBeforePush;

    public enum PushEvent
    {
      Manual,
      ButtonClick,
      Awake,
      Start,
      Enable
    }

    private void Awake()
    {
      if (pushOnEvent == PushEvent.Awake)
      {
        Push();
      }
      else if (pushOnEvent == PushEvent.ButtonClick)
      {
        GetComponent<Button>()?.onClick.AddListener(Push);
      }
    }

    private void Start()
    {
      if (pushOnEvent == PushEvent.Start)
      {
        Push();
      }
    }

    private void OnEnable()
    {
      if (pushOnEvent == PushEvent.Enable)
      {
        Push();
      }
    }

    public virtual void Push()
    {
      if (clearBeforePush)
      {
        ClearAndPush();
        return;
      }

      IPanelViewController controller;

      if (provider != null)
      {
        controller = provider.GetOrCreate(panelType);
      }
      else
      {
        controller = new PanelViewController(panelType);
      }

      stack.Push(controller);
    }

    public void ClearAndPush()
    {
      IPanelViewController controller;

      if (provider != null)
      {
        controller = provider.GetOrCreate(panelType);
      }
      else
      {
        controller = new PanelViewController(panelType);
      }

      stack.ClearAndPush(controller);
    }

  }
}

