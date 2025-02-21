using UnityEngine;


[CreateAssetMenu(menuName = "Configs/GameRules", fileName = "GameRules")]
public class GameRulesConfig : ScriptableObject
{
    [field: SerializeField] public int MinBallsRequireForExplosion;


}
