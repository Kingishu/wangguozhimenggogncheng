using UnityEngine;

[System.Serializable]
public class SerializeVector3
{
    public float x,y,z;

    public SerializeVector3(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 GetVector3()
    {
        return new Vector3(x,y,z);
    }

    public Vector2Int GetVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }

}
