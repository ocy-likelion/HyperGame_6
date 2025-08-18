public class Constants
{
    #region PlayerPrefKey
    //임시 키워드
    public const string Volume = "Volume";
    public const string Mute = "Mute";

    #endregion
    
    #region SceneNames
    public const string TitleSceneName = "Title";
    public const string GameSceneName = "InGame";
    #endregion

    #region ButtonSprites

    public const string SprAcceptUp = "AP_up";
    public const string SprAcceptDown = "AP_down";

    public const string SprNegativeUp = "DE_up";
    public const string SprNegativeDown = "DE_down";

    public const string SprPauseUp = "Pause_up";
    public const string SprPauseDown = "Pause_down";

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