using UnityEngine;

public class InteractionTestCube : MonoBehaviour
{

    public GameObject particles;
    public Color color;
    public void SpawnParticles()
    {
        var f = Instantiate(particles, transform.position, Quaternion.identity);
#pragma warning disable CS0618 // Type or member is obsolete
        f.GetComponent<ParticleSystem>().startColor = color;
#pragma warning restore CS0618 // Type or member is obsolete
        Destroy(f,2);

    }
}
