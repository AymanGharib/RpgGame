using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public interface IAbstractFactory
{
    void Spawn(Vector3 position  , Vector3 position1, Vector3 position2);
}




public class Enemy1Factory : IAbstractFactory
{
    private GameObject enemy1;
    private GameObject item1;
    private GameObject item2;

    public Enemy1Factory(GameObject enemy1Prefab, GameObject item1Prefab, GameObject item2Prefab)
    {
        enemy1 = enemy1Prefab;
        item1 = item1Prefab;
        item2 = item2Prefab;
    }

    public void Spawn(Vector3 position , Vector3 pos1 , Vector3 pos2)
    {
        GameObject.Instantiate(enemy1, position, Quaternion.identity);
     
        GameObject.Instantiate(item1, pos1, Quaternion.identity);
        GameObject.Instantiate(item2, pos2 , Quaternion.identity);
    }
}

public class Enemy2Factory : IAbstractFactory
{
    private GameObject enemy2;
    private GameObject item2;
    private GameObject item3;

    public Enemy2Factory(GameObject enemy2Prefab, GameObject item2Prefab, GameObject item3Prefab)
    {
        enemy2 = enemy2Prefab;
        item2 = item2Prefab;
        item3 = item3Prefab;
    }

    public void Spawn(Vector3 position, Vector3 pos1, Vector3 pos2)
    {
        GameObject.Instantiate(enemy2, position  , Quaternion.identity);
       

        GameObject.Instantiate(item2, pos1, Quaternion.identity);
        GameObject.Instantiate(item3, pos2, Quaternion.identity);
    }
}

