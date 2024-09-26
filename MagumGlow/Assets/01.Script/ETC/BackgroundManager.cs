using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private List<ParralaxBackground> _backgrounds = new List<ParralaxBackground>();
        
    private void Awake()
    {
        _backgrounds.AddRange(GetComponentsInChildren<ParralaxBackground>());
    }

    public void Running(bool IsDetected)
    {
        _backgrounds.ForEach(b => b.isStop = !IsDetected);
    }
}
