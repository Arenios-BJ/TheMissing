using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class LoadChapterScene : MonoBehaviour {
    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.scene.name)
            {
                case "Chapter1":
                    SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
                    break;
                case "Chapter2":
                    if (SceneManager.GetSceneByName("Chapter1").isLoaded)
                        SceneManager.UnloadSceneAsync(2);
                    SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
                    break;
                case "Chapter3":
                    if (SceneManager.GetSceneByName("Chapter2").isLoaded)
                        SceneManager.UnloadSceneAsync(3);
                    SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
                    break;
                case "Chapter4":
                    if (SceneManager.GetSceneByName("Chapter3").isLoaded)
                        SceneManager.UnloadSceneAsync(4);
                    SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive);
                    break;
                case "Chapter5":
                    if (SceneManager.GetSceneByName("Chapter4").isLoaded)
                        SceneManager.UnloadSceneAsync(5);
                    SceneManager.LoadSceneAsync(7, LoadSceneMode.Additive);
                    break;
                case "Chapter6":
                    if (SceneManager.GetSceneByName("Chapter5").isLoaded)
                        SceneManager.UnloadSceneAsync(6);
                    break;
                default:
                    break;
            }

            Destroy(gameObject);
        }
    }
}
