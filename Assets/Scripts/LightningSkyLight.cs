using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class LightningSkyLight : MonoBehaviour
{
    Vector3 StartPos;
    


    float timer;
    public float freq;
    public float randomDelay;
    public float speed;
    public float Intensity;
    Light light;
    bool triggeredTween;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Store Start position
        StartPos = transform.position;

        //Generate a random delay so lights dont enable simultaneously
        randomDelay = Random.Range(0, 10);


        //Get components and initialize
        light = GetComponent<Light>();
        light.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //incrememnt timer and make the flash
        timer += Time.deltaTime;
        if (timer >= freq + randomDelay && !triggeredTween)
        {
            triggeredTween = true;
            float angle = 0;
            DOVirtual.Float(0, 180, speed, angle => {
            light.intensity = Intensity * Mathf.Sin(angle * Mathf.Deg2Rad);}).OnComplete(DoLightningReset);


        }


    }


    

    void DoLightningReset()
    {
        randomDelay = Random.Range(0, 5);
        transform.position = StartPos + new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        triggeredTween = false;
        timer = 0;
    }



}
