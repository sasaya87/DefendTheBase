using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject itemCategoryPanel;

    private bool isPicking; //アイテムをピッキング中であるか否か
    GameObject pickedItem; //ピック中のアイテム表示用スプライト
    GameObject cameraManager; //CameraMnager

    // Start is called before the first frame update
    void Start()
    {
        itemCategoryPanel.SetActive(false);
        isPicking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPicking)
        {
            if (Input.GetMouseButton(0)) //ピック中かつ左クリックでそのタイルにアイテム生成
            {
                //指定の位置から発射されるRayを作成
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Rayとオブジェクトの接触を調べる
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f, LayerMask.GetMask("Ground"));

                //接触してたらアイテムを生成
                if (hit2d)
                {
                    GameObject clickedGameObject = hit2d.transform.gameObject;
                    GameObject generatedItem = (GameObject)Resources.Load(pickedItem.name);
                    Instantiate(generatedItem, clickedGameObject.transform.position, clickedGameObject.transform.rotation, clickedGameObject.transform);

                }
                pickedItem.SetActive(false);
                isPicking = false;

            } else if (Input.GetMouseButton(1)) //ピック中かつ右クリックでピック中止
            {
                pickedItem.SetActive(false);
                isPicking = false;
            }
        }
    }

    public void OpenItemCategoryPanel()
    {
        if (itemCategoryPanel.activeSelf)
        {
            itemCategoryPanel.SetActive(false);
        }
        else
        {
            itemCategoryPanel.SetActive(true);
        }
    }

    public void SelectItemCategory(GameObject itemIcon)
    {
        pickedItem = itemIcon;
        pickedItem.SetActive(true);
        Debug.Log(pickedItem.name);
        isPicking = true;
        itemCategoryPanel.SetActive(false);
    }
}
