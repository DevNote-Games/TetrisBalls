using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Configs/GameRules", fileName = "GameRules")]
public class GameRulesConfig : ScriptableObject
{
    [System.Serializable]
    private struct ScoreMultiplierForChain
    {
        public int balls;
        public float multiplier;
    }


    
    [field: SerializeField] public int TimeToLimitLose { get; private set; }
    [field: SerializeField] public int MinBallsChainRequire { get; private set; }
    [field: SerializeField] public int LevelCoinsReward { get; private set; }


    [SerializeField] private int _baseScoreForOneBallOfChain;
    [SerializeField] private List<ScoreMultiplierForChain> _scoreMultipliersForChain;





    public int GetScoreForChainExplosion(int ballsAmount)
    {
        float multiplier = 1f;
        foreach (var scoreMultiplier in _scoreMultipliersForChain)
        {
            if (ballsAmount < scoreMultiplier.balls) break;
            multiplier = scoreMultiplier.multiplier;
        }

        return (int)(_baseScoreForOneBallOfChain * ballsAmount * multiplier);
    }




}
