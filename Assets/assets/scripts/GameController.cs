using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

  
    private static float health  =6 ;
    private static float maxHealth = 6;

    private static float moveSpeed  =5f;
    public static int amount =   0;
    public static int Totalamount = 0;
    public static float fireRate = 5f;
    private static float bulletsize = 0.1f;
    public  Text healthText;
    public Text amountText;
    public Text TotalamountText;


    public static float Health { get => health; set => health  =value;  }
    public static float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float Bullet { get => bulletsize; set => bulletsize = value; }

    void Start()
    {
        

        

    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "health " + health;
        amountText.text = "Amount " + amount;
        TotalamountText.text = "Total" + Totalamount; 


    }
   public void Awake()
    {
         if  (instance == null) { instance = this;
        }

    }  
   
    public static void DamagePlayer (int damage) {


        health -= damage; 




        if (Health <= 0 ) { KillPlayer();  }
    
    
    
    
    
    
    
    
    
    }

    public static void HealPlayer(float healAmount)
    {


        health = Mathf.Min(MaxHealth, health + healAmount); 

    }
    public static void KillPlayer() { }
    public static void movespeed(float speed) {

        moveSpeed += speed;
        amount++;
        Totalamount++;
       
    }

    public static void firerete(float rate)
    {

        FireRate += rate;
        
    }

    public static void bulet(float rate)
    {

        bulletsize += rate;
       

    }







}
