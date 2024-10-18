using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] public float speed = 3f;

    private void Update()
    {
        // Move to right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        // Move to left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        // Move up
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
        }
        // Move down
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0f, 0f, speed * Time.deltaTime);
        }
    }
}
