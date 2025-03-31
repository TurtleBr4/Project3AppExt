using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player's transform
    public Transform playerTarget;
    public float offsetY = 4.75f;
    public float rotation = 60f;
    public float followSpeed = 5f; // Speed at which the camera follows
    private Vector3 offset;
    private Quaternion fixedRotation = Quaternion.Euler(60f, 180f, 0f); // Converted from your quaternion

    void Start()
    {
        playerTarget = target;
    }

    private void Update()
    {
        offset = new Vector3(0, offsetY, 2f); // Fixed offset based on your given position
        fixedRotation = Quaternion.Euler(rotation, 180f, 0f);
    }
    void LateUpdate()
    {
        if (target == null){
            Debug.LogWarning("Camera cannot follow null object.");
            return;
        } 

        // Target position with offset
        Vector3 targetPosition = target.position + offset;

        // Smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Maintain the fixed rotation
        transform.rotation = fixedRotation;
    }

    public void switchFocus(Transform newTarget){
        target = newTarget;
    }

    public void switchFocus(Transform newTarget, int newOffset, int newFollowSpeed){
        target = newTarget;
        offsetY = newOffset;
        followSpeed = newFollowSpeed;
    }
}