using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TilemapVisualizer : MonoBehaviour
{
    // [SerializeField] private Tilemap tilemap;
    public Tilemap floorTilemap;
    public Tilemap Tilemap;
    [SerializeField] private TileBase floorTile = null;
    [SerializeField] private TileBase Tile = null;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject wallPrefab2;
    public GameObject enemy;
    public GameObject enemy2;
   
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public NodeGenerator nodegen;
    public GameObject a;
    public GameObject b;

    public List<Vector3> tiles = new List<Vector3>();

    public List<GraphNode> Grid = new List<GraphNode>();
    Player player;

    /*ISpawner<int> enemySpawner;
    ISpawner<int> itemSpawner; */
    private IAbstractFactory enemy1Factory;
    private IAbstractFactory enemy2Factory;

    void Start()
    {
        player = new Player();


        enemy1Factory = new Enemy1Factory(enemy, item1, item2);
        enemy2Factory = new Enemy2Factory(enemy2, item2, item3);




        //PaintWholeTilemapExcept(tiles, Tilemap, floorTile);

 StartCoroutine(CreateWallsWithDelay());





        




        
    }

    public List<GraphNode> Tpath;

    /* void CreateGrid()
     {

         foreach (Vector3 tile in tiles)
         {
             GraphNode a = new GraphNode(tile);
             Grid.Add(a);


         }

     }
    void CreateGrid()
    {
        BoundsInt bounds = floorTilemap.cellBounds;
        BoundsInt a = floorTilemap.cellBounds;
        Vector3Int minTilePosition = bounds.min;
        Vector3Int maxTilePosition = bounds.max;

        for (int x = minTilePosition.x; x < maxTilePosition.x; x++)
        {
            for (int y = minTilePosition.y; y < maxTilePosition.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Vector3 tileWorldPosition = floorTilemap.CellToWorld(tilePosition);
                GraphNode node = new GraphNode(tileWorldPosition);
                Grid.Add(node);
             

            }
        }
    }
    */

    public int getMaxSize() { return tiles.Count; }



    public void FindPathWithDijkstra(Vector3 startPos, Vector3 endPos)
    {
        GraphNode startNode = GetNode(startPos);
        GraphNode endNode = GetNode(endPos);

        // Create a priority queue (min-heap) to store nodes based on their costs
        Heap<GraphNode> openSet = new Heap<GraphNode>(getMaxSize());

        // Initialize the cost of each node to infinity
        foreach (GraphNode node in Grid)
        {
            node.gCost = Mathf.Infinity;
        }

        // Set the cost of the starting node to 0
        startNode.gCost = 0;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GraphNode currentNode = openSet.RemoveFirst(); // Extract the node with the lowest cost

            // If the current node is the destination, stop the search
            if (currentNode == endNode)
            {
                RetracePathDjkstra(startNode, endNode);
                return;
            }

            foreach (GraphNode neighbor in GetNeighbours(currentNode))
            {
                // Calculate the total cost to reach the neighbor node via the current node
                float newCost = currentNode.gCost + Vector3.Distance(currentNode.Position, neighbor.Position);

                // If the total cost is less than the neighbor's current cost, update the neighbor's cost
                if (newCost < neighbor.gCost)
                {
                    neighbor.gCost = newCost;
                    neighbor.parent = currentNode;

                    // Add the neighbor to the priority queue
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else
                    {
                        openSet.UpdateItem(neighbor);
                    }
                }
            }
        }

        // If no path is found, handle the case here
        UnityEngine.Debug.Log("No path found!");
    }








    public List<GraphNode> RetracePathDjkstra(GraphNode startNode, GraphNode endNode)
    {
        List<GraphNode> path = new List<GraphNode>();
        GraphNode currentNode = endNode;

        while (currentNode != null && currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;

            //  UnityEngine.Debug.DrawLine(


            //  startNode.Position, currentNode.Position, Color.cyan);


        }
        /* for (int i = 0; i < path.Count - 1; i++)
         {
             GraphNode nodeA = path[i];
             GraphNode nodeB = path[i + 1];
             UnityEngine.Debug.DrawLine(nodeA.Position, nodeB.Position, Color.black);
         }*/

        path.Reverse();
        foreach (GraphNode node in path)
        {

             foreach(GraphNode n in GetNeighbours(node)) {
                  if (path.Contains(n))
                UnityEngine.Debug.DrawLine(node.Position, n.Position, Color.black, 500f);
            }


          










          



        }



        nodegen.DjikstraPath = path;
        return path;



    }

    public void FindPath(Vector3 startPos, Vector3 endPos)
    {
        GraphNode startNode = GetNode(startPos);
        GraphNode endNode = GetNode(endPos);
        Heap<GraphNode> openSet = new Heap<GraphNode>(getMaxSize());

        //  List<GraphNode> openSet = new List<GraphNode>();
        HashSet<GraphNode> closedSet = new HashSet<GraphNode>();

        foreach (GraphNode node in Grid)
        {
            node.gCost = Mathf.Infinity;
            node.hCost = Vector3.Distance(node.Position, endNode.Position);
          //  UnityEngine.Debug.Log("HHHHHHHHHHHHHHHHHHHHHHHHHHHHH" + node.Position +     node.hCost);
          //  UnityEngine.Debug.DrawLine(startPos, node.Position, Color.black);

        }

        startNode.gCost = 0;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GraphNode currentNode = openSet.RemoveFirst();
            /* GraphNode currentNode = openSet[0];

             for (int i = 1; i < openSet.Count; i++)
             {
                 if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                 {
                     currentNode = openSet[i];
                 }
             }

             openSet.Remove(currentNode); */
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);


                return;
            }

            foreach (GraphNode neighbor in GetNeighbours(currentNode) )
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float newMovementCostToNeighbor = currentNode.gCost + Vector3.Distance(currentNode.Position, neighbor.Position);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }



    }

 public   List<GraphNode> RetracePath(GraphNode startNode, GraphNode endNode)
    {
        List<GraphNode> path = new List<GraphNode>();
        GraphNode currentNode = endNode;

        while (currentNode != null && currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;

          //  UnityEngine.Debug.DrawLine(


              //  startNode.Position, currentNode.Position, Color.cyan);
          












        }
        for (int i = 0; i < path.Count - 1; i++)
        {
            GraphNode nodeA = path[i];
            GraphNode nodeB = path[i + 1];
            UnityEngine.Debug.DrawLine(nodeA.Position, nodeB.Position, Color.cyan);
        }
        
        path.Reverse();

        nodegen.Path = path;
        return path;



    }

   
    

    IEnumerator CreateWallsWithDelay()
    {
        yield return new WaitForSeconds(2f); 

        spawnwalls(tiles);
spawnEnemies(tiles);
       // spawnItems(tiles);
        // CreateGrid();
Destroy(floorTilemap.gameObject);

       // PaintWholeTilemapExcept(tiles, Tilemap, Tile); 
       



        //  s = nodegenerator.s;
        //  UnityEngine.Debug.Log(s);
        //  UnityEngine.Debug.Log(e);
        //  e = nodegenerator.e;
        // pathfinding.FindPath(s, e);



        //// UnityEngine.Debug.Log(a.transform.position);
        //// UnityEngine.Debug.Log(b.transform.position);








      //  OnDrawGizmosSelected();









    }














    public List<GraphNode> GetNeighbours(GraphNode node)
    {
        List<GraphNode> neighbours = new List<GraphNode>();

        foreach (GraphNode neighbor in Grid)
        {
            if (neighbor == node)
                continue;

            float distance = Vector3.Distance(node.Position, neighbor.Position);
            if (distance <= 1.1f) // Assuming nodes are connected if they are within a certain distance
            {
                neighbours.Add(neighbor);
            }
        }

        return neighbours;
    }





    public GraphNode GetNode(Vector3 Pos)
    {
        foreach(GraphNode n in Grid) { if (n.Position == Pos ) 
                    return n;  }


        return null; 
    }







    /*


    private void OnDrawGizmos()
    {
        if (Grid != null)
        {
            foreach (GraphNode node in Grid)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(node.Position, Vector3.one);
            }
        }
    }*/

    public int maxEnemies = 5;
   
 
    public void spawnEnemies(List<Vector3> tiles)
    {
        int random = UnityEngine.Random.Range(1, 3);
        Vector3 randomTile = new Vector3();
        Vector3 randomTile2 = new Vector3();
        Vector3 randomTile3 = new Vector3();
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < 10; i++)
        {
            while (list.Contains(randomTile))
            {
                randomTile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
                randomTile2 = tiles[UnityEngine.Random.Range(0, tiles.Count)];
                randomTile3 = tiles[UnityEngine.Random.Range(0, tiles.Count)];
            }
      
            list.Add(randomTile); 

             
      

           // Randomly generates 1 or 2

            if (nodegen.playerType == 1)
            {
                enemy1Factory.Spawn(randomTile , randomTile2, randomTile3);
            } 
            else  if(nodegen.playerType  == 2)
            {
                enemy2Factory.Spawn(randomTile, randomTile2, randomTile3);
            }
            else
            {
                UnityEngine.Debug.Log("we coudnt find the foundable"); 


            }
        }
    }






















    /* 
    public void spawnEnemies(List<Vector3> tiles)
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            Vector3 randomTile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
            int random = UnityEngine.Random.Range(1, 3); // Randomly generates 1 or 2

            enemySpawner.Spawn(random, randomTile);
        }
    }

    public void spawnItems(List<Vector3> tiles)
    {
        for (int i = 0; i < maxItems; i++)
        {
            Vector3 randomTile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
            int random = UnityEngine.Random.Range(1, 4); // Randomly generates 1, 2, or 3

            itemSpawner.Spawn(random, randomTile);
        }
    }













public void spawnEnemies(List<Vector3> tiles)
     {
         for (int i = 0; i < maxEnemies; i++)
         {
             Vector3 randomTile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
             int random = UnityEngine.Random.Range(1, 3); // Randomly generates 1 or 2

             if (random == 1)
             {
                 Instantiate(enemy, randomTile, Quaternion.identity); // Instantiate enemy type 1
             }
             else
             {
                 Instantiate(enemy2, randomTile, Quaternion.identity); // Instantiate enemy type 2
             }
         }
     }

     public void spawnItems(List<Vector3> tiles)
     {
         for (int i = 0; i < maxItems; i++)
         {
             Vector3 randomTile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
             int random = UnityEngine.Random.Range(1, 4); // Randomly generates 1, 2, or 3

             if (random == 1)
             {
                 Instantiate(item1, randomTile, Quaternion.identity); // Instantiate item type 1
             }
             else if (random == 2)
             {
                 Instantiate(item2, randomTile, Quaternion.identity); // Instantiate item type 2
             }
             else
             {
                 Instantiate(item3, randomTile, Quaternion.identity); // Instantiate item type 3
             }
         }
     }
     */
    public void PaintFloorTiles(IEnumerable<Vector3> floorpos)
    {
        PaintTiles(floorpos, floorTilemap, floorTile, floorPrefab);
    }
  public   List<Vector3> getTiles()
    {


        return tiles;
    }



    private void PaintTiles(IEnumerable<Vector3> pos, Tilemap tilemap, TileBase tile, GameObject prefab)
    {
        foreach (var position in pos)
        {
            paintSingleTile(tilemap, tile, position);
          //  Instantiate(prefab, position, Quaternion.identity);
            tiles.Add(position);
        }
    }

    private void paintSingleTile(Tilemap tilemap, TileBase tile, Vector3 position)
    {
        // Convert the world position to cell position
        Vector3Int tilePosition = tilemap.WorldToCell(position);

        // Set the tile at the calculated cell position
        tilemap.SetTile(tilePosition, tile);
    }


    public void CreateRoom(Vector3 pos, int x, int y)
    {
        int startX = Mathf.RoundToInt(pos.x - x / 2f);
        int startY = Mathf.RoundToInt(pos.y - y / 2f);

        // Loop through each tile in the room
        for (int i = startX; i < startX + x; i++)
        {
            for (int j = startY; j < startY + y; j++)
            {
                // Set the tile at the calculated position
                Vector3 tilePosition = floorTilemap.CellToWorld(new Vector3Int(i, j, Mathf.RoundToInt(pos.z)));
                UnityEngine.Debug.DrawLine(tilePosition, tilePosition, Color.red);
                floorTilemap.SetTile(floorTilemap.WorldToCell(tilePosition), floorTile);

               
                //   Instantiate(floorPrefab, tilePosition, Quaternion.identity);
                tiles.Add(tilePosition);
            }
        }
      
    }

    public void spawnwalls(List<Vector3> tiles)
    {
        CreateWalls(tiles, wallPrefab, wallPrefab2);
    }

    public void CreateWalls(List<Vector3> pos, GameObject wallPrefab, GameObject wallPrefab2)
    {
        float margin = 0.05f; // Adjust the margin value as needed

        foreach (var position in pos)
        {
            for (var x = -1; x <= 1; ++x)
            {
                for (var y = -1; y <= 1; ++y)
                {
                    if (x == 0 && y == 0) continue; // Skip the center tile

                    var targetPos = new Vector3(position.x + x, position.y + y, position.z);
                    var marginPos = new Vector2(targetPos.x, targetPos.y) + new Vector2(margin * x, margin * y);

                    // Check if there's a floor tile at the target position
                    var tilePosition = floorTilemap.WorldToCell(targetPos);
                    var floorTileAtPosition = floorTilemap.GetTile(tilePosition);

                    // If there's no floor tile, instantiate a wall
                    if (floorTileAtPosition == null)
                    {
                        GameObject wallToInstantiate = (x == 0) ? wallPrefab : wallPrefab2;
                        Instantiate(wallToInstantiate, targetPos, Quaternion.identity);
                    }
                }
            }
        }
    }
    private void PaintWholeTilemapExcept(IEnumerable<Vector3> positions, Tilemap tilemap, TileBase tile)
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            if (!positions.Contains(tilemap.GetCellCenterWorld(pos)))
            {
                tilemap.SetTile(pos, tile);
            }
        }
    }
    /* public List<Node> GenerateNodes()
     {
         List<Node> nodes = new List<Node>();

         foreach (var position in tiles)
         {
             // Check if the tile position is walkable (you may need to adjust this condition based on your game logic)
             bool walkable = IsTileWalkable(position);

             // Create a node for the tile position
             Node node = new Node(walkable, position, Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
             nodes.Add(node);
         }

         return nodes;
     } 

     // Check if the tile at the given position is walkable
     private bool IsTileWalkable(Vector3 position)
     {
         return true;
    


    public void FindPath(Vector3 startPos, Vector3 endPos)
    {
        GraphNode startNode = GetNode(startPos);
        GraphNode endNode = GetNode(endPos);


        List<GraphNode> openSet = new List<GraphNode>();
        HashSet<GraphNode> closedSet = new HashSet<GraphNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            GraphNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return ;
            }

            foreach (GraphNode neighbor in GetNeighbours(currentNode))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float newMovementCostToNeighbor = currentNode.gCost + GettDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GettDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        UnityEngine.Debug.Log("No path found!");
     
    }


    void RetracePath(GraphNode a, GraphNode b)
    {

        List<GraphNode> path = new List<GraphNode>();
        GraphNode curr = b;

        while (curr != a)
        {
            path.Add(curr);
            curr = curr.parent;
            UnityEngine.Debug.Log("AYMAN AYMAN AYMAN"); 


        }
        path.Reverse();

       Tpath = path; 












    }


    float GettDistance(GraphNode a, GraphNode b)
    {

        float distX = Mathf.Abs(a.Position.x - b.Position.x);
        float distY = Mathf.Abs(a.Position.y - b.Position.y);


        if (distX > distY)
        {

            return (14 * distY + 10 * (distX - distY));



        }
        else
        {
            return (14 * distX + 10 * (distY - distX));
        }






     }*/


}











































