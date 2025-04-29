using UnityEngine;
using TRTools;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
public class DetectionIndicator : MonoBehaviour
{
    public EnemyBrain enemy;
    public Vector3 DetectionLocation;
    public Transform PlayerObject;
    public Transform DetectionImagePivot;
    public Image AgroImage;
    public Image FillImage;
    public bool Agro;
    public CanvasGroup DetectionImageCanvas;
    public float FadeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Agro = false;
        FillImage.fillAmount = enemy.perception.DetectionMeter;

        PlayerObject = Camera.main.GetComponentInParent<CameraController>().orientation;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null || enemy.perception.DetectionMeter < 0.1f)
        {
            StartCoroutine(FadeOut());
            return;
        }
        FillImage.fillAmount = enemy.perception.DetectionMeter + 0.1f;
        
        DetectionLocation = enemy.transform.position;
        DetectionLocation.y = PlayerObject.position.y;
        Vector3 Direction = VecOp.Direction(DetectionLocation, PlayerObject.position);
        float angle = (Vector3.SignedAngle(Direction, PlayerObject.forward, Vector3.up));
        DetectionImagePivot.transform.localEulerAngles = new Vector3(0,0, angle);
    }

    
    public void IsAgro(bool tf)
    {
        Agro = tf;
        AgroImage.enabled = tf;
    }

    public IEnumerator FadeOut()
    {
        float timer = FadeTime;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            DetectionImageCanvas.alpha = Mathf.Clamp01(timer / FadeTime);

            yield return null; // Wait one frame
        }
        HUDController.instance.DetectionIndicators.Remove(this);
        DetectionImageCanvas.alpha = 0f; // Make sure it ends at 0
        Destroy(this.gameObject);
    }

}
