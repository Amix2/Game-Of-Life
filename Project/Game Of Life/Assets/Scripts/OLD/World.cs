using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    private static readonly float RANDOM_ALIVE_CHANCE = 0.05f;

    public string gameRules = "23/3";
    public GameObject cellPrefab;
    public Camera mainCamera;
    public int SceneX=10, SceneY=10;
    public Slider slider;


    private GameObject[,] cellsObjectArray;
    private Cell[,] cellScriptArray;
    private Vector2Int lastClickPos;

    public ComputeShader shader;
    ComputeBuffer buf;

    static public GameOfLifeRules gameOfLife;
    // Start is called before the first frame update
    void Start()
    {
        Transform cameraTransform = mainCamera.GetComponent<Transform>();
        cameraTransform.position = new Vector3(SceneX / 2.0f, SceneY / 2.0f, mainCamera.GetComponent<Transform>().position.z);

        CreateAllCells();

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.offset = new Vector2(SceneX / 2.0f, SceneY / 2.0f);
        boxCollider.size = new Vector2(SceneX, SceneY);

        gameOfLife = new GameOfLifeRules(gameRules);

       // Invoke("PararelUpdate", this.slider.value);
        buf = new ComputeBuffer(64, sizeof(float), ComputeBufferType.Default);
        Shader.SetGlobalBuffer(Shader.PropertyToID("Result"), buf);
        Shader.SetGlobalInt("WORLD_SIZE_X", 200);
        shader.Dispatch(shader.FindKernel("CSMain"), 1, 1, 1);
        float[] data = new float[64];
        buf.GetData(data);
        print(data[0]);
    }

    private void CreateAllCells()
    {
        cellsObjectArray = new GameObject[SceneX, SceneY];
        cellScriptArray = new Cell[SceneX, SceneY];

        for (int x = 0; x < SceneX; x++)
        {
            for (int y = 0; y < SceneY; y++)
            {
                CreateCell(x, y);
            }
        }

        for (int x = 0; x < SceneX; x++)
        {
            for (int y = 0; y < SceneY; y++)
            {
                AssignNeighbours(x, y);
            }
        }
    }

    private void CreateCell(int x, int y)
    {
        cellsObjectArray[x, y] = Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity);
        cellScriptArray[x, y] = cellsObjectArray[x, y].GetComponent<Cell>();
        bool isOnEdge = (x == 0 || x == SceneX - 1 || y == 0 || y == SceneY - 1);
        cellScriptArray[x, y].isOnEdge = isOnEdge;
        cellsObjectArray[x, y].transform.parent = this.transform;
        cellScriptArray[x, y].m_state = isOnEdge ? State.DEAD : UnityEngine.Random.value < RANDOM_ALIVE_CHANCE ? State.ALIVE : State.DEAD;

    }

    private void AssignNeighbours(int x, int y)
    {
        bool isOnEdge = (x == 0 || x == SceneX - 1 || y == 0 || y == SceneY - 1);
        if (!isOnEdge)
        {
            cellScriptArray[x, y].neighbourList = new Cell[] {
                cellScriptArray[x-1, y+1],  cellScriptArray[x, y+1],    cellScriptArray[x+1, y+1],
                cellScriptArray[x-1, y],                                cellScriptArray[x+1, y],
                cellScriptArray[x-1, y-1],  cellScriptArray[x, y-1],    cellScriptArray[x+1, y-1]
            };
        }
    }

    private void Update()
    {
        PararelUpdate();
    }

    private void OnMouseDrag()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int clickPos = new Vector2Int((int)pz.x, (int)pz.y);
        if (clickPos.x < 0 || clickPos.x >= SceneX || clickPos.y < 0 || clickPos.y >= SceneY) return;
        if (lastClickPos != clickPos)
        {
            lastClickPos = clickPos;
            cellScriptArray[clickPos.x, clickPos.y].SetState(State.ALIVE);
        }
    }

    void PararelUpdate()
    {
        //Invoke("PararelUpdate", this.slider.value);
        Parallel.For(0, SceneY, y =>
        //for(int y=0; y<SceneY; y++)
        {
            for (int x = SceneX - 1; x >= 0; x--)
            {
                cellScriptArray[x, y].PararelUpdatePrep();
            }
        }
        );
        //Thread.Sleep(100);

        foreach (Cell cellScript in cellScriptArray) cellScript.SequentialUpdateFinal();
    }

    private void OnDestroy()
    {
        buf.Dispose();
    }

}
