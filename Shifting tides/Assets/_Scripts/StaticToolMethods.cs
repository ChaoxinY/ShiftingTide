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

    public static int GenerateARandomNumber(int numbersToChoseFrom) {
      
        int[] numbersCorrect = new int[numbersToChoseFrom];

        //rolls for each attribute number of times
        for (int i = 0; i < 20; i++)
            for (int j = 0; j < numbersCorrect.Length - 1; j++)
            {
                int guessedNumber = Random.Range(0, 99);
                if (guessedNumber > 30) numbersCorrect[j]++;
            }
        //Example outcome: numbersCorret{21,22,12}

        //Sort
        //Determine which slot has the highest number
        int randomNumber = 0;

        for (int i = 1; i < numbersCorrect.Length; i++)
        {
            int maxValue = numbersCorrect[0];
            if (numbersCorrect[i] > maxValue)
            {
                maxValue = numbersCorrect[i];
                randomNumber = i;
            }
        }
        return randomNumber;
    }

}
