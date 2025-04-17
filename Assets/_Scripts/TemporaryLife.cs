using UnityEngine;

public class TemporaryLife : MonoBehaviour
{
    public float Lifetime = 2;

    public void Start()
    {
        Destroy(gameObject, Lifetime);
    }
}
