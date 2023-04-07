using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }

    [SerializeField] private List<Level> Levels = new List<Level>();

    private int _Index { get { return Statistics.LevelIndex; } set { Statistics.LevelIndex = value; } }
    public int GetIndex() => _Index;
    public void SetIndex(int value) => _Index = value;

    public Level ActualLevel => Levels[GetIndex() > (Levels.Count - 1) ? GetIndex() % Levels.Count : GetIndex()];

    [SerializeField] private Transform bufferForLevel;

    void Awake() => Instance = this;

    void Start()
    {
        LoadOnStart();
        StartLevel();
    }

    void LoadOnStart()
    {
        LoadLevel();
    }

    public void StartLevel()
    {
        ActualLevel.StartLevel();
    }

    public void EndLevel()
    {
        
    }

    void LoadLevel()
    {
        BufferingLevel();
        Level level = ActualLevel;
        level.On();
    }

    public void NextLevel()
    {
        OffLevel(ActualLevel);

        GameObject levelPref = GetBufferLevel(); 
        GameObject go = Instantiate(levelPref, transform);
        Level level = go.GetComponent<Level>();

        Levels[GetIndex()] = level;

        OffLevel(level);

        int index = GetIndex();
        index += 1;
        SetIndex(index);

        LoadLevel();

        StartLevel();
    }

    public void PreviousLevel()
    {
        OffLevel(ActualLevel);

        int index = GetIndex();
        index -= 1;
        SetIndex(index);

        LoadLevel();

        StartLevel();
    }

    public void RestartLevel()
    {
        OffLevel(ActualLevel);

        GameObject levelPref = GetBufferLevel(); 
        GameObject go = Instantiate(levelPref, transform);
        Level level = go.GetComponent<Level>();

        Levels[GetIndex()] = level;
        LoadLevel();

        StartLevel();
    }

    void OffLevel(Level level)
    {
        level.Off();
        /* level.Eliminate(); */
    }

    GameObject GetBufferLevel()
    {
        return bufferForLevel.GetChild(0).gameObject;
    }

    void BufferingLevel()
    {
        if(bufferForLevel.childCount > 0) 
        {
            Destroy(GetBufferLevel());
        }

        Level level = ActualLevel;
        GameObject go = Instantiate(level.gameObject, bufferForLevel);

        level = go.GetComponent<Level>();
        level.Off();
    }
}
