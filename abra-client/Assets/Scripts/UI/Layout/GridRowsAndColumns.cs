﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TalofaGames.UI.Layout
{

  [RequireComponent(typeof(GridLayoutGroup))]
  public class GridRowsAndColumns : UIBehaviour
  {

    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float maxAspectRatio = 1;

    protected override void Start()
    {
      base.Start();
      RefreshLayout();
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
      base.OnValidate();

      if (rows < 1)
      {
        rows = 1;
      }

      if (columns < 1)
      {
        columns = 1;
      }

      RefreshLayout();
    }

#endif

    protected override void OnRectTransformDimensionsChange()
    {
      RefreshLayout();
    }

    private void RefreshLayout()
    {
      var gridLayout = GetComponent<GridLayoutGroup>();
      var rectTransform = transform as RectTransform;

      var width = rectTransform.rect.width - gridLayout.padding.left - gridLayout.padding.right;
      var height = rectTransform.rect.height - gridLayout.padding.top - gridLayout.padding.bottom;

      var cellWidth = width / columns;
      var cellHeight = height / rows;

      //Clamp aspect ratio
      if ((cellHeight / cellWidth) > maxAspectRatio)
      {
        cellHeight = cellWidth * maxAspectRatio;
      }

      gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }

  }

}