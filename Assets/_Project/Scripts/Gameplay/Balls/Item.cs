using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [field: SerializeField] public float Radius { get; private set; }

    public abstract void SetAvailableForPlacing(bool available);

    public abstract void Activate();



    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

}
