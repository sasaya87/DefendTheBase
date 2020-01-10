using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float maxHP;
    public float HP { get; set; }
    public float passCost; //通過するのに必要なコスト（speedにpassCostをかけたものが実際の速さ）
    
    // Start is called before the first frame update
    void Start()
    {
        this.HP = this.maxHP;
    }

    public void Damaged(float damage)
    {
        this.HP = this.HP - damage;

        if (this.HP < 0)
        {
            GameManager.LateDestroy(this.gameObject);
        }
    }
}
