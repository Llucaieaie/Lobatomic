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
        //StartCoroutine(StartShake(10));
    }

    IEnumerator StartShake(int Intensity)
    {
        Vector2 startPos = transform.position;

        transform.position = new Vector3(startPos.x + Random.insideUnitCircle.x * Intensity * Time.deltaTime, startPos.y + Random.insideUnitCircle.y * Intensity * Time.deltaTime, - 10);
        yield return null;
    }
}
