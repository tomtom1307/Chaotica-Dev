using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text text;
    public float DestroyTime;
    Vector3 upVector;
    public float PopupTime = 0.2f;
    public float Size;
    public float ExistanceTime = 0.2f;
    public float MoveAmount;
    public float OffsetAmount;
    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.LookAt(Camera.main.transform.position, upVector);
        transform.position += Random.Range(-OffsetAmount, OffsetAmount) * transform.right;
        transform.DOScale(Size*Vector3.one, PopupTime).SetEase(Ease.OutElastic);

        DOTween.Sequence().Append(transform.DOMoveY(transform.position.y + MoveAmount, ExistanceTime)).Append(transform.DOScale(Vector3.zero, PopupTime));
        Destroy(gameObject, DestroyTime);
        upVector = Vector3.up + Random.Range(-.2f, .2f) * Vector3.right;
        
        Destroy(gameObject, DestroyTime);
    }

    public void SetValue(float value)
    {
        text.text = value.ToString();
    }

    private void Update()
    {
        transform.LookAt(Camera.main.transform.position, upVector);
    }

}
