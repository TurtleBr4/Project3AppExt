using UnityEngine;

public class EventTriggerThing : MonoBehaviour
{
    BossEnemy Boss;
    public CameraFollow cameraFollow;

    private void Start()
    {
        Boss = GameObject.Find("BossBot").GetComponent<BossEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            cameraFollow.offsetY += 2f;
            Boss.startFight();
        }
    }
}
