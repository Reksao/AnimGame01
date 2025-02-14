using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Stan : MonoBehaviour
{
    public Animator anim;
    public GameObject visualEffectPrefab; // Префаб визуального эффекта (например, Particle System)
    public float heightOffset = 2.0f; // Высота над головой персонажа
    public float effectLifetime = 3.0f;
    

    private GameObject currentEffect; // Текущий созданный эффект

    void Start()
    {
        // Получаем компонент Animator
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
            Debug.LogError("Не назначен префаб визуального эффекта!");
            return;
        }

        // Уничтожаем предыдущий эффект, если он существует
        if (currentEffect != null)
        {
            Destroy(currentEffect);
        }

        // Получаем позицию головы персонажа
        Vector3 headPosition = GetHeadPosition();

        // Вычисляем позицию для нового эффекта
        Vector3 spawnPosition = headPosition + Vector3.up * heightOffset;

        // Создаем эффект на сцене
        currentEffect = Instantiate(visualEffectPrefab, spawnPosition, Quaternion.identity);

        // Делаем эффект дочерним к персонажу (опционально)
        currentEffect.transform.parent = this.transform;

        // Уничтожаем эффект через заданное время
        Destroy(currentEffect, effectLifetime);
    }

    Vector3 GetHeadPosition()
    {
        anim = GetComponent<Animator>();
        Vector3 headPosition = anim.GetBoneTransform(HumanBodyBones.Head).position;
        // Здесь можно использовать разные способы для определения позиции головы:
        // 1. Если у персонажа есть кость головы, используйте ее позицию.
        // 2. Если нет, используйте позицию персонажа + смещение.

        // Пример: предполагаем, что голова находится на высоте 1.5 единицы над позицией персонажа
        return transform.position + Vector3.up * 1.5f;
    }
    
}
