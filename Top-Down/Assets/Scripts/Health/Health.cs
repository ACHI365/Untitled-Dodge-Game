using UnityEngine;
using UnityEngine.UI;

namespace Health
{
    public class Health : MonoBehaviour
    {
        private float hp;
        public Health(float endurance)
        {
            hp = endurance;
        }
        
        public float getHp()
        {
            return this.hp;
        }

        public void setHealth(float newHealth)
        {
            hp = newHealth;
        }
    }
}

