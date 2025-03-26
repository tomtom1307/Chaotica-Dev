using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TreeNode : MonoBehaviour
{
    public AbilitySystemUI ASUI;

    public Ability data;

    public Image outline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ASUI = GetComponentInParent<AbilitySystemUI>();
    }

    public void Selected()
    {
        //Holds selected Ability

        //ASUI.cursel = this;

        ASUI.SelectTreeNode(this);
    }

    public void Highlight(bool x)
    {
        if (ASUI.cursel != this)
        {
            outline.enabled = x;
        }
        
    }
}
