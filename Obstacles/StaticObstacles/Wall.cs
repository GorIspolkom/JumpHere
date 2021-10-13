using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Obstacles
{
    private Vector3 _dir;
    private void Start() => _dir = new Vector3(Random.Range(-20f, -5f), 0 ,Random.Range(-20f, -5f));
    public override void OnCollisionEnter(Collision collision)
    {
        Assets.Scripts.Players.CharacterController.Instance.SpeedChange(1f);
        collision.transform.GetComponent<Rigidbody>().AddForce(_dir, ForceMode.Impulse);
        Debug.Log("Force get");
        base.OnCollisionEnter(collision);
    }
}
