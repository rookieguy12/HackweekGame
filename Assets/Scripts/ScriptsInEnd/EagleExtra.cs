using UnityEngine;

public class EagleExtra : MonoBehaviour
{
    private Rigidbody2D rd;
    public Transform up;
    public Transform down;
    public float speed;
    private float upy, downy;
    public bool fly;
    public Transform head;
    public LayerMask ground;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        rd.velocity = new Vector2(rd.velocity.x, -speed);
        upy = up.position.y;
        downy = down.position.y;
        Destroy(up.gameObject);
        Destroy(down.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
            if (transform.position.y < downy)
            {
                rd.velocity = new Vector2(rd.velocity.x, speed);
            }
            if (transform.position.y > upy)
            {
                rd.velocity = new Vector2(rd.velocity.x, -speed);
            }
            if (rd.velocity.x != 0)
            {
                rd.velocity = new Vector2(0, rd.velocity.y);
            }
            if (rd.velocity.y == 0)
            {
                rd.velocity = new Vector2(0, speed);
            }
    }
}