using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TreeNode : MonoBehaviour
{
    public AbilitySystemUI ASUI;

    public Ability data;

    public Image outline;

    //Enum of abilitystates

    public enum TreeNodeState
    {
        Locked,
        Unlockable,
        Slottable,
    }

    public TreeNodeState AState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ASUI = GetComponentInParent<AbilitySystemUI>();

        AState = TreeNodeState.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AState = TreeNodeState.Slottable;
        } else if (Input.GetKeyDown(KeyCode.O))
        {
            AState = TreeNodeState.Unlockable;
        } else if (Input.GetKeyDown(KeyCode.P))
        {
            AState = TreeNodeState.Locked;
        }
    }
    public void Selected()
    {
        //Holds selected Ability

        //ASUI.cursel = this;

        switch (AState)
        {
            case TreeNodeState.Locked:
                //Ability is not able to be selected
                break;
            case TreeNodeState.Unlockable:
                //Ability is only able to be unlocked
                break;
            case TreeNodeState.Slottable:
                //Ability is able to be selected
                ASUI.SelectTreeNode(this);
                break;
        }
    }

    public void Highlight(bool x)
    {
        if (ASUI.cursel != this && AState != TreeNodeState.Locked)
        {
            outline.enabled = x;
        }
        
    }
}
