using UnityEngine;

public class BridgeObject : _CellObject
{
    public enum Direction { Vertical, Horizontal }
    public Direction direction;

    private void Update()
    {
        if (direction == Direction.Vertical)
        {
            if (transform.eulerAngles != new Vector3(0, 0, 90))
                transform.eulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            if (transform.eulerAngles != new Vector3(0, 0, 0))
                transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public override bool OnLineEnter(LineManager line)
    {
        bool canContinueLine;

        line.EnterBridge(this);

        canContinueLine = true;
        return canContinueLine;
    }

    public override bool CanContinueLine(LineManager line, bool isBackTracking)
    {
        bool canPass;

        if (isBackTracking)
        {
            canPass = !cell.lines.Contains(line.LineBeingUsed);
        }
        else
        {
            canPass = true;
        }

        return canPass;
    }
}
