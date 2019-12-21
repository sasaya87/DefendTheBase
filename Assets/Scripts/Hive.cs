using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : Spawn
{
    public GameObject Alien1;
    public GameObject Alien2;
    public float aln1Ratio;
    public float genInterval;

    IEnumerator Start ()
	{
		while (true) {
            if(Random.value < aln1Ratio){
                Instantiate (Alien1, transform.position, transform.rotation);
            }else{
                Instantiate (Alien2, transform.position, transform.rotation);
            }

			// genInterval秒待つ
			yield return new WaitForSeconds (genInterval);
		}
	}

}
