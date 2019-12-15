using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int Height;
    public int Width;
    public int[,] Map;
    private int soil=0;
    private int grass=1;
    private int wall=2;
    private int mtn=4;
    public GameObject Soil;
    public GameObject Grass;
    public GameObject Wall;
    public GameObject Mtn;

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
            }
        }
    }

    private void CreateMapData() {
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                if (Map[i, j] == soil) {
                    Instantiate(Soil, new Vector3(j - Width/2, i - Height/2, 0), Quaternion.identity);
                }
            }
        }
    }
}
