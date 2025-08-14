using System;

[Serializable]
public struct GameResultData
{
    public int Day;
    public int MaxCombo;
    public int Score;
     
    public GameResultData(int day, int maxCombo, int score)
    {
        Day = day;
        MaxCombo = maxCombo;
        Score = score;
    }
}