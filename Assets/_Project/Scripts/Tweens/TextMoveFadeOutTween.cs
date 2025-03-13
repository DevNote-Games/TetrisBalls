using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextMoveFadeOutTween : MonoBehaviour
{
    [SerializeField] private Vector2 _move;
    [SerializeField] private float _duration;
    [SerializeField] private TextMeshProUGUI _text;


    private void Start()
    {
        var rect = transform as RectTransform;

        DOTween.Sequence(transform)
            .Append(rect.DOMove((Vector2)rect.position + _move, _duration))
            .AppendInterval(_duration / 2f)
            .Join(_text.DOFade(0f, _duration / 2f))
            .SetEase(Ease.OutFlash)
            .onComplete += () => Destroy(gameObject);
    }




}
