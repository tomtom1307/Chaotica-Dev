using UnityEngine;

public class EAIState_Trigger
{
    
}

public enum EAI_TriggerID
{
    TookDamage = 1
}

public struct EAI_Trigger
{
    public EAI_TriggerID TriggerID;
    public IEAI_TriggerPayload Payload;

    public EAI_Trigger(EAI_TriggerID triggerID, IEAI_TriggerPayload payload)
    {
        TriggerID = triggerID;
        Payload = payload;
    }
}

public interface IEAI_TriggerPayload { }

public sealed class EAI_DamagePayload : IEAI_TriggerPayload
{
    public Transform whoCaused;
    public Vector3 HitPoint;
    public float Amount;
}




