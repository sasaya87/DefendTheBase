using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float coolTime; //発砲間隔
    public float turningSpeed; //旋回速度
    private bool isRotating; //タレットが回転中か否か
    public GameObject rocket; //発射するロケット

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shot()
    {
        while (true)
        {
            if (!isRotating)
            { //タレットが回転中でなければ射撃開始
                for (int i =0; i < transform.Find("ShotPosition").childCount; i++)
                {

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
        while (Vector2.Angle(target.transform.position - this.transform.position, transform.up.normalized) > 5f)
        {
            this.GetComponent<Rigidbody2D>().angularVelocity = (Vector2.SignedAngle(transform.up.normalized, target.transform.position - this.transform.position) < 0 ? -1f : 1f) * turningSpeed;
            isRotating = true;
            yield return null;
        }
        //回転の停止処理を行い、コルーチン終了
        this.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        this.isRotating = false;
        yield break;
    }
}
