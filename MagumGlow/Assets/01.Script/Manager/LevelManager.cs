using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> backgrounds;

    //UI 1-1 ¿Ã∑∏∞‘
    public int currentStage = 1;
    public int currentLevel = 1;

    void Update()
    {
        
    }
}
