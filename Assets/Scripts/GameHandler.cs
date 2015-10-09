using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    public static GameHandler instance;

    public enum LevelState {
        WAVE_SETUP, WAVE_BUSY, WAVE_COMPLETED, GAME_OVER, EMPTY
    }

    private int level = 1;
    public int Level { get { return level; } }
    private Player[] players;
    public Player[] Players {get { return players; } }

    private LevelState state;
    public LevelState State { get { return state; }set {
            state = value;
            onStateChanged(state);
    } }
    private WaveGenerator waveGenerator;
    private PlayerControls controller;

    void Awake()
    {
        if (instance == null)
        {
            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Debug.LogError("To many instances of " + GetType());
                return;
            } else instance = this;
        }

        players = FindObjectsOfType<Player>();
        if (players.Length < 1)
            Debug.LogError("No player exists!");
    }

    void Start()
    {
        waveGenerator = WaveGenerator.instance;
        State = LevelState.WAVE_SETUP;
        StartCoroutine("LevelCycle");
    }

    void onStateChanged(LevelState state)
    {
        Debug.Log(state);
        Debug.Log("Level: "+level);
    }

    IEnumerator LevelCycle()
    {
        while (true)
        {
            //players can die in any wave
            if (!existAlivePlayer())
                State = LevelState.GAME_OVER;

            switch (state)
            {
                case LevelState.WAVE_SETUP:
                    yield return new WaitForSeconds(5);
                    waveGenerator.setWave(level);
                    waveGenerator.StartSpawning();
                    State = LevelState.WAVE_BUSY;
                    break;
                case LevelState.WAVE_BUSY:
                    if (waveGenerator.isWaveCompleted()){
                        waveGenerator.StopSpawning();
                        State = LevelState.WAVE_COMPLETED;
                    }
                    break;
                case LevelState.WAVE_COMPLETED:
                    yield return new WaitForSeconds(10);
                    level++;
                    State = LevelState.WAVE_SETUP;
                    break;
                case LevelState.GAME_OVER:
                    Application.LoadLevel(Application.loadedLevel);
                    break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    bool existAlivePlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isDead) return true;
        }
        return false;
    }
}
