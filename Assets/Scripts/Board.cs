using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int rowLength;
    public int cubeCountInARow;

    public GameObject[,] CubeLocation;
    public Item[] whichItems;

    public GameObject CubePrefab;
    
    int extraSpacing;
    Transform CubeSpawnPoint;
    
    [HideInInspector]
    public bool canTouch = true;
    [HideInInspector]
    public int playableMoveCount = 0;

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
            extraSpacing = 0;
        }
        extraSpacing = cubeCountInARow;
        StartCheckCubeChain();
    }
    public void SpawnCubes(int currentRow, int currentCube)
    {
        int randomCube = Random.Range(0, whichItems.Length); 

        CubeSpawnPoint.localPosition = new Vector2(currentRow, extraSpacing);
        extraSpacing++;
        GameObject newCube = Instantiate(CubePrefab);
        CubeObjects newCubeScrpt = newCube.AddComponent<CubeObjects>();
        newCubeScrpt.SetQuad(whichItems[randomCube]);
        newCubeScrpt.board = gameObject.GetComponent<Board>();
        newCube.transform.position = CubeSpawnPoint.transform.position;
        newCube.name = currentRow + "  " + currentCube;
        newCubeScrpt.Location = new Vector2(currentRow, currentCube);

        CubeLocation[currentRow, currentCube] = newCube;
    }
    public IEnumerator CanTouchStatus(float objectCount)
    {
        canTouch = false;
        yield return new WaitForSeconds(0.5f*(objectCount/2));
        canTouch = true;
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
                if (ChainCount > 0)
                {
                    
                    GameObject Group = new GameObject();
                    if (ChainCount > 1) playableMoveCount++;
                    for (int i = 0; i < ChainCount; i++)
                    {
                        CubeChainStack.Peek().neighborCount = ChainCount;
                        CubeChainStack.Peek().ArrangeTexture();

                        AttachParent(CubeChainStack.Peek(), Group);
                        CubeChainStack.Pop();
                    }
                    
                    Group.name = "Group of " + Group.transform.childCount + " Members";
                }
            }
        }
        if (playableMoveCount < 1) Shuffle();
    }
    private void AttachParent(CubeObjects Cube, GameObject Group)
    {
        if(Cube.transform.parent) DetachParent(Cube);
        Cube.transform.SetParent(Group.transform); 
    }

    private void DetachParent(CubeObjects Cube)
    {
        Transform oldGroup = Cube.transform.parent;
        Destroy(oldGroup.gameObject);
    }

    void CheckMainCubeNeighbors(int whichRow, int whichCube) //from left to right, bottom to top
    {
        CubeObjects mainCubeObject = CubeLocation[whichRow, whichCube].GetComponent<CubeObjects>();
        

        if (!mainCubeObject.isChecked)
        {
            CubeChainStack.Push(mainCubeObject);
            mainCubeObject.isChecked = true;

            if (whichRow - 1 >= 0)                        CheckNeighborNeighbors(whichRow - 1, whichCube, mainCubeObject);     //Check Left
            if (whichCube - 1 >= 0)                       CheckNeighborNeighbors(whichRow, whichCube - 1, mainCubeObject);     //Check Bottom
            if (whichCube + 1 < cubeCountInARow)          CheckNeighborNeighbors(whichRow, whichCube + 1, mainCubeObject);     //Check Upper
            if (whichRow + 1 < rowLength)                 CheckNeighborNeighbors(whichRow + 1, whichCube, mainCubeObject);     //Check Right
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
    public void DestroyGroup(Transform group)
    {
        int childCount = group.transform.childCount;
        StartCoroutine(CanTouchStatus(childCount));
        if (childCount > 1)
        {
            for (int i = 0; i < childCount; i++)
            {
                extraSpacing++;
                group.GetChild(i).GetComponent<CubeObjects>().DestroyCube();

            }
            Destroy(group.gameObject);
            extraSpacing = cubeCountInARow;
            ResetNeighborhood();
            StartCheckCubeChain();
        }
    }
    public void DestroyCube(int locationX, int locationY, GameObject cube)
    {
        for (int i = locationY+1; i < cubeCountInARow; i++)
        {
            CubeLocation[locationX, i].GetComponent<CubeObjects>().MoveQuad();
        }
        SpawnCubes(locationX, cubeCountInARow - 1);
    }

    private void ResetNeighborhood()
    {
        for (int currentRowLength = 0; currentRowLength < rowLength; currentRowLength++) //Column
        {
            for (int currentCubeCountInARow = 0; currentCubeCountInARow < cubeCountInARow; currentCubeCountInARow++) //itemCount in this column
            {
                CubeLocation[currentRowLength, currentCubeCountInARow].GetComponent<CubeObjects>().isChecked = false;
                playableMoveCount = 0;
            }
        }
    }
    private void Shuffle()
    {
        for (int i = 0; i < rowLength; i++)
        {
            for (int a = 0; a < cubeCountInARow; a++)
            {
                GameObject tempObject = CubeLocation[i, a];
                int randomXIndex = Random.Range(i, rowLength);
                int randomYIndex = Random.Range(a, cubeCountInARow);
                
                CubeLocation[i, a] = CubeLocation[randomXIndex, randomYIndex];
                CubeLocation[i, a].GetComponent<CubeObjects>().Location = new Vector2(i, a);
                CubeLocation[i, a].name = i + "  " + a;
                CubeSpawnPoint.localPosition = new Vector2(i, a);
                CubeLocation[i, a].transform.position = CubeSpawnPoint.position;
                
                CubeLocation[randomXIndex, randomYIndex] = tempObject;
                CubeLocation[randomXIndex, randomYIndex].GetComponent<CubeObjects>().Location = new Vector2(randomXIndex, randomYIndex);
                CubeLocation[randomXIndex, randomYIndex].name = randomXIndex + "  " + randomYIndex;
                CubeSpawnPoint.localPosition = new Vector2(randomXIndex, randomYIndex);
                CubeLocation[randomXIndex, randomYIndex].transform.position = CubeSpawnPoint.position;

            }
        }
        ResetNeighborhood();
        StartCheckCubeChain();
        if (playableMoveCount < 1) Shuffle();
    }
}
