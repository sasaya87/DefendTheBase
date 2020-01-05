using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public bool canShot; //発砲許可
    public float coolTime; //発砲間隔
    public int shotsNum; //連射数
    public float turningSpeed; //旋回速度
    private bool isRotating; //タレットが回転中か否か
    public float accuracy; //射撃精度（目標からの角度のずれ幅の最大値）


    // Start is called before the first frame update
    void Start()
    {
        canShot = true;
        isRotating = false;
        GameManager.turretList.Add(this.transform.parent.parent.gameObject); //タレット生成時にGamaManagerのturretListにタレットベースを追加
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public IEnumerator Shot()
    {
        while (true) {
            if (!isRotating) { //タレットが回転中でなければ射撃開始
                for (int i = 0; i <= shotsNum; i++) {
                    GameObject bulletObj = Instantiate(bullet, transform.position, transform.rotation, transform);
                    bulletObj.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, Random.Range(-accuracy, accuracy)) * transform.up.normalized * bulletObj.GetComponent<Bullet>().speed;
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(coolTime); //連射後のクールタイム
                this.transform.GetChild(1).GetComponent<EnemySearchRange>().setTarget = false; //ターゲットを未セットにしてコルーチン終了
                yield break;
            }
            yield return null; //タレットが回転中の間は射撃しない.毎フレーム回転停止判定を行う
        }
    }

    public IEnumerator Rotate(GameObject target) //targetの方を向くまで一定速度で回転
    {
        while (Vector2.Angle(target.transform.position - this.transform.parent.position, transform.up) > 5f)
        {
            this.transform.parent.GetComponent<Rigidbody2D>().angularVelocity = (Vector2.SignedAngle(transform.up, target.transform.position - this.transform.parent.position) < 0 ? -1f : 1f) * turningSpeed;
            isRotating = true;
            yield return null;
        }
        //回転の停止処理を行い、コルーチン終了
        this.transform.parent.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        this.isRotating = false;
        yield break;
    }

}
