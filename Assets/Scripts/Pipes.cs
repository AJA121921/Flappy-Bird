using UnityEngine;

public class Pipes : MonoBehaviour
{    
    
    public float speed = 0.05f;
    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 0.05f;
    }


    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge) 
        {
            Destroy(gameObject);
        }
    }
}
