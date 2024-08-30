using UnityEngine;

public class GameController : MonoBehaviour
{
    public MagicSword magicSword;  // 게임에 배치된 마검 참조

    void Update()
    {
        // 예시: 일정 시간 경과마다 마검 레벨업
        if (ShouldLevelUp())
        {
            magicSword.LevelUpSword();
            magicSword.DisplaySwordStats();
        }
    }

    bool ShouldLevelUp()
    {
        // 레벨업 조건 로직 (예: 일정 시간 경과, 경험치 획득 등)
        return Time.time % 10 == 0;  // 예시로 10초마다 레벨업
    }
}
