using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimator : MonoBehaviour, IMonsterComponent
{
    private Monster _monster;

    private Animator _animator;

    public void Initialize(Monster monster)
    {
        _monster = monster;

        _monster.GetCompo<MonsterAttack>().AtkEvent += AttackEventHandler;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    #region 다른곳에서 쓸 함수
    public void SetTrigger(string hash)
    {
        _animator.SetTrigger(hash);
    }

    public void SetBool(string hash, bool value)
    {
        _animator.SetBool(hash, value);
    }
    #endregion

    #region 지정
    public void IndexChanger(int index)
    {
        _animator.SetInteger("AtkIndex", index);
    }

    private void AttackEventHandler(bool value)
    {
        _animator.SetBool("Atk", value);
    }
    #endregion
}
