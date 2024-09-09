using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageViewer : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI textMP;

    private MagicSword _magicSword;

    public float textUpSpeed, alphaSpeed, destroyTime;

    private void Start()
    {
        textMP = GetComponent<TextMeshProUGUI>();

        _magicSword = GameObject.Find("Player").GetComponent<MagicSword>();

        textMP.text = _magicSword.swordStats.stats.Find(stat => stat.statName == "���ݷ�").currentValue.ToString();

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
        //���߿� Ǯ�Ŵ���
    }
}
