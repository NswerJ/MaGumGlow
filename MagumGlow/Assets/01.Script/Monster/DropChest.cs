using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropChest : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(OpenChest), 1);   
    }

    void OpenChest()
    {
        Time.timeScale = 0;
        Destroy(gameObject);
        // 원준이가 만든 창 실행
    }
}
