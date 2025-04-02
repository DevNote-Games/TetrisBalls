using DG.Tweening;
using UnityEngine;

public class CursorMoveTween : MonoBehaviour
{
    private const float DURATION = 1.5f;

    private Tween _currentTween;

    public void Run(Vector2 fromPosition, Vector2 toPosition)
    {
        transform.position = fromPosition;

        _currentTween = transform.DOMove(toPosition, DURATION).SetEase(Ease.InOutFlash)
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        _currentTween?.Kill();
    }

}
