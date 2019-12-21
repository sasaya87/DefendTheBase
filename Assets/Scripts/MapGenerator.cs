using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int Height;
    public int Width;
    public int[,] Map;
    public float hvratio; //エイリアンの巣の生成割合
    public float mtratio;
    public int smoothrep; //smoothcntの繰り返し回数
    private int mtcnt = 0;
    private const int SOIL = 0;
    private const int MTN = 1;
    private const int BASE = 2;
    private const int HIVE1 = 3;
    private const int HIVE2 = 4;
    private const int HIVE3 = 5;
    public GameObject[,] Tiles; //タイルを格納
    public GameObject Soil;
    public GameObject Mtn;
    public GameObject Base;
    public GameObject Hive1;
    public GameObject Hive2;
    public GameObject Hive3;

    // Start is called before the first frame update
    void Start()
    {
        ResetMapData();
        CreateMapData();
        
    }

    private void ResetMapData(){

        Map = new int[Height, Width];
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                Map[i, j] = 0;
                 if (Random.value < mtratio) {
                    Map[i,j] = 1;
                    mtcnt += 1;
                }
            }
        }
    }

    private bool TwoTimesTwoTilesAreVacant(int y, int x){
        if(Map[y,x+1] == SOIL && Map[y+1,x] == SOIL && Map[y+1,x+1] == SOIL){
            return true;
        }else{
            return false;
        }
    }

    //そのタイルの近くにBASEがあるか
    private bool AreNearBase(int y, int x){
        for(int i = y-5; i <= y+5; i++){
            for(int j = x-5; j <= x+5; j++){
                if(i >= 0 && i < Height && j >= 0 && j < Width){
                    if(Map[i,j] == BASE){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void CreateMapData() {
        //マップ中央付近に基地が作れるような山岳形状になるまで繰り返す
        while(true){
            SmoothMooreCellularAutomata();
            if(CreateBase()){
                break;
            }
        }
        CreateHive();
        Tiles = new GameObject[Height, Width];
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                GameObject soilClone;
                soilClone = Instantiate(Soil, new Vector3(j - Width/2, i - Height/2, 0), Quaternion.identity);
                soilClone.GetComponent<Tile>().i = i;
                soilClone.GetComponent<Tile>().j = j;
                Tiles[i,j] = soilClone;
                if (Map[i,j] != SOIL){
                    switch(Map[i,j]){
                        case MTN:
                            GameObject mtnObj = Instantiate(Mtn, soilClone.transform.position, Quaternion.identity);
                            mtnObj.transform.parent = soilClone.transform;
                            break;
                        case BASE:
                            GameObject baseObj = Instantiate(Base, soilClone.transform.position, Quaternion.identity);
                            baseObj.transform.parent = soilClone.transform;
                            break;
                        case HIVE1:
                            GameObject hive1Obj = Instantiate(Hive1, soilClone.transform.position, Quaternion.identity);
                            hive1Obj.transform.parent = soilClone.transform;
                            break;
                        case HIVE2:
                            GameObject hive2Obj = Instantiate(Hive2, soilClone.transform.position, Quaternion.identity);
                            hive2Obj.transform.parent = soilClone.transform;
                            break;
                        default:
                            GameObject hive3Obj = Instantiate(Hive3, soilClone.transform.position, Quaternion.identity);
                            hive3Obj.transform.parent = soilClone.transform;
                            break;
                    }
                }
            }
        }
    }

    //エイリアンの巣の初期位置を決める。必ずCreateBaseより後に実行すること
    private void CreateHive(){
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                if (Map[i,j] == SOIL && Random.value < hvratio && !AreNearBase(i,j)){
                    float rand = Random.value;
                    if (rand > 0.1){
                        Map[i,j] = HIVE1;
                    }else if(rand > 0.01){
                        Map[i,j] = HIVE2;
                        Debug.Log("hive2");
                    }else{
                        Map[i,j] = HIVE3;
                    }
                }
            }
        }
    }

    //最初の拠点の位置を決める。拠点にできるような場所がなければfalseを返す
    private bool CreateBase(){
        int[] y = new int[0];
        int[] x = new int[0];
        for(int i = 3*Height/8; i < 5*Height/8; i++){
            for(int j =3*Width/8; j < 5*Height/8; j++){
                if(Map[i,j] == SOIL){
                    System.Array.Resize(ref y, y.Length + 1);
                    y[y.Length - 1] = i;
                    System.Array.Resize(ref x, x.Length + 1);
                    x[x.Length - 1] = j;
                }
            }
        }
        if(y.Length == 0){
            return false;
        }else{
            int rand = Random.Range(0, y.Length);
            Map[y[rand], x[rand]] = BASE;
            return true;
        }
    }


    private int GetMooreSurroundingTiles(int[,] map, int x, int y)
            {
    /* ムーア近傍は次のようになっている（「T」はタイル、「N」は近傍）。
     *
     * N N N
     * N T N
     * N N N
     *
     *ただし今回はマップサイズが大きいため5*5のムーア近傍を採用
     */

        int tileCount = 0;
        
        for(int neighbourX = x-2; neighbourX <= x+2; neighbourX++)
        {
            for(int neighbourY = y-2; neighbourY <= y+2; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourY >= 0 && neighbourX < Width && neighbourY < Height)
                {
                    // 周囲のチェックが行われている、中心のタイルはカウントしない
                    if(neighbourX != x || neighbourY != y)
                    {
                        tileCount += map[neighbourX, neighbourY];
                    }
                }
            }
        }
        return tileCount;
    }

    private void SmoothMooreCellularAutomata(){
        int[,] tempMap = new int[Map.GetLength(0), Map.GetLength(1)]; //あるループ時のマップの状態
        System.Array.Copy(Map, tempMap, Map.Length); //Mapの内容をtempMapに取り込む

        for(int smoothcnt = 0; smoothcnt < smoothrep; smoothcnt++){

            for(int i = 0; i < Height; i++){
                for(int j = 0; j < Width; j++){
                    int surroundingTiles = GetMooreSurroundingTiles(Map,i,j);
                    if(surroundingTiles > 12){
                        tempMap[i,j] = MTN;
                        mtcnt += 1;
                    }else if(surroundingTiles < 12){
                        tempMap[i,j] = SOIL;
                    }
                }
            }

            System.Array.Copy(tempMap, Map, tempMap.Length); //ループが終わったらMapをtenpMapで更新           
        }
	
    }
    
}
