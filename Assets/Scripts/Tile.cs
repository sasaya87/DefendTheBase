using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isPassable;
    public int i;
    public int j;

    void Awake(){
        isPassable = true;
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
        if(this.transform.childCount == 0 || (this.transform.childCount != 0 && this.transform.GetChild(0).gameObject.tag != "Obstacle")){
            this.isPassable = true;
        }
        if(this.transform.GetChild(0) && this.transform.GetChild(0).gameObject.tag == "Obstacle"){
            this.isPassable = false;
        }
    }

    //タイル同士が隣接しているか否か
    public bool IsNextTo(GameObject tile){
        if(Mathf.Abs(this.i - tile.GetComponent<Tile>().i) <= 1 && Mathf.Abs(this.j - tile.GetComponent<Tile>().j) <= 1){
            return true;
        }
        return false;
    }


}
