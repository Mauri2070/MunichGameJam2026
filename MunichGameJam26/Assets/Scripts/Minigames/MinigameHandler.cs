using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHandler : MonoBehaviour
{


    [Header("Minigames")]
    [SerializeField] Button minigameOpener;
    [SerializeField] GameObject fraudBackground;

    [Header("Result")]
    public bool success = false;

    public void OpenMinigame(GameObject minigameBackground)
    {

        minigameBackground.SetActive(true);

        Debug.Log("Minigame is " + minigameBackground.name);

        MinigameHandler handler = minigameBackground.GetComponent<MinigameHandler>();

        handler.StartMinigame();

    }

    protected virtual void StartMinigame()
    {

        Debug.Log("Start Minigame");
        success = false;

    }

    public void CloseMinigame(GameObject minigameBackground)
    {



    }

    protected virtual void EndMinigame()
    {



    }

}
