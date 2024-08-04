using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TMP_Text scoreText; // 显示分数
    public int score; // 当前得分

    void Start()
    {
        UpdateScoreText();
    }

    // 增加分数并更新显示
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // 更新分数文本显示
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    // 计算宝石的得分
    public int CalculatePoints(Gamestone c)
    {
        return 1;
    }
}