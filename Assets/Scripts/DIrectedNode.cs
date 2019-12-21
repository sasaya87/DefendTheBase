using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedNode {
  private DirectedNode childNode;
  private DirectedNode parentNode;
  
  public DirectedNode ParentNode {
    set {
      this.parentNode = value;
      value.ChildNode = this;
    }
    get { return parentNode; }
  }

  public DirectedNode ChildNode {
    set {
      this.childNode = value;
      value.ParentNode = this;
    }
    get { return childNode; }
  }
  
  public Vector2 position;
  public DirectedNode(Vector2 v){
    this.position = v;
  }
}
