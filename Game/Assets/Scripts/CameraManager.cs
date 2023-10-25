using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public Camera cam;

    public float DefaultSize;
    public float CurrentSize;

    void Start()
    {
        //cam = GetComponent<Camera>();
        DefaultSize = cam.orthographicSize;
        CurrentSize = DefaultSize;
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    public IEnumerator StartShake(int Intensity, float timeShaking)
    {

        while (timeShaking >= 0)
        {
            timeShaking -= Time.deltaTime;

            Vector2 startPos = transform.position;
            transform.position = new Vector3(startPos.x + Random.insideUnitCircle.x * Intensity * Time.deltaTime, startPos.y + Random.insideUnitCircle.y * Intensity * Time.deltaTime, -10);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    public IEnumerator ChangeZoom(float targetZoom, float LerpDuration)
    {
        float startTime = Time.time;
        float timeElapsed = 0.0f;

        CurrentSize = targetZoom;

        while (timeElapsed < LerpDuration)
        {
            timeElapsed = Time.time - startTime;
            float t = timeElapsed / LerpDuration;

            // Interpolar el valor del zoom suavemente utilizando Mathf.Lerp
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, t);

            yield return null;
        }

        // Asegurarse de que el valor final sea exactamente el deseado
        cam.orthographicSize = targetZoom;
    }

    public void MapDestroy()
    {
        StartCoroutine(ChangeZoom(20, 0.5f));
    }
}
