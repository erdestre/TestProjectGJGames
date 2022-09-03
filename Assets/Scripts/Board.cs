using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    public int howManyColumns, howManyItems;
    public Item[] whichItems;
    public GameObject QuadObjectPrefab;

    GameObject[] Columns;
    GameObject[][] items;
    
    void Start()
    {
        Columns = new GameObject[howManyColumns];

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

            Columns[currentColumn] = boardColumn;
            SpawnNewQuads(howManyItems, currentColumn);
        }
    }

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
    private void SpawnNewQuads(int quadNumber, int columnNumber)
    {
        for (int i = 0; i < quadNumber; i++){
            int randomQuad = Random.Range(0, whichItems.Length);
            
            GameObject newQuad = Instantiate(QuadObjectPrefab);
            newQuad.name = "Quad";
            newQuad.transform.SetParent(Columns[columnNumber].transform);
            newQuad.transform.SetAsFirstSibling();

            QuadObjects newQuadScrpt = newQuad.AddComponent<QuadObjects>();
            newQuadScrpt.SetQuad(whichItems[randomQuad]);
            newQuadScrpt.board = gameObject.GetComponent<Board>();
        }
    }
}
