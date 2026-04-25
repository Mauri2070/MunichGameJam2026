using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlakatHandler : MinigameHandler
{

    [Header("Scrolling")]
    RectTransform rectTransform;
    [SerializeField] Vector3 origin;
    [SerializeField] private float scrollSpeed = 0.5f;
    private bool isScrolling = false;

    [Header("Prefabs")]
    [SerializeField] Button obstacle;
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

    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        origin = rectTransform.position;

    }

    private void LateUpdate()
    {

        if (isScrolling)
            ScrollBackground();

    }

    protected override void StartMinigame()
    {

        if(placatedObstacleInstance != null)
            Destroy(placatedObstacleInstance.transform);

        foreach (Transform child in plakateOrigin.transform)
            Destroy(child.gameObject);

        rectTransform.position = origin;

        base.StartMinigame();

        plakatOriginCollider = plakateOrigin.GetComponent<BoxCollider2D>();

        boundWidth = plakatOriginCollider.size.x;
        boundHeight = plakatOriginCollider.size.y;


        int plakateNR = Random.Range(4, 6);

        SpawnPlakate(plakateNR);

        MoveObstacle();

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

        float offsetX = ((averageDistance * i) + (averageDistance / 2)) - (boundWidth/2);
        float randomOffset = averageDistance * 0.5f;
        offsetX = offsetX + Random.Range(-randomOffset, randomOffset);

        float offsetY = Random.Range(-boundHeight/2, boundHeight/2);

        Debug.Log("Offset x: " + offsetX + " offset y: " + offsetY);

        return new Vector2(offsetX, offsetY);

    }

    private void MoveObstacle()
    {

        float x = Random.Range(1800, 120);
        Debug.Log("Obstacle x = " + x);
        RectTransform obstacleRect = obstacle.GetComponent<RectTransform>();

        obstacleRect.position = new Vector3(x, obstacleRect.position.y, 0);

    }

    private void ScrollBackground()
    {

        if (rectTransform.position.x > 0)
        {

            rectTransform.position -= new Vector3(scrollSpeed, 0, 0);

        }

        if (rectTransform.position.x < 0)
            rectTransform.position = new Vector3(0, rectTransform.position.y, rectTransform.position.z);

        if (rectTransform.position.x == 0)
        {

            CheckPlakate();
            isScrolling = false;

        }

    }

    private void CheckPlakate()
    {

        foreach (Plakat plakat in enemyPlakate)
        {

            if (!plakat.isGlued)
            {

                OpenFailedScreen();
                return;

            }


        }

        OpenVictoryScreen();

    }

    private IEnumerator WaitThenStartScrolling()
    {

        yield return new WaitForSeconds(2);
        isScrolling = true;

    }

}
