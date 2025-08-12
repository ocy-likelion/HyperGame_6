public class Constants
{
    #region PlayerPrefKey
    //임시 키워드
    public const string Volume = "Volume";
    public const string Mute = "Mute";

    #endregion
    
    #region AudioClipNames
    //임시 키워드
    public const string MainBGM = "MainBGM";
    public const string OnClick = "ClickSFX";
    #endregion
    
    //
    public enum GameState
    {
        None,
        Title,
        InGame,
        Pause
    }
}