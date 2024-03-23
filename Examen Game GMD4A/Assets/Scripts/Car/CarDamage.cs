using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class CarDamage : MonoBehaviour
{

    [Header("Health")]
    public float maxHealth = 100.0F;
    public float currentHealth = 100.0F;
    public float damageThreshold = 3.0F;
    public float latestDamageVal;

    [Header("Particle Settings")]
    public ParticleSystem engineSmokeParticleSystem;
    public ParticleSystem engineExplodeParticle;
    public float maxEmission = 3.0F;

    [HideInInspector]
    public bool isDead;

    [HideInInspector]
    public Action<float> OnTakeDamage = delegate { };

    [HideInInspector]
    public Action<Car> OnDeath = delegate { };

    private Car car;
    private Vector3 previousVelocity;
    private bool shouldCalculateDamage = true;

    private void Start()
    {
        //set default particle 
        ParticleSystem.EmissionModule emission = engineSmokeParticleSystem.emission;
        float rate = (maxHealth - currentHealth) / maxHealth * maxEmission;
        emission.rateOverTime = rate;
        emission.rateOverDistance = rate;
        engineSmokeParticleSystem.Play();

        car = GetComponent<Car>();
    }

    private void Update()
    {
        if (!isDead && shouldCalculateDamage)
        {
            float difference = Vector3.Distance(car.rb.velocity, previousVelocity);
            float factor = difference;
            Debug.Log(factor);
            latestDamageVal = factor;
            if (factor >= damageThreshold)
            {
                currentHealth -= factor;
                OnTakeDamage.Invoke(factor);

                float rate = (maxHealth - currentHealth) / maxHealth * maxEmission;
                ParticleSystem.EmissionModule emission = engineSmokeParticleSystem.emission;

                emission.rateOverTime = rate;
                emission.rateOverDistance = rate;
                
                if (currentHealth <= 0)
                {
                    currentHealth = 0;
                    isDead = true;
                    this.Death();
                }

            }
            shouldCalculateDamage = false;
        }
        previousVelocity = car.rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        shouldCalculateDamage = true;
    }

    public void Death()
    {
        engineExplodeParticle.Play();
        OnDeath.Invoke(car);
    }

}
