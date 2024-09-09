[System.Serializable]
public class Stat
{
    public string statName;  // 스탯 이름 (예: "공격력", "방어력")
    public float baseValue;  // 기본 값
    public float currentValue;  // 현재 값 (레벨업이나 버프에 의해 변화할 수 있음)
    public float maxValue; // 최댓값
    public float statLevel; // 현재 스탯의 레벨
    public float baseCost; // 레벨업에 처음 필요한 골드 비용
    public float levelUpCost; // 레벨업에 필요한 골드 비용
}
