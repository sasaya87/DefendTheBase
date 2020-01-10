using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    GameObject clickedGameObject;
    public GameObject turret;
    public GameObject rotaryturret;
    public GameObject wire;
    public static DirectedNode[,] tileNodes;


    void Start()
    {
        tileNodes = new DirectedNode[MapGenerator.Tiles.GetLength(0), MapGenerator.Tiles.GetLength(1)];
        for (int i = 0; i < tileNodes.GetLength(0); i++)
        {
            for (int j = 0; j < tileNodes.GetLength(1); j++)
            {
                tileNodes[i, j] = new DirectedNode(i, j);
                tileNodes[i, j].position = MapGenerator.Tiles[i, j].gameObject.transform.position;
            }
        }
    }

    void Update()
    {
        
    }

}
