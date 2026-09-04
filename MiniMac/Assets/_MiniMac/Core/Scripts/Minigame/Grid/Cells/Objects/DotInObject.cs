using UnityEngine;

public class DotInObject : _CellObject
{
    public override bool OnLineEnter(LineManager line)
    {
        bool canContinueLine;

        line.EnterDotIn(this);

        canContinueLine = false;
        return canContinueLine;
    }
}
