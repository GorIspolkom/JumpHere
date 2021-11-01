using Assets.Scripts.Level;
using HairyEngine.HairyCamera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobblingPlatforms : MonoBehaviour
{
    [SerializeField] Vector3 moveAxis = Vector3.right;
    [SerializeField] float maxVelocity = 20f;
    [SerializeField] float minVelocity = 10f;
    [SerializeField] float maxOffset = 5f;
    [SerializeField] float minOffset = 3f;
    [SerializeField] bool isOnAwake = false;
    [SerializeField] bool isNextTilePositionChange = false;
    [SerializeField] float delay;
    private Transform _transform;
    private float timer;
    private int side;

    private void Awake()
    {
        _transform = transform;
        if (isOnAwake)
            InitPosition();
    }
    private void InitPosition()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        maxOffset = Random.Range(minOffset, maxOffset);
        moveAxis.Normalize();
        if (collider)
        {
            Vector3 sizes = collider.size.NablaMultiply(moveAxis);
            if (sizes == Vector3.zero)
                moveAxis *= maxOffset / 2f;
            else
                moveAxis = sizes * maxOffset / 2f;
        }
        else
            moveAxis *= maxOffset;
        maxVelocity = moveAxis.magnitude / Random.Range(minVelocity, maxVelocity);
        timer = 0;
        side = 1;
        _transform.localPosition = moveAxis;
    }
    void Start()
    {
        if (!isOnAwake)
            InitPosition();
        if (isNextTilePositionChange)
            _transform.parent.localPosition += moveAxis;
        StartCoroutine(Updater());
    }

    IEnumerator Updater()
    {
        while (_transform != null)
        {
            _transform.localPosition = Vector3.Lerp(-moveAxis, moveAxis, timer / maxVelocity) * side;
            timer += Time.deltaTime;
            if (timer > maxVelocity)
            {
                side *= -1;
                timer = 0f;
                yield return new WaitForSeconds(delay);
            }
            else
                yield return null;
        }
    }
}
