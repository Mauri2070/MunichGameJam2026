using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ElectionBill : MonoBehaviour
{

    public bool isUs = false;
    private int parties = 3;

    [SerializeField] List<Image> ourBills;
    [SerializeField] List<Image> otherBills;

    [SerializeField] Image billImage;

    public void CreateBill()
    {

        isUs = false;

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

}
