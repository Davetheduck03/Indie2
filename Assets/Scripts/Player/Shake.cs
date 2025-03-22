using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    public AnimationCurve curve;
    public bool startShake;
    void Update()
    {
        if (startShake)
        {
            startShake = false;
            StartCoroutine(Shaking());
        }
       
    }
    IEnumerator Shaking()
        {
            Vector3 startPos = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            { 
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / duration);
                transform.position = startPos + Random.insideUnitSphere * strength;
                yield return null;
            }
            transform.position = startPos;
        }
}
