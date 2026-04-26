using System;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHandler : MonoBehaviour
{

    public event Action<bool> OnMinigameCompleted;

    [SerializeField] public MinigameHandler mainHandler;
    [SerializeField] protected GameObject centerPosition;

    [Header("Minigames")]
    [SerializeField] Button minigameOpener;
    [SerializeField] MinigameHandler activeMinigame;
    [SerializeField] GameObject fraudBackground;
    [SerializeField] GameObject plakatBackground;

    [Header("Result")]
    public bool success = false;
    [SerializeField] private GameObject failedScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;

    public void OpenMinigame(GameObject minigameBackground)
    {

        minigameBackground.SetActive(true);

        Debug.Log("Minigame is " + minigameBackground.name);

        MinigameHandler handler = minigameBackground.GetComponent<MinigameHandler>();
        activeMinigame = handler;

        handler.StartMinigame();

    }

    protected virtual void StartMinigame()
    {

        mainHandler.success = false;

    }

    public void CloseMinigame()
    {

        activeMinigame.EndMinigame();

        activeMinigame.gameObject.SetActive(false);
        activeMinigame = null;

        failedScreen.SetActive(false);
        victoryScreen.SetActive(false);

    }

    public void OpenFailedScreen()
    {

        failedScreen.SetActive(true);
        audioSource.PlayOneShot(defeatSound);
        OnMinigameCompleted?.Invoke(false);

    }

    public void OpenVictoryScreen()
    {

        success = true;
        victoryScreen.SetActive(true);
        audioSource.PlayOneShot(victorySound);

        OnMinigameCompleted?.Invoke(true);
    }

    protected virtual void EndMinigame()
    {



    }

}
