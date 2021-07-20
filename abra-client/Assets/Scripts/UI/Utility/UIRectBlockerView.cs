﻿using TalofaGames.UI.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace TalofaGames.UI.Utility
{
  public class UIRectBlockerView : MonoBehaviour
  {
    private RectTransform _blockerParent;

    private RectTransform _leftBlocker;
    private RectTransform _rightBlocker;
    private RectTransform _topBlocker;
    private RectTransform _bottomBlocker;

    [SerializeField]
    private RectTransform _passThroughRect;

    [SerializeField, Tooltip("Highlight should be a child of this object. It will overlay the dimensions of the pass through rect or be disabled if there isn't one.")]
    protected RectTransform _highlight;

    private Canvas _parentCanvas;

    private void Awake()
    {
      _blockerParent = new GameObject("blocker").GetOrAddComponent<RectTransform>();
      _blockerParent.transform.SetParent(transform);
      _blockerParent.localScale = Vector3.one;
      _blockerParent.localPosition = Vector3.zero;
      _blockerParent.gameObject.SetActive(_passThroughRect != null);
      _blockerParent.anchorMin = Vector2.zero;
      _blockerParent.anchorMax = Vector2.one;
      _blockerParent.sizeDelta = Vector2.zero;
      _blockerParent.anchoredPosition = Vector2.zero;

      _leftBlocker = new GameObject("leftBlocker").GetOrAddComponent<RectTransform>();
      _rightBlocker = new GameObject("rightBlocker").GetOrAddComponent<RectTransform>();
      _topBlocker = new GameObject("topBlocker").GetOrAddComponent<RectTransform>();
      _bottomBlocker = new GameObject("bottomBlocker").GetOrAddComponent<RectTransform>();

      _leftBlocker.SetParent(_blockerParent);
      _rightBlocker.SetParent(_blockerParent);
      _topBlocker.SetParent(_blockerParent);
      _bottomBlocker.SetParent(_blockerParent);

      _leftBlocker.localScale = Vector3.one;
      _rightBlocker.localScale = Vector3.one;
      _topBlocker.localScale = Vector3.one;
      _bottomBlocker.localScale = Vector3.one;

      _leftBlocker.localPosition = Vector3.zero;
      _rightBlocker.localPosition = Vector3.zero;
      _topBlocker.localPosition = Vector3.zero;
      _bottomBlocker.localPosition = Vector3.zero;

      _leftBlocker.pivot = new Vector2(0, 0.5f);
      _bottomBlocker.pivot = new Vector2(0.5f, 0f);
      _topBlocker.pivot = new Vector2(0.5f, 1f);
      _rightBlocker.pivot = new Vector2(1, 0.5f);

      _leftBlocker.anchoredPosition = Vector2.zero;
      _rightBlocker.anchoredPosition = Vector2.zero;
      _topBlocker.anchoredPosition = Vector2.zero;
      _bottomBlocker.anchoredPosition = Vector2.zero;

      _parentCanvas = GetComponentInParent<Canvas>();

      var img = _leftBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;

      img = _rightBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;

      img = _topBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;

      img = _bottomBlocker.gameObject.GetOrAddComponent<Image>();
      img.color = Color.clear;
      img.raycastTarget = true;

      _blockerParent.gameObject.SetActive(false);

      if (_highlight != null)
      {
        _highlight.gameObject.SetActive(false);
      }
    }

    public void Show(RectTransform passThrough)
    {
      _blockerParent.gameObject.SetActive(true);
      _passThroughRect = passThrough;
      Refresh();
    }

    [ContextMenu("Show")]
    public void Show()
    {
      _blockerParent.gameObject.SetActive(true);
      Refresh();
    }

    [ContextMenu("Dismiss")]
    public void Dismiss()
    {
      _blockerParent.gameObject.SetActive(false);

      if (_highlight != null)
      {
        _highlight.gameObject.SetActive(false);
      }
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
      Vector2 topLeft = Vector2.zero;
      Vector2 bottomRight = Vector2.zero;

      if (_passThroughRect != null)
      {
        Vector3[] corners = new Vector3[4];
        _passThroughRect.GetWorldCorners(corners);

        //Check for a world space camera if we have one
        var worldCamera = _parentCanvas.worldCamera;

        //OK for camera to be null here if canvas render mode is screen space overlay
        Vector3 topLeftScreenPoint = RectTransformUtility.WorldToScreenPoint(worldCamera, corners[0]);
        Vector3 bottomRightScreenPoint = RectTransformUtility.WorldToScreenPoint(worldCamera, corners[2]);

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_blockerParent, topLeftScreenPoint, worldCamera,
          out topLeft))
        {
          Debug.LogError("plane of the RectTransform was not hit");
          return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_blockerParent, bottomRightScreenPoint, worldCamera,
          out bottomRight))
        {
          Debug.LogError("plane of the RectTransform was not hit");
          return;
        }

        if (_highlight != null)
        {
          _highlight.gameObject.SetActive(true);
          _highlight.pivot = new Vector2(0.5f, 0.5f);
          _highlight.anchorMin = new Vector2(0.5f, 0.5f);
          _highlight.anchorMax = new Vector2(0.5f, 0.5f);
          var rect = _passThroughRect.rect;
          _highlight.sizeDelta = new Vector2(rect.width, rect.height);
          _highlight.anchoredPosition = (topLeft + bottomRight) * 0.5f;
        }

      }
      else if (_highlight != null)
      {
        _highlight.gameObject.SetActive(false);
      }

      _leftBlocker.anchorMin = Vector2.zero;
      _leftBlocker.anchorMax = new Vector2(0.5f, 1f);
      _leftBlocker.sizeDelta = new Vector2(topLeft.x, 0);

      _bottomBlocker.anchorMin = new Vector2(0f, 0f);
      _bottomBlocker.anchorMax = new Vector2(1f, 0.5f);
      _bottomBlocker.sizeDelta = new Vector2(0, topLeft.y);

      _rightBlocker.anchorMin = new Vector2(0.5f, 0f);
      _rightBlocker.anchorMax = new Vector2(1f, 1f);
      _rightBlocker.sizeDelta = new Vector2(-bottomRight.x, 0);

      _topBlocker.anchorMin = new Vector2(0f, 0.5f);
      _topBlocker.anchorMax = new Vector2(1f, 1f);
      _topBlocker.sizeDelta = new Vector2(0, -bottomRight.y);
    }

  }
}