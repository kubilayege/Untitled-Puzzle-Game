public interface IPlayerDataView
{
   int GetVideoReward(string key);
   int GetThemePrice(int whichTheme,string key);
   bool GetThemeState(int whichTheme,string key);
}
