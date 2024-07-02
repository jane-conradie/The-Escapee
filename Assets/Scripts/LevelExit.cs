using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // if player collided with, then do things
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerMovement>().SetMoveStatus(false);
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            // reset game session
            FindObjectOfType<GameSession>().ResetGameSession();
        }

        // reset scene persist so next level can be properly populated
        FindObjectOfType<ScenePersist>().ResetScenePersists();

        SceneManager.LoadSceneAsync(nextSceneIndex);
    }
}
