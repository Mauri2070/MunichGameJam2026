using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlakatHandler : MinigameHandler
{


    [Header("Scrolling")]
    RectTransform rectTransform;
    [SerializeField] Vector3 origin;
    [SerializeField] private float maxScrollSpeed = 4.0f;
    private float scrollSpeed = 0;
    [SerializeField] private float scrollSpeedStartDuration = 1.0f;
    private float scrollSpeedAcceleration; 
    public bool isScrolling = false;

    [Header("Prefabs")]
    [SerializeField] Obstacle obstacle;
    [SerializeField] GameObject placatedObstacle;
    [SerializeField] Plakat plakatPref;

    [Header("Obstacle")]
    [SerializeField] RectTransform obstacleRect;
    [SerializeField] GameObject placatedObstacleInstance;

    [Header("Plakate")]
    [SerializeField] GameObject plakateOrigin;
    [SerializeField] BoxCollider2D plakatOriginCollider;
    [SerializeField] Plakat[] enemyPlakate;
    [SerializeField] float tolerance;
    [SerializeField] Vector2 previousDist;
    [SerializeField] float boundWidth;
    [SerializeField] float boundHeight;

    [Header("Ambience")]
    public AudioClip streetAmbience;
    public AudioClip bureoAmbience;
    public AudioSource ambienceSource;

    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        origin = rectTransform.position;

    }

    private void FixedUpdate()
    {

        if (isScrolling)
            ScrollBackground();

        scrollSpeedAcceleration = maxScrollSpeed / 60 * scrollSpeedStartDuration;

    }

    protected override void StartMinigame()
    {

        if (placatedObstacleInstance != null)
            Destroy(placatedObstacleInstance.transform);

        foreach (Transform child in plakateOrigin.transform)
            Destroy(child.gameObject);

        rectTransform.position = origin;
        scrollSpeed = 0;

        base.StartMinigame();

        plakatOriginCollider = plakateOrigin.GetComponent<BoxCollider2D>();

        boundWidth = plakatOriginCollider.size.x;
        boundHeight = plakatOriginCollider.size.y;


        int plakateNR = Random.Range(4, 6);

        SpawnPlakate(plakateNR);

        obstacle.SetObstacle();

        ambienceSource.clip = streetAmbience;
        ambienceSource.Play();

        StartCoroutine(WaitThenStartScrolling());

    }

    private void SpawnPlakate(int plakateNR)
    {

        enemyPlakate = new Plakat[plakateNR];

        for (int i = 0; i < enemyPlakate.Length; i++)
        {

            enemyPlakate[i] = plakatPref;

            Plakat plakatInstance = Instantiate(plakatPref);
            plakatInstance.transform.SetParent(plakateOrigin.transform);

            plakatInstance.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            Vector2 instancePosition = RollForPosition(plakateNR, i);
            plakatInstance.GetComponent<RectTransform>().localPosition = instancePosition;

            enemyPlakate[i] = plakatInstance;

        }

    }

    private Vector2 RollForPosition(int plakateNR, int i)
    {

        Vector2 position = new Vector2(0, 0);

        //x 3610,192 y 143,9644
        float averageDistance = boundWidth / (plakateNR);

        Debug.Log("Average distance: " + averageDistance);

        float offsetX = ((averageDistance * i) + (averageDistance / 2)) - (boundWidth / 2);
        float randomOffset = averageDistance * 0.35f;
        offsetX = offsetX + Random.Range(-randomOffset, randomOffset);

        float offsetY = Random.Range(-boundHeight / 2, boundHeight / 2);

        Debug.Log("Offset x: " + offsetX + " offset y: " + offsetY);

        return new Vector2(offsetX, offsetY);

    }

    private void ScrollBackground()
    {

        if (scrollSpeed < maxScrollSpeed) scrollSpeed += scrollSpeedAcceleration;
        if (scrollSpeed > maxScrollSpeed) scrollSpeed = maxScrollSpeed;

        if (rectTransform.position.x > 0)
        {

            rectTransform.position -= new Vector3(scrollSpeed, 0, 0);

        }

        if (rectTransform.position.x <= 0)
        {

            rectTransform.position = new Vector3(0, rectTransform.position.y, rectTransform.position.z);
            isScrolling = false;
            StartCoroutine(WaitThenCheckResult());

        }


    }

    private void CheckResult()
    {

        foreach (Plakat plakat in enemyPlakate)
        {

            plakat.button.enabled = false;

            if (!plakat.isGlued || obstacle.isGlued)
            {

                mainHandler.OpenFailedScreen();
                return;

            }


        }

        obstacle.button.enabled = false;

        mainHandler.OpenVictoryScreen();

    }

    private IEnumerator WaitThenStartScrolling()
    {

        foreach (Plakat plakat in enemyPlakate)
        {

            plakat.button.enabled = true;

        }

        obstacle.button.enabled = true;

        yield return new WaitForSeconds(0.0f);
        isScrolling = true;

    }

    private IEnumerator WaitThenCheckResult()
    {

        yield return new WaitForSeconds(0.5f);
        CheckResult();

    }

    protected override void EndMinigame()
    {

        base.EndMinigame();
        ambienceSource.clip = bureoAmbience;
        ambienceSource.Play();

    }

}