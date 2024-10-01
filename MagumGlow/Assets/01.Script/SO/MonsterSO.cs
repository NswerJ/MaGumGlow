using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MonsterData")]
public class MonsterSO : ScriptableObject
{
    public Sprite Sprite;

    public MonsterStats StatSO;

    public int MonsterLV;

    public float DropGold;
}
