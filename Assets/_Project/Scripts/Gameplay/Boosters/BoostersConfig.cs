using UnityEngine;


[CreateAssetMenu(menuName = "Configs/Boosters", fileName = "Boosters")]
public class BoostersConfig : ScriptableObject
{

    [field: SerializeField] public BombBooster BombPrefab { get; private set; }
    [field: SerializeField] public ItemGroup BombGroupPrefab { get; private set; }
    [field: SerializeField] public int SpawnBombChainBallsRequire { get; private set; }
    [field: SerializeField] public int BombPrice { get; private set; }
    [field: SerializeField] public int BombAvailableFromLevel { get; private set; }



}
