using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    public enum LevelState
    {
        WAVE_SETUP, WAVE_BUSY, WAVE_COMPLETED, GAME_OVER, EMPTY
    }

    public WaveGenerator m_WaveGenerator;
    public UnityEvent m_OnstateChangeAction;

    private int m_Level = 1;
    public int Level {
        get { return m_Level; }
    }
    private Player[] m_Players;
    public Player[] Players {get { return m_Players; } }

    private LevelState m_State;
    public LevelState State {
        get { return m_State; }
        private set {
            m_State = value;
            m_OnstateChangeAction.Invoke();
    } }

    void Awake()
    {
        m_Players = FindObjectsOfType<Player>();

        if (m_Players.Length < 1)
            Debug.LogError("No player exists!");
    }

    void Start()
    {
        State = LevelState.WAVE_SETUP;
        StartCoroutine("LevelCycle");
    }

    IEnumerator LevelCycle()
    {
        while (true)
        {
            //players can die in any wave
            if (!existAlivePlayer())
                State = LevelState.GAME_OVER;

            switch (m_State)
            {
                case LevelState.WAVE_SETUP:
                    yield return new WaitForSeconds(5);
                    m_WaveGenerator.setWave(m_Level);
                    m_WaveGenerator.StartSpawning();
                    State = LevelState.WAVE_BUSY;
                    break;
                case LevelState.WAVE_BUSY:
                    if (m_WaveGenerator.isWaveCompleted()){
                        m_WaveGenerator.StopSpawning();
                        State = LevelState.WAVE_COMPLETED;
                    }
                    break;
                case LevelState.WAVE_COMPLETED:
                    yield return new WaitForSeconds(10);
                    m_Level++;
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
        for (int i = 0; i < m_Players.Length; i++)
        {
            if (!m_Players[i].isDead) return true;
        }
        return false;
    }
}
