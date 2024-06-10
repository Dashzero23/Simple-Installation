using UnityEngine;

public class Puck : MonoBehaviour
{
    public GameObject collideEffect; // Reference to the particle effect prefab
    private Rigidbody rb; // Reference to the puck's Rigidbody
    public float forceAmount = 10f; // The amount of force to apply to the puck when space bar is pressed

    void Awake()
    {
        // Initialize the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player presses the space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call the function to add force toward the mouse cursor position
            AddForceTowardMouse();
        }
    }

    // Function to add force toward the mouse cursor position
    void AddForceTowardMouse()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates (z = 10 is an example; adjust as needed)
        mousePosition.z = 10f; // Distance from camera to the puck (adjust as needed)
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the puck to the mouse cursor position
        Vector3 direction = worldMousePosition - transform.position;
        direction.y = 0f; // Ensure the force is only in the x/z plane

        // Normalize the direction
        direction.Normalize();

        // Apply force in the calculated direction with the specified amount
        rb.AddForce(direction * forceAmount, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the puck collided with an edge of the board
        if (collision.gameObject.CompareTag("BoardEdge"))
        {
            // Get the contact point and normal of the collision
            ContactPoint contact = collision.GetContact(0);
            Vector3 collisionPoint = contact.point;
            Vector3 collisionNormal = contact.normal;

            // Reflect the puck's velocity based on the collision normal
            Vector3 reflectedDirection = Vector3.Reflect(rb.velocity.normalized, collisionNormal);

            // Apply force in the reflected direction
            rb.AddForce(reflectedDirection * forceAmount/2, ForceMode.Impulse);
            // Spawn the collideEffect at the collision point with the edge's rotation
            GameObject collideEFfect = Instantiate(collideEffect, collisionPoint, collision.gameObject.transform.rotation);
            Destroy(collideEFfect, 1.5f);
        }
    }
}