using UnityEngine;

public class Container : MonoBehaviour
{
    public int itemInside; //the id for the item 
    private GameManager manager;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            //play an opening animation
            
        }
    }
}
