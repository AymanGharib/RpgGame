using UnityEngine;
using System.Collections;

public class Node {
	
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
    public float gCost;
    public float hCost;
  
	public Node parent;
	
	public Node( Vector3 _worldPos) {
	
		worldPosition = _worldPos;
	
	}

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
