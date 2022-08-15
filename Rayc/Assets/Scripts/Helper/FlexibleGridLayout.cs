using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public int rows;
    int prevRows = -1;
    // public int columns;
    // const int rows = 3;
    public int columns = 5;
    Vector2 cellSize;

    public Vector2 spacing;

    public override void CalculateLayoutInputHorizontal()
    {

        base.CalculateLayoutInputHorizontal();
        rows = Mathf.FloorToInt(rectTransform.childCount / columns);
        // columns = Mathf.CeilToInt(sqrRt);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)columns) - ((spacing.x / (float)columns) * 2) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = (parentHeight / (float)rows) - ((spacing.y / (float)rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        // cellSize.x = cellWidth;
        // cellSize.y = cellHeight;
        cellSize.x = 144.392f;
        cellSize.y = 144.392f;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / columns;
            columnCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;; 

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);

        }

        // update rect transform height for scroll rect
        if (prevRows != rows)
        {
            prevRows = rows;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 
                                            (rows + 1) * (cellSize.y + spacing.y));
        }
    }

    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}
