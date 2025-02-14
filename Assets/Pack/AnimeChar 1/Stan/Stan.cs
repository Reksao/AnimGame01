using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Stan : MonoBehaviour
{
    public Animator anim;
    public GameObject visualEffectPrefab; // ������ ����������� ������� (��������, Particle System)
    public float heightOffset = 2.0f; // ������ ��� ������� ���������
    public float effectLifetime = 3.0f;
    

    private GameObject currentEffect; // ������� ��������� ������

    void Start()
    {
        // �������� ��������� Animator
        anim = GetComponent<Animator>();


    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SpawnEffect();



            anim.SetTrigger("Stan");
            



        }
        
    }

    void SpawnEffect()
    {
        if (visualEffectPrefab == null)
        {
            Debug.LogError("�� �������� ������ ����������� �������!");
            return;
        }

        // ���������� ���������� ������, ���� �� ����������
        if (currentEffect != null)
        {
            Destroy(currentEffect);
        }

        // �������� ������� ������ ���������
        Vector3 headPosition = GetHeadPosition();

        // ��������� ������� ��� ������ �������
        Vector3 spawnPosition = headPosition + Vector3.up * heightOffset;

        // ������� ������ �� �����
        currentEffect = Instantiate(visualEffectPrefab, spawnPosition, Quaternion.identity);

        // ������ ������ �������� � ��������� (�����������)
        currentEffect.transform.parent = this.transform;

        // ���������� ������ ����� �������� �����
        Destroy(currentEffect, effectLifetime);
    }

    Vector3 GetHeadPosition()
    {
        anim = GetComponent<Animator>();
        Vector3 headPosition = anim.GetBoneTransform(HumanBodyBones.Head).position;
        // ����� ����� ������������ ������ ������� ��� ����������� ������� ������:
        // 1. ���� � ��������� ���� ����� ������, ����������� �� �������.
        // 2. ���� ���, ����������� ������� ��������� + ��������.

        // ������: ������������, ��� ������ ��������� �� ������ 1.5 ������� ��� �������� ���������
        return transform.position + Vector3.up * 1.5f;
    }
    
}
