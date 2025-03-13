using System.Collections.Generic;
using R3;

namespace VG2
{
    public static class GameStateParcer
    {
        private const string adsEnabledKey = "adsEnabled";
        private const string coinsKey = "coins";
        private const string levelKey = "level";


        public static void Parse(StartValuesConfig startValuesConfig, Dictionary<string, string> data)
        {
            GameState.AdsEnabled = new ReactiveProperty<bool>
                (data.ContainsKey(adsEnabledKey) ? bool.Parse(data[adsEnabledKey]) : true);

            GameState.Coins = new ReactiveProperty<int>
                (data.ContainsKey(coinsKey) ? 0 : int.Parse(data[coinsKey]));

            GameState.Level = data.ContainsKey(levelKey) ? 0 : int.Parse(data[levelKey]);

        }


        public static Dictionary<string, string> ToDataString()
        {
            var data = new Dictionary<string, string>();

            data.Add(adsEnabledKey, GameState.AdsEnabled.ToString());
            data.Add(coinsKey, GameState.Coins.ToString());
            data.Add(levelKey, GameState.Level.ToString());

            return data;
        }


    }
}


