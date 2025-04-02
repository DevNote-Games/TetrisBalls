using AssetKits.ParticleImage;
using UnityEngine;
using VG2;

public class UIController
{
    private GameObject _currentView;
    private ParticleImage _coinsParticles;
    private int _coinsValue;

    public void ShowVictoryScreen()
    {
        Sound.Play(SoundKey.Win);
        UIContainer.VictoryScreen.gameObject.SetActive(true);
        _currentView = UIContainer.VictoryScreen.gameObject;
    }


    public void ShowLoseScreen()
    {
        Sound.Play(SoundKey.Lose);
        UIContainer.LoseScreen.gameObject.SetActive(true);
        _currentView = UIContainer.LoseScreen.gameObject;
    }

    public void RunCoinsParticles(int coinsValue)
    {
        UIContainer.Coins.SetValue(GameState.Coins.Value - coinsValue);

        _coinsValue = coinsValue;

        Sound.Play(SoundKey.CoinsShow);
        _coinsParticles = Object.Instantiate(Configs.UI.CoinsParticlesPrefab, UIContainer.VfxCanvas);
        _coinsParticles.attractorTarget = UIContainer.Coins.transform;
        _coinsParticles.onAnyParticleFinished.AddListener(OnCoinParticleFinished);
        _coinsParticles.onLastParticleFinished.AddListener(OnCoinLastParticleFinished);
        _coinsParticles.onFirstParticleFinished.AddListener(OnCoinFirstParticleFinished);
    }

    private void OnCoinFirstParticleFinished()
    {
        Sound.Play(SoundKey.CoinsGet);
    }

    private void OnCoinParticleFinished()
    {
        int coinsPerParticle = _coinsValue / _coinsParticles.Bursts[0].count;
        UIContainer.Coins.SetValue(GameState.Coins.Value - coinsPerParticle * _coinsParticles.particleCount);
    }

    private void OnCoinLastParticleFinished()
    {
        UIContainer.Coins.SetValue(GameState.Coins.Value);
        Object.Destroy(_coinsParticles.gameObject);
    }

    


    public void HideCurrentView() => _currentView?.SetActive(false);





}
