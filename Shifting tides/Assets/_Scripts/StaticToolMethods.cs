using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class StaticToolMethods
{
    public static AnimatorStateInfo GetAnimatorStateInfo(int layerIndex,Animator animatorTocheck)
    {
        AnimatorStateInfo currentAnimatorStateInfo = animatorTocheck.GetCurrentAnimatorStateInfo(layerIndex);
        return currentAnimatorStateInfo;
    }
    public static IEnumerator DisplaySliderBars(Slider[] sliderBars, float delay,bool display)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (Slider sliderbar in sliderBars)
        {
            sliderbar.gameObject.SetActive(display);
        }
        yield break;
    }

    public static IEnumerator LerpSliderBarValue(Slider[] sliderBars, float targetValue, float foreGroundLerpSpeed, float backGroundLerpSpeed, float updateSpeed)
    {

        while (Mathf.Abs(sliderBars[1].value - targetValue) > 0.3f)
        {

            if (sliderBars[0].value != targetValue)
            {
                sliderBars[0].value = Mathf.Lerp(sliderBars[0].value, targetValue, foreGroundLerpSpeed);
            }
            sliderBars[1].value = Mathf.Lerp(sliderBars[1].value, sliderBars[0].value, backGroundLerpSpeed);
            yield return new WaitForSeconds(updateSpeed);
        }
        yield break;
    }

}
