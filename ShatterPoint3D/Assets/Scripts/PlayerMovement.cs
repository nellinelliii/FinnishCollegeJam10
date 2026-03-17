using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        // Get input from the horizontal and vertical axes
        float h = Input.GetAxis("Horizontal");

        float v = Input.GetAxis("Vertical");

        // Move the player based on input
        Vector3 move = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}
