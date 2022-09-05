using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObjects : MonoBehaviour
{
    public Item cubeProperties;
    [HideInInspector]
    public Board board;
    public Vector2 Location;
    
    public bool isChecked = false;


    int maxGroup;
    int midGroup;
    int minGroup;
    public int neighborCount = 0;

    public void SetCube(Item cubeProperties, int minGroup, int midGroup, int maxGroup)
    {
        this.cubeProperties = cubeProperties;
        this.minGroup = minGroup;
        this.midGroup = midGroup;
        this.maxGroup = maxGroup;
        

        ArrangeTexture();
    }

    public void ArrangeTexture()
    {
        if(neighborCount >= maxGroup)         GetComponent<MeshRenderer>().material.mainTexture = cubeProperties.textures[3];
        else if(neighborCount >= midGroup)    GetComponent<MeshRenderer>().material.mainTexture = cubeProperties.textures[2];
        else if(neighborCount >= minGroup)    GetComponent<MeshRenderer>().material.mainTexture = cubeProperties.textures[1]; 
        else                                     GetComponent<MeshRenderer>().material.mainTexture = cubeProperties.textures[0];
    }
    public void DestroyCube()
    {
        board.DestroyCube((int)Location.x, (int)Location.y, gameObject);
    }
    public void MoveCube()
    {
        board.CubeLocation[(int)Location.x, (int)Location.y - 1] = gameObject;
        Location = new Vector2(Location.x, Location.y - 1);
    }

    private void OnMouseDown()
    {
        if(board.canTouch)
        {
            board.DestroyGroup(gameObject.transform.parent);
        }
        
    }
}
