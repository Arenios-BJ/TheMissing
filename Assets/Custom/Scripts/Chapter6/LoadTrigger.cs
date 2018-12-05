using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTrigger : MonoBehaviour {

    private bool LoadCheck;

	void Start () {

        LoadCheck = false;

    }
	

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "Player")
        {
            if (LoadCheck == false)
            {
                SceneManager.LoadSceneAsync(8, LoadSceneMode.Additive);
                LoadCheck = true;
            }
        }

    }
}
