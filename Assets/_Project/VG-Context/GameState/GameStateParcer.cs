using System.Collections.Generic;
using R3;

namespace VG2
{
    public static class GameStateParcer
    {
        private const string adsEnabledKey = "adsEnabled";
        private const string coinsKey = "coins";
        private const string levelKey = "level";
        private const string bombBoostersKey = "bombs";


        public static void Parse(StartValuesConfig startValuesConfig, Dictionary<string, string> data)
        {
            GameState.AdsEnabled = new ReactiveProperty<bool>
                (data.ContainsKey(adsEnabledKey) ? bool.Parse(data[adsEnabledKey]) : true);

            GameState.Coins = new ReactiveProperty<int>
                (data.ContainsKey(coinsKey) ? int.Parse(data[coinsKey]) : 0);

            GameState.Level = new ReactiveProperty<int>(
                data.ContainsKey(levelKey) ? int.Parse(data[levelKey]) : 1);

            GameState.BombBoosters = new ReactiveProperty<int>
                (data.ContainsKey(bombBoostersKey) ? int.Parse(data[bombBoostersKey]) : 3);
        }


        public static Dictionary<string, string> ToDataString()
        {
            var data = new Dictionary<string, string>();

            data.Add(adsEnabledKey, GameState.AdsEnabled.ToString());
            data.Add(coinsKey, GameState.Coins.ToString());
            data.Add(levelKey, GameState.Level.ToString());
            data.Add(bombBoostersKey, GameState.BombBoosters.ToString());

            return data;
        }


    }
}


