using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    Transform t;
    [SerializeField] float rotationSpeed = 5f;
    [Range(-180,0)]
    [SerializeField] float rotatationPoint = -90f;
    private void Awake()
    {
        t = transform;
        
    }

    public void TriggerTrap()
    {
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        float adjustedRotationPoint = rotatationPoint / 180;
        while(t.rotation.z> adjustedRotationPoint)
        {
            t.Rotate(-rotationSpeed * Time.deltaTime * Vector3.forward);
            yield return new WaitForEndOfFrame();
        }

        while (t.rotation.z < 0)
        {
            t.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
            yield return new WaitForEndOfFrame();
        }

    }

}
