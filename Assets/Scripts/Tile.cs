using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isPassable; //通行可能か否か
    public bool isVacant; //目的に設定できるか否か
    public int i;
    public int j;

    void Awake(){
        this.isPassable = true;
        this.isVacant = true;
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
        if (this.transform.GetChild(0) && this.transform.GetChild(0).gameObject.tag == "Obstacle")
        {
            this.isPassable = false;
            this.isVacant = false;
        } else if(this.transform.GetChild(0) && (this.transform.GetChild(0).gameObject.tag == "Character" || this.transform.GetChild(0).gameObject.tag == "PassableItem"))
        {
            this.isPassable = true;
            this.isVacant = false;
        } else {
            this.isPassable = true;
            this.isVacant = true;
        }
    }

}
