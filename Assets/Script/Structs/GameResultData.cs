using System;

[Serializable]
public struct GameResultData
{
    public int Day;
    public int MaxCombo;
    public float Score;
     
    public GameResultData(int day, int maxCombo, float score)
    {
        Day = day;
        MaxCombo = maxCombo;
        Score = score;
    }
}