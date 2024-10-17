using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMine : MonoBehaviour
{
    
    /**
     * Deactivate the object when it has collided with a "Robot" game object
     */
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Robot")
        {
            this.gameObject.SetActive(false);
        }
    }
    
}
