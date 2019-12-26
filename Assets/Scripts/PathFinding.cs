using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private List<Vector2> pathList = new List<Vector2>(); //最終的な経路のリスト
    public static Tile[,] Tiles; //タイル情報.これをこのスクリプトから書き換えてはいけない（元のTilesの情報まで変わってしまうため）
    public static Tile goalTile; //ゴールのタイル情報.基本的には全てのエイリアンで共有
    private int nodeIndex; //現在の出発点のノードのインデックス. nodeIndex + 1 が現在の目的地

    //A*関連
    private List<DirectedNode> closedList = new List<DirectedNode>(); //処理済みのノードが入っているリスト
    private List<DirectedNode> openList = new List<DirectedNode>(); //これから処理をするノードが入っているリスト
    private DirectedNode[,] tileNodes; //マップ全体のノードが入っている配列（このインスタンス専用）


    // Start is called before the first frame update
    void Start()
    {
        //Tiles = FindObjectOfType<MapGenerator>().Tiles; //タイル情報の読み込み
        nodeIndex = 0;
        ResetTileNodes(); //tileNodesのデータを初期化

        //openListに最初のノードを付け加えたうえでAstarPathFind実行
        //startNodeの設定
        Tile startTile = this.GetComponent<Enemy>().hive.transform.parent.gameObject.GetComponent<Tile>();
        DirectedNode startNode = tileNodes[startTile.i, startTile.j];
        startNode.ActualCost = 0;
        //openListにstartNodeを付け加える
        openList.Add(startNode);
        startNode.isOpen = true;
        //goalNodeの設定.基地を目標地点とする
        //Tile goalTile = GameObject.Find("base").transform.parent.gameObject.GetComponent<Tile>();
        DirectedNode goalNode = tileNodes[goalTile.i, goalTile.j];
        //A*法実行
        AstarPathFind(startNode, goalNode);
        //goalNodeを引数に経路のベクトルリストを作る.この時点では逆順になっていることに注意
        GetPathVectorList(goalNode);
        //Debug.Log("start"+pathList.Count);
        pathList.Reverse();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nodeIndex == pathList.Count - 1){
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }else{
            Vector2 direction = (pathList[nodeIndex+1] - (Vector2)transform.position).normalized; 
            GetComponent<Rigidbody2D>().velocity = direction * GetComponent<Enemy>().speed;
            if(Vector2.Distance(transform.position, pathList[nodeIndex+1]) <= 0.1){
                nodeIndex++;
            }
        }
        
    }

    //tileNodes初期化.必ずTilesの情報を読み込んでから行うこと
    private void ResetTileNodes()
    {
        tileNodes = new DirectedNode[Tiles.GetLength(0), Tiles.GetLength(1)];
        
        for (int i =0; i < tileNodes.GetLength(0); i++)
        {
            for (int j = 0; j < tileNodes.GetLength(1); j++)
            {
                tileNodes[i, j] = new DirectedNode(i,j);
                tileNodes[i, j].position = Tiles[i, j].gameObject.transform.position;
            }
        }
    }

    //A*アルゴリズム
    private void AstarPathFind(DirectedNode startNode, DirectedNode goalNode){
        //もしopenListに何もなければ探索失敗
        if(openList.Count <= 0){
            return;
        }
        while(openList.Count > 0){
            DirectedNode nextSearchedNode = openList[0];
            //openListのうち実コストと推定値の和が最も小さいノード探してそれをnextSearchedNodeとする
            foreach (DirectedNode node in openList)
            {
                if(nextSearchedNode.ActualCost + nextSearchedNode.Heuristics(goalNode) > node.ActualCost + node.Heuristics(goalNode)){
                    //nextSearchedNodeを新たなノードに置き換える
                    nextSearchedNode = node;
                }
            }
            //もしnextSearchedNodeがゴールノードなら終了
            if(nextSearchedNode == goalNode){
                return;
            }
            //nextSearchedNodeに隣接するノードのうち通行可能なものをopenListに付け加える
            for(int i = nextSearchedNode.i - 1; i <= nextSearchedNode.i + 1; i++){
                for(int j = nextSearchedNode.j - 1; j <= nextSearchedNode.j + 1; j++){
                    if(i >= 0 && i < Tiles.GetLength(0) && j >= 0 && j < Tiles.GetLength(1)){
                        //closedListにもopenListにも入っていないノードは新たにopenListに付け加える
                        //openListに入っているものは実コストを計算しなおしてより少ない値で更新
                        //同時に親も更新する
                        if(Tiles[i,j].isPassable ){
                            DirectedNode inspectedNode = tileNodes[i,j];
                            if (!inspectedNode.isClosed) {
                                //ノード間のコストの計算.斜め移動はルート2,縦横は1
                                float nToNCost;
                                if (i != inspectedNode.i && j != inspectedNode.j) {
                                    nToNCost = Mathf.Sqrt(2);
                                } else {
                                    nToNCost = 1;
                                }

                                if (!inspectedNode.isOpen) {
                                    openList.Add(inspectedNode);
                                    inspectedNode.isOpen = true;
                                    inspectedNode.ActualCost = nextSearchedNode.ActualCost + nToNCost;
                                    inspectedNode.ParentNode = nextSearchedNode;
                                } else {
                                    if (inspectedNode.ActualCost > nextSearchedNode.ActualCost + nToNCost) {
                                        inspectedNode.ActualCost = nextSearchedNode.ActualCost + nToNCost;
                                        inspectedNode.ParentNode = nextSearchedNode;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //nextSearchedNodeをclosedListに移し、openListから削除する
            nextSearchedNode.isClosed = true;
            openList.Remove(nextSearchedNode);
            closedList.Add(nextSearchedNode);
            
        }
    }

    //ゴールからノードをたどってベクトルのリストとしてpathListに書き込む
    private void GetPathVectorList(DirectedNode goalNode){
        pathList.Add(goalNode.position);
        if(goalNode.ParentNode != null){
            GetPathVectorList(goalNode.ParentNode);
        }
    }


}
