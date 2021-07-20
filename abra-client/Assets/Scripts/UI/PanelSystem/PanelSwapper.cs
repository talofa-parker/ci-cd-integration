using TalofaGames.UI.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace TalofaGames.UI.PanelSystem
{
  public class PanelSwapper : MonoBehaviour
  {
    [SerializeField] private PanelSwapSystem swapSystem;

    [SerializeField, Help("If controller is null PanelSwapper will attempt to get the controller from the provider using PanelType")]
    private PanelViewControllerBehaviour controller;

    [SerializeField] private PanelType panelType;

    [SerializeField] private PanelViewControllerProvider controllerProvider;

    [SerializeField] private SwapEvent swapEvent = SwapEvent.Manual;

    [Header("Button"), SerializeField] private Button button;
    [SerializeField] private bool controlButtonInteractivity;

    [Header("Image Colors"), SerializeField] private Image image;
    [SerializeField] private bool controlImageTint;
    [SerializeField] private Color activeImageColor = Color.white;
    [SerializeField] private Color disabledImageColor = Color.grey;

    [Header("Text Colors"), SerializeField] private Text text;
    [SerializeField] private bool controlTextColor;
    [SerializeField] private Color activeTextColor = Color.white;
    [SerializeField] private Color disabledTextColor = Color.grey;

    [Header("Activate/Deactivate Objects"), SerializeField]
    private GameObject[] OnWhenShowing;
    [SerializeField]
    private GameObject[] OffWhenShowing;
    [SerializeField]
    private GameObject[] OnWhenHiding;
    [SerializeField]
    private GameObject[] OffWhenHiding;

    public enum SwapEvent
    {
      Manual,
      ButtonClick,
      Awake,
      Start,
      Enable
    }

    private void Awake()
    {
      if (swapEvent == SwapEvent.Awake)
      {
        Swap();
      }
      else if (swapEvent == SwapEvent.ButtonClick)
      {
        button?.onClick.AddListener(Swap);
      }
    }

    private void Start()
    {
      if (swapEvent == SwapEvent.Start)
      {
        Swap();
      }
    }

    private void OnEnable()
    {
      if (swapSystem != null)
      {
        swapSystem.OnSwap.AddListener(OnSwapped);
      }
      if (swapEvent == SwapEvent.Enable)
      {
        Swap();
      }
    }

    private void OnDisable()
    {
      if (swapSystem != null)
      {
        swapSystem.OnSwap.RemoveListener(OnSwapped);
      }
    }

    private void OnSwapped()
    {
      Refresh();
    }

    private void Refresh()
    {
      if (swapSystem == null)
      {
        return;
      }

      //If the current view contr
      var currentViewController = swapSystem.CurrentViewController;
      var showingOurPanel = false;

      if (controller != null)
      {
        showingOurPanel = ReferenceEquals(controller, currentViewController);
      }
      else
      {
        showingOurPanel = currentViewController.PanelType == panelType;
      }

      if (controlButtonInteractivity && button != null)
      {
        if (showingOurPanel)
        {
          //Current showing panel is the panel for this button so disable this button
          button.interactable = false;
        }
        else
        {
          //Currently showing a panel different than the panel this button will show so let's enable the button
          button.interactable = true;
        }
      }
      if (controlImageTint && image != null)
      {
        image.color = showingOurPanel ? activeImageColor : disabledImageColor;
      }

      if (controlTextColor && text != null)
      {
        text.color = showingOurPanel ? activeTextColor : disabledTextColor;
      }

      if (showingOurPanel)
      {
        ToggleObjects(OnWhenShowing, true);
        ToggleObjects(OffWhenShowing, false);
      }
      else
      {
        ToggleObjects(OnWhenHiding, true);
        ToggleObjects(OffWhenHiding, false);

      }
    }

    public async void Swap()
    {
      IPanelViewController swapController = controller;
      if (swapController == null)
      {
        swapController = controllerProvider.Get(panelType);
      }
      await swapSystem.ShowAsync(swapController);
      Refresh();
    }

    private void ToggleObjects(GameObject[] objects, bool state)
    {
      if (objects == null || objects.Length == 0)
        return;

      foreach (GameObject item in objects)
      {
        if (item == null)
          continue;

        item.SetActive(state);
      }
    }

    private void OnValidate()
    {
      if (button == null)
      {
        button = GetComponent<Button>();
      }
      if (image == null)
      {
        image = GetComponent<Image>();
      }
    }

  }
}


