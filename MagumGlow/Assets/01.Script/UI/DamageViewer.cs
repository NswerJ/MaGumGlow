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

    private void Start()
    {
        textMP = GetComponent<TextMeshPro>();

        _magicSword = GameObject.Find("Player").GetComponent<MagicSwordPlayer>();

        textMP.text = _magicSword.swordStats.stats.Find(stat => stat.statName == "공격력").currentValue.ToString();

        StartCoroutine(nameof(DestroyUI));
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, textUpSpeed * Time.deltaTime, 0));

        textMP.color = new Color(textMP.color.r, textMP.color.g, textMP.color.b, Mathf.Lerp(textMP.color.a, 0, Time.deltaTime * alphaSpeed));
    }

    private IEnumerator DestroyUI()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
        //나중에 풀매니저
    }
}
