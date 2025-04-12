using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredObject : MonoBehaviour
{
    public List<GameObject> SubPieces;

    private void Start()
    {
        Invoke("DoDeletion", 4f);
    }


    public void DoDeletion()
    {
        foreach (var piece in SubPieces)
        {
            piece.transform.DOScale(0, 0.5f);
            
        }

        Destroy(this);
    }


}
