using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource audioSource;
    public Rigidbody rigidBody;

    public AudioClip[] collisionClips;

    [Range(0, 2)] public float minSpeed;
    [Range(5, 50)] public float maxSpeed;

    public AnimationCurve speedVolumeMapping = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve speedClipMapping = AnimationCurve.Linear(0, 0, 1, 1);

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 relativeVelocity = collision.relativeVelocity;
        float speed = relativeVelocity.magnitude;

        //use the speed to map our volume linearly:
        //below min speed - we do nothing
        //above max speed - set speed to maxSpeed;
        //min speed - volume is at 0;
        //max speed - volume is at 1;
        Debug.Log("collision speed: " + speed);

        if (speed < minSpeed)
        {
            return;
        }
        
        Mathf.Clamp(speed, minSpeed, maxSpeed);

        //scaled between 0 and 1;
        float mappedSpeed = Mathf.InverseLerp(minSpeed, maxSpeed, speed);

        float scaledVolume = speedVolumeMapping.Evaluate(mappedSpeed);
        float scaledClipSelector = speedClipMapping.Evaluate(mappedSpeed);

        audioSource.volume = scaledVolume;

        float clipSelection = scaledClipSelector * (collisionClips.Length - 1);

        int clipIndex = Mathf.FloorToInt(clipSelection);


        Debug.Log("collision Clip: " + clipIndex);

        audioSource.clip = collisionClips[clipIndex];
        audioSource.Play();

    }

}
