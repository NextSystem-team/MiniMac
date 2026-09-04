using UnityEngine;

public abstract class _CellObject : MonoBehaviour
{
    public CellData cell;

    public virtual bool OnLineEnter(LineManager line)
    {
        bool canContinueLine;


        canContinueLine = true;
        return canContinueLine;
    }

    public virtual bool CanContinueLine(LineManager line, bool isBackTracking)
    {
        return cell.lines.Count == 0;
    }
}
