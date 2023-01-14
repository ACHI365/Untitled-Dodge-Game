using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [Range(1,10)]
    [SerializeField] float smoothFactor;
    [SerializeField] Vector3 minVal, maxVal;

    private void FixedUpdate()
    {
        if(target != null)
         Follow();
    }


    void Follow()
    {
        Vector3 targetPosition = target.position + offset;


        Vector3 bound = new Vector3(
            Mathf.Clamp(targetPosition.x, minVal.x, maxVal.x),
            Mathf.Clamp(targetPosition.y, minVal.y, maxVal.y),
            Mathf.Clamp(targetPosition.z, minVal.z, maxVal.z));
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothedPosition;
    }
}
