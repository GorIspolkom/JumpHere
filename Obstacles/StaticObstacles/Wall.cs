using Assets.Scripts.Level;
using UnityEngine;

public class Wall : Obstacles
{
    private Vector3 _dir;
    private void Start()
    {
        //_dir = TrackBuilder.Instance.obstacleInflunce * TrackBuilder.Instance.CurrentDirect * Random.Range(-1f, 1f);
        _dir = new Vector3(TrackBuilder.Instance.CurrentDirect.z, 0, TrackBuilder.Instance.CurrentDirect.x) * Random.Range(-1f, 1f) * TrackBuilder.Instance.obstacleInflunce;
        Debug.Log(_dir);
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Assets.Scripts.Players.CharacterController.Instance.moveDirection += _dir;
            Debug.Log(Assets.Scripts.Players.CharacterController.Instance.moveDirection);
            //collision.transform.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(_dir));//AddForce(_dir, ForceMode.Impulse);
            Debug.Log("Force get");
            base.OnCollisionEnter(collision);
        }
    }
}
