using UnityEngine;
using System.Collections;

public class Tile
{
    public int xPos, zPos;
    public float score;
    public Tile parent = null;
    //public bool occupied = false;
    
    public Vector2 pos
    {
        get { return new Vector2(xPos, zPos); }
    }

    public static Tile CreateTile(int p_X, int p_Z, float p_Score)
    {
        Tile t = new Tile();
        t.xPos = p_X;
        t.zPos = p_Z;
        t.score = p_Score;
        
        return t;
    }
    public static Tile CreateTile(Vector2 p_Pos, float p_Score)
    {
        Tile t = new Tile();
        t.xPos = (int)p_Pos.x;
        t.zPos = (int)p_Pos.y;
        t.score = p_Score;
        return t;
    }
    public void SetParent(Tile p_Parent)
    {
        parent = p_Parent;
    }
}

