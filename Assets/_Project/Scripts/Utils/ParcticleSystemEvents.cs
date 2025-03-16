using System;
using UnityEngine;

public class ParcticleSystemEvents : MonoBehaviour
{
    public event Action onStopped;

    private void OnParticleSystemStopped()
    {
        onStopped?.Invoke();
    }

}
