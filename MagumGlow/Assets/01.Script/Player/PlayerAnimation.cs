using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private MagicSword magicSword;

    private readonly int _runningHash = Animator.StringToHash("isRun");
    private readonly int _attackingHash = Animator.StringToHash("isAttack");
    private readonly int _dyingHash = Animator.StringToHash("isDie");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        magicSword = GetComponentInParent<MagicSword>();

        // AttackEvent를 구독하여 애니메이션 제어
        magicSword.AttackEvent += PlayerAttack;
        magicSword.DieEvent += PlayerDie;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (magicSword != null)
        {
            magicSword.AttackEvent -= PlayerAttack;
            magicSword.DieEvent -= PlayerDie;
        }
    }

    public void PlayerAttack(bool isAttacking)
    {
        anim.SetBool(_runningHash, !isAttacking);
        anim.SetBool(_attackingHash, isAttacking);
    }

    public void PlayerDie(bool isDie)
    {
        anim.SetBool(_runningHash, !isDie);
        anim.SetBool(_attackingHash, isDie);
        anim.SetBool(_dyingHash, isDie);
    }
}
