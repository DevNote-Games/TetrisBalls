using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Configs/Levels", fileName = "Levels")]
public class LevelsConfig : ScriptableObject
{

    [System.Serializable]
    private struct LevelData
    {
        public int requiredScore;
        public GameObject levelPrefab;
    }

    [SerializeField] [Range(0, 1)] private float _ballPanelSizePart; public float BallPanelSizePart => _ballPanelSizePart;
    [SerializeField] private float _baseScoreRequire;
    [SerializeField] private int _repeatFromLevelIndex;

    [SerializeField] private List<Level> _levelPrefabs;

    public int GetLevelRequiredScore(int levelNumber) => (int)(_levelPrefabs[levelNumber - 1].ScoreMultiplier * _baseScoreRequire);


    public Level GetLevelPrefab(int levelNumber)
    {
        int index = levelNumber - 1;

        if (index < _levelPrefabs.Count) return _levelPrefabs[index];

        else
        {
            index = (index - _levelPrefabs.Count) % (_levelPrefabs.Count - _repeatFromLevelIndex) + _repeatFromLevelIndex;
            return _levelPrefabs[index];
        }
    }

}
