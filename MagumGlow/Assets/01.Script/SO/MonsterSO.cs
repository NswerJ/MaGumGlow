using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MonsterData")]
public class MonsterSO : ScriptableObject
{
    public Sprite Sprite;

    public MonsterStats StatSO;

    public int MonsterLV = 1;

    public float DropGold;

    public void LevelUP()
    {
        MonsterLV++;
    }
}
