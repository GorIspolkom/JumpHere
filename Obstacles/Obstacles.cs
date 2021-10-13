using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public virtual void OnCollisionEnter(Collision collision) 
    {
        Debug.Log("Collision with obstacles detect");
        Destroy(gameObject);
    }
}
