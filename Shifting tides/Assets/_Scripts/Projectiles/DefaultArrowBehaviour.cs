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
               arrowSpeed = 25f;
               baseDamage += 10;
                break;
            case 2:
                arrowSpeed = 35f;
                baseDamage += 15;
                break;
            case 3:
                arrowSpeed = 60f;
                baseDamage += 25;
                break;
            case 4:
                arrowSpeed = 80f;
                baseDamage += 35;
                break;

        }
    }
}
