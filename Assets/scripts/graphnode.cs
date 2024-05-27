using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphNode : IHeapItem<GraphNode>  
{
    public int gridX;
    public int gridY;
    public float gCost;
    public float hCost;

    public GraphNode parent;
    public Vector3 Position { get; set; }
    public List<GraphNode> ConnectedNodes { get; private set; }
    public List<GraphNode> ConnectedNodesAfterPrim  = new List<GraphNode>();
    public GameObject AssociatedObject { get; set; } // Reference to the associated GameObject

    public GraphNode(Vector3 pos)
    {
        Position = pos;
        ConnectedNodes = new List<GraphNode>();
    }

    public void ConnectTo(GraphNode otherNode)
    {
        if (!ConnectedNodes.Contains(otherNode))
        {
            ConnectedNodes.Add(otherNode);
            otherNode.ConnectedNodes.Add(this);
        }
    }
    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    private int heapIndex;
   
    
      
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }



    public string Name; 

public int CompareTo(GraphNode node)
    {
        int compare = Math.Round(fCost).CompareTo(Math.Round(node.fCost));
        if (compare == 0)
        {
            compare = hCost.CompareTo(node.hCost);
        }
        return -compare;
    }






}
