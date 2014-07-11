using UnityEngine;
using System.Collections;

public class WonGame : MonoBehaviour {

    public string sceneToLoad;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Senior"))
        {
            if (sceneToLoad != "")
            {
                Application.LoadLevel(sceneToLoad);
            }
            else
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
