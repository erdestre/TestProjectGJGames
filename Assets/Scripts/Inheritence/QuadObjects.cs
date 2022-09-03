using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadObjects : MonoBehaviour
{
    public Item quadProperties;
    [HideInInspector]
    public Board board;

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
        else                                    GetComponent<MeshRenderer>().material.mainTexture = quadProperties.textures[0];
    }
    public void DestroyQuad()
    {
        Debug.Log("Object Destroyed");
    }
    public void MoveQuad()
    {

    }
    private void CheckNeighbor()
    {

    }
    private void OnMouseDown()
    {
        DestroyQuad();
    }
}
