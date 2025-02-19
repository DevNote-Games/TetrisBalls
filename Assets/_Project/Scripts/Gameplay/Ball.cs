using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;


    public BallType BallType { get; private set; }


    public void SetType(BallType ballType)
    {
        BallType = ballType;
        _meshRenderer.material = Configs.BallVisual.GetBallMaterial(ballType);
    }


    public void TogglePhysics(bool enabled)
    {
        _collider.enabled = enabled;
        _rigidbody.isKinematic = !enabled;
    }


}
