using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

public class World : MonoBehaviour
{

    public GameObject cellPrefab;
    public Camera mainCamera;
    public int SceneX=10, SceneY=10;
    private GameObject[,] cellsObjectArray;
    private Cell[,] cellScriptArray;

    // Start is called before the first frame update
    void Start()
    {
        Transform cameraTransform = mainCamera.GetComponent<Transform>();
        cameraTransform.position = new Vector3(SceneX / 2.0f, SceneY / 2.0f, mainCamera.GetComponent<Transform>().position.z);

        CreateAllCells();
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

    // Update is called once per frame
    void Update()
    {
        //float time = Time.time;
        //Parallel.ForEach<CellScript>(cellScriptArray, (cellScript) => cellScript.PararelUpdate(time));
        foreach (Cell cellScript in cellScriptArray)  cellScript.mUpdate();
    }
}
