using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public GameObject heart;
    private float fillValue; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillValue = (float)GameController.Health;
        fillValue = fillValue / GameController.MaxHealth;
        heart.GetComponent<Image>().fillAmount = fillValue; 

    }
}
