using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    List<Vector2> pathList = new List<Vector2>();
    GameObject[,] Tiles;
    Vector2 start; //経路の1辺のスタート位置
    Vector2 goal; //経路の1辺のゴール位置
    private int nodeIndex = 0;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[,] Tiles = FindObjectOfType<MapGenerator>().Tiles;
        DirectedNode startNode = new DirectedNode(transform.position);
        DirectedNode goalNode = new DirectedNode(new Vector2(0, 0));
        GetNode(startNode, goalNode);
        GetPath(startNode, pathList);
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(nodeIndex == pathList.Count - 1){
            gameObject.GetComponent<Character>().speed = 0;
        }else{
            Vector2 direction = (pathList[nodeIndex+1] - pathList[nodeIndex]).normalized; 
            GetComponent<Rigidbody2D>().velocity = direction * gameObject.GetComponent<Character>().speed;
            if(Vector2.Distance(transform.position, pathList[nodeIndex+1]) <= 1){
                nodeIndex++;
            }
        }
        
    }

    //引数のタイルの周囲のタイルのうち通行可能なタイルの位置をランダムで返す
    private Vector2 GetVacantTilePos(GameObject tile){
        List<GameObject> vacantTiles = new List<GameObject>();
        for(int i = tile.GetComponent<Tile>().i - 1; i <= tile.GetComponent<Tile>().i + 1; i++){
            for(int j = tile.GetComponent<Tile>().j - 1; j <= tile.GetComponent<Tile>().j + 1; j++){
                if(Tiles[i,j].GetComponent<Tile>().isPassable == true){
                    vacantTiles.Add(Tiles[i,j]);
                }
            }
        }
        return vacantTiles[Random.Range(0,vacantTiles.Count)].transform.position;
    }


    //RaycastHit2D[]を引数として連続している最初の部分だけを返す
    private RaycastHit2D[] GetConsecutivePart(RaycastHit2D[] hit){
        Debug.Log(hit.Length);
        RaycastHit2D[] newhit = new RaycastHit2D[0];
        Debug.Log(newhit.Length);
        //Debug.Log(hit[0].transform.parent.gameObject.name);
            for(int i = 0; i < hit.Length - 1; i++){
                //Debug.Log("newhitLength="+newhit.Length);
                Debug.Log(hit[i].transform);
                if (hit[i].transform.parent.gameObject.GetComponent<Tile>().IsNextTo(hit[i+1].transform.parent.gameObject)){
                    Debug.Log("newhitLength="+newhit.Length);
                    System.Array.Resize(ref newhit, newhit.Length + 1);
                    newhit[newhit.Length - 1] = hit[i];
                    //Debug.Log("newhitLength="+newhit.Length);
                }else{
                    break;
                }
            }
        return newhit;
    }


    //startからgoalまでのノードを取得。取得できなければfalseを返す
    private bool GetNode(DirectedNode startNode, DirectedNode goalNode){
        Vector2 rayDirection = goalNode.position - startNode.position;
        //startからgoalに向かってRaycastAll実行
        RaycastHit2D[] hitAll = Physics2D.RaycastAll(startNode.position, rayDirection, Mathf.Infinity, layerMask);
        //最初に連続している部分のみを取り出す
        RaycastHit2D[] hit = GetConsecutivePart(hitAll);
        //光の発射方向のフラグ
        int flag = 0;
        
        if(hit.Length != 0){
            
            //通過した障害物列の真ん中あたりから直角に再度光線を発射
            RaycastHit2D[] raypathAll = Physics2D.RaycastAll(hit[hit.Length / 2].transform.position, Quaternion.Euler(0, 0, 90)*rayDirection, Mathf.Infinity, layerMask);
            //最初に連続している部分のみを取り出す
            RaycastHit2D[] raypath = GetConsecutivePart(raypathAll);
            //最後に通過した障害物の周りの空いているタイルを新たな中継ノードとする
            Vector2 vacantTilePos = GetVacantTilePos(raypath[raypath.Length - 1].transform.parent.gameObject);
            if(vacantTilePos == null){ //壁際等で空いているマスがない場合は逆方向に光を発射
                raypathAll = Physics2D.RaycastAll(hit[hit.Length / 2].transform.position, Quaternion.Euler(0, 0, -90)*rayDirection,  Mathf.Infinity, layerMask);
                raypath = GetConsecutivePart(raypathAll);
                vacantTilePos = GetVacantTilePos(raypath[raypath.Length - 1].transform.parent.gameObject);
                flag = 1;
            }
            if(vacantTilePos == null && flag == 1){
                return false;
            }
            //startNodeに子を設定
            startNode.ChildNode = new DirectedNode(vacantTilePos);
            //goalNodeに親を設定
            goalNode.ParentNode = startNode.ChildNode;
            //再度GetNodeを実行
            GetNode(startNode, startNode.ChildNode);
            GetNode(goalNode.ParentNode, goalNode);

        }

        return true;
    }

    //startNodeから順番に子をたどって経路を取得
    private void GetPath(DirectedNode startNode, List<Vector2> pList){
        pList.Add(startNode.position);
        if(startNode.ChildNode != null){
            GetPath(startNode.ChildNode, pList);
        }
    }
    
    //targetを攻撃
    private void Attack(GameObject target){
        
    }
}
