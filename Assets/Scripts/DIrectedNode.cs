using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectedNode {
  public DirectedNode Parent {get; set;}
  public DirectedNode Child {get; set;}
  public Vector2 position {get; set;}
}
