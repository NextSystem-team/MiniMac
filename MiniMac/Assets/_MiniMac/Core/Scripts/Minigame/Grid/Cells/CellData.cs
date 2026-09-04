using System.Collections.Generic;
using UnityEngine;

public class CellData   
{
    public Vector2Int gridPosition;
    public List<LineRenderer> lines;
    public _CellObject containedObject;

    public CellData()
    {
        gridPosition = Vector2Int.zero;
        lines = new List<LineRenderer>();
    }

    public CellData(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;
        lines = new List<LineRenderer>();
    }

    public bool CanPass(LineManager line, bool isBackTracking)
    {
        bool canPass;

        if (containedObject != null)
        {
            canPass = containedObject.CanContinueLine(line, isBackTracking);
        }
        else
        {
            canPass = lines.Count == 0;
        }

        return canPass;
    }
}
