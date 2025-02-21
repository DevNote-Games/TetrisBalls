using UnityEngine;
using VG2;

public static class Configs
{

    public static StartValuesConfig StartValues => Resources.Load<StartValuesConfig>("StartValues");

    public static BallVisualConfig BallVisual => Resources.Load<BallVisualConfig>("BallVisual");

    public static BallSpawnConfig BallSpawn => Resources.Load<BallSpawnConfig>("BallSpawn");

    public static GameRulesConfig GameRules => Resources.Load<GameRulesConfig>("GameRules");

}



