using Assets.Scripts.Data;
using Assets.Scripts.Level;
using Assets.Scripts.Players;
using UnityEngine;

namespace JumpHere.Obstacles
{
    public class PlayerDirectionInflunce : Obstacles
    {
        [SerializeField] float obstacleInflunce;
        [SerializeField] bool isRigidbody;
        Rigidbody _rigidbody;
        private void Awake()
        {
            if(isRigidbody)
                _rigidbody = GetComponent<Rigidbody>();
        }
        private void CollisionHandle(Collision collision)
        {
            CharacterRunnerController controller = collision.transform.GetComponent<CharacterRunnerController>();
            if (controller)
            {
                Vector3 dir = new Vector3(1, 0, 1);
                if (isRigidbody)
                {
                    dir.x *= _rigidbody.velocity.x;
                    dir.z *= _rigidbody.velocity.z;
                }
                else
                {
                    if (controller.delta.magnitude < 0.02f)
                        dir *= UnityEngine.Random.Range(-1f, 1f);
                    else if (controller.delta.x == 0)
                        dir *= UnityEngine.Random.value * Mathf.Sign(controller.delta.z);
                    else
                        dir *= UnityEngine.Random.value * Mathf.Sign(controller.delta.x);
                    dir *= obstacleInflunce;
                }
                controller.AddDirection(dir);
            }
        }
        public override void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                CollisionHandle(collision);
                base.OnCollisionEnter(collision);
            }
        }
    }
}