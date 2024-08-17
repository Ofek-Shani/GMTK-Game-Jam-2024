using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometObject : MonoBehaviour
{
    public int[] resources { get; private set; } = new int[4];
    public void Collect(ResourceTypeEnum.ResourceType toCollect)
    {
        resources[(int)toCollect]++;
    }
}
