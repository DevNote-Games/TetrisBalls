using UnityEngine;

public class BlastParticle : MonoBehaviour
{
    [SerializeField] private Vector2 _randomScaleFromTo;
    [SerializeField] private ParticleSystem _splashParticles;
    [SerializeField] private ParticleSystem _dropsParticles;


    private void OnEnable()
    {
        transform.localScale = Vector3.one * Random.Range(_randomScaleFromTo.x, _randomScaleFromTo.y);
    }


    public void SetColor(Color color)
    {
        var mainBlock = _splashParticles.main;
        mainBlock.startColor = color;

        mainBlock = _dropsParticles.main;
        mainBlock.startColor = color;

    }

    

}
