using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 class Pathfinding : MonoBehaviour {
    /*
	 public TilemapVisualizer grid;
  public  NodeGenerator nodegenerator;
    public GameObject a;
    public GameObject b;
   
  
    private Vector3 s = new Vector3()  ;
    private Vector3 e = new Vector3();

   private void Update()
    {
        StartCoroutine(MyCoroutine()); 
    }

    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(2f);
     
        FindPath(a.transform.position, b.transform.position);
    }


        public void FindPath (Vector3 startPos , Vector3 endPos)
    {
        Node start = grid.GetNode(startPos);
        Node end = grid.GetNode(endPos);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            Node currNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {

                if (openSet[i].fCost > currNode.fCost || openSet[i].fCost == currNode.fCost && openSet[i].hCost < currNode.hCost)



                {
                    currNode = openSet[i];



                }

            }
            openSet.Remove(currNode);
            closed.Add(currNode);
            if (currNode == end) {


                RetracePath(start, end); 
                
                
                
                
                
                return; }
        //    foreach(Node neighbor in grid.GetNeighbours(currNode))
            {

                if (closed.Contains(neighbor))
                    continue;



                float newMovement = currNode.gCost - GettDistance(currNode, neighbor); 
                 if (newMovement   < neighbor.gCost || openSet.Contains(neighbor) ){

                    neighbor.gCost = newMovement;
                    neighbor.hCost = GettDistance(neighbor, end);
                    neighbor.parent = currNode; 
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor); 




                    }

                }










            }

        }









    }

    void RetracePath(Node a, Node b)
    {

        List<Node> path = new List<Node>();
        Node curr = b; 

        while(curr!= a)
        {
            path.Add(curr);
            curr = curr.parent; 



        }
        path.Reverse();













    }


    float GettDistance(Node a  ,  Node b)
    {

        float distX = Mathf.Abs(a.worldPosition.x - b.worldPosition.x);
        float distY = Mathf.Abs(a.worldPosition.y - b.worldPosition.y);


        if (distX> distY)
        {

            return (14*distY + 10*(distX - distY));



        }
        else
        {
            return (14 * distX + 10 * (distY - distX));
        }









    }














    */








}
