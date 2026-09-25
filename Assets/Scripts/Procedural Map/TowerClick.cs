using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerClick : MonoBehaviour, IClickable
{
    public static TowerClick chosenTower;

    [Header("Tower")]
    [SerializeField] private string label = "Wie¿a";

    [Header("References")]
    [SerializeField] private Camera baseCamera;

    private string towerSceneName;

    private PlayerMover player;
    private List<Vector3> pathToTower;
    private List<Vector3> pathFromTowerToExit;

    private bool isTowerSceneLoaded;
    private Scene baseScene;

    private void Awake()
    {
        baseScene = gameObject.scene;

        if (baseCamera == null)
        {
            baseCamera = Camera.main;
        }
    }

    public void Configure(
        PlayerMover playerMover,
        List<Vector3> toTower,
        List<Vector3> fromTowerToExit,
        string towerLabel,
        string sceneName)
    {
        player = playerMover;
        pathToTower = toTower;
        pathFromTowerToExit = fromTowerToExit;

        label = towerLabel;
        towerSceneName = sceneName;
    }

    public List<Vector3> GetPathFromTowerToExit()
    {
        return pathFromTowerToExit;
    }

    public PlayerMover GetPlayer()
    {
        return player;
    }

    public void OnClicked()
    {
        if (chosenTower != null)
        {
            return;
        }

        if (player == null || pathToTower == null || pathToTower.Count == 0)
        {
            Debug.LogWarning($"TowerClick ({label}): brak poprawnej œcie¿ki do wie¿y.");
            return;
        }

        chosenTower = this;

        Debug.Log($"TowerClick ({label}): wybrano trasê do wie¿y.");

        player.MoveAlongWorldPositions(
            pathToTower,
            OnPlayerArrivedToTower
        );
    }

    private void OnPlayerArrivedToTower()
    {
        if (isTowerSceneLoaded)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(towerSceneName))
        {
            Debug.LogWarning(
                $"TowerClick ({label}): nazwa sceny wie¿y nie zosta³a ustawiona."
            );
            return;
        }

        if (LevelLoader.Instance == null)
        {
            Debug.LogError(
                $"TowerClick ({label}): brak LevelLoader.Instance."
            );
            return;
        }

        if (player != null)
        {
            player.enabled = false;
        }

        StartCoroutine(LoadTowerSceneAdditiveWithTransition());
    }

    private IEnumerator LoadTowerSceneAdditiveWithTransition()
    {
        LevelLoader.Instance.LoadSceneAdditiveWithTransition(
            towerSceneName
        );

        Scene towerScene;

        do
        {
            towerScene = SceneManager.GetSceneByName(towerSceneName);
            yield return null;
        }
        while (!towerScene.IsValid() || !towerScene.isLoaded);

        isTowerSceneLoaded = true;

        SceneManager.SetActiveScene(towerScene);

        if (baseCamera != null)
        {
            baseCamera.enabled = false;
        }

        LevelLoader.Instance.PlayFadeIn();

        Debug.Log(
            $"TowerClick ({label}): za³adowano scenê: {towerSceneName}"
        );
    }

    public void ReturnFromTower()
    {
        if (!isTowerSceneLoaded)
        {
            return;
        }

        StartCoroutine(UnloadTowerSceneAndReturn());
    }

    private IEnumerator UnloadTowerSceneAndReturn()
    {
        LevelLoader loader = LevelLoader.Instance;

        // Fade out
        if (loader != null)
        {
            loader.transition.SetTrigger("Start");

            yield return new WaitForSeconds(
                loader.transitionTime
            );
        }
        else
        {
            Debug.LogWarning(
                $"TowerClick ({label}): brak LevelLoader podczas powrotu."
            );
        }

        // Unload sceny wie¿y
        Scene towerScene =
            SceneManager.GetSceneByName(towerSceneName);

        if (towerScene.IsValid() && towerScene.isLoaded)
        {
            AsyncOperation unloadOperation =
                SceneManager.UnloadSceneAsync(towerScene);

            while (!unloadOperation.isDone)
            {
                yield return null;
            }
        }

        isTowerSceneLoaded = false;

        // Przywrócenie sceny bazowej
        if (baseScene.IsValid())
        {
            SceneManager.SetActiveScene(baseScene);
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetState(
                GameState.Gameplay
            );
        }

        if (baseCamera != null)
        {
            baseCamera.enabled = true;
        }

        if (player != null)
        {
            player.enabled = true;
        }

        if (loader != null)
        {
            loader.PlayFadeIn();
        }

        Debug.Log(
            $"TowerClick ({label}): powrót do sceny bazowej."
        );
    }

    [ContextMenu("Debug Return From Tower")]
    private void DebugReturnFromTower()
    {
        ReturnFromTower();
    }
}