using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowStoyka : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.I; // ������� ��� ��������� ��������
    public string equipLayerName = "Bow"; // ��� ����, ������� ����� ��������/��������� ��� ����������
    public string unequipAnimationName = "UnequipBow"; // ��� �������� ��� �������� ����
    public string equipAnimationName = "EquipBow"; // ��� �������� ��� ����������
    public float transitionSpeed = 5f; // �������� �������� ��������

    private Animator animator;
    private bool isBowEquipped = false;
    private bool isUnequipping = false; // ����, �����������, ��� �������� �������� ���� �����������
    private float targetWeight = 0f; // ������� ��� ���� (0 ��� 1)
    private int layerIndex; // ������ ����
    
    void Start()
    {
        // �������� ��������� Animator
        animator = GetComponent<Animator>();

        // �������� ������ ���� �� �����
        layerIndex = animator.GetLayerIndex(equipLayerName);

        // ���������, ���������� �� ���� � ����� ������
        if (layerIndex == -1)
        {
            Debug.LogError($"���� � ������ '{equipLayerName}' �� ������ � Animator!");
            enabled = false; // ��������� ������, ���� ���� �� ������
            return;
        }

        // ���������� ��������� ����
        animator.SetLayerWeight(layerIndex, 0f);
        animator.SetBool("IsBowEquipped", false);
    }

    void Update()
    {
        // ���������, ������ �� �������
        if (Input.GetKeyDown(toggleKey))
        {
            // ����������� ��������
            if (isBowEquipped)
            {
                UnequipBow();
            }
            else
            {
                EquipBow();
            }
        }

        // ������ �������� ��� ����
        float currentWeight = animator.GetLayerWeight(layerIndex);
        float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * transitionSpeed);
        animator.SetLayerWeight(layerIndex, newWeight);

        // ���� �������� �������� ���� ���������, ���������� ����
        if (isUnequipping && IsAnimationComplete(unequipAnimationName))
        {
            isUnequipping = false;
            // ���������, ��� ��� ���� ���������� ����� � 0
            animator.SetLayerWeight(layerIndex, 0f);
        }
    }

    void EquipBow()
    {
        // �������� �������� ����������
        animator.SetBool("IsBowEquipped", true);
        isBowEquipped = true;
        targetWeight = 1f; // ������������� ������� ��� � 1 ��� �������� ���������

        // ��������� �������� ����������
        animator.Play(equipAnimationName, layerIndex, 0f); // ������������� �������� � ������
    }

    void UnequipBow()
    {
        // �������� �������� ��������
        animator.SetBool("IsBowEquipped", false);
        isBowEquipped = false;
        isUnequipping = true; // ������������� ����, ��� �������� �������� ���� �����������
        targetWeight = 0f; // ������������� ������� ��� � 0 ��� �������� ����������

        // ��������� �������� ��������
        animator.Play(unequipAnimationName, layerIndex, 0f); // ������������� �������� � ������
    }

    bool IsAnimationComplete(string animationName)
    {
        // �������� ���������� � ������� ��������� ��������
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        // ���������, ����������� �� ��������
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }
}



