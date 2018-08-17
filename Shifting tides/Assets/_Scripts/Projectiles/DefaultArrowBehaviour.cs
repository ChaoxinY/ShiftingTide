using UnityEngine;
using System.Collections;

public class DefaultArrowBehaviour : ArrowBehaviour
{
    public override void ApplyArrowStageValues(int stage)
    {
       
        switch (stage) {
            case 0:
                arrowSpeed = 1f;
                baseDamage = 0;
                break;
            case 1:
               arrowSpeed = 50f;
               baseDamage = 10;
                break;
            case 2:
                arrowSpeed = 60f;
                baseDamage = 25;
                break;
            case 3:
                arrowSpeed = 80f;
                baseDamage = 50;
                break;
            case 4:
                arrowSpeed = 150f;
                baseDamage = 85;
                break;

        }
    }
}
