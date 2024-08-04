using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Gamestone gamestone;
    public int rowNum = 7; // 行数
    public int colNum = 5; // 列数
    public List<List<Gamestone>> gemstoneList;

    // Start is called before the first frame update
    void Start()
    {
        gemstoneList = new List<List<Gamestone>>();
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            List<Gamestone> temp = new List<Gamestone>();
            for (int colIndex = 0; colIndex < colNum; colIndex++)
            {
                Gamestone c = AddGemstone(rowIndex, colIndex);
                temp.Add(c);
            }
            gemstoneList.Add(temp);
        }
    }

    public Gamestone AddGemstone(int rowIndex, int colIndex)
    {
        Gamestone c = Instantiate(gamestone, transform, true);
        c.RandomCreateGamestone();
        c.UpdatePosition(rowIndex, colIndex);
        return c;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select(Gamestone c)
    {
        Destroy(c.gameObject);
    }
}
