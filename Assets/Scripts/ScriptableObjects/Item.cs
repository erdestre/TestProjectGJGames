using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "QuadItem")]

public class Item : ScriptableObject
{
    [TextArea] public string itemName;
    
    public Texture2D[] textures;

    [Range(0, 10)] public int minimumGroupNumber, middleGroupNumber, maximumGroupNumber;
}
