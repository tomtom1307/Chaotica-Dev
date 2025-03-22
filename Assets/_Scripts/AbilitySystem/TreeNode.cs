using UnityEngine;

public class TreeNode : MonoBehaviour
{
    public AbilitySystemUI ASUI;

    public Ability data;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ASUI = GetComponentInParent<AbilitySystemUI>();
    }

    public void Selected()
    {
        // Holds selected Ability

        ASUI.cursel = this;
    }
}
