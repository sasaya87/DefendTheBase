using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // マウスホイールの回転値を格納する変数
    private float scroll;
    //マウスポインターのワールド座標の位置
    private Vector3 mousePointerPos;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(MapGenerator.Width + 100, MapGenerator.Height + 100);
    }

    // Update is called once per frame
    void Update()
    {
        // マウスホイールの回転値を変数 scroll に渡す
        scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize * (1 - scroll), 3, 30);
    }

    private void OnMouseDown()
    {
        //マウスポインターの位置を設定
        mousePointerPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        //現在の位置から発射されるRayを作成
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Rayとオブジェクトの接触を調べる
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f, LayerMask.GetMask("DragArea"));

        //現在の位置から発射されるRayを作成
        Ray previousRay = Camera.main.ScreenPointToRay(mousePointerPos);
        //Rayとオブジェクトの接触を調べる
        RaycastHit2D previousHit2d = Physics2D.Raycast((Vector2)previousRay.origin, (Vector2)previousRay.direction, 10f, LayerMask.GetMask("DragArea"));

        //1つ前のフレームのマウスポインターの位置と今のマウスポインターの位置のワールド座標における差分を取得
        Vector2 dx = hit2d.point - previousHit2d.point;
        //カメラの位置情報を更新
        //カメラ位置がMap上にくるように調整する
        float x = Mathf.Clamp((Camera.main.transform.position - (Vector3)dx).x, -MapGenerator.Width * 0.5f, MapGenerator.Width * 0.5f);
        float y = Mathf.Clamp((Camera.main.transform.position - (Vector3)dx).y, -MapGenerator.Height * 0.5f, MapGenerator.Height * 0.5f);
        Camera.main.transform.position = new Vector3(x, y, Camera.main.transform.position.z);
        //マウスポインターの位置を設定
        mousePointerPos = Input.mousePosition;

    }
}
