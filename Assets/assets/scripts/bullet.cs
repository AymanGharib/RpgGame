using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private Vector2 lastPos;
    private Vector2 CurrPos;

    public bool isEnemyBullet =  false; 
    public float LifeTime;
    private Vector2 playerPos;
    // Start is called before the first frame update
    void Start()
    {   if(!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.Bullet, GameController.Bullet);
        }



        StartCoroutine(DeathDelay());
    }
    private void Update()
    {
         if (isEnemyBullet)
        {
            CurrPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime)
;           if (CurrPos == lastPos) { Destroy(gameObject); }

            lastPos = CurrPos; 

        }
    }
    public void GetPlayer (Transform player)
    {

        playerPos = player.position; 




    }
    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<enemy>().Death();
            collision.gameObject.GetComponent<NodeGenerator>().destroyEnemy();

            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        

        if (collision.gameObject.CompareTag("Enemy") && !isEnemyBullet)
        {
            Destroy(gameObject);

            collision.gameObject.GetComponent<enemy>().Death();
          //  collision.gameObject.GetComponent<NodeGenerator>().destroyEnemy();
        }
        if (collision.gameObject.CompareTag("Player") && isEnemyBullet)
        {


            GameController.DamagePlayer(1);
            Destroy(gameObject); 




        }


    }









    // Update is called once per frame
   
} 
