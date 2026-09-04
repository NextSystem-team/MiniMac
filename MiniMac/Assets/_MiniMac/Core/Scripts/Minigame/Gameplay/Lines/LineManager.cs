using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineManager : MonoBehaviour 
{ 
    class PathStep
    {
        public CellData cell;
        public LineRenderer segment;
    }

    [SerializeField] private Camera mainCamera; 
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LineRenderer linePrefab;
    
    public LineRenderer LineBeingUsed { get; private set; }
    private Dot currentDotOut;
    private Color currentColor;
    private CellData currentCell;
    private CellData[] currentAdjacentCells;
    private List<LineRenderer> currentLines;
    private List<PathStep> path;

    private bool inBridge = false;
    private BridgeObject currentBridge;
    private BridgeObject.Direction currentBridgeTraversal;

    public bool canCreateLine = true;

    [SerializeField] private StageManager stageManager;
    
    private void Update() 
    { 
        if (!canCreateLine) return; 

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }
        
        LineCreator(); 
    } 
    
    private void LineCreator() { 
        if (Input.GetMouseButtonDown(0)) { 
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition); 
            Collider2D hit = Physics2D.OverlapPoint(worldPoint); 
            
            if (hit) { 
                if (hit.transform.CompareTag("DotOut")) {
                    currentDotOut = hit.GetComponent<Dot>();
                    if (currentDotOut.Line == null)
                    {
                        currentColor = currentDotOut.GetColor();

                        Vector2Int gridPosition = new(gridManager.Grid.WorldToCell(worldPoint).x, gridManager.Grid.WorldToCell(worldPoint).y); 
                        CellData starterCell = gridManager.GetCellData(gridPosition);
                     
                        currentAdjacentCells = gridManager.GetAdjacentCellData(gridPosition);

                        path = new List<PathStep>();
                        currentLines = new List<LineRenderer>();

                        LineBeingUsed = StartNewLine(
                                            currentColor, 
                                            gridPosition, 
                                            currentDotOut.transform, 
                                            starterCell
                                        );

                        path.Add(new PathStep
                        {
                            cell = starterCell,
                            segment = LineBeingUsed
                        });

                        currentDotOut.Line = LineBeingUsed;

                        currentCell = starterCell;
                    }
                } 
            } 
        } 
        else if (Input.GetMouseButton(0) && LineBeingUsed != null) 
        { 
            Vector2 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = gridManager.Grid.WorldToCell(worldPoint);

            CellData cell = gridManager.GetCellData(new Vector2Int(gridPosition.x, gridPosition.y));

            if (cell == null ||
                cell == currentCell ||
                !currentAdjacentCells.Contains(cell))
                return;

            int currentLineCount = LineBeingUsed.positionCount;
            bool isBacktracking = path.Count >= 2 && cell == path[^2].cell;

            if (inBridge)
            {
                bool isLeavingBridge = currentCell == currentBridge.cell;

                if (GetDirection(cell, currentCell) != currentBridgeTraversal)
                {
                    return;
                }
                else if (isLeavingBridge)
                {
                    currentBridge = null;
                    inBridge = false;
                }
            }

            if (isBacktracking)
            {
                if (cell.containedObject)
                {
                    cell.containedObject.OnLineEnter(this);
                }

                PathStep removedStep = path[^1];

                removedStep.segment.positionCount--;

                bool stillUsesSegment = path.Any(path => path != removedStep &&
                                                         path.cell == removedStep.cell &&
                                                         path.segment == removedStep.segment);

                if (!stillUsesSegment)
                {
                    removedStep.cell.lines.Remove(removedStep.segment);
                }

                path.RemoveAt(path.Count - 1);

                PathStep currentStep = path[^1];
                LineBeingUsed = currentStep.segment;
                currentCell = currentStep.cell;
                currentAdjacentCells = gridManager.GetAdjacentCellData(cell.gridPosition);

                if (removedStep.segment.positionCount < 1)
                {
                    currentLines.Remove(removedStep.segment);
                    Destroy(removedStep.segment.gameObject);
                }
            } 
            else if (cell.CanPass(this, isBacktracking)) 
            {
                _CellObject cellObject = cell.containedObject;

                if (cellObject)
                {
                    if (cell.containedObject.OnLineEnter(this))
                    {
                        ContinueLine(
                            LineBeingUsed, 
                            gridManager.Grid.GetCellCenterWorld(gridPosition), 
                            cell
                        );
                    }
                }
                else
                {
                    ContinueLine(
                        LineBeingUsed, 
                        gridManager.Grid.GetCellCenterWorld(gridPosition), 
                        cell
                    );
                }

                if (currentCell != null && currentLineCount != LineBeingUsed.positionCount)
                {
                    currentCell = cell;
                    currentAdjacentCells = gridManager.GetAdjacentCellData(cell.gridPosition);
                }
            }

        } 
        else if (Input.GetMouseButtonUp(0)) 
        {
            if (LineBeingUsed != null)
            {
                foreach (var pathStep in path)
                {
                    pathStep.cell.lines.Remove(pathStep.segment);
                }

                foreach (var line in currentLines)
                {
                    Destroy(line.gameObject);
                }

                currentDotOut = null;
                currentColor = default;
                LineBeingUsed = null;
                currentCell = null;
                currentLines = null;
                path = null;
            }
        } 
    } 

    private LineRenderer StartNewLine(Color color, Vector2Int startPosition, Transform parent, CellData starterCell)
    {
        Vector3Int starterCellCenter = new(startPosition.x, startPosition.y, 0);
        Vector3 lineStartPosition = gridManager.Grid.GetCellCenterWorld(starterCellCenter);

        LineRenderer newLine = Instantiate(linePrefab);
        newLine.transform.parent = parent;
        newLine.transform.position = lineStartPosition;
        newLine.startColor = color;
        newLine.endColor = color;

        newLine.SetPosition(0, lineStartPosition);

        currentLines.Add(newLine);

        newLine.name = $"Line_{color.ToHexString()}_({currentLines.IndexOf(newLine)})";
        starterCell.lines.Add(newLine);

        return newLine;
    }

    public void ContinueLine(LineRenderer line, Vector3 newPosition, CellData cell) {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, newPosition);

        if (!cell.lines.Contains(line))
        {
            cell.lines.Add(line);
        }

        path.Add(new PathStep
        {
            cell = cell,
            segment = line
        });
    }

    private void ResetLine()
    {
        currentDotOut = null;
        LineBeingUsed = null;
        currentColor = default;
        currentCell = null;
        currentAdjacentCells = null;
        currentLines = null;
        path = null;
    }

    public void EnterDotIn(DotInObject dotInObject)
    {
        Dot dotIn = dotInObject.GetComponent<Dot>();

        if (dotIn.GetColor() == currentColor)
        {
            ContinueLine(
                LineBeingUsed, 
                CellPositionToVector3(dotInObject.cell.gridPosition), 
                dotInObject.cell
            );

            dotIn.Line = LineBeingUsed;

            ResetLine();

            stageManager.dotsConnected++;
        }
    }

    public void EnterBridge(BridgeObject bridgeObject)
    {
        BridgeObject.Direction movementDirection =
        GetDirection(
            bridgeObject.cell,
            currentCell
        );

        bool shouldBeUpper = movementDirection == bridgeObject.direction;

        int targetSorting = shouldBeUpper ? 
                            SortingLayers.UPPER_LINE
                          : SortingLayers.LINE;

        if (LineBeingUsed.sortingOrder != targetSorting)
        {
            LineBeingUsed = StartNewLine(
                currentColor,
                currentCell.gridPosition,
                currentDotOut.transform,
                bridgeObject.cell
            );

            LineBeingUsed.sortingOrder = targetSorting;
        }

        currentBridgeTraversal = movementDirection;
        currentBridge = bridgeObject;
        inBridge = true;
    }

    private BridgeObject.Direction GetDirection(CellData nextCell, CellData currentCell)
    {
        Vector2Int positionCalculation = currentCell.gridPosition - nextCell.gridPosition;
        Vector2Int nextCellDirection = new(Mathf.Abs(positionCalculation.x), Mathf.Abs(positionCalculation.y));

        if (nextCellDirection == Vector2Int.up)
        {
            return BridgeObject.Direction.Vertical;
        }
        else
        {
            return BridgeObject.Direction.Horizontal;
        }
    }
        
    private Vector3 CellPositionToVector3(Vector2Int position)
    {
        return gridManager.Grid.GetCellCenterWorld(new Vector3Int(position.x, position.y, 0));
    }
}