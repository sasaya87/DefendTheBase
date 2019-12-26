﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedNode {
    //ノードのインデックス座標
    public int i;
    public int j;
    //ノードのxy座標位置
    public Vector2 position;
    //このノードの親ノード（スタートにより近いノード）
    public DirectedNode ParentNode {get; set;}

    //A*関連パラメータ
    public bool isOpen = false;
    public bool isClosed = false;
    //startNodeからこのノードまでの実際のコスト
    public float ActualCost = Mathf.Infinity;
    //goalNodeを引数としてこのノードの評価値を返す
    public float Heuristics(DirectedNode goalNode){
        return Vector2.Distance(this.position, goalNode.position);
    }

    //コンストラクタ
    public DirectedNode(int i, int j)
    {
        this.i = i;
        this.j = j;
    }
}
