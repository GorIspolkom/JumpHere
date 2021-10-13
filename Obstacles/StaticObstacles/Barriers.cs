using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barriers : Obstacles
{
    public override void OnCollisionEnter(Collision collision)
    {
        Assets.Scripts.Players.CharacterController.Instance.SpeedChange(1f);
        base.OnCollisionEnter(collision);
    }
}
