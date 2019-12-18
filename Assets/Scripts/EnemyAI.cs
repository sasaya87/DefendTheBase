using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int pathreplim;
    List<DirectedNode> pathNodes = new List<DirectedNode>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //startからgoalまでの経路を取得
    private void GetPath(Vector2 start, Vector2 goal){
        RaycastHit2D[] hit = Physics2D.RaycastAll(start, goal - start);
        if(hit.Length != 0){
            DirectedNode startNode = new DirectedNode();
            startNode.position = start;
            DirectedNode goalNode = new DirectedNode();
            goalNode.position = goal;
        }
    }
    
    //targetを攻撃
    private void Attack(GameObject target){
        
    }
}
