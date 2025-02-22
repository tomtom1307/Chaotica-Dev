using UnityEngine;

public class RandomizeAnimationSpeed : MonoBehaviour
{
    Animator anim;
    public float LowerBound;
    public float UpperBound;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetFloat("speed", Random.Range(LowerBound, UpperBound));
    }

}
