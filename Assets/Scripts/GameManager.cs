using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event System.Action<GameState> OnGameStateChangeAction;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public float spaceDepth = 100f;
    public float fontLength = 10f;

    public float metorDepthSpeed = 10f;
    private float spaceBaseZ = -10;
    public float SpaceBaseZ { get { return spaceBaseZ; } }
    public float spaceShipZ = 0;

    private Vector2 spaceShipPlaneSize;
    public Vector2 SpaceShipPlaneSize { get { return spaceShipPlaneSize; } }

    private SpaceShipController spaceShip;

    public GameState CurrentGameState { get { return currentGameState; } }
    private GameState currentGameState;

    public enum GameState
    {
        Playing,
        PlayerWon,
        PlayerLost
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spaceBaseZ = Camera.main.transform.position.z;

        float maxHeight = 2 * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * (spaceShipZ - SpaceBaseZ);
        float maxWidth = 2 * Mathf.Tan(0.5f * Camera.VerticalToHorizontalFieldOfView(Camera.main.fieldOfView, Camera.main.aspect) * Mathf.Deg2Rad) * (spaceShipZ - SpaceBaseZ);

        spaceShipPlaneSize = new Vector2(maxWidth, maxHeight);

        spaceShip = FindObjectOfType<SpaceShipController>();

        currentGameState = GameState.Playing;

        spaceShip.OnSpaceShipDeathAction += OnSpaceShipDeath;
    }

    public float GetSizeRatio(float depth)
    {
        return Mathf.Clamp01( (spaceDepth - depth) / spaceDepth);
    }

    public float GetMetorDepthSpeed()
    {
        return metorDepthSpeed;
    }

    private void OnDrawGizmos()
    {
        float maxHeight = 2 * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * (spaceShipZ - SpaceBaseZ);
        float maxWidth = 2 * Mathf.Tan(0.5f * Camera.VerticalToHorizontalFieldOfView(Camera.main.fieldOfView, Camera.main.aspect) * Mathf.Deg2Rad) * (spaceShipZ - SpaceBaseZ);

        Color c = Color.red;
        c.a = 0.5f;
        Gizmos.color = c;
        Gizmos.DrawCube(new Vector3(0, 0, spaceShipZ), new Vector3(maxWidth, maxHeight, 1)); 
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSpaceShipDeath()
    {
        currentGameState = GameState.PlayerLost;

        OnGameStateChangeAction?.Invoke(currentGameState);
    }

    private void OnDestroy()
    {
        spaceShip.OnSpaceShipDeathAction -= OnSpaceShipDeath;
    }

    public void RePlayGame()
    {
        SceneManager.LoadScene(0);
    }
}
