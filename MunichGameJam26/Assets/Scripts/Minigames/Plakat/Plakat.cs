using UnityEngine;
using UnityEngine.UI;

public class Plakat : MonoBehaviour
{

    [Header("Button")]
    [SerializeField] Button button;

    [Header("Plakat")]
    [SerializeField] GameObject gluedPlakat;

    [Header("Result")]
    public bool isGlued = false;

    public void GluePlakat()
    {

        gluedPlakat.SetActive(true);
        gluedPlakat.transform.Rotate(new Vector3(0, 0, Random.Range(-20, 20)));
        isGlued = true;
        button.enabled = false;

    }

}
