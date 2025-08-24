
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "ChainLightning", menuName = "Abilities/ChainLightning")]
public class ChainLightning : Ability
{
    public float DPS = 3;
    public int MaxEnemiesInChain = 1;
    public float JumpRange = 10;
    public float MaxViewAngle = 15;
    [SerializeField] GameObject LightningPrefab;
    //time between each hop
    public float LinkDelay = 0.12f;

    List<GameObject> spawnedLRs = new List<GameObject>();


    public override bool Check(GameObject parent, AbilityHolder holder)
    {
        var target = GetBestEnemy(JumpRange, MaxViewAngle, parent);
        return target != null && base.Check(parent, holder);
    }

    public override void Activate(GameObject parent, AbilityHolder holder)
    {
        base.Activate(parent, holder);

        // Ensure there is a runtime component on the caster
        var rt = parent.GetComponent<ChainLightningRuntime>();
        if (rt == null) rt = parent.AddComponent<ChainLightningRuntime>();

        // Initialize and begin
        rt.Init(this, holder, LightningPrefab);
        rt.Begin();
    }

    public override void AbilityUpdate(GameObject parent, AbilityHolder holder)
    {
        base.AbilityUpdate(parent, holder);
    }

    public override void Deactivate(GameObject parent, AbilityHolder holder)
    {
        base.Deactivate(parent, holder);

        var rt = parent.GetComponent<ChainLightningRuntime>();
        if (rt != null) rt.End();
    }






    public void NewLineRenderer(Transform startPos, Transform endPos, bool fromPlayer = false)
    {
        GameObject lineRend = Instantiate(LightningPrefab);
        Debug.Log(lineRend);
        spawnedLRs.Add(lineRend);
        holder.StartCoroutineUpdateLineRenderer(lineRend, startPos, endPos, fromPlayer);
    }



}



