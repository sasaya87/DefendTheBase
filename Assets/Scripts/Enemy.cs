using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject hive; //このエイリアンを生産した巣
    private int nodeIndex; //現在の出発点のノードのインデックス. nodeIndex + 1 が現在の目的地
    public List<Vector2> pathList;

    public bool isKnocked; //ノックバック中か否か

    // Start is called before the first frame update
    void Start()
    {
        nodeIndex = 0;
        isKnocked = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnocked)
        {
            if (this.GetComponent<Rigidbody2D>().velocity.magnitude < 0.5)
            {
                this.isKnocked = false;
            }
        }
        else
        {
            TracePath();
        }
    }

    private void TracePath()
    {
        if (nodeIndex == pathList.Count - 1)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            Vector2 direction = (pathList[nodeIndex + 1] - (Vector2)transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = direction * GetComponent<Character>().speed;
            if (Vector2.Distance(transform.position, pathList[nodeIndex + 1]) <= 0.1)
            {
                nodeIndex++;
            }
        }
    }
}
