using UnityEngine;

public struct NetworkPosition
{
    public int Id;
    public float positionX;
    public float positionY;
    public float positionZ;

    public NetworkPosition(int id, Vector3 position)
    {
        Id = id;
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;
    }
}