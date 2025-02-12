using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    public ParticleSystem particleAnimation;

    void Start()
    {
        particleAnimation = gameObject.GetComponent<ParticleSystem>();
        particleAnimation.Play();
    }

    void Update()
    {
        if (particleAnimation.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
