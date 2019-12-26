using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject tile;
    
    // Start is called before the first frame update
    void Start()
    {
        tile = this.transform.parent.gameObject;
    }
}
