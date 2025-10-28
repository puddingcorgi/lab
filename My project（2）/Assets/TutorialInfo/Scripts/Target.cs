using UnityEngine;

public class PhysicsTarget : MonoBehaviour
{
    
    void Start()
    {
        
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }
}