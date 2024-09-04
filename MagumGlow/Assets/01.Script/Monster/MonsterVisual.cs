using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVisual : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private SpriteRenderer _spriteRenderer;

    public void Initialize(Monster monster)
    {
        _monster = monster;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = monster.GetCompo<MonsterGetSO>().SO.Sprite;
    }
}
