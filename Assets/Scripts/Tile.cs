using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isPassable;
    public int i;
    public int j;

    void Awake(){
        this.isPassable = true;
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTransformChildrenChanged(){
        if(this.transform.GetChild(0) && this.transform.GetChild(0).gameObject.tag == "Obstacle"){
            this.isPassable = false;
        }else{
            this.isPassable = true;
        }
    }

}
