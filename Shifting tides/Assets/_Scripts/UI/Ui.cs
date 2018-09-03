using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    public Enemy currentTarget;
    public Slider[] sliders;
    public Text[] texts;
    public Image[] dashCharges;
    public GameObject[] arrowHeadIcons;

    private bool tracking;
    private Slider[] targetStatusSliders = new Slider[4];
    private IEnumerator statusbarVisibilityCoroutine;

    public void DisplayCurrentArrowHead(GameObject currentArrowHead)
    {
        foreach (GameObject arrowheadIcon in arrowHeadIcons)
        {
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

    public void Update()
    {
        if (tracking) {
            int j = 0;
            for (int i = 3; i < 6; i++)
            {
                sliders[i].value = targetStatusSliders[j].value;
                j++;
            }
        }
    }

    public void TrackTargetEnemyStatus(string name, Slider[] healthbars, Slider[] armorbars)
    {
        texts[3].text = name;

        //statusSliders = null ?

        Array.Clear(targetStatusSliders, 0, targetStatusSliders.Length);
        healthbars.CopyTo(targetStatusSliders, 0);
        armorbars.CopyTo(targetStatusSliders, healthbars.Length);

        Slider[] displayStatusSliders = new Slider[4];

        int j = 0;
        for (int i = 3; i < 6; i++)
        {
            sliders[i].gameObject.SetActive(true);
            sliders[i].maxValue = targetStatusSliders[j].maxValue;
            sliders[i].value = targetStatusSliders[j].value;
            displayStatusSliders[i] = sliders[i];
            j++;
        }

        if (statusbarVisibilityCoroutine != null)
        {
            StopCoroutine(statusbarVisibilityCoroutine);
        }
        statusbarVisibilityCoroutine = StaticToolMethods.DisplaySliderBars(displayStatusSliders, 6,false);
        StartCoroutine(statusbarVisibilityCoroutine);
        tracking = true;
    }
}
