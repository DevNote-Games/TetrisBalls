using UnityEngine;
using UnityEngine.UI;
using VG2;

public class ButtonClickSound : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => Sound.Play(SoundKey.Click));
    }
}
