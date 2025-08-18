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
    
    #region Network

#if true 
    //디버그용 로컬서버
    public const string ViteURL = "http://localhost:5174";
#else
    //토스와 통신한 Vite 통신주소
    public const string ViteURL = "https://apps-in-toss-api.toss.im";
#endif
    //public const string SID = "sid";
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