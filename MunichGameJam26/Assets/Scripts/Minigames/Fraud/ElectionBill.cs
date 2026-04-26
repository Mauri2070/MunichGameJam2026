using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.EventSystems;

public class ElectionBill : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IPointerExitHandler, IBeginDragHandler
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
    [SerializeField] private GameObject lineFixPrefab;
    public AudioClip scribbleSound;
    private bool wasDrawing = false;
    private bool drag;

    AudioSource audioSource;

    private void Start()
    {

        fraudHandler = FindFirstObjectByType<FraudHandler>();
        audioSource = GetComponent<AudioSource>();

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (current && !wasDrawing)
        {

            Image stampInstance = Instantiate(stampPrefab, transform);
            //Debug.Log(eventData.position);
            stampInstance.transform.position = eventData.position;
            audioSource.PlayOneShot(stampSound);
            //Debug.DebugBreak();

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

    public void OnPointerExit(PointerEventData eventData)
    {
        EndDrag();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (current)
        {

            drag = true;
            wasDrawing = true;

            newLine = Instantiate(linePrefab, this.transform);
            Vector3 linePosition = eventData.position;
            linePosition.z = 0;
            newLine.transform.SetPositionAndRotation(linePosition, Quaternion.identity);
            line = newLine.GetComponent<LineRenderer>();
            line.positionCount = 1;
            line.SetPosition(0, newLine.transform.position);
            audioSource.PlayOneShot(scribbleSound);

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (drag)
        { 
            Vector3 position = eventData.position;
            position.z = -100;
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, position);

            GameObject go = Instantiate(lineFixPrefab, transform);
            go.transform.position = position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Break();
        EndDrag();
    }

    public void EndDrag()
    {

        audioSource.Stop();
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

        // Debug
        //yield return new WaitForSeconds(5f);

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
