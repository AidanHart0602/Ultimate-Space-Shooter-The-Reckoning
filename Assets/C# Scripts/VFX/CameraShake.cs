using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Vector3 containerPosition;
    private float cameraTimer = 0;
    [SerializeField]
    private float shakeDuration = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        containerPosition = this.transform.position;
    }
    public IEnumerator ShakeTheCamera()
    {
        while (cameraTimer < shakeDuration)
        {
            float magnitude = Random.Range(1f, 3f);
            float x = Random.Range(-0.25f, 0.25f) * magnitude;
            float y = Random.Range(-0.25f, 0.25f) * magnitude;
            this.transform.position = new Vector3(x, y, containerPosition.z);
            cameraTimer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = containerPosition; 
        cameraTimer = 0;
    }
}
