using AssetKits.ParticleImage;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UI", fileName = "UI")]
public class UIConfig : ScriptableObject
{

    [field: SerializeField] public ParticleImage CoinsParticlesPrefab { get; private set; }




}
