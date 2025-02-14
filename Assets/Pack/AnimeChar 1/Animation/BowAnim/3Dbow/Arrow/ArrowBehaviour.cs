using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX; // Добавляем пространство имен для работы с Visual Effect

public class ArrowBehaviour : MonoBehaviour
{
    public float stickForce = 10f; // Сила, с которой стрела втыкается в объект
    public VisualEffect hitEffectPrefab; // Префаб Visual Effect для столкновения

    private Rigidbody rb;
    private bool hasStuck = false; // Флаг, указывающий, втыкалась ли стрела

    void Start()
    {
        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();

        // Убедимся, что Rigidbody существует
        if (rb == null)
        {
            Debug.LogError("У стрелы отсутствует компонент Rigidbody!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Если стрела уже втыкалась, выходим
        if (hasStuck) return;

        // Останавливаем физику стрелы
        rb.isKinematic = true;

        // Отключаем коллайдер, чтобы стрела не взаимодействовала с другими объектами
        GetComponent<Collider>().enabled = false;

        // Прикрепляем стрелу к объекту, в который она попала
        transform.SetParent(collision.transform);

        // Добавляем небольшую силу, чтобы стрела "втыкалась" в объект
        Vector3 stickDirection = collision.contacts[0].normal; // Нормаль поверхности
        rb.AddForce(stickDirection * stickForce, ForceMode.Impulse);

        // Устанавливаем флаг, что стрела втыкалась
        hasStuck = true;

        // Воспроизводим Visual Effect на месте столкновения
        if (hitEffectPrefab != null)
        {
            // Создаем эффект на месте столкновения
            VisualEffect hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);

            // Направляем эффект по нормали поверхности
            hitEffect.transform.forward = stickDirection;

            // Уничтожаем эффект через 2 секунды (чтобы не засорять сцену)
            Destroy(hitEffect.gameObject, 2f);
        }

        // Уничтожаем стрелу через 10 секунд (опционально)
        Destroy(gameObject, 10f);
    }
}
