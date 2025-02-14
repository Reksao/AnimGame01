using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowStoyka : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.I; // Клавиша для включения анимации
    public string equipLayerName = "Bow"; // Имя слоя, который нужно включать/выключать для экипировки
    public string unequipAnimationName = "UnequipBow"; // Имя анимации для убирания лука
    public string equipAnimationName = "EquipBow"; // Имя анимации для экипировки
    public float transitionSpeed = 5f; // Скорость плавного перехода

    private Animator animator;
    private bool isBowEquipped = false;
    private bool isUnequipping = false; // Флаг, указывающий, что анимация убирания лука выполняется
    private float targetWeight = 0f; // Целевой вес слоя (0 или 1)
    private int layerIndex; // Индекс слоя

    void Start()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();

        // Получаем индекс слоя по имени
        layerIndex = animator.GetLayerIndex(equipLayerName);

        // Проверяем, существует ли слой с таким именем
        if (layerIndex == -1)
        {
            Debug.LogError($"Слой с именем '{equipLayerName}' не найден в Animator!");
            enabled = false; // Отключаем скрипт, если слой не найден
            return;
        }

        // Изначально выключаем слой
        animator.SetLayerWeight(layerIndex, 0f);
        animator.SetBool("IsBowEquipped", false);
    }

    void Update()
    {
        // Проверяем, нажата ли клавиша
        if (Input.GetKeyDown(toggleKey))
        {
            // Переключаем анимацию
            if (isBowEquipped)
            {
                UnequipBow();
            }
            else
            {
                EquipBow();
            }
        }

        // Плавно изменяем вес слоя
        float currentWeight = animator.GetLayerWeight(layerIndex);
        float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * transitionSpeed);
        animator.SetLayerWeight(layerIndex, newWeight);

        // Если анимация убирания лука завершена, сбрасываем флаг
        if (isUnequipping && IsAnimationComplete(unequipAnimationName))
        {
            isUnequipping = false;
            // Убедитесь, что вес слоя установлен точно в 0
            animator.SetLayerWeight(layerIndex, 0f);
        }
    }

    void EquipBow()
    {
        // Включаем анимацию экипировки
        animator.SetBool("IsBowEquipped", true);
        isBowEquipped = true;
        targetWeight = 1f; // Устанавливаем целевой вес в 1 для плавного включения

        // Запускаем анимацию экипировки
        animator.Play(equipAnimationName, layerIndex, 0f); // Воспроизводим анимацию с начала
    }

    void UnequipBow()
    {
        // Включаем анимацию убирания
        animator.SetBool("IsBowEquipped", false);
        isBowEquipped = false;
        isUnequipping = true; // Устанавливаем флаг, что анимация убирания лука выполняется
        targetWeight = 0f; // Устанавливаем целевой вес в 0 для плавного выключения

        // Запускаем анимацию убирания
        animator.Play(unequipAnimationName, layerIndex, 0f); // Воспроизводим анимацию с начала
    }

    bool IsAnimationComplete(string animationName)
    {
        // Получаем информацию о текущем состоянии анимации
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        // Проверяем, завершилась ли анимация
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }
}



