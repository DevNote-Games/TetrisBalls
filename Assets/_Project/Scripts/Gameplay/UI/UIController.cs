
using UnityEngine;

public class UIController
{
    private VictoryScreenView _victoryScreen;
    private LoseScreenView _loseScreen;

    private GameObject _currentView;


    public UIController(VictoryScreenView victoryScreen, LoseScreenView loseScreen)
    {
        _victoryScreen = victoryScreen;
        _loseScreen = loseScreen;
    }



    public void ShowVictoryScreen()
    {
        _victoryScreen.gameObject.SetActive(true);
        _currentView = _victoryScreen.gameObject;
    }


    public void ShowLoseScreen()
    {
        _loseScreen.gameObject.SetActive(true);
        _currentView = _loseScreen.gameObject;
    }



    public void HideCurrentView() => _currentView?.SetActive(false);





}
