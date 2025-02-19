using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Configs/BallVisual", fileName = "BallVisual")]
public class BallVisualConfig : ScriptableObject
{
    [System.Serializable]
    private struct BallMaterial
    {
        public BallType ballType;
        public Material material;
    }

    [SerializeField] private List<BallMaterial> _ballMaterials;



    public Material GetBallMaterial(BallType ballType) 
        => _ballMaterials.Find(bm => bm.ballType == ballType).material;



}
