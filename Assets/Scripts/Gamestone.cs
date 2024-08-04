using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gamestone : MonoBehaviour
{
    public float xoffset = -3;
    public float yoffset = -4;
    public int rowIndex = 0;
    public int columIndex = 0;
    
    public GameObject[] gamestonePrefab; //数组
    public int gamestoneType; //类型
    public GameController gameController;
    private GameObject gamestone;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePosition(int _rowIndex, int _columIndex)
    {
        rowIndex = _rowIndex;
        columIndex = _columIndex;
        transform.position = new Vector3(_columIndex+xoffset , _rowIndex+yoffset, 0);
    }

    public void RandomCreateGamestone()
    {
        if(gamestone != null) return; //如果已经生成了，就不再生成
        gamestoneType = Random.Range(0, gamestonePrefab.Length);
        gamestone = Instantiate(gamestonePrefab[gamestoneType], transform, true);
    }

    public void OnMouseDown()
    {
        gameController.Select(this);
    }
}
