using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera mainCamera;
    public Transform playerHead;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateToMouse();
    }

    void RotateToMouse()
    {
        // Get mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, playerHead.position); // Plane at object's height

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - playerHead.position).normalized;

            // Rotate towards the mouse
            Quaternion lookRotation = Quaternion.LookRotation(-direction, Vector3.up);
            playerHead.rotation = Quaternion.Slerp(playerHead.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}
