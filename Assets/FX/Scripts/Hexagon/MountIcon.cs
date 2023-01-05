using UnityEngine;
using DG.Tweening;

public class MountIcon : MonoBehaviour
{
    public WorkHexagon mountIconPrefab;
    public WorkHexagon scaledIconPrefab;
    private Vector3 scl;

    private void Start()
    {
        scl = scaledIconPrefab.transform.localScale;
        scaledIconPrefab.transform.localScale = Vector3.zero;
        scaledIconPrefab.transform.DOScale(scl, 0.1f).SetEase(Ease.Linear);
    }
}
