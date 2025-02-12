using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatedParticleBehaviour : MonoBehaviour
{
    public ParticleSystem particleAnimation;

    private void OnEnable()
    {
        particleAnimation.Play();
    }

    private void Update()
    {
        if (particleAnimation.isStopped)
        {
            gameObject.SetActive(false);
        }
    }
}
