using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed; //弾速
    public float damage; //1発当たりのダメージ量

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer); //衝突したオブジェクトのレイヤー名を取得

        if (layerName == "Enemy" || layerName == "Player" )
        {
            collision.GetComponent<Character>().Damaged(damage, GetComponent<Rigidbody2D>().velocity);
            Destroy(this.gameObject);
        }
    }
}
