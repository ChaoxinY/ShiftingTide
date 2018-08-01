using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    private BasicMovement basicMovement;
    public Enemy currentTarget;
    public Slider[] sliders;
    public Text[] texts;
    public Image[] dashCharges;
    public GameObject[] arrowHeadIcons;

    void Start()
    {
        basicMovement = GameObject.Find("Player").GetComponent<BasicMovement>();
    }

    public void DisplayCurrentArrowHead(GameObject currentArrowHead)
    {
        foreach (GameObject arrowheadIcon in arrowHeadIcons) {
            arrowheadIcon.SetActive(false);
        }
        switch (currentArrowHead.name)
        {
            case "DefaultArrow":
                arrowHeadIcons[0].SetActive(true);
                break;
            case "TimeZoneArrow":
                arrowHeadIcons[1].SetActive(true);
                break;
            case "CloudArrow":
                arrowHeadIcons[2].SetActive(true);
                break;
        }

    }


}
