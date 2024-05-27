using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.WSA;

public class Player : MonoBehaviour
{
    bool djiksra = false;
    private Canvas canvas;
    public bool CollidesWithItem = false;
    public GameObject Astar;
    public GameObject Djikstra;
    // public GameObject Menu;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    // private Pathfinding pathfinding;
    private Vector2 moveDirection = Vector2.zero;

    // private bool isMovingToTail = false; // Flag to indicate if the player is moving to the tail
    private List<Node> pathToTail = new List<Node>(); // List to store the path to the tail

    public GameObject bullet;
    public float lastFire;
    public float FireDelay;
    public float bulletSpeed;
    public Text collected;
    public static int amount = 0;
    public GameObject tail;

    public GameObject a;
    public TilemapVisualizer tile;
    public NodeGenerator gen;

    private void FixedUpdate()
    {
        Move();
        //MoveAlongPath();

    }

    private void Move()
    {
        rb.velocity = moveDirection.normalized * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // No need for further action when exiting the wall
    }
    // Singleton instance


    // Rest of your Player class...





    private void Update()
    {



      


if (transform.position == nodeGenerator.e)
        {
            SceneManager.LoadScene("menu");


        }

        
            FireDelay = GameController.FireRate;
            moveSpeed = GameController.MoveSpeed;
            ProcessInputs();
            float shootH = Input.GetAxis("shootHorizontal");
            float shootV = Input.GetAxis("shootVertical");
            if (shootH != 0 || shootV != 0 && Time.time > lastFire + FireDelay)
            {
                shoot(shootH, shootV);


                lastFire = Time.time;
            }
        



        if (djiksra && nodeGenerator
                )
        {


            nodeGenerator.StartPathfindingDjikstra(transform.position);

            GetComponent<Collider2D>().isTrigger = true;
            //moveSpeed = 0;
            moveDirection = Vector2.zero;
            StartCoroutine(MoveAlongPathAfterDelayDjikstra(0.0001f));










        }















        //  if (nodeGenerator == null) { Debug.Log("jdjdjdjdjdj"); }
        // Check if the player has collided with an enemy
        if ((collidedWithEnemy) && nodeGenerator != null)
        //  if (==5     &&      nodeGenerator != null)
        {
            GetComponent<Collider2D>().isTrigger = false;
            FireDelay = GameController.FireRate;
            moveSpeed = GameController.MoveSpeed;
            float shootH1 = Input.GetAxis("shootHorizontal");
            float shootV1 = Input.GetAxis("shootVertical");
            if (shootH != 0 || shootV != 0 && Time.time > lastFire + FireDelay)
            {
                shoot(shootH, shootV);


                lastFire = Time.time;
            }
            //Debug.Log("Trigger pathfinding with the player's position as the start node");

            //   nodeGenerator.StartPathfinding(transform.position);
            //   nodeGenerator.StartPathfindingDjikstra(transform.position); 




            // Check the chosen algorithm and call the corresponding pathfinding method























            nodeGenerator.StartPathfinding(transform.position);



















            // GetComponent<Collider2D>().isTrigger = true;
            //moveSpeed = 0;
            moveDirection = Vector2.zero;
            StartCoroutine(MoveAlongPathAfterDelay(0.00001f));
            // MoveAlongPath();
            //collidedWithEnemy = false; // Reset the collision flag
        }

        if (GameController.Health <= 0)
        {
            SceneManager.LoadScene("RestartMenu");
            GameController.Health = 6;
            GameController.amount = 0;


            //  Destroy(gameObject);


        }







    }
    void shoot(float x, float y)
    {
        GameObject b = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        b.AddComponent<Rigidbody2D>().gravityScale = 0;
        b.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,

            0










            );
    }

    /* void TriggerPathfinding(Vector3 targetPosition)
     {
         if (pathfinding != null)
         {
             pathfinding.FindPath(transform.position, targetPosition);
         }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag( "Item") )
        {
            Debug.Log("dududududuuddu"); 
            isMovingToTail = true; // Set the flag to indicate that the player is moving to the tail
            Pathfinding nodeGenerator = FindObjectOfType<Pathfinding>(); // Find the NodeGenerator component
            if (nodeGenerator != null)
            {
                // Trigger pathfinding to find the path to the tail
                pathToTail = nodeGenerator.FindPath(transform.position, tail.transform.position);
            }
        }
    } }*/
    private void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY);




    }



    private IEnumerator MoveAlongPathAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int i = 0;


        while (i < nodeGenerator.Path.Count)
        {

            Vector3 targetPosition = nodeGenerator.Path[i].Position;


            Vector3 moveDirection = (targetPosition - transform.position).normalized;


            transform.position += moveDirection * 0.1f * Time.deltaTime;


            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                i++;
            }

            yield return null;
        }


        collidedWithEnemy = false;
    }

    private IEnumerator MoveAlongPathAfterDelayDjikstra(float delay)
    {
        yield return new WaitForSeconds(delay);

        int i = 0;


        while (i < nodeGenerator.DjikstraPath.Count)
        {

            Vector3 targetPosition = nodeGenerator.DjikstraPath[i].Position;


            Vector3 moveDirection = (targetPosition - transform.position).normalized;


            transform.position += moveDirection * 0.1f * Time.deltaTime;


            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                i++;
            }

            yield return null;
        }


        collidedWithEnemy = false;
    }



    private int currentWaypointIndex = 0;

    private void MoveAlongPath()
    {
        /* Check if the current waypoint index is within the bounds of the path
        if (currentWaypointIndex >= 0 && currentWaypointIndex < nodeGenerator.Path.Count)
        {
            // Get the position of the current waypoint
            Vector3 currentWaypoint = nodeGenerator.Path[currentWaypointIndex].Position;

            // Move the player towards the current waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, 10f * Time.deltaTime);

            // Check if the player has reached the current waypoint
            if (Vector3.Distance(transform.position, currentWaypoint) < 0.1f)
            {
                // Move to the next waypoint
                currentWaypointIndex++;

                // If reached the end of the path, stop moving
                if (currentWaypointIndex >= nodeGenerator.Path.Count)
                {
                    // Reset waypoint index for future use
                    currentWaypointIndex = 0;
                    return;
                }
            }
        }

        */
    }



    private void ContinueWithPathfinding()
    {
        // Retrieve the chosen algorithm


        // Continue with moving along the path
        StartCoroutine(MoveAlongPathAfterDelay(0.00001f));
    }




    private NodeGenerator nodeGenerator;
    private bool collidedWithEnemy = false;

    private void Start()

    {
        canvas = FindObjectOfType<Canvas>();  // Find the NodeGenerator component dynamically
        nodeGenerator = FindObjectOfType<NodeGenerator>();
        if (nodeGenerator == null)
        {
            Debug.LogError("NodeGenerator not found in the scene!");
        }



    }




    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("prizes"))
        {

            amount++;
            Debug.Log("amount =  " +
            amount
                );

        }

        if (other.gameObject.CompareTag("tale"))
        {


            SceneManager.LoadScene("menu");
        }



        if (GameController.amount >= 3)
        {
            Debug.Log(gameObject.name + " collided with an enemy.");


            GameObject menuButton = Instantiate(Astar, canvas.transform);
            GameObject b = Instantiate(Djikstra, canvas.transform);

            // You might want to position the button based on your UI layout

            // Optionally, you can add listeners to the button's onClick event to handle its functionality
            Button buttonComponent = menuButton.GetComponent<Button>();
            Button butto = b.GetComponent<Button>();

            if (butto != null)

            {
                Debug.Log("Button clicked!");
                butto.onClick.AddListener(() =>
                {
                    Destroy(menuButton);
                    Destroy(b);

                    djiksra = true;
                });
            }








            if (buttonComponent != null)

            {
                Debug.Log("Button clicked!");
                buttonComponent.onClick.AddListener(() =>
                {
                    Destroy(menuButton);
                    Destroy(b);
                    // Handle button click action here, such as reloading the level or going to the main menu
                    collidedWithEnemy = true;
                });
            }
        }


















        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(gameObject.name + " collided with an enemy.");

            // Instantiate the menu panel prefab

            /* 
                 if (other.gameObject.CompareTag("Enemy"))
                    {
                        Debug.Log(gameObject.name + " collided with an enemy.");
                        //SceneManager.LoadScene("menu");

                       collidedWithEnemy = true; 
                    }
            


            if (other.gameObject.CompareTag("Enemy"))*/


        }

    }
}




/* private void OnCollisionEnter2D(Collision2D other)
 {


     if (other != null)
     {
         //   Debug.Log("Collision2D object: " + other.gameObject + "hahhaahhhahhahhahhahhaahahahah"); // Debug log to check Collision2D object
         if (other.gameObject.CompareTag("Enemy"))


         {


             Debug.Log(gameObject.transform.position + " hahhaahhhahhahhahhahhaahahahah");
             x = gameObject.transform.position;
             GraphNode node = new GraphNode(x);
             Debug.Log(node.Position + " TTTTTTTTTTTTTTTTTTTTTTTTTTTTTTtttt");
             tile.Grid.Add(node);
             if (tile.Grid.Contains(node))
             {
                 Debug.Log("yesyesyes");
             }

             foreach (GraphNode z in gen.Grid())
             {

                // tile.Grid.Add(z);
                 if (z.Name == "the end of the map ")
                 {
                     Debug.DrawRay(node.Position, z.Position, Color.black);


                 }
             }



         }






             CollidesWithItem = true;



             }

















     else
     {
         Debug.LogError("Collision2D object is null!"); // Log error if Collision2D object is null
     }
 }
















 private void MoveAlongPath()
 {
     if (pathToTail != null && pathToTail.Count > 0) // Check if pathToTail is not null and not empty
     {
         Vector3 nextPosition = pathToTail[0].worldPosition;
         transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

         if (Vector3.Distance(transform.position, nextPosition) < 0.01f)
         {
             pathToTail.RemoveAt(0);

             if (pathToTail.Count == 0)
             {
                 isMovingToTail = false;
             }
         }
     }
 }*/

