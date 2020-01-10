using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject hive; //このエイリアンを生産した巣

    public bool isKnocked; //ノックバック中か否か
    public GameObject attackEffect; //攻撃エフェクト
    public float attackPoint; //攻撃力
    public float attackInterval; //攻撃間隔

    private Tile currentTile; //今いるタイル
    private DirectedNode fromNode; //今現在の出発地点であるノード（MapManagerのtileNodes）
    public DirectedNode startNode; //スタート地点のノード（MapManagerのtileNodes）

    // Start is called before the first frame update
    void Start()
    {
        isKnocked = false;
        fromNode = startNode;
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTile = MapGenerator.Tiles[(int)(transform.position.y + MapGenerator.Height / 2f + 0.5f), (int)(transform.position.x + MapGenerator.Width / 2f + 0.5f)];
        if (isKnocked)
        {
            if (this.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1)
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
        if (fromNode.ChildNode == null)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            Move(transform.position, fromNode.ChildNode.position);
            if (Vector2.Distance(transform.position, fromNode.ChildNode.position) <= 0.1)
            {
                fromNode = fromNode.ChildNode;
            }
        }
    }

    private void Move(Vector2 fromPos, Vector2 toPos)
    {
        Vector2 direction = (toPos - fromPos).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * (GetPassCost() * GetComponent<Character>().speed);
        //進む向きによってSpriteを左右反転させる
        Vector3 scale = transform.localScale;
        scale.x = GetComponent<Rigidbody2D>().velocity.x < 0 ? 1 : -1;
        transform.localScale = scale;
    }

    private float GetPassCost()
    {
        if (currentTile.transform.childCount == 0 || currentTile.transform.GetChild(0).GetComponent<Item>().passCost == 0)
        {
            return 1f;
        }
        else
        {
            return currentTile.transform.GetChild(0).GetComponent<Item>().passCost;
        }
    }

    IEnumerator Attack(GameObject targetItem)
    {
        while (targetItem.GetComponent<Item>().HP > 0) {
            Beat(targetItem);
            yield return new WaitForSeconds(attackInterval);
        }
    }
    
    private void Beat(GameObject targetItem) //1回の攻撃
    {
        targetItem.GetComponent<Item>().Damaged(this.attackPoint);
        GameObject attackObj = Instantiate(attackEffect, (transform.position - targetItem.transform.position) / 2f, transform.rotation); //攻撃エフェクト生成
        Destroy(attackObj, attackObj.GetComponent<ParticleSystem>().main.duration); //継続時間が終わればエフェクトを削除
    }
}
