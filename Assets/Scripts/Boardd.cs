using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boardd : MonoBehaviour
{
    public int[] cubeCountInColumns; // 7 column, 11 cube will be default

    public GameObject[,] Cubes;
    public Item[] whichItems;

    public GameObject CubePrefab;
    int extraSpacing = 0;
    Transform CubeSpawnPoint;
    LinkedList<GameObject>[] Columns;
    private void Start()
    {
        CubeSpawnPoint = transform.Find("Spawner").transform;
        Cubes = new GameObject[cubeCountInColumns.Length, cubeCountInColumns.Max()];


        for (int currentColumn = 0; currentColumn < cubeCountInColumns.Length; currentColumn++) //Column
        {
            for (int currentCubeCount = 0; currentCubeCount < cubeCountInColumns[currentColumn]; currentCubeCount++) //itemCount in this column
            {
                SpawnCubes(currentCubeCount,currentColumn);
            }
        }
        extraSpacing = cubeCountInColumns.Max();
    }
    void SpawnCubes(int currentCube, int currentColumn)
    {
        int randomCube = Random.Range(0, whichItems.Length);

        CubeSpawnPoint.localPosition = new Vector2(currentColumn, currentCube + extraSpacing);
        
        GameObject newCube = Instantiate(CubePrefab);
        CubeObjects newCubeScrpt = newCube.AddComponent<CubeObjects>();
        newCubeScrpt.SetQuad(whichItems[randomCube]);
        newCubeScrpt.board = gameObject.GetComponent<Boardd>();
        newCube.transform.position = CubeSpawnPoint.transform.position;

        newCubeScrpt.ColumnLocationInArray = currentColumn;
        newCubeScrpt.CubeLocationInColumn = currentCube;

        Cubes[currentColumn, currentCube] = newCube;
    }
    Stack<CubeObjects> CubeStack;
    int neighborCount;
    void StartChecking()
    {
        for (int currentColumn = 0; currentColumn < cubeCountInColumns.Length; currentColumn++) //Column
        {
            for (int currentCube = 0; currentCube < cubeCountInColumns[currentColumn]; currentCube++) //itemCount in this column
            {
                CheckMainCubeNeighbors(currentColumn,currentCube);
            }
        }
    }
    void CheckMainCubeNeighbors(int columnCount, int cubeCount)
    {
        CubeObjects mainQuadObject = Cubes[columnCount, cubeCount].GetComponent<CubeObjects>();

        if (!mainQuadObject.isChecked)
        {
            mainQuadObject.isChecked = true;
            CubeStack.Push(mainQuadObject);

            if (columnCount - 1 >= 0)               CheckNeighborNeighbors(columnCount - 1, cubeCount, mainQuadObject);     //Check Left
            if (cubeCount - 1 >= 0)                 CheckNeighborNeighbors(columnCount, cubeCount - 1, mainQuadObject);     //Check Bottom
            if (cubeCount + 1 < cubeCount)          CheckNeighborNeighbors(columnCount, cubeCount + 1, mainQuadObject);     //Check Upper
            if (columnCount + 1 < columnCount)      CheckNeighborNeighbors(columnCount + 1, cubeCount, mainQuadObject);     //Check Right
        }
    }
    void CheckNeighborNeighbors(int columnNumber, int rowNumber, CubeObjects mainQuadObject)
    {
        CubeObjects neighborQuadObject = Cubes[columnNumber, rowNumber].GetComponent<CubeObjects>();

        if (mainQuadObject.quadProperties == neighborQuadObject.quadProperties && !neighborQuadObject.isChecked)
        {
            CheckMainCubeNeighbors(columnNumber, rowNumber);
        }
    }
}
