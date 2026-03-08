using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState GameState;

    [SerializeField] public Transform cam;

    public Vector2 mousePosition;

    public plants selectedPlant;
    public Tile hoverTile;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    void Update()
    {
        mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    public void SetSelectedUnit(plants plant)
    {
        selectedPlant = plant;
    }
}

public enum GameState
{
    GenerateGrid = 0,
    Running = 1
}
