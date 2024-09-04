using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MonsterData")]
public class MonsterSO : ScriptableObject
{
    [SerializeField] private string _monsterName;
    public string MonsterName => _monsterName;

    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField] private int _monsterHP;
    public int MonsterHP => _monsterHP;
}
