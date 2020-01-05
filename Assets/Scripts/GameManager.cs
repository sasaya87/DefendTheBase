using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<GameObject> turretList = new List<GameObject>(); //マップ上に存在するタレットベースのリスト
    public static List<GameObject> launcherList = new List<GameObject>(); //マップ上に存在するロケットランチャーベースのリスト

    private static List<GameObject> lateDestroyedEnemies = new List<GameObject>(); //このフレームの最後でまとめてDestroyする敵のリスト
    private static List<GameObject> lateDestroyedItems = new List<GameObject>(); //このフレームの最後でまとめてDestroyするアイテムのリスト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LateDestroy(GameObject lateDestroyedObj)
    {
        if (lateDestroyedObj.tag == "Enemy") {
            lateDestroyedEnemies.Add(lateDestroyedObj);
        }
        else
        {
            lateDestroyedItems.Add(lateDestroyedObj);
        }
    }
    private void LateUpdate() //フレームの最後にDestroy関連の処理
    {
        foreach (GameObject enemy in lateDestroyedEnemies)
        {
            foreach (GameObject turret in turretList)
            {
                turret.transform.Find("RotatingCenter").GetChild(0).Find("EnemySearchRange").GetComponent<EnemySearchRange>().targetList.Remove(enemy);
            }
            Destroy(enemy);
        }
        lateDestroyedEnemies.Clear();
        lateDestroyedItems.Clear();
    }
}
