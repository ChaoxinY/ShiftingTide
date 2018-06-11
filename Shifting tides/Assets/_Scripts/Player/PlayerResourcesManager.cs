using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourcesManager : MonoBehaviour
{
    /// <summary>
    ///  0 : health , 1 : sourceReserve, 2 : jumps , 3 : dashes 4 : arrows 5： SourceFused Arrows
    /// </summary>
    private static float[] playerResources = new float[6];
    private static int scrapSource;

    /// <summary>
    ///  0 : health , 1 : sourceReserve, 2 : jumps , 3 : dashes 4 : arrows
    /// </summary>
    public static float[] playerResourcesCaps = { 100, 40, 1, 0, 100, 3 };
    public static Ui ui;

    private void Start()
    {
        ui = GameObject.Find("UI").GetComponent<Ui>();
        Health = playerResourcesCaps[0];
        SourceReserve = 0;
        JumpsLeft = playerResourcesCaps[2];
        Dashes = playerResourcesCaps[3];
        Arrows = 100;
        SourceFusedArrows = 0;
        ScrapSource = 0;

    }

    /// <summary>
    /// Check if there is enough resource left to carry out this action. 
    ///  0 : health , 1 : sourceReserve, 2 : jumps , 3 : dashes 4 : arrows
    /// </summary>
    public static bool IsThereEnoughResource(int resourceNumber, int resourceMin)
    {
        bool thereIsEnoughResource = false;
        if (playerResources[resourceNumber] > resourceMin)
        {
            thereIsEnoughResource = true;
        }
        return thereIsEnoughResource;
    }

    public static bool IsThisResourceAtMax(int resourceNumber)
    {
        bool resourceAtMax = false;
        if (playerResources[resourceNumber] == playerResourcesCaps[resourceNumber])
        {
            resourceAtMax = true;
        }
        return resourceAtMax;
    }

    //public static bool AllResourceAtMax() {

    //    bool resourceAtMax = true;
    //    for (int i = 0; i < 4; i++) {
    //        if(!IsThisResourceAtMax(i))
    //            resourceAtMax = false;
    //    }
    //    return resourceAtMax;
    //}

    public static void AddScrapSource()
    {
        ScrapSource += 1;
    }
    public static float Health
    {
        get
        {
            return playerResources[0];
        }
        set
        {
            playerResources[0] = value;
            ui.sliders[1].value = Health;
            if (playerResources[0] < 0)
            {
                playerResources[0] = 0;
            }
            if (playerResources[0] > playerResourcesCaps[0])
            {
                playerResources[0] = playerResourcesCaps[0];
                if (!IsThisResourceAtMax(5)) AddScrapSource();
            }
        }
    }
    public static float SourceReserve
    {
        get { return playerResources[1]; }
        set
        {
            playerResources[1] = value;
            if (playerResources[1] < 0)
            {
                playerResources[1] = 0;
            }
            else if (playerResources[1] > playerResourcesCaps[1])
            {
                playerResources[1] = playerResourcesCaps[1];
                if(!IsThisResourceAtMax(5)) AddScrapSource();
            }
            ui.sliders[2].value = SourceReserve;
        }
    }
    public static float JumpsLeft
    {
        get
        {
            return playerResources[2];
        }
        set
        {
            playerResources[2] = value;           
            if
                (playerResources[2] < 0)
            {
                playerResources[2] = 0;
            }
            if (playerResources[2] > playerResourcesCaps[2])
            {
                playerResources[2] = playerResourcesCaps[2];
                if (!IsThisResourceAtMax(5)) AddScrapSource();
            }
            ui.sliders[0].value = JumpsLeft;
        }
    }

    public static float Dashes
    {
        get
        {
            return playerResources[3];
        }
        set
        {
            playerResources[3] = value;
            if (playerResources[3] < 0)
            {
                playerResources[3] = 0;
            }
            else if (playerResources[3] > playerResourcesCaps[3])
            {
                playerResources[3] = playerResourcesCaps[3];
                if (!IsThisResourceAtMax(5)) AddScrapSource();
            }
        }
    }

    public static float Arrows
    {
        get
        {
            return playerResources[4];
        }
        set
        {
            playerResources[4] = value;
            ui.texts[0].text = playerResources[4].ToString();
            if (playerResources[4] < 0)
            {
                playerResources[4] = 0;
            }
            else if (playerResources[4] > playerResourcesCaps[4])
            {   
                playerResources[4] = playerResourcesCaps[4];
            }
        }
    }
    public static float SourceFusedArrows
    {
        get
        {
            return playerResources[5];
        }
        set
        {
            playerResources[5] = value;
            ui.texts[1].text = playerResources[5].ToString();
            if (playerResources[5] < 0)
            {
                playerResources[5] = 0;
            }
            else if (playerResources[5] > playerResourcesCaps[5])
            {
                playerResources[5] = playerResourcesCaps[5];
            }
        }
    }
    public static int ScrapSource
    {
        get
        {
            return scrapSource;
        }
        set
        {
            scrapSource = value;
            ui.texts[2].text = scrapSource.ToString();

            if (scrapSource < 0)
            {
                scrapSource = 0;
            }
            else if (IsThisResourceAtMax(5)) {
                scrapSource = 0;
                return;
            }
            else if (scrapSource > 10)
            {
                SourceFusedArrows += 1;
                scrapSource = 0;
            }
        }
    }
   
}
