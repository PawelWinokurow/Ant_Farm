using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutCellSize : MonoBehaviour
{
    private  RectTransform rt;
    private GridLayoutGroup grid;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(rt.rect.width / grid.transform.childCount, rt.rect.height);
    }


    #if UNITY_EDITOR
    private void Update()
    {
        grid.cellSize = new Vector2(rt.rect.width/ grid.transform.childCount, rt.rect.height);
    }
#endif
}
