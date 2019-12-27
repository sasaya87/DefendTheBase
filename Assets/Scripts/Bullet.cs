using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed; //弾速

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.parent.up.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
