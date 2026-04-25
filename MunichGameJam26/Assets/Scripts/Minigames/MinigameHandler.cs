using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHandler : MonoBehaviour
{


    [Header("Minigames")]
    [SerializeField] Button minigameOpener;
    [SerializeField] MinigameHandler activeMinigame;
    [SerializeField] GameObject fraudBackground;

    [Header("Result")]
    public bool success = false;
    [SerializeField] private GameObject failedScreen;
    [SerializeField] private GameObject victoryScreen;

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

        Debug.Log("Start Minigame");
        success = false;

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

    }

    public void OpenVictoryScreen()
    {

        success = true;
        victoryScreen.SetActive(true);

    }

    protected virtual void EndMinigame()
    {



    }

}
