using UnityEngine;

namespace Other
{
    public class Arrow : MonoBehaviour
    {
        public GameObject arrow;
        public float offset;
        
        private bool isVisible;
        
        void Update()
        {
            var reviver = GameObject.FindGameObjectWithTag("Revive"); // find revive object
            if (reviver != null && !isVisible) // if there is revive activate arrow and follow

            {
                arrow.gameObject.SetActive(true);
                Follow(reviver);
            }
            else
            {
                arrow.gameObject.SetActive(false);
            }
        }

        private void Follow(GameObject reviver)
        {
            var toPosition = reviver.transform.position;
            if (UnityEngine.Camera.main != null)
            {  // detect position and then angle for z rotation
                var fromPosition = UnityEngine.Camera.main.transform.position;
                fromPosition.z = 0f;
                var dir = (toPosition - fromPosition).normalized;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 180;
                arrow.transform.localEulerAngles = new Vector3(0, 0, angle);
            }

            if (UnityEngine.Camera.main != null)
            {
                
                // attach position to arrow
                Vector3 targetPositionScreenPoint = UnityEngine.Camera.main.WorldToScreenPoint(reviver.transform.position);
                bool isOffScreen = targetPositionScreenPoint.x >= Screen.width - offset|| targetPositionScreenPoint.x <= offset ||
                                   targetPositionScreenPoint.y >= Screen.height - offset|| targetPositionScreenPoint.y <= offset;
            

                if (isOffScreen)
                {
                    isVisible = false;
                
                    Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;

                    if (cappedTargetScreenPosition.x <= offset)
                    {
                        cappedTargetScreenPosition.x = offset;
                    }

                    if (cappedTargetScreenPosition.x >= Screen.width - offset)
                    {
                        cappedTargetScreenPosition.x = Screen.width - offset;
                    }

                    if (cappedTargetScreenPosition.y <= offset)
                    {
                        cappedTargetScreenPosition.y = offset;
                    }

                    if (cappedTargetScreenPosition.y >= Screen.height - offset)
                    {
                        cappedTargetScreenPosition.y = Screen.height - offset;
                    }
                    
                    arrow.transform.position = cappedTargetScreenPosition;
                    arrow.transform.localPosition = new Vector3( arrow.transform.localPosition .x,  arrow.transform.localPosition .y, 0f);
                }else
                    isVisible = true;
            }
        }
    }
}