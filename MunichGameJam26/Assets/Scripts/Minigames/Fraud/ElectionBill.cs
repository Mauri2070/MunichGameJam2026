using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public class ElectionBill : MonoBehaviour
{

    [SerializeField] FraudHandler fraudHandler;

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
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject newLine;
    [SerializeField] private LineRenderer line;
    private bool wasDrawing = false;
    private bool drag;

    private void Start()
    {

        fraudHandler = FindFirstObjectByType<FraudHandler>();

    }

    private void LateUpdate()
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

            Debug.Log("Dragging");
            Vector3 position = Input.mousePosition;
            position.z = 0;
            Debug.Log("Position: " + position);
            line.positionCount++;
            Debug.Log("Points: " + line.positionCount);
            line.SetPosition(line.positionCount - 1, position);

            //yield return null;

        }

    }

    public void CreateBill()
    {

        drag = false;
        isUs = false;
        current = false;
        wasDrawing = false;

        Debug.Log("Setting vote");
        SetVote();

        Debug.Log("Setting image");
        GetImage();

    }

    private void SetVote()
    {

        Debug.Log("Rolling vote");

        int vote = Random.Range(1, parties);

        Debug.Log("Vote NR = " + vote);

        if (vote == 1)
        {

            Debug.Log("Vote is for us");
            isUs = true;

        }

    }

    private void GetImage()
    {

        Debug.Log("Rolling image");

        if (isUs)
        {

            Debug.Log("Our image");
            int imageNR = Random.Range(0, ourBills.Count);
            billImage.color = new Color(ourBills[imageNR].color.r, ourBills[imageNR].color.g, ourBills[imageNR].color.b);

        }
        else
        {

            Debug.Log("Enemy image");
            int imageNR = Random.Range(0, otherBills.Count);
            billImage.color = new Color(otherBills[imageNR].color.r, otherBills[imageNR].color.g, otherBills[imageNR].color.b);

        }

        Debug.Log("New Image color is: " + billImage.color.r + " " + billImage.color.g + " " + billImage.color.b);

    }

    public void CreateStamp()
    {

        if (current && !wasDrawing)
        {

            Debug.Log("Create Stamp");
            Image stampInstance = Instantiate(stampPrefab);

            stampInstance.transform.position = Input.mousePosition;
            stampInstance.transform.parent = this.transform;

            if (!isUs)
                fraudHandler.OpenFailedScreen();
            else
            {

                fraudHandler.billsDone++;
                fraudHandler.OnBillsChanged();
                StartCoroutine(WaitThenDestroyThis());

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

        Debug.Log("End Drag");
        drag = false;

        if (wasDrawing)
        {

            if (isUs)
                fraudHandler.OpenFailedScreen();
            else
            {

                fraudHandler.billsDone++;
                fraudHandler.OnBillsChanged();
                StartCoroutine(WaitThenDestroyThis());

            }

        }

    }

    private IEnumerator WaitThenDestroyThis()
    {

        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);

    }

}
