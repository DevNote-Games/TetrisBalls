using R3;

namespace VG2
{
    public static class GameState
    {
        public static ReactiveProperty<bool> AdsEnabled { get; set; }
        public static int Level { get; set; }
        public static ReactiveProperty<int> Coins { get; set; }
        public static ReactiveProperty<int> BombBoosters { get; set; }



    }
}

