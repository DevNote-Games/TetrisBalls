using System.Collections.Generic;
using AssetKits.ParticleImage;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[CreateAssetMenu(menuName = "Configs/BallVisual", fileName = "BallVisual")]
public class BallVisualConfig : ScriptableObject
{
    [System.Serializable]
    private struct BallMaterial
    {
        public BallType ballType;
        public Material material;
    }

    [field: SerializeField] public TextMeshProUGUI ScorePartPrefab { get; private set; }
    [field: SerializeField] public DecalProjector SplashDecalPrefab { get; private set; }
    [field: SerializeField] public BlastParticle BlastParticlePrefab { get; private set; }
    [field: SerializeField] public ParticleImage SmallBallsParticlePrefab { get; private set; }



    [SerializeField] private List<Texture2D> _splashTextures;
    [SerializeField] private List<BallMaterial> _ballMaterials;

    public Material GetBallMaterial(BallType ballType) 
        => _ballMaterials.Find(bm => bm.ballType == ballType).material;

    public Texture2D GetRandomSplashTexture() => _splashTextures[Random.Range(0, _splashTextures.Count)];



}
