using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        // target to follow
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset; // offset to draw distance between camera and player
        [Range(1, 10)] [SerializeField] float smoothFactor; // smoothen camera following

        private void FixedUpdate()
        {
            if (target != null)
                Follow();
        }


        void Follow()
        {
            Vector3 targetPosition = target.position + offset;
            targetPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);  // follow only on x coordinate
            Vector3 smoothedPosition =
                Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime); // lerp for smoothing
            transform.position = smoothedPosition;
        }
    }
}