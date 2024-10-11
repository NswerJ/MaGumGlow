using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageViewer : MonoBehaviour
{
    [HideInInspector] public TextMeshPro textMP;

    private MagicSwordPlayer _magicSword;

    public float textUpSpeed, alphaSpeed, destroyTime;
    public float damageValue;

    private void Start()
    {
        textMP = GetComponent<TextMeshPro>();

        _magicSword = GameObject.Find("Player").GetComponent<MagicSwordPlayer>();
        if (_magicSword.criticalCheck)
        {
            damageValue = _magicSword.swordStats.stats.Find(stat => stat.statName == "공격력").currentValue *
                          _magicSword.swordStats.stats.Find(stat => stat.statName == "치명타데미지").currentValue;
        }
        else
        {
            damageValue = _magicSword.swordStats.stats.Find(stat => stat.statName == "공격력").currentValue;
        }

        textMP.text = NumberFormatter.FormatWithUnit(damageValue).ToString(); 
        StartCoroutine(nameof(DestroyUI)); 

        if (_magicSword.criticalCheck)
        {
            StartCoroutine(DamageTextColor());
        }
        else
        {
            textMP.color = Color.white;
        }
    }

    private IEnumerator DamageTextColor()
    {
        while (true)
        {
            textMP.color = Color.red; 
            yield return new WaitForSeconds(0.1f); 
            textMP.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, textUpSpeed * Time.deltaTime, 0));
    }

    private IEnumerator DestroyUI()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
        // 나중에 풀 매니저 추가 예정
    }
}
