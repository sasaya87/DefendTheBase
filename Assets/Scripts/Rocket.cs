using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed; //弾速

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //一瞬だけ待ってから発射
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().velocity = transform.parent.up.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
