using UnityEngine;

namespace Player
{
    public class Blade : MonoBehaviour
    {
        private Vector3 direction;

        private UnityEngine.Camera mainCamera;

        private Collider sliceCollider;
        private TrailRenderer sliceTrail;

        public float minSliceVelocity = 0.01f;

        private bool slicing;

        private Touch touch;

        private void Awake()
        {
            mainCamera = UnityEngine.Camera.main;
            sliceCollider = GetComponent<Collider>();
            sliceTrail = GetComponentInChildren<TrailRenderer>();
        }

        private void OnEnable()
        {
            StopSlice();
        }

        private void OnDisable()
        {
            StopSlice();
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
                StartSlice();
            } else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
                StopSlice();
            } else if (slicing) {
                ContinueSlice();
            }
        }

        private void StartSlice()
        {   // get position and check i on worldPoint perspective
            Vector2 mousePos = Vector3.zero;

            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;
        
            Vector3 worldPoint =
                mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane + 4.8f));
        
            // change position and enable collider for slicing
            transform.position = worldPoint;

            slicing = true;
            sliceCollider.enabled = true;
            sliceTrail.enabled = true;
            sliceTrail.Clear();
        }

        private void StopSlice()
        { 
            slicing = false;
            sliceCollider.enabled = false;
            sliceTrail.enabled = false;
        }

        private void ContinueSlice()
        {
            
            //get position and slice according to direction of slicing
            Vector2 mousePos = Vector3.zero;

            mousePos.x = Input.mousePosition.x;
            mousePos.y = Input.mousePosition.y;
        
            Vector3 worldPoint =
                mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane + 4.8f));

            direction = worldPoint - transform.position;

            float velocity = direction.magnitude / Time.deltaTime;
            sliceCollider.enabled = velocity > minSliceVelocity;

            transform.position = worldPoint;
        }

    }
}
