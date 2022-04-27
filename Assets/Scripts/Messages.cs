using UnityEngine;

public class Messages : MonoBehaviour
{
    public float duration;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
