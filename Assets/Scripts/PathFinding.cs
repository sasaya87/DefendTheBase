using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static Tile[,] Tiles; //タイル情報.これをこのスクリプトから書き換えてはいけない（元のTilesの情報まで変わってしまうため）
    public static Tile goalTile; //ゴールのタイル情報.基本的には全てのエイリアンで共有

    //A*関連
    private List<DirectedNode> openList = new List<DirectedNode>(); //これから処理をするノードが入っているリスト
    private DirectedNode[,] tileNodes; //マップ全体のノードが入っている配列（このインスタンス専用）

    private bool AstarIsSuspended = false; //A*が途中で打ち切られか否か


    // Start is called before the first frame update
    void Start()
    {
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
        
        if (!AstarIsSuspended)
        {
            TraceAncestors(goalNode);
        }
        GetComponent<Enemy>().startNode = MapManager.tileNodes[startNode.i, startNode.j];
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //もしnectSearchedNodeがすでに他のenemyが探索した経路と被っていれば探索を打ち切ってその結果を利用する
            if (MapManager.tileNodes[nextSearchedNode.i, nextSearchedNode.j].ChildNode != null)
            {
                TraceAncestors(nextSearchedNode);
                AstarIsSuspended = true;
                return;
            }
            //nextSearchedNodeに隣接するノードのうち通行可能なものをopenListに付け加える
            for (int i = nextSearchedNode.i - 1; i <= nextSearchedNode.i + 1; i++){
                for(int j = nextSearchedNode.j - 1; j <= nextSearchedNode.j + 1; j++){
                    if(i >= 0 && i < Tiles.GetLength(0) && j >= 0 && j < Tiles.GetLength(1)){
                        //closedListにもopenListにも入っていないノードは新たにopenListに付け加える
                        //openListに入っているものは実コストを計算しなおしてより少ない値で更新
                        //同時に親も更新する
                        if(Tiles[i,j].isPassable ){
                            //今から調べるノード
                            DirectedNode inspectedNode = tileNodes[i,j];
                            //ノード間のコストの計算.斜め移動はルート2,縦横は1
                            float nToNCost;
                            if (i != inspectedNode.i && j != inspectedNode.j)
                            {
                                nToNCost = 1.414214f;
                            }
                            else
                            {
                                nToNCost = 1f;
                            }

                            if (inspectedNode.isClosed) //ノードがclosedの場合
                            {
                                //そのノード(inspentedNode)の今の実コストとnextSeachedNodeを経由した場合の実コストを比較して、
                                //それが今の実コストより小さければ新しい値で更新し、親も付け替える
                                //また、inspectedNodeをopenListに復帰させる
                                if (inspectedNode.ActualCost > nextSearchedNode.ActualCost + nToNCost)
                                {
                                    inspectedNode.ActualCost = nextSearchedNode.ActualCost + nToNCost;
                                    inspectedNode.ParentNode = nextSearchedNode;
                                    inspectedNode.isClosed = false;
                                    inspectedNode.isOpen = true;
                                    openList.Add(inspectedNode);
                                }
                            } else{ //ノードがclosedでない場合
                                if (!inspectedNode.isOpen) { //ノードが未openの場合はopenする
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
            
        }
    }

    //ゴールから親をたどっていき、MapManagerのtileNodesに子ノードの情報を登録
    private void TraceAncestors(DirectedNode goalNode)
    {
        if (goalNode.ParentNode != null)
        {
            MapManager.tileNodes[goalNode.ParentNode.i, goalNode.ParentNode.j].ChildNode = MapManager.tileNodes[goalNode.i, goalNode.j];
            TraceAncestors(goalNode.ParentNode);
        }
    }


}
