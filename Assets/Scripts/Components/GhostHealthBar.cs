using UnityEngine;
using UnityEngine.UI;

public class GhostHealthBar : MonoBehaviour
{
    [SerializeField] private Image realFill;
    [SerializeField] private Image ghostFill;
    [SerializeField] private float drainSpeed = 0.5f;

    void Update()
    {
        if (realFill == null || ghostFill == null)
        {
            return;
        }
        if (ghostFill.fillAmount > realFill.fillAmount)
        {
            // moves ghostFill.fillAmount towards realFill.Amount
           
            // a little bit each frrame- Mathf.moveTowards (current, target, maxStep)
            //                                         CURRENT: Ghost is   target : Real Bar         Max step this frame
            ghostFill.fillAmount = Mathf.MoveTowards(ghostFill.fillAmount, realFill.fillAmount, drainSpeed * Time.deltaTime);

           
        }
        else
        {
            //TODO 1: Account for health gain later: Completed 
            // in other words can be see as ghost.fillAmount = realFill.fillAmount
            ghostFill.fillAmount = realFill.fillAmount;
        }
    }

 
}
