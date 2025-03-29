using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [field: SerializeField] public float ScoreMultiplier { get; private set; } = 1f;
    [SerializeField] private int _ballGroupsAmount = 3;
    [SerializeField] private Rect _cameraArea; public Rect CameraArea => _cameraArea;
    [field: SerializeField] public List<BallType> BallTypes { get; private set; }
    [field: SerializeField] public List<ItemGroup> BallGroupPrefabs { get; private set; }


    private Rect? _mainFieldRect;
    public Rect MainFieldRect
    {
        get
        {
            if (_mainFieldRect == null) UpdateMainFieldRect();
            return _mainFieldRect.Value;
        }
    }

    private void UpdateMainFieldRect()
    {
        var mainFieldRect = new Rect(_cameraArea);
        mainFieldRect.center += Vector2.up * _cameraArea.height * Configs.Levels.BallPanelSizePart / 2f;
        mainFieldRect.height *= (1f - Configs.Levels.BallPanelSizePart);
        mainFieldRect.position -= mainFieldRect.size / 2f;
        _mainFieldRect = mainFieldRect;
    }



    public List<Vector2> GetBallGroupSpawnPositions()
    {
        var resultList = new List<Vector2>();

        var ballPanelRect = GetBallPanelRect();

        float offsetXPerPoint = ballPanelRect.width / (_ballGroupsAmount + 1);
        float startOffsetX = ballPanelRect.x + offsetXPerPoint - ballPanelRect.width / 2f;

        for (int i = 0; i < _ballGroupsAmount; i++)
        {
            resultList.Add(new Vector2(startOffsetX + offsetXPerPoint * i, ballPanelRect.y - ballPanelRect.height / 2f + ballPanelRect.height / 2f));
        }

        return resultList;
    }


    private Rect GetBallPanelRect()
    {
        Vector2 size = new Vector2(_cameraArea.width, _cameraArea.height * Configs.Levels.BallPanelSizePart);
        Vector2 position = new Vector2(_cameraArea.x, _cameraArea.y + (size.y - _cameraArea.height) * 0.5f);

        return new Rect(position, size);
    }


    private void OnDrawGizmos()
    {
        UpdateMainFieldRect();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_cameraArea.position, _cameraArea.size);

        Gizmos.color = Color.cyan;
        Rect ballPanelRect = GetBallPanelRect();
        Gizmos.DrawWireCube(ballPanelRect.position, ballPanelRect.size);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(MainFieldRect.center, MainFieldRect.size);

        foreach (var ballGroupPosition in GetBallGroupSpawnPositions())
            Gizmos.DrawWireSphere(ballGroupPosition, 0.25f);

    }



}
