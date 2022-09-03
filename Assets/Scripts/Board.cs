using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public int howManyColumns, howManyItems;
    public Item[] whichItems;
    public GameObject QuadObjectPrefab;

    GameObject[,] Quads;

    

    void Start()
    {
        Quads = new GameObject[howManyColumns, howManyItems];

        for (int currentColumn = 0; currentColumn < howManyColumns; currentColumn++)
        {
            GameObject boardColumn = new GameObject();
            //Instantiate(boardColumn, new Vector3(0, 0, 0), Quaternion.identity);
            boardColumn.name = "Columns";
            boardColumn.transform.parent = gameObject.transform;
            boardColumn.AddComponent<LayoutElement>();            
            boardColumn.AddComponent<VerticalLayoutGroup>();
            
            boardColumn.GetComponent<VerticalLayoutGroup>().spacing = 1;
            boardColumn.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);

            SpawnNewQuads(howManyItems, currentColumn);
        }
        for (int currentColumn = 0; currentColumn < howManyColumns; currentColumn++)
        {
            for (int currentItem = 0; currentItem < howManyItems; currentItem++)
            {
                CheckQuad(currentColumn, currentItem);
                totalNeighbor = 1;
            }
        }
    }
    /*
    public void QuadSpawner()
    {
        CheckLocations(); // --> SpawnNewQuads();
    }

    private void CheckLocations()
    {
        for(int i = 0; Columns[i].transform.childCount < howManyItems-1; i++)
        {
            SpawnNewQuads(howManyItems - Columns[i].transform.childCount - 1, i);
        }
    }
    */
    private void SpawnNewQuads(int quadNumber, int columnNumber)
    {
        for (int i = 0; i < quadNumber; i++){
            int randomQuad = Random.Range(0, whichItems.Length);
            
            GameObject newQuad = Instantiate(QuadObjectPrefab);
            newQuad.name = "Quad";
            newQuad.transform.SetParent(transform.GetChild(columnNumber));

            QuadObjects newQuadScrpt = newQuad.AddComponent<QuadObjects>();
            newQuadScrpt.SetQuad(whichItems[randomQuad]);
            newQuadScrpt.board = gameObject.GetComponent<Board>();

            Quads[columnNumber,i] = newQuad;

        }
    }

    int totalNeighbor = 1;
    private void CheckQuad(int columnNumber, int rowNumber)
    {
        QuadObjects mainQuadObject = Quads[columnNumber, rowNumber].GetComponent<QuadObjects>();
        
        if (!mainQuadObject.isChecked)
        {
            mainQuadObject.isChecked = true;
            if (columnNumber - 1 >= 0)                  CheckNeighbor(columnNumber - 1, rowNumber, mainQuadObject);     //Check Left
            if (rowNumber - 1 >= 0)                     CheckNeighbor(columnNumber, rowNumber - 1, mainQuadObject);     //Check Bottom
            if (rowNumber + 1 < howManyItems)           CheckNeighbor(columnNumber, rowNumber + 1, mainQuadObject);     //Check Upper
            if (columnNumber + 1 < howManyColumns)      CheckNeighbor(columnNumber + 1, rowNumber, mainQuadObject);     //Check Right
            mainQuadObject.neighborCount = totalNeighbor;
            mainQuadObject.ArrangeTexture();
            Debug.Log("Object " + columnNumber + " " + rowNumber + " : " + "Neighbor Number: " + mainQuadObject.neighborCount);
        }
    }

    private void CheckNeighbor(int columnNumber, int rowNumber, QuadObjects mainQuadObject)
    {
        QuadObjects neighborQuadObject = Quads[columnNumber, rowNumber].GetComponent<QuadObjects>();
        
        if (mainQuadObject.quadProperties == neighborQuadObject.quadProperties && !neighborQuadObject.isChecked)
        {
            totalNeighbor++;
            CheckQuad(columnNumber, rowNumber);
        }
    }
}
