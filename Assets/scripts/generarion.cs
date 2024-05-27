using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


//using TriangleNet.Geometry;
//using TriangleNet.Meshing;
//using TriangleNet.Topology;


public class NodeGenerator : MonoBehaviour
{

    public List<Vector3> PATH = new List<Vector3>();
    public List<GraphNode> DjikstraPath = new List<GraphNode>();

    Vector3 seeker = new Vector3();
    Vector3 target = new Vector3();
    public Vector3 x;
    public int gridSpacing = 50;
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public int numberOfNodes = 16;
    public int maxConnections = 4;
    public GameObject nodePrefab;
    public GameObject start;
    public GameObject enemy;
    public GameObject enemy2;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject a;
    public GameObject b;
    public Vector3 s = new Vector3();
    public Vector3 e = new Vector3();
    private Pathfinding pathfinding;
    Player p ;
    public GameObject door;
    public GameObject tail;

    public TilemapVisualizer tilemapVisualizer;
    private List<GraphNode> nodes = new List<GraphNode>();
    public GameObject player;


    void Start()
    {

        Generate();

    }






    /*
     private void Update()
       {
           // Check if the player object exists and its health is zero or below
           if (player != null && GameController.Health <= 0)
           {
              // player.SetActive(false); // Deactivate the player object
           }
       }
    */ 
    void Generate()
    {


        p = GetComponent<Player>();
        PlaceNodes();
        GenerateRandomConnections();

        //parcours_dfs(nodes);
        PRIM(nodes); 
      
        head_tail(nodes);
        Creategrid();
      
        
        
        
        
        
        
        foreach(GraphNode node in nodes)
        {
            Debug.Log(node.ConnectedNodesAfterPrim.Count  + "AAAAAAAAAAAAAAAA") ; 




        }
        
        
        
        
        
        
        
        
        /* int randpm = Random.Range(1, 5);
        int i = 0;

       foreach (GraphNode node in tilemapVisualizer.Grid)
        {
            Debug.Log(node.Position);

            if (i == 1)
            {


                seeker = node.Position;

                Debug.Log("seeker " + node.Position);
            }

            if (i == 3)
            {
                target = node.Position;
                Debug.Log("target " + node.Position);
            }

            i++;
        }
       */








    }



    public GraphNode FIndTale(List<GraphNode> nodes   , GraphNode start) {

        int i = 0;
        GraphNode tail  = new GraphNode(start.Position)  ; 
        foreach(GraphNode node in nodes
        ) {
           
                 tilemapVisualizer.FindPath(start.Position, node.Position);

                if (i > PATH.Count) {

                    i = PATH.Count;

                tail = node; 

             

                }


            

        
        
        
        }
        Debug.DrawRay(start.Position, tail.Position, Color.cyan  , 500f);
        Debug.Log(  "tail "  + tail.Position); 
        return tail; 
             
    
    
    }














 public   List<GraphNode> Path = new List<GraphNode>(); 
    public void StartPathfinding(Vector3 startPosition)
    {
        // Ensure that the tilemapVisualizer reference is not null
        if (tilemapVisualizer != null)
        {
            GraphNode n = new GraphNode(startPosition);
            tilemapVisualizer.Grid.Add(n); 
            tilemapVisualizer.FindPath(startPosition, e);
        }
        else
        {
            Debug.LogError("TilemapVisualizer reference is null.");
        }
    }

    public void StartPathfindingDjikstra(Vector3 startPosition)
    {
        // Ensure that the tilemapVisualizer reference is not null
        if (tilemapVisualizer != null)
        {
            GraphNode n = new GraphNode(startPosition);
            tilemapVisualizer.Grid.Add(n);
            tilemapVisualizer.FindPathWithDijkstra(startPosition, e);
        }
        else
        {
            Debug.LogError("TilemapVisualizer reference is null.");
        }
    }









    /* private void Update()
         {



               tilemapVisualizer.FindPath(s, e);
         foreach (GraphNode n  in tilemapVisualizer.Grid)
         {

             if (n.Name == "end of the map ")
             {
                 Debug.Log(n.Name + n.Position); 



             }

         }






         }  */


    private void Awake()
    {
        a = GetComponent<GameObject>();
        b = GetComponent<GameObject>();


    }
 
    public static List<GraphNode> Grid; 
   
    void Creategrid()
    {

        foreach (Vector3 tile in tilemapVisualizer.tiles)
        {
            GraphNode a = new GraphNode(tile);
            tilemapVisualizer.Grid.Add(a);
            
          //  Grid.Add(a); 
            /*   for (int x = -1; x <= 1; x++)
               {
                   for (int y = -1; y <= 1; y++)
                   {

                       if (x == 0 && y == 0)
                           continue;
                       float X = a.Position.x + x;

                       float Y = a.Position.y + y;

                       GraphNode new1 = new GraphNode(new Vector3(X, Y, 0));
                       tilemapVisualizer.Grid.Add(new1);






                   }*/










            }

        }
    
        void PlaceNodes()
    {
        for (int i = 0; i <= numberOfNodes; i++)
        {
            UnityEngine.Vector3 randomPosition = GetRandomGridPosition();
            
            GameObject newNode = Instantiate(nodePrefab, randomPosition, Quaternion.identity);
            foreach (var n in nodes)
            {
                if ( Vector3.Distance(randomPosition, n.Position) < 50) {continue; }
            }
            GraphNode node = new GraphNode(randomPosition);
            tilemapVisualizer.Grid.Add(node);
            Debug.Log(node.Position); 
            int x = Random.Range(6, 8);
            int y = Random.Range(6, 8);
            tilemapVisualizer.CreateRoom(node.Position, x, y);
           
            node.AssociatedObject = newNode;
            nodes.Add(node);
        }
    }

    Vector3 GetRandomGridPosition()
    {
        float x = Random.Range(0, gridSizeX) * gridSpacing;
        float y = Random.Range(0, gridSizeY) * gridSpacing;
        return new Vector3(x, y, 0f);
    }

     void Generate11RandomConnections()
     {
         foreach (GraphNode node in nodes)
         {
             for (int i = 0; i < maxConnections; i++)
             {
                 GraphNode connectedNode = nodes[Random.Range(0, nodes.Count)];
                 float distance = Vector3.Distance(connectedNode.Position, node.Position);
                 if (distance > 30f && distance < 35f) continue;
                 if (connectedNode != node && !node.ConnectedNodes.Contains(connectedNode))
                 {
                     node.ConnectedNodes.Add(connectedNode);
                     connectedNode.ConnectedNodes.Add(node);
                 }
             }
         }
     }


    void GenerateRandomConnections()
    {
        foreach (GraphNode node in nodes)
        {
            foreach (GraphNode otherNode in nodes)
            {
                if (node != otherNode && !node.ConnectedNodes.Contains(otherNode))
                {
                    node.ConnectedNodes.Add(otherNode);
                    otherNode.ConnectedNodes.Add(node);
                }
            }
        }
    }


    /* void GenerateRandomConnections()
     {
         // Extract node positions
         var points = nodes.Select(node => new TriangleNet.Geometry.Vertex(node.Position.x, node.Position.y)).ToList();

         // Perform Delaunay triangulation
         var mesh = (TriangleNet.Mesh)null;
         var quality = new TriangleNet.Meshing.QualityOptions() { MinimumAngle = 25 };
         var polygon = new TriangleNet.Geometry.Polygon();
         polygon.Add(new TriangleNet.Geometry.Contour(points));
         mesh = (TriangleNet.Mesh)polygon.Triangulate(quality);

         // Extract edges from the triangles
         var edges = new HashSet<(int, int)>();
         foreach (var triangle in mesh.Triangles)
         {
             for (int i = 0; i < 3; i++)
             {
                 int vertex1 = triangle.vertices[i];
                 int vertex2 = triangle.vertices[(i + 1) % 3];
                 if (vertex1 < vertex2)
                 {
                     edges.Add((vertex1, vertex2));
                 }
                 else
                 {
                     edges.Add((vertex2, vertex1));
                 }
             }
         }

         // Add connections based on the extracted edges
         foreach (var (vertex1, vertex2) in edges)
         {
             var node1 = nodes[vertex1];
             var node2 = nodes[vertex2];
             if (node1 != node2 && !node1.ConnectedNodes.Contains(node2))
             {
                 node1.ConnectedNodes.Add(node2);
                 node2.ConnectedNodes.Add(node1);
             }
         }
     }


     void hea11d_tail(List<GraphNode> nodes)
     {
         GraphNode source = null;
         GraphNode destination = null;
         var (diameterLength, diameterHead, diameterTail) = Diameter(nodes);
         foreach (GraphNode node in nodes)
         {
             if (node.Position == diameterHead.Position)
             {
                 Debug.Log("Destroyed nodePrefab at diameterHead" + node.Position);
                 GameObject newNode = Instantiate(start, node.Position, Quaternion.identity);
                 source = node;

                 Instantiate(player, node.Position, Quaternion.identity);
                 Destroy(node.AssociatedObject);
             }
             if (node.Position == diameterTail.Position)
             {
                 Debug.Log("Destroyed nodePrefab at diameterTail");
                 GameObject newNode = Instantiate(tail, node.Position, Quaternion.identity);
                 Instantiate(door, node.Position, Quaternion.identity);
                 destination = node;
                 Destroy(node.AssociatedObject);
             }

        }

         foreach (GraphNode node in nodes) {

             if (node != source && node!=destination)
             {
                 float distanceToSource = Vector3.Distance(node.Position, source.Position);
                 float totalDistance = Vector3.Distance(source.Position, destination.Position);
                 // Check if the current node is more than halfway between the source and destination nodes
                 if (distanceToSource > totalDistance / 2)
                 {
                     Instantiate(enemy, node.Position, Quaternion.identity);
                     Instantiate(enemy2, node.Position + new Vector3(0,0,0), Quaternion.identity);

                 }else { Instantiate(enemy, node.Position, Quaternion.identity); }

             }
         }


     }*/
    public void destroyEnemy()
    {
        Destroy(enemy.gameObject); 
    }
   
    void parcours_dfs(List<GraphNode> nodes)
    {
        // Ensure the list of nodes is not empty
        if (nodes.Count == 0)
        {
            Debug.LogError("No nodes to traverse.");
             return;
        }

        // Select a random node to start the DFS traversal
        GraphNode startNode = nodes[Random.Range(0, nodes.Count)];

        // Create a HashSet to keep track of visited nodes
        HashSet<GraphNode> visited = new HashSet<GraphNode>();

        // Start the recursive DFS traversal from the random start node
        DFSUtil(startNode, visited);
    }

    void DFSUtil(GraphNode node, HashSet<GraphNode> visited)
    {
        // Mark the current node as visited
        visited.Add(node);

        // Perform any desired action on the current node (e.g., print its position)
     //   Debug.Log("Visited node at position: " + node.Position);

        // Traverse all connected nodes that haven't been visited yet
        foreach (GraphNode neighbor in node.ConnectedNodes)
        {
            if (!visited.Contains(neighbor))
            {
                

               
                DFSUtil(neighbor, visited);
           
             
             // Debug.DrawLine(neighbor.Position, node.Position, Color.yellow, 500f);
               // Debug.Log("Connection made between " + node.Position + " and " + neighbor.Position);
          


            }
        }
    }



    void PRIM(List<GraphNode> nodes)
    {
        HashSet<GraphNode> visited = new HashSet<GraphNode>(); // Set to track visited nodes
        List<GraphNode> minimumSpanningTree = new List<GraphNode>(); // List to store the minimum spanning tree

        // Select a starting node arbitrarily
        GraphNode startNode = nodes[Random.Range(0, nodes.Count)];
        visited.Add(startNode);

        while (visited.Count < nodes.Count)
        {
            float minWeight = float.MaxValue;
            GraphNode minNode = null;
            GraphNode minNeighbor = null;

            foreach (GraphNode node in visited)
            {
                foreach (GraphNode neighbor in node.ConnectedNodes)
                {
                    if (!visited.Contains(neighbor))
                    {
                        // Find the edge with the minimum weight connecting visited nodes to unvisited nodes
                        float weight = Vector3.Distance(node.Position, neighbor.Position);
                        if (weight < minWeight)
                        {
                            minWeight = weight;
                            minNode = node;
                            minNeighbor = neighbor;
                        }
                    }
                }
            }

            if (minNode != null && minNeighbor != null)
            {
                // Add the minimum weight edge to the minimum spanning tree
                minimumSpanningTree.Add(minNeighbor);
                List<Vector3> corridors = createCorridor(minNode.Position, minNeighbor.Position);

                tilemapVisualizer.PaintFloorTiles((corridors));
                Debug.DrawLine(minNode.Position, minNeighbor.Position, Color.yellow, 500f);
                minNeighbor.ConnectedNodesAfterPrim.Add(minNode);
               minNode.ConnectedNodesAfterPrim.Add(minNeighbor);



                visited.Add(minNeighbor);
            }
        }

        // At this point, the minimum spanning tree is constructed in the minimumSpanningTree list
        // You can further process or return this list as needed
    }












    private List<Vector3> createCorridor(Vector3 current, Vector3 destination)
    {
        List<Vector3> corridor = new List<Vector3>();
        var position = current;
        corridor.Add(position);
       

        // Vertical movement loop
        while (position.y != destination.y)
        {
            if (destination.y > position.y)
                position += Vector3.up;
            else if (destination.y < position.y)
                position += Vector3.down;

            corridor.Add(position );
           // corridor.Add(position+ new Vector3(0,5,0));
           // corridor.Add(position + new Vector3(0, -5, 0));
        }

        // Horizontal movement loop
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
                position += Vector3.right;
            else if (destination.x < position.x)
                position += Vector3.left;

            corridor.Add(position);
          //  corridor.Add(position + new Vector3(5, 0, 0));
          //  corridor.Add(position + new Vector3(-5, 0, 0));
        }

        return corridor;
    }
   
    public Vector3 head_tail(List<GraphNode> nodes)
    {
        GraphNode source = null;
        GraphNode destination = null;
        int random = Random.Range(0, nodes.Count);
        int i = 0; 
        foreach(GraphNode node in nodes)
        { 



            if (node.ConnectedNodesAfterPrim.Count  == 1 )
            {
                Debug.Log("Destroyed nodePrefab at diameterHead" + node.Position);
                GameObject newNode = Instantiate(start, node.Position, Quaternion.identity);
              
                source = node;
                s = node.Position;
                tilemapVisualizer.Grid.Add(source); 
                source.Name = "end of the map ";    
                Instantiate(player, node.Position, Quaternion.identity);

                Destroy(node.AssociatedObject);
                break; 
            }
     



        }
        destination = Dijkstra(source);
        foreach (GraphNode node in nodes)
        {
             
            if (node.Position== destination.Position)
            {

                Debug.Log("Destroyed nodePrefab at diameterTail");
                GameObject newNode = Instantiate(tail, node.Position, Quaternion.identity);
                Instantiate(door, node.Position, Quaternion.identity);
                e = node.Position;
                destination = node;
                Destroy(node.AssociatedObject);

            }
        }

      //  pathfinding.FindPath(s, e);
        return destination.Position; 


    }

  
















    GraphNode Dijkstra(GraphNode startNode)
    {
        Dictionary<GraphNode, float> distances = new Dictionary<GraphNode, float>();
        Dictionary<GraphNode, GraphNode> previous = new Dictionary<GraphNode, GraphNode>();
        HashSet<GraphNode> visited = new HashSet<GraphNode>();

        foreach (var node in nodes)
        {
            distances[node] = Mathf.Infinity;
            previous[node] = null;
        }

        distances[startNode] = 0;

        while (visited.Count < nodes.Count)
        {
            GraphNode current = null;
            float shortestDistance = Mathf.Infinity;

            foreach (var node in nodes)
            {
                if (!visited.Contains(node) && distances[node] < shortestDistance)
                {
                    current = node;
                    shortestDistance = distances[node];
                }
            }

            visited.Add(current);

            foreach (var neighbor in current.ConnectedNodes)
            {
                float newDistance = distances[current] + Vector3.Distance(current.Position, neighbor.Position);
                if (newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    previous[neighbor] = current;
                }
            }
        }

        // Find the node with the maximum distance
        GraphNode farthestNode = null;
        float maxDistance = 0;

        foreach (var kvp in distances)
        {
            if (kvp.Value > maxDistance)
            {
                maxDistance = kvp.Value;
                farthestNode = kvp.Key;
            }
        }

        return farthestNode;
    }  


   














}
