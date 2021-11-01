using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    [SerializeField] Rigidbody bulletPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float shootStrength;
    [SerializeField] float shootDelay;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }
    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }
    private IEnumerator ShootCoroutine()
    {
        while (_transform)
        {
            yield return new WaitForSeconds(shootDelay);
            Vector3 forward = spawnPoint.position - _transform.position;
            Rigidbody bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            bullet.AddForce(forward.normalized * shootStrength, ForceMode.Impulse);
            Destroy(bullet.gameObject, 3f);
        }
    }
}
