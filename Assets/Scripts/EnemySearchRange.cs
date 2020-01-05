using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchRange : MonoBehaviour
{
    public bool setTarget; //ターゲットが設定されているか否か
    public List<GameObject> targetList = new List<GameObject>(); //索敵範囲内にいるターゲットのリスト

    // Start is called before the first frame update
    void Start()
    {
        setTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!setTarget && targetList.Count > 0) //ターゲットがセットされておらず、かつ索敵範囲内に敵がいる場合
        {
            GameObject target = targetList[Random.Range(0, targetList.Count)]; //ターゲットをランダムに選択
            setTarget = true;
            StartCoroutine(this.transform.parent.GetComponent<Turret>().Rotate(target));
            StartCoroutine(this.transform.parent.GetComponent<Turret>().Shot());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        targetList.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targetList.Remove(collision.gameObject);
    }
}
