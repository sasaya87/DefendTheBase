using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    public GameObject Alien1;
    public GameObject Alien2;
    public float aln1ratio;
    public float genInterval;

    IEnumerator Start ()
	{
		while (true) {
            if(Random.value < aln1ratio){
                GameObject alien1 = Instantiate (Alien1, transform.position, transform.rotation);
                alien1.GetComponent<Enemy>().hive = this.gameObject;
            }else{
                GameObject alien2 = Instantiate(Alien2, transform.position, transform.rotation);
                alien2.GetComponent<Enemy>().hive = this.gameObject;
            }

			// genInterval秒待つ
			yield return new WaitForSeconds (genInterval);
		}
	}

}
