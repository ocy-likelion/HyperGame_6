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
    
    //
    public enum GameState
    {
        None,
        Title,
        InGame,
        Pause
    }
}