using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{

    [Header("Images")]
    [SerializeField] PlakatHandler plakatHandler;
    [SerializeField] GameObject gluedPerson;
    [SerializeField] Image ungluedPerson;
    [SerializeField] public Button button;
    [SerializeField] public bool isGlued = false;

    [SerializeField] AudioSource screamSource;
    [SerializeField] AudioSource glueSource;

    public void SetObstacle()
    {

        gluedPerson.SetActive(false);
        ungluedPerson.enabled = true;
        isGlued = false;

        MoveObstacle();

    }

    private void MoveObstacle()
    {

        float x = Random.Range(1800, 120);
        Debug.Log("Obstacle x = " + x);
        RectTransform obstacleRect = GetComponent<RectTransform>();

        obstacleRect.position = new Vector3(x, obstacleRect.position.y, 0);

    }

    public void ClickObstackle()
    {

        button.enabled = false;

        ungluedPerson.enabled = false;
        gluedPerson.SetActive(true);

        isGlued = true;

        plakatHandler.isScrolling = false;

        StartCoroutine(WaitThenOpenDefeatScreen());

        screamSource.Play();
        glueSource.Play();

    }

    private IEnumerator WaitThenOpenDefeatScreen()
    {

        yield return new WaitForSeconds(2f);

        plakatHandler.mainHandler.OpenFailedScreen();

    }

}
