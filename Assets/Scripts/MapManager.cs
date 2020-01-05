using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    GameObject clickedGameObject;
    public GameObject turret;
    public GameObject rotaryturret;
    public GameObject wire;
    public static DirectedNode[,] tileNodes;

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("MouseDown");

        if (Input.GetMouseButtonDown(1))
        {
            

            clickedGameObject = null;

            //指定の位置から発射されるRayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Rayとオブジェクトの接触を調べる
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f, LayerMask.GetMask("Ground"));

            //接触してたらTurretを生成
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                GameObject turretObj = Instantiate(turret, clickedGameObject.transform.position, clickedGameObject.transform.rotation, clickedGameObject.transform);

            }

            Debug.Log(clickedGameObject);
        }
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Mouse");
            clickedGameObject = null;

            //指定の位置から発射されるRayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Rayとオブジェクトの接触を調べる
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f, LayerMask.GetMask("Ground"));

            //接触してたらTurretを生成
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                GameObject rotaryturretObj = Instantiate(rotaryturret, clickedGameObject.transform.position, clickedGameObject.transform.rotation, clickedGameObject.transform);

            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse");
            clickedGameObject = null;

            //指定の位置から発射されるRayを作成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Rayとオブジェクトの接触を調べる
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f, LayerMask.GetMask("Ground"));

            //接触してたらwireを生成
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
                GameObject rotaryturretObj = Instantiate(wire, clickedGameObject.transform.position, clickedGameObject.transform.rotation, clickedGameObject.transform);

            }
        }
    }

}
