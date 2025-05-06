using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShatteredObject : MonoBehaviour
{
    public List<GameObject> SubPieces;
    public UnityEvent OnStart;
    private void Start()
    {
        Invoke("DoDeletion", 1f);
        OnStart.Invoke();
    }


    public void DoDeletion()
    {
        foreach (var piece in SubPieces)
        {
            piece.transform.DOScale(0, 0.5f);
            
        }

        Destroy(gameObject,4);
    }


}
