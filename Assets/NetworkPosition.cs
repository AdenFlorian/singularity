using System;
using UnityEngine;

public struct NetworkPosition
{
    public Guid Id;
    public float positionX;
    public float positionY;
    public float positionZ;

    public NetworkPosition(Guid id, Vector3 position)
    {
        Id = id;
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;
    }
}