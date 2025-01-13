using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBarScript : MonoBehaviour
{
    public ScoreVersusController scoreVersusController;

    private void Update()
    {
        float aux = (scoreVersusController.score / 100f);
        gameObject.transform.localScale = new Vector3(aux, 1, 1);
    }
}
