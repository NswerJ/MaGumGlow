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

        UpdateSprite();
    }

    public void UpdateSprite()
    {
        _spriteRenderer.sprite = _monster.SO.Sprite;
    }
}
