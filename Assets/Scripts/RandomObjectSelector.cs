using UnityEngine;

public class RandomObjectSelector : MonoBehaviour
{
    public int CountToLeave = 1;

    private void Start()
    {
        while (transform.childCount > CountToLeave)
        {
            var childToDestroy = transform.GetChild(Random.Range(0, transform.childCount));
            DestroyImmediate(childToDestroy.gameObject);
        }
    }
}
