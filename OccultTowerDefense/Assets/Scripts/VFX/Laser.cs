using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Range(0.1f, 0.6f)] // Min and max length the laser can be.
	public float laserLength;

    private ParticleSystem laserEffect;
    private ParticleSystem.MainModule laserEffectMain;

    // Start is called before the first frame update
    void Start()
    {
        laserEffect = GetComponent<ParticleSystem>();
        laserEffectMain = GetComponent<ParticleSystem.MainModule>();
    }

    // Update is called once per frame
    void Update()
    {
        laserEffect.Stop();
        // Set laser length
	    laserEffect.startLifetime = laserLength;
	    laserEffect.Play();
    }
}
