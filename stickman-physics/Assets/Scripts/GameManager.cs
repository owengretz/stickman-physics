using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public StickmanController[] playerControllers;
    public int numPlayers;
    public List<Rigidbody2D> torsos = new List<Rigidbody2D>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        numPlayers = playerControllers.Length;
        for (int i = 0; i < playerControllers.Length; i++)
        {
            torsos.Add(playerControllers[i].torso);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
