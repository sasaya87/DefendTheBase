using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private float HP;
    public float speed; //移動の速さ
    public GameObject splash1; //血しぶき
    public GameObject splash2;
    public GameObject splash3;

    private Slider hpSlider; //HP表示用スライダー

    public void Damaged(float damage, Vector2 dmgDirection)
    {
        this.HP = this.HP - damage;

        if (this.HP < 0)
        {
            Destroy(gameObject);
        }

        this.GetComponent<Rigidbody2D>().AddForce(-dmgDirection, ForceMode2D.Impulse);
        this.GetComponent<Enemy>().isKnocked = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        hpSlider = transform.GetChild(0).Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
