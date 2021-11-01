using Assets.Scripts.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] float high = 5; 
    private Rigidbody _rigidbody;
    private float timer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        //transform.localPosition = -Vector3.up * Floor.size;
        _rigidbody.velocity = -transform.forward * velocity;
        velocity = high / velocity;
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > velocity)
        {
            _rigidbody.velocity *= -1;
            timer = 0f;
        }
    }
}
