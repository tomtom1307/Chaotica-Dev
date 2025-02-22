using UnityEngine;

public class ThunderSoundBox : MonoBehaviour
{
    AudioSource AS;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void PlayThunder()
    {
        AS.pitch = Random.Range(0.5f, 1.2f);
        //AS.panStereo = Random.Range(-1f, 1f);
        AS.volume = Random.Range(0f, 0.1f);
        AS.Play();
    }



}
