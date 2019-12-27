using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public bool canShot; //発砲許可
    public float coolTime; //発砲間隔
    public int shotsNum; //連射数


    // Start is called before the first frame update
    void Start()
    {
        canShot = true;
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    IEnumerator Shot()
    {
        for(int i = 0; i <= shotsNum; i++){
            Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }
        this.transform.GetChild(1).GetComponent<EnemySearchRange>().setTarget = false;
        yield break;
    }

    public void CoShot() //外部からShotコルーチンを呼ぶためのメソッド
    {
        StartCoroutine("Shot");
    }
}
