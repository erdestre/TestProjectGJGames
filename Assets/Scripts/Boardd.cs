using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boardd : MonoBehaviour
{
    public int rowLength;
    public int cubeCountInARow;

    public GameObject[,] CubeLocation;
    public Item[] whichItems;

    public GameObject CubePrefab;
    int extraSpacing = 0;
    Transform CubeSpawnPoint;
    private void Start()
    {
        CubeSpawnPoint = transform.Find("Spawner").transform;
        CubeLocation = new GameObject[rowLength, cubeCountInARow];
        CubeChainStack = new Stack<CubeObjects>();

        for (int currentRowLength = 0; currentRowLength < rowLength; currentRowLength++) //Column
        {
            for (int currentCubeCountInARow = 0; currentCubeCountInARow < cubeCountInARow; currentCubeCountInARow++) //itemCount in this column
            {
                SpawnCubes(currentRowLength, currentCubeCountInARow);
            }
        }
        extraSpacing = cubeCountInARow/2;
        StartCheckCubeChain();
    }
    public void SpawnCubes(int currentRow, int currentCube)
    {
        int randomCube = Random.Range(0, whichItems.Length); 

        CubeSpawnPoint.localPosition = new Vector2(currentRow, currentCube + extraSpacing);
        
        GameObject newCube = Instantiate(CubePrefab);
        CubeObjects newCubeScrpt = newCube.AddComponent<CubeObjects>();
        newCubeScrpt.SetQuad(whichItems[randomCube]);
        newCubeScrpt.board = gameObject.GetComponent<Boardd>();
        newCube.transform.position = CubeSpawnPoint.transform.position;
        newCube.name = currentRow + "  " + currentCube;
        newCubeScrpt.Location = new Vector2(currentRow, currentCube);

        CubeLocation[currentRow, currentCube] = newCube;
    }
    
    Stack<CubeObjects> CubeChainStack;
    void StartCheckCubeChain() //from left to right, bottom to top
    {
        for (int currentRowLength = 0; currentRowLength < rowLength; currentRowLength++) //Column
        {
            for (int currentCubeCountInARow = 0; currentCubeCountInARow < cubeCountInARow; currentCubeCountInARow++) //itemCount in this column
            {
                CheckMainCubeNeighbors(currentRowLength, currentCubeCountInARow);
                int ChainCount = CubeChainStack.Count();
                for (int i = 0; i < ChainCount; i++)
                {
                    CubeChainStack.Peek().neighborCount = ChainCount;
                    CubeChainStack.Peek().ArrangeTexture();
                    CubeChainStack.Pop();
                }
            }
        }
        
    }
    void CheckMainCubeNeighbors(int whichRow, int whichCube) //from left to right, bottom to top
    {
        CubeObjects mainCubeObject = CubeLocation[whichRow, whichCube].GetComponent<CubeObjects>();
        

        if (!mainCubeObject.isChecked)
        {
            CubeChainStack.Push(mainCubeObject);
            mainCubeObject.isChecked = true;

            if (whichRow - 1 >= 0)                  CheckNeighborNeighbors(whichRow - 1, whichCube, mainCubeObject);     //Check Left
            if (whichCube - 1 >= 0)                 CheckNeighborNeighbors(whichRow, whichCube - 1, mainCubeObject);     //Check Bottom
            if (whichCube + 1 < cubeCountInARow)          CheckNeighborNeighbors(whichRow, whichCube + 1, mainCubeObject);     //Check Upper
            if (whichRow + 1 < rowLength)            CheckNeighborNeighbors(whichRow + 1, whichCube, mainCubeObject);     //Check Right
        }
    }
    void CheckNeighborNeighbors(int columnNumber, int rowNumber, CubeObjects mainCubeObject)
    {
        CubeObjects neighborQuadObject = CubeLocation[columnNumber, rowNumber].GetComponent<CubeObjects>();

        if (mainCubeObject.quadProperties == neighborQuadObject.quadProperties)
        {
            CheckMainCubeNeighbors(columnNumber, rowNumber);
        }
    }

    public void DestroyCube(int locationX, int locationY, GameObject cube)
    {
        for (int i = locationY+1; i < cubeCountInARow; i++)
        {
            CubeLocation[locationX, i].GetComponent<CubeObjects>().MoveQuad();
        }
        SpawnCubes(locationX, cubeCountInARow - 1);
        Destroy(cube);
        ResetNeighborhood();
        StartCheckCubeChain();
    }

    private void ResetNeighborhood()
    {
        for (int currentRowLength = 0; currentRowLength < rowLength; currentRowLength++) //Column
        {
            for (int currentCubeCountInARow = 0; currentCubeCountInARow < cubeCountInARow; currentCubeCountInARow++) //itemCount in this column
            {
                CubeLocation[currentRowLength, currentCubeCountInARow].GetComponent<CubeObjects>().isChecked = false;
            }
        }
    }
}
