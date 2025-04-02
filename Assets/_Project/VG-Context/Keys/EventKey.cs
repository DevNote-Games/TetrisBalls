
namespace VG2
{
    public static class EventKey
    {
        public static string LevelCompleted(int level) => $"level_completed_{level}";
        public static string LevelLose(int level) => $"level_lose_{level}";

        public static string AdRewardedShown(string adKey) => $"ad_rewarded_{adKey}";

        public static string InterstitialShown => "interstitial_shown";

        public static string BombUsed => "booster_bomb_used";


    }

}


