using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public float TimeCount;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] GameObject MapGenerator;

    public Camera camera;

    // Update is called once per frame
    void Update()
    {
        TimeCount -= Time.deltaTime;
        
        timer.text = Mathf.RoundToInt(TimeCount).ToString();

        if(Input.GetKeyDown(KeyCode.Escape)) TimeCount = 0;
        if (TimeCount <= 0)
        {
            TimeCount = 0;
            StartCoroutine(CleanMap());
        }
    }

    private IEnumerator CleanMap()
    {
        Debug.Log("LOSE");
        camera.GetComponent<CameraManager>().MapDestroy();

        yield return new WaitForSeconds(0.1f);

        MapGenerator.GetComponent<MapGenerator>().CleanUp();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("DeathScene");
        Destroy(this.gameObject);
    }
}
