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

        Debug.Log("Create Bill list");
        electionBills = new ElectionBill[4];

        Debug.Log("Filling bills");

        for (int i = 0; i < electionBills.Length; i++)
        {

            Debug.Log("Bill " + i);
            electionBills[i] = billPrefab;


            Debug.Log("Create Bill");

            Vector3 billTransformPosition = new Vector3(billOrigin.transform.position.x + (i * distanceToPrevious), billOrigin.transform.position.y + (i * distanceToPrevious), 0);

            ElectionBill billInstance = Instantiate(electionBills[i], billOrigin.transform);
            billInstance.transform.position = billTransformPosition;

            billInstance.CreateBill();

            electionBills[i] = billInstance;
            electionBills[i].billNR = i + 1;

            if (electionBills[i].billNR == activeBillNR)
                electionBills[i].current = true;
            else
                electionBills[i].current = false;

        }

    }

    public virtual void OnBillsChanged()
    {

        Debug.Log("On Bills Changed");

        if (billsDone == electionBills.Length)
            OpenVictoryScreen();
        else
        {

            activeBillNR--;
            
            foreach (ElectionBill bill in electionBills)
            {

                if (bill.billNR == activeBillNR)
                {

                    bill.current = true;

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
