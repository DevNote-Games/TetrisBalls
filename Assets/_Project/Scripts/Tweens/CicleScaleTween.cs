using DG.Tweening;
using UnityEngine;

public class CicleScaleTween : MonoBehaviour
{
    [SerializeField] private Vector2 _fromToScale;
    [SerializeField] private float _durariton;
    

    private Tween _tween;



    private void OnEnable()
    {
        transform.localScale = Vector3.one * _fromToScale.x;

        _tween = transform.DOScale(_fromToScale.y, _durariton)
            .SetEase(Ease.OutFlash).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }


}
