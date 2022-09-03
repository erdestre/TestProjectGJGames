using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadObjects : MonoBehaviour
{
    public Item quadProperties;
    [HideInInspector]
    public Board board;

    public void SetQuad(Item quadProperties)
    {
        this.quadProperties = quadProperties;

        ArrangeTexture();
    }

    private void ArrangeTexture()
    {
        GetComponent<MeshRenderer>().material.mainTexture = quadProperties.textures[0];
    }
    public void DestroyQuad()
    {
        
    }
    public void MoveQuad()
    {

    }
    private void CheckNeighbor()
    {

    }
}
