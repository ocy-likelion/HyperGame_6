using System;

[Serializable]
public struct GameInfo
{
    public UserInfo userInfo;
    public int currentScore;
    public int maxCombo;
    public int maxDay;
    public int maxWorkTime;
}