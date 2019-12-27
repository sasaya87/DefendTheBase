using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchRange : MonoBehaviour
{
    public bool setTarget; //ターゲットが設定されているか否か
    private GameObject target; //ターゲット

    // Start is called before the first frame update
    void Start()
    {
        setTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!setTarget)
        {
            target = collision.gameObject;
            setTarget = true;
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            this.transform.parent.rotation = targetRotation;
        }
        else
        {
            this.transform.parent.GetComponent<Turret>().CoShot();
        }
    }
}
