using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public Camera cam;

    void Start()
    {
        //cam = GetComponent<Camera>();
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
}
