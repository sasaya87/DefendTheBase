using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject hive; //このエイリアンを生産した巣
    private int nodeIndex; //現在の出発点のノードのインデックス. nodeIndex + 1 が現在の目的地
    public List<Vector2> pathList; //経路の入っているリスト

    public bool isKnocked; //ノックバック中か否か
    public GameObject attackEffect; //攻撃エフェクト
    public float attackPoint; //攻撃力
    public float attackInterval; //攻撃間隔

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
        if (nodeIndex == pathList.Count - 1)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            //Vector2 direction = (pathList[nodeIndex + 1] - (Vector2)transform.position).normalized;
            //GetComponent<Rigidbody2D>().velocity = direction * GetComponent<Character>().speed;
            Move(transform.position, pathList[nodeIndex + 1]);
            if (Vector2.Distance(transform.position, pathList[nodeIndex + 1]) <= 0.1)
            {
                nodeIndex++;
            }
        }
    }

    private void Move(Vector2 fromPos, Vector2 toPos)
    {
        Vector2 direction = (toPos - fromPos).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * (GetPassCost(transform.position) * GetComponent<Character>().speed);
        //進む向きによってSpriteを左右反転させる
        Vector3 scale = transform.localScale;
        scale.x = GetComponent<Rigidbody2D>().velocity.x < 0 ? 1 : -1;
        transform.localScale = scale;
    }

    private float GetPassCost(Vector2 objPos)
    {
        Tile currentTile = MapGenerator.Tiles[(int)(objPos.y + MapGenerator.Height / 2f + 0.5f), (int)(objPos.x + MapGenerator.Width / 2f + 0.5f)];
        if (currentTile.transform.childCount == 0 || currentTile.transform.GetChild(0).GetComponent<Item>().passCost == 0)
        {
            return 1f;
        }
        else
        {
            return currentTile.transform.GetChild(0).GetComponent<Item>().passCost;
        }
    }

    private void Attack(GameObject targetItem)
    {
        targetItem.GetComponent<Item>().Damaged(this.attackPoint);
        GameObject attackObj = Instantiate(attackEffect, (transform.position - targetItem.transform.position) / 2f, transform.rotation); //攻撃エフェクト生成
        Destroy(attackObj, attackObj.GetComponent<ParticleSystem>().main.duration); //継続時間が終わればエフェクトを削除
    }
}
