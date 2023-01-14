using UnityEngine;

namespace Other
{
    public class Ground : MonoBehaviour
    {
        // if powerUp touches the ground, make it disappear
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PowerUp") || other.gameObject.CompareTag("Revive") || other.gameObject.CompareTag("Golden"))
            {
                other.transform.gameObject.SetActive(false);
                other.gameObject.SetActive(false);
            }
        }
    }
}
