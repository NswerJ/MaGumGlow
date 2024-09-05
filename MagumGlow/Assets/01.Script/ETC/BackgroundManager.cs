using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private List<ParralaxBackground> _backgrounds = new List<ParralaxBackground>();
        
    private void Awake()
    {
    
        _backgrounds.AddRange(GetComponentsInChildren<ParralaxBackground>());

        //Dead += Running;

    }

    public void Running()
    {

        StartCoroutine(RunningCoroutine());
        
    }

    private IEnumerator RunningCoroutine()
    {

        _backgrounds.ForEach(b => b.isStop = false);

        yield return new WaitForSeconds(1f);
        
        _backgrounds.ForEach(b => b.isStop = true);

    }
}
