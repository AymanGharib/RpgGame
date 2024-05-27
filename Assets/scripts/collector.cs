using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class item
{

    public string name;



    public Sprite ItemImage; 





}
public class collector : MonoBehaviour
{
    public item item;
    public float healtchChange;
    public float moveSpeed;
    public float AttackSpeed;
    public float bulletSize;


    // Start is called before the first frame update
    void Start()
    {
        // Set the sprite
        GetComponent<SpriteRenderer>().sprite = item.ItemImage;

        // Destroy the old PolygonCollider2D component
        Destroy(GetComponent<PolygonCollider2D>());

        // Add a new PolygonCollider2D component
        gameObject.AddComponent<PolygonCollider2D>();

        // Set isTrigger property to true on the new PolygonCollider2D
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameController.HealPlayer(healtchChange);
            GameController.movespeed(moveSpeed);
            GameController.firerete(AttackSpeed);
            GameController.bulet(bulletSize);

           
            Player.amount++;
        }
    }
}
