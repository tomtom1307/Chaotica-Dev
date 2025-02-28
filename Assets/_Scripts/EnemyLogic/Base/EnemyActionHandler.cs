using DG.Tweening;
using UnityEngine;

public class EnemyActionHandler : MonoBehaviour
{
    [HideInInspector] public EnemyBrain brain;
    public bool DoingAction;

    public void StartAction(System.Action someFunction)
    {
        if (!DoingAction)
        {
            DoingAction = true; // Set the action flag to true, so no other actions can be started

            someFunction.Invoke();
        }
    }

    public void EndAction()
    {
        DoingAction = false;
    }

    public void DoAFlip()
    {
        transform.DOJump(transform.position + transform.forward, brain.JumpPower, 7, 1).OnComplete(EndAction);
    }

    public void DoError()
    {
        Debug.LogError("This action does not exist");
    }


    

}
