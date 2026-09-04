using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 7;
    [SerializeField] private int columns = 7;
    [SerializeField] private float margin = 1f;

    [SerializeField] private GameObject dotOutPrefab;
    [SerializeField] private GameObject dotInPrefab;
    [SerializeField] private GameObject bridgePrefab;

    [SerializeField] private GameObject gridBackground;

    public Grid Grid { get; private set; }
    private CellData[,] gridData; //posi��es do grid

    private void Start()
    {
        Grid = GetComponent<Grid>();
        gridData = new CellData[columns, rows];

        for (int y = 0; y < rows; y++) //coordenada Y
        {
            for (int x = 0; x < columns; x++) //coordenada X
            {
                Vector2Int currentTilePosition = new(x, y);
                gridData[x, y] = new CellData(currentTilePosition);
                //Cria uma c�lula na posi��o atual do grid

                if (x == 0 && y == 0)
                {
                    GameObject dotOut = Instantiate(dotOutPrefab);
                    dotOut.transform.parent = transform;
                    dotOut.transform.position = Grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                }else if (x == rows-1 && y == columns-1)
                {
                    GameObject dotIn = Instantiate(dotInPrefab);
                    dotIn.transform.parent = transform;
                    dotIn.transform.position = Grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    dotIn.GetComponent<_CellObject>().cell = gridData[x, y];
                    gridData[x, y].containedObject = dotIn.GetComponent<_CellObject>();
                }
                else if (x == rows - 1 && y == columns - 4)
                {
                    GameObject dotIn = Instantiate(dotInPrefab);
                    dotIn.transform.parent = transform;
                    dotIn.transform.position = Grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    dotIn.GetComponent<Dot>().SetColor(Color.blue);
                    dotIn.GetComponent<_CellObject>().cell = gridData[x, y];
                    gridData[x, y].containedObject = dotIn.GetComponent<_CellObject>();
                } 
                else if (x == 3 && y == 0)
                {
                    GameObject dotOut = Instantiate(dotOutPrefab);
                    dotOut.transform.parent = transform;
                    dotOut.transform.position = Grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    dotOut.GetComponent<Dot>().SetColor(Color.blue);
                } 
                else if (x == 2 && y == 4)
                {
                    GameObject bridge = Instantiate(bridgePrefab);
                    bridge.transform.parent = transform;
                    bridge.transform.position = Grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    bridge.GetComponent<_CellObject>().cell = gridData[x, y];
                    gridData[x, y].containedObject = bridge.GetComponent<_CellObject>();
                }
                else if (x == 5 && y == 4)
                {
                    GameObject bridge = Instantiate(bridgePrefab);
                    bridge.transform.parent = transform;
                    bridge.transform.position = Grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    bridge.GetComponent<_CellObject>().cell = gridData[x, y];
                    bridge.GetComponent<BridgeObject>().direction = BridgeObject.Direction.Vertical;
                    gridData[x, y].containedObject = bridge.GetComponent<_CellObject>();
                }
            }
        }

        CenterGrid();
        FitGridInCamera();
        CreateAndResizeBackground();
    }

    private void CenterGrid()
    {
        float gridWidth =
            (columns * Grid.cellSize.x) + //Calcula a largura total da c�lula
            ((columns - 1) * Grid.cellGap.x); //Calcula a largura total dos espa�os entre as c�lulas

        float gridHeight = 
            (rows * Grid.cellSize.y) +
            ((rows - 1) * Grid.cellGap.y);

        Vector3 gridCenterOffset = new Vector3(
            gridWidth / 2f, //Calcula o deslocamento horizontal para centralizar a grade. 
            gridHeight / 2f,
            0f

            //(2f - grid.cellSize.x) serve para poder centralizar de verdade o "pivot" da c�lula)
        );

        transform.position = -gridCenterOffset; //Move o grid toda para tr�s considerando o tamanho das c�lulas
    }

    public void FitGridInCamera()
    {
        float gridWidth =
            (columns * Grid.cellSize.x) + //Calcula a largura total da c�lula
            ((columns - 1) * Grid.cellGap.x); //Calcula a largura total dos espa�os entre as c�lulas

        float gridHeight =
            (rows * Grid.cellSize.y) +
            ((rows - 1) * Grid.cellGap.y);

        float screenRatio =
            (float)UnityEngine.Device.Screen.width / UnityEngine.Device.Screen.height; //Pega o centro da tela

        float targetHeight =
            gridHeight / 2f; //Diz onde � pra centralizar verticalmente

        float targetWidth =
            (gridWidth / screenRatio) / 2f; //Diz onde � pra centralizar horizontalmente

        Camera.main.orthographicSize =
            Mathf.Max(targetHeight, targetWidth) + margin; //Ajusta o tamanho ortogr�fico da c�mera para garantir que o grid caiba na tela,
                                                           //adicionando uma margem para n�o ficar t�o apertado

        //Mathf.Max � usado para garantir que a c�mera seja grande o suficiente para mostrar toda a grade,
        //independentemente da propor��o da tela.
        //Ele escolhe o maior valor entre targetHeight e targetWidth para garantir que ambos os eixos sejam adequadamente ajustados.
    }

    public CellData GetCellData(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.x >= columns ||
            gridPosition.y < 0 || gridPosition.y >= rows)
        {
            return null; // Retorna null se a posi��o estiver fora dos limites do grid
        }

        return gridData[gridPosition.x, gridPosition.y];
    }

    public CellData[] GetAdjacentCellData(Vector2Int gridPosition)
    {
        CellData[] adjacentCells = new CellData[4];
        adjacentCells[0] = GetCellData(new Vector2Int(gridPosition.x, gridPosition.y + 1)); // Cima
        adjacentCells[1] = GetCellData(new Vector2Int(gridPosition.x, gridPosition.y - 1)); // Baixo
        adjacentCells[2] = GetCellData(new Vector2Int(gridPosition.x - 1, gridPosition.y)); // Esquerda
        adjacentCells[3] = GetCellData(new Vector2Int(gridPosition.x + 1, gridPosition.y)); // Direita

        return adjacentCells;
    }

    private void CreateAndResizeBackground()
    {
        if (gridBackground == null) return;

        GameObject bgInstance = Instantiate(gridBackground, transform);
        bgInstance.name = "Grid_Background";

        SpriteRenderer sr = bgInstance.GetComponent<SpriteRenderer>();

        // tamanho REAL ocupado pelas c�lulas
        float gridWidth =
            ((columns - 1) * (Grid.cellSize.x + Grid.cellGap.x))
            + Grid.cellSize.x;

        float gridHeight =
            ((rows - 1) * (Grid.cellSize.y + Grid.cellGap.y))
            + Grid.cellSize.y;

        // pega a primeira e �ltima c�lula
        Vector3 bottomLeft =
            Grid.GetCellCenterWorld(new Vector3Int(0, 0, 0));

        Vector3 topRight =
            Grid.GetCellCenterWorld(new Vector3Int(columns - 1, rows - 1, 0));

        // centro exato entre elas
        Vector3 center = (bottomLeft + topRight) / 2f;

        bgInstance.transform.position = new Vector3(
            center.x,
            center.y,
            1f
        );

        // escala sprite
        Vector2 spriteSize = sr.sprite.bounds.size;

        bgInstance.transform.localScale = new Vector3(
            gridWidth / spriteSize.x,
            gridHeight / spriteSize.y,
            1f
        );
    }
}
