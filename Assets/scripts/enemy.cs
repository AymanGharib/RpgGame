using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  enum enemyState { Wander , Follow , Die , attack};
public enum enemyType {      Melee , Ranged}
public class enemy : MonoBehaviour
{
    GameObject player;

    public enemyState curr = enemyState.Wander;
    public enemyType a = enemyType.Melee; 

    public GameObject bulletPrefab;
    public Rigidbody2D rb;
    public float range;
    public float speed;
    private bool dir = false;
    private bool dead = false;
    private Vector3 randomDir;
    public float attakingRange;
    private bool coolDownAttack = false;
    public float coolDown; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


    }

    // Update is called once per frame
    void Update()
    {

        switch (curr)
        {



            case (enemyState.Wander):
                Wander();
                break;
            case (enemyState.Follow):
                Follow();
                break;
            case (enemyState.Die):
                Death(); 
                break;
            case (enemyState.attack):
                Attack();
                break;













        }

        if (isPlayerinRange(range) && curr != enemyState.Die) { curr = enemyState.Follow; }
        else if (!isPlayerinRange(range) && curr != enemyState.Die) { curr = enemyState.Wander; }
        if (Vector3.Distance (transform.position , player.transform.position) <= attakingRange)
        {

            curr = enemyState.attack; 

        }
    }

    private bool isPlayerinRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range
     ;
    }
    private IEnumerator chooseDirection()
    {
        dir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        dir = false;

    }

    public void Death() { Destroy(gameObject);
        curr = enemyState.Die;
    }
    void Wander()
    {
        // Check if the enemy is not currently in follow mode
        if (curr != enemyState.Follow)
        {
            // If not currently choosing a direction, start choosing a new direction
            if (!dir)
            {
                StartCoroutine(chooseDirection());
            }

            // Stop the enemy's movement if the player is not within range
            if (!isPlayerinRange(range))
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                // Move forward in the chosen direction
                transform.position += -transform.right * speed * Time.deltaTime;
            }
        }
    }

    void Follow() {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
      
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
     

        if (other.CompareTag("Wall") && !isPlayerinRange(range))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(chooseDirection());
        }
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet hit enemy!");
            Death(); // Call the Death method to destroy the enemy
        }
    }



private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Wall") && !isPlayerinRange(range))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(chooseDirection());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // No need for further action when exiting the wall
    }




    void Attack()
    {
        if (!coolDownAttack)
        {

            switch (a)
            {
                case (enemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;
                case (enemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab,transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<bullet>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<bullet>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                    break; 
            }
           
        }
      


    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new  WaitForSeconds(coolDown);
        coolDownAttack = false;


    }
}
