using UnityEngine;
using UnityEngine.UI;

namespace TalofaGames.UI.Layout
{

  public abstract class HorizontalOrVerticalPagedLayoutGroup : LayoutGroup, ILayoutSelfController
  {
    [SerializeField]
    protected float spacing;

    [SerializeField]
    protected RectTransform viewport;
    public RectTransform Viewport => viewport;
  }
}