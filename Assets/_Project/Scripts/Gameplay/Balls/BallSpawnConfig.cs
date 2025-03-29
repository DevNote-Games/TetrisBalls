using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Configs/BallSpawn", fileName = "BallSpawn")]
public class BallSpawnConfig : ScriptableObject
{
    [field: SerializeField] public List<ItemGroup> BallGroupPrefabs { get; private set; }
    [field: SerializeField] public List<BallType> SpawnableBallTypes { get; private set; }




}
