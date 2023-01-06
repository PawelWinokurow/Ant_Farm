using UnityEngine;
using DG.Tweening;

public class MountIcon : MonoBehaviour
{
    public WorkHexagon mountIconPrefab;
    public WorkHexagon scaledIconPrefab;
    public Vector3 scl;


    private void Start()
    {
        scl = scaledIconPrefab.transform.localScale;
        scaledIconPrefab.transform.localScale = Vector3.zero;
        scaledIconPrefab.transform.DOScale(scl, 0.05f).SetEase(Ease.Linear);
    }
}
