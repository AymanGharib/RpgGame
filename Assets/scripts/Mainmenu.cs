using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import the UnityEngine.UI namespace for UI elements

public class Mainmenu : MonoBehaviour
{
    public Button Restart;
    public Button level2 = null;

    private void Start()
    {
        // Add click event listeners to the buttons
        Restart.onClick.AddListener(SelectDijkstra);
        level2.onClick.AddListener(SelectAStar);
    }

    // Method called when Dijkstra button is clicked
    private void SelectDijkstra()
    {
        // Store the selected algorithm (e.g., Dijkstra) in PlayerPrefs
       // PlayerPrefs.SetString("SelectedAlgorithm", "Dijkstra");
        // Load the gameplay scene
        SceneManager.LoadScene("SampleScene");
    }

    // Method called when A* button is clicked
    private void SelectAStar()
    {
        // Store the selected algorithm (e.g., A*) in PlayerPrefs


        /*
         
       
    

         
         
         
         
         */
       // PlayerPrefs.SetString("SelectedAlgorithm", "AStar");
        // Load the gameplay scene
        SceneManager.LoadScene("level2");

    }
}
