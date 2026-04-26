using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public class ElectionBill : MonoBehaviour
{

    [SerializeField] private FraudHandler fraudHandler;

    public bool isUs = false;
    private int parties = 3;
    public bool current = false;
    public int billNR = 0;

    [Header("Bills")]
    [SerializeField] List<Image> ourBills;
    [SerializeField] List<Image> otherBills;

    [SerializeField] Image billImage;

    [Header("Stamp and Lines")]
    [SerializeField] private Image stampPrefab;
    [SerializeField] private AudioClip stampSound;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject newLine;
    [SerializeField] private LineRenderer line;
    [SerializeField] private AudioClip scribbleSound;
    private bool wasDrawing = false;
    private bool drag;

    AudioSource audioSource;
    private void Start()
    {

        fraudHandler = FindFirstObjectByType<FraudHandler>();
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {

        if (drag)
        {

            newLine = Instantiate(linePrefab, this.transform);
            Vector3 linePosition = Input.mousePosition;
            linePosition.z = 0;
            newLine.transform.position = linePosition;
            newLine.transform.rotation = Quaternion.identity;

            line = newLine.GetComponent<LineRenderer>();
            line.positionCount = 0;

            Vector3 position = Input.mousePosition;
            position.z = 0;
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, position);

            //yield return null;

        }

    }

    public void CreateBill()
    {

        drag = false;
        isUs = false;
        wasDrawing = false;

        SetVote();

        GetImage();

    }

    private void SetVote()
    {

        int vote = Random.Range(1, parties);

        if (vote == 1)
        {

            isUs = true;

        }

    }

    private void GetImage()
    {

        if (isUs)
        {

            int imageNR = Random.Range(0, ourBills.Count);
            billImage.sprite = ourBills[imageNR].sprite;

        }
        else
        {

            int imageNR = Random.Range(0, otherBills.Count);
            billImage.sprite = otherBills[imageNR].sprite;

        }

    }

    public void CreateStamp()
    {

        if (current && !wasDrawing)
        {

            Image stampInstance = Instantiate(stampPrefab);

            stampInstance.transform.position = Input.mousePosition;
            stampInstance.transform.SetParent(this.transform);

            if (!isUs)
                StartCoroutine(WaitThenDestroyThis(false));
            else
            {

                Debug.Log("Stamp");
                StartCoroutine(WaitThenDestroyThis(true));

            }

            current = false;

        }


    }

    public void BeginDrag()
    {

        if (current)
        {

            drag = true;
            wasDrawing = true;

        }

    }

    public void EndDrag()
    {

        drag = false;

        if (wasDrawing)
        {

            if (isUs)
                StartCoroutine(WaitThenDestroyThis(false));
            else
            {
                Debug.Log("Drag");

                StartCoroutine(WaitThenDestroyThis(true));

            }

        }

    }

    public void CheckResult()
    {

        if (wasDrawing && !isUs)
            StartCoroutine(WaitThenDestroyThis(true));
        else
            StartCoroutine(WaitThenDestroyThis(false));

    }

    private IEnumerator WaitThenDestroyThis(bool success)
    {

        fraudHandler.currentBill = null;

        yield return new WaitForSeconds(0.5f);

        if (!success)
        {

            fraudHandler.mainHandler.OpenFailedScreen();

        }
        else
        {

            fraudHandler.billsDone++;
            fraudHandler.OnBillsChanged();

        }

        Destroy(this.gameObject);

    }

}
