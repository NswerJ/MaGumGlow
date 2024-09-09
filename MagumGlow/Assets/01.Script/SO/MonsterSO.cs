using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MonsterData")]
public class MonsterSO : ScriptableObject
{
    //[SerializeField] private Sprite _sprite;
    public Sprite Sprite;

    //[SerializeField] private float _monsterHP;
    public float MonsterHP;

    [SerializeField] private int _dropGold;
    public int DropGold => _dropGold;
}
