using Lean.Pool;
using UnityEngine;
using static Utils.ContainerFacade;

public class ExplosionController : MonoBehaviour
{
    private float timer = 0f;

    private void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
            
        if (timer > SpellSettings.attackExplosionDuration )
        {
            LeanPool.Despawn(gameObject);
        }
    }
}