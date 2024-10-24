using UnityEngine;

public class MoveObject : MonoBehaviour
{
    
    [Header("Settings")]
    public int numberOfCase = 1;

    private void Update()
    {
        // Move to right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(numberOfCase, 0f, 0f);
        }
        // Move to left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(numberOfCase, 0f, 0f);
        }
        // Move up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0f, 0f, numberOfCase);
        }
        // Move down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0f, 0f, numberOfCase);
        }
    }
}
