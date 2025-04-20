using UnityEngine;
using TRTools;
public class ChoasCore : MonoBehaviour
{
    public float Attraction;
    Transform player;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameManager.instance.player;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float r = (Vector3.Distance(player.position, transform.position));
        rb.AddForce((Attraction * Mathf.Clamp((1 / Mathf.Pow(4*r, 1 / 2)) - 0.3f, 0, 10000) * (VecOp.Direction(player, transform))));
        if(r < 2)
        {
            player.GetComponent<PlayerInventory>().AddChaosCores(1);
            Destroy(gameObject);
        }
    }

}

