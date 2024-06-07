using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner<T>
{
    void Spawn(T type, Vector3 position);
}
public class EnemySpawner : ISpawner<int>
{
    private GameObject enemy1;
    private GameObject enemy2;

    public EnemySpawner(GameObject enemy1Prefab, GameObject enemy2Prefab)
    {
        enemy1 = enemy1Prefab;
        enemy2 = enemy2Prefab;
    }

    public void Spawn(int type, Vector3 position)
    {
        if (type == 1)
        {
            GameObject.Instantiate(enemy1, position, Quaternion.identity);
        }
        else if (type == 2)
        {
            GameObject.Instantiate(enemy2, position, Quaternion.identity);
        }
    }
}

public class ItemSpawner : ISpawner<int>
{
    private GameObject item1;
    private GameObject item2;
    private GameObject item3;

    public ItemSpawner(GameObject item1Prefab, GameObject item2Prefab, GameObject item3Prefab)
    {
        item1 = item1Prefab;
        item2 = item2Prefab;
        item3 = item3Prefab;
    }

    public void Spawn(int type, Vector3 position)
    {
        if (type == 1)
        {
            GameObject.Instantiate(item1, position, Quaternion.identity);
        }
        else if (type == 2)
        {
            GameObject.Instantiate(item2, position, Quaternion.identity);
        }
        else if (type == 3)
        {
            GameObject.Instantiate(item3, position, Quaternion.identity);
        }
    }
}
