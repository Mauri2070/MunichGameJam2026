using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FraudHandler : MinigameHandler
{

    [Header("Bills")]
    [SerializeField] ElectionBill[] electionBills;
    [SerializeField] private GameObject billOrigin;
    [SerializeField] private ElectionBill billPrefab;
    [SerializeField] private int distanceToPrevious = 5;
    public int billsDone = 0;
    private int activeBillNR = 4;

    protected override void StartMinigame()
    {

        base.StartMinigame();

        foreach (Transform child in billOrigin.transform)
            GameObject.Destroy(child.gameObject);
        billsDone = 0;
        activeBillNR = 4;

        electionBills = new ElectionBill[4];

        for (int i = 0; i < electionBills.Length; i++)
        {

            electionBills[i] = billPrefab;

            Vector3 billTransformPosition = new Vector3(billOrigin.transform.position.x + (i * distanceToPrevious), billOrigin.transform.position.y + (i * distanceToPrevious), 0);

            ElectionBill billInstance = Instantiate(electionBills[i], billOrigin.transform);
            billInstance.transform.position = billTransformPosition;
            billInstance.transform.Rotate(new Vector3(0, 0, Random.Range(-20, 20)));

            electionBills[i] = billInstance;
            electionBills[i].billNR = i + 1;

            if (electionBills[i].billNR == activeBillNR)
            {

                electionBills[i].current = true;
                MoveBill(electionBills[i]);

            }
            else
                electionBills[i].current = false;

        }

    }

    private void MoveBill(ElectionBill bill)
    {

        bill.transform.position = centerPosition.transform.position;
        bill.CreateBill();

    }

    public virtual void OnBillsChanged()
    {

        Debug.Log("On Bills Changed");

        if (billsDone == electionBills.Length)
            OpenVictoryScreen();
        else
        {

            Debug.Log("Last active number: " + activeBillNR);
            activeBillNR--;
            Debug.Log("New active number: " + activeBillNR);

            foreach (ElectionBill bill in electionBills)
            {

                if (bill.billNR == activeBillNR)
                {

                    bill.current = true;
                    MoveBill(bill);

                }
                else
                    bill.current = false;

            }

        }

    }

    protected override void EndMinigame()
    {

        base.EndMinigame();


    }

}
