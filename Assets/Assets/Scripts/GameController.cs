using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Gamestone gamestone;
    public int rowNum = 7; // 行数
    public int colNum = 5; // 列数
    public List<List<Gamestone>> GemstoneList;
    public List<Gamestone> matchesGamestone;
    public Score scoreManager; // 计分
    public Gamestone currentGemstone;

    void Start()
    {
        GemstoneList = new List<List<Gamestone>>();
        matchesGamestone = new List<Gamestone>();

        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            List<Gamestone> temp = new List<Gamestone>();
            for (int colIndex = 0; colIndex < colNum; colIndex++)
            {
                Gamestone c = AddGemstone(rowIndex, colIndex);
                temp.Add(c);
            }

            GemstoneList.Add(temp);
        }

        RemoveMatches();
    }
    
    public Gamestone AddGemstone(int rowIndex, int colIndex)
    {
        Gamestone c = Instantiate(gamestone, transform, true);
        c.RandomCreateGamestone();
        c.UpdatePosition(rowIndex, colIndex);
        return c;
    }
    
    public void Select(Gamestone c)
    {
        if (currentGemstone == null)
        {

            currentGemstone = c;
            currentGemstone.IsSelected = true;
        }
        else
        {
            // 如果选择的两个宝石相邻，则进行交换
            if (Mathf.Abs(currentGemstone.rowIndex - c.rowIndex) +
                Mathf.Abs(currentGemstone.columnIndex - c.columnIndex) == 1)
            {
                StartCoroutine(ExchangeAndCheckMatches(currentGemstone, c));
            }

            // 取消选择当前宝石
            currentGemstone.IsSelected = false;
            currentGemstone = null;
        }
    }

    // 交换宝石并检查是否有匹配
    IEnumerator ExchangeAndCheckMatches(Gamestone c1, Gamestone c2)
    {
        Exchange(c1, c2); // 交换宝石
        yield return new WaitForSeconds(0.5f);
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            // 如果有匹配则移除匹配的宝石
            RemoveMatches();
        }
        else
        {
            // 否则交换回去
            Exchange(c1, c2);
        }
    }

    // 添加匹配的宝石到列表中
    void AddMatches(Gamestone c)
    {
        if (!matchesGamestone.Contains(c))
        {
            matchesGamestone.Add(c);
        }
    }

    // 移除匹配的宝石并更新分数
    void RemoveMatches()
    {
        int totalPoints = 0;
        foreach (Gamestone c in matchesGamestone)
        {
            RemoveGemstone(c); // 移除宝石
            totalPoints += CalculatePoints(c); // 计算分数
        }

        matchesGamestone.Clear(); // 清空匹配列表
        StartCoroutine(WaitForCheckMatchesAgain()); // 等待再次检查匹配

        scoreManager.AddScore(totalPoints); // 更新分数
    }

    // 计算每个宝石的分数
    private int CalculatePoints(Gamestone c)
    {
        return 1;
    }

    // 检查匹配
    public IEnumerator WaitForCheckMatchesAgain()
    {
        yield return new WaitForSeconds(0.5f); 
        if (CheckHorizontalMatches() || CheckVerticalMatches())
        {
            RemoveMatches(); 
        }
    }

    // 移除宝石并处理下落逻辑
    public void RemoveGemstone(Gamestone c)
    {
        c.Dispose(); 
        // 让上面的宝石下落
        for (int i = c.rowIndex + 1; i < rowNum; i++)
        {
            Gamestone tempGemstone = GetGemstone(i, c.columnIndex);
            tempGemstone.rowIndex--;
            SetGemstone(tempGemstone.rowIndex, tempGemstone.columnIndex, tempGemstone);
            tempGemstone.TweenToPosition(tempGemstone.rowIndex, tempGemstone.columnIndex);
        }

        // 添加新的宝石到顶部
        Gamestone newGemstone = AddGemstone(rowNum, c.columnIndex);
        newGemstone.rowIndex--;
        SetGemstone(newGemstone.rowIndex, newGemstone.columnIndex, newGemstone);
        newGemstone.TweenToPosition(newGemstone.rowIndex, newGemstone.columnIndex);
    }

    // 检查水平匹配
    public bool CheckHorizontalMatches()
    {
        bool isMatches = false;
        for (int rowIndex = 0; rowIndex < rowNum; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < colNum - 2; columnIndex++)
            {
                if (GetGemstone(rowIndex, columnIndex).gamestoneType ==
                    GetGemstone(rowIndex, columnIndex + 1).gamestoneType &&
                    GetGemstone(rowIndex, columnIndex).gamestoneType ==
                    GetGemstone(rowIndex, columnIndex + 2).gamestoneType)
                {
                    AddMatches(GetGemstone(rowIndex, columnIndex));
                    AddMatches(GetGemstone(rowIndex, columnIndex + 1));
                    AddMatches(GetGemstone(rowIndex, columnIndex + 2));
                    isMatches = true;
                }
            }
        }

        return isMatches;
    }

    // 检查垂直匹配
    public bool CheckVerticalMatches()
    {
        bool isMatches = false;
        for (int columnIndex = 0; columnIndex < colNum; columnIndex++)
        {
            for (int rowIndex = 0; rowIndex < rowNum - 2; rowIndex++)
            {
                if (GetGemstone(rowIndex, columnIndex).gamestoneType ==
                    GetGemstone(rowIndex + 1, columnIndex).gamestoneType &&
                    GetGemstone(rowIndex, columnIndex).gamestoneType ==
                    GetGemstone(rowIndex + 2, columnIndex).gamestoneType)
                {
                    AddMatches(GetGemstone(rowIndex, columnIndex));
                    AddMatches(GetGemstone(rowIndex + 1, columnIndex));
                    AddMatches(GetGemstone(rowIndex + 2, columnIndex));
                    isMatches = true;
                }
            }
        }

        return isMatches;
    }

    // 获取指定位置的宝石
    public Gamestone GetGemstone(int rowIndex, int colIndex)
    {
        return GemstoneList[rowIndex][colIndex];
    }

    // 设置指定位置的宝石
    public void SetGemstone(int rowIndex, int colIndex, Gamestone c)
    {
        GemstoneList[rowIndex][colIndex] = c;
    }

    // 交换两个宝石的位置
    public void Exchange(Gamestone c1, Gamestone c2)
    {
        SetGemstone(c1.rowIndex, c1.columnIndex, c2);
        SetGemstone(c2.rowIndex, c2.columnIndex, c1);

        // 交换宝石的行列索引
        (c1.rowIndex, c2.rowIndex) = (c2.rowIndex, c1.rowIndex);
        (c1.columnIndex, c2.columnIndex) = (c2.columnIndex, c1.columnIndex);

        // 让宝石移动到新的位置
        c1.TweenToPosition(c1.rowIndex, c1.columnIndex);
        c2.TweenToPosition(c2.rowIndex, c2.columnIndex);
    }
}