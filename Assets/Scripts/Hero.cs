using UltimateJoystickExample.Spaceship;
using UnityEngine;

namespace DefaultNamespace
{
    public class Hero
    {
        public HeroVitals vitals;

        public Hero()
        {
            vitals = new HeroVitals();
        }
    }
    
    public class HeroVitals
    {
        public float health;

        public HeroVitals()
        {
            health = 0;
        }
    }

    public class HeroStats
    {
        
    }
}