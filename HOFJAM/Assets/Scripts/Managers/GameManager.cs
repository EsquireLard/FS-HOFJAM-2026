using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState GameState;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                TileManager.instance.GenerateGrid();
                    break;
            case GameState.Running:
                    break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    Running = 1
}
