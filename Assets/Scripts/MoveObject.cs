using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] public float speed = 3f;

    private void Update()
    {
        // Move to right
        if ((Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D))))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        // Move to left
        if ((Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A))))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        // Move up
        if ((Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.W))))
        {
            transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
        }
        // Move down
        if ((Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S))))
        {
            transform.position -= new Vector3(0f, 0f, speed * Time.deltaTime);
        }
    }
}
