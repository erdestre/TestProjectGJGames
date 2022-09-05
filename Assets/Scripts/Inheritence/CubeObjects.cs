using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObjects : MonoBehaviour
{
    public Item quadProperties;
    [HideInInspector]
    public Board board;
    public Vector2 Location;
    
    public bool isChecked = false;


    int maxNeighbor = 6;
    int midNeighbor = 4;
    int lowNeighbor = 2;
    public int neighborCount = 0;

    public void SetQuad(Item quadProperties)
    {
        this.quadProperties = quadProperties;

        ArrangeTexture();
    }

    public void ArrangeTexture()
    {
        if(neighborCount >= maxNeighbor)         GetComponent<MeshRenderer>().material.mainTexture = quadProperties.textures[3];
        else if(neighborCount >= midNeighbor)    GetComponent<MeshRenderer>().material.mainTexture = quadProperties.textures[2];
        else if(neighborCount >= lowNeighbor)    GetComponent<MeshRenderer>().material.mainTexture = quadProperties.textures[1]; 
        else                                     GetComponent<MeshRenderer>().material.mainTexture = quadProperties.textures[0];
    }
    public void DestroyCube()
    {
        board.DestroyCube((int)Location.x, (int)Location.y, gameObject);
    }
    public void MoveQuad()
    {
        board.CubeLocation[(int)Location.x, (int)Location.y - 1] = gameObject;
        Location = new Vector2(Location.x, Location.y - 1);
    }
    private void CheckNeighbor()
    {

    }
    private void OnMouseDown()
    {
        if(board.canTouch)
        {
            board.DestroyGroup(gameObject.transform.parent);
        }
        
    }
}
