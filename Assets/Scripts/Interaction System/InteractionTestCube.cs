using UnityEngine;

public class InteractionTestCube : MonoBehaviour
{

    public GameObject particles;
    public Color color;
    public void SpawnParticles()
    {
        var f = Instantiate(particles, transform.position, Quaternion.identity);
        f.GetComponent<ParticleSystem>().startColor = color;
        Destroy(f,2);

    }
}
