using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitHandler : MonoBehaviour
{
    [SerializeField]
    private Animator portraitAnim;

    private float expressionTime;
    private bool isExpressing = false;

    private void Update()
    {
        expressionTime -= Time.deltaTime;
        if(expressionTime < 0.0f && isExpressing)
        {
            portraitAnim.SetTrigger("Neutral");
            isExpressing = false;
        }
    }

    public void ChangePortraitElated(float time)
    {
        portraitAnim.SetTrigger("Elated");
        expressionTime = time;
        isExpressing = true;
    }

    public void ChangePortraitVictorious(float time)
    {
        portraitAnim.SetTrigger("Victorious");
        expressionTime = time;
        isExpressing = true;
    }
}
