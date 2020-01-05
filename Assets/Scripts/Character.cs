using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float maxHP;
    private float HP;
    public float speed; //移動の速さ
    public GameObject splash; //血しぶき

    private Slider hpSlider; //HP表示用スライダー
    private SpriteRenderer charaRenderer;
    private bool isRunnig; //被ダメージコルーチンが続行中か否か

    public void Damaged(float damage, Vector2 dmgDirection)
    {
        this.HP = this.HP - damage;

        if (this.HP < 0)
        {
            GameManager.LateDestroy(gameObject);
        }

        hpSlider.value = this.HP / maxHP;
        //this.GetComponent<Rigidbody2D>().AddForce(dmgDirection.normalized, ForceMode2D.Impulse); //ノックバック
        StartCoroutine(Knocked(dmgDirection));
        this.GetComponent<Enemy>().isKnocked = true;
        if (!isRunnig) //現在他のCoDamagedコルーチンが実行中でない場合のみ被ダメージ表現コルーチンを実行
        {
            StartCoroutine("CoDamaged");
        }

    }

    //ノックバック処理
    IEnumerator Knocked(Vector2 dmgDirection)
    {
        Debug.Log("knocked");
        float knockedTime = 0f;
        while (knockedTime < Mathf.PI / 6f)
        {
            knockedTime = knockedTime + 0.05f;
            float level = 0.5f * Mathf.Abs(Mathf.Cos(knockedTime * 3));
            this.GetComponent<Rigidbody2D>().velocity =level * dmgDirection.normalized;
            yield return new WaitForSeconds(0.05f);
        }
    }

    //被ダメージ時に赤くなる表現＋血しぶきエフェクト
    IEnumerator CoDamaged()
    {
        isRunnig = true;
        float damagedTime = 0f;
        GameObject splashObj = Instantiate(splash, transform.position, transform.rotation); //血しぶきエフェクト生成
        Destroy(splashObj, splashObj.GetComponent<ParticleSystem>().main.duration); //継続時間が終わればエフェクトを削除
        while (damagedTime < Mathf.PI / 6f)
        {
            damagedTime = damagedTime + 0.1f;
            float level = Mathf.Abs(Mathf.Sin(damagedTime * 3));
            charaRenderer.color = new Color(1f, level, level);
            yield return new WaitForSeconds(0.1f);
        }
        charaRenderer.color = Color.white; //コルーチン終了前にスプライトの色を元に戻す
        isRunnig = false; 
        yield break;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        hpSlider = transform.Find("HPUI/HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
        this.HP = maxHP;
        charaRenderer = GetComponent<SpriteRenderer>();
        isRunnig = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
