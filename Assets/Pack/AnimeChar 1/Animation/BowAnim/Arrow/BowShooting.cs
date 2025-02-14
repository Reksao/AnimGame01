using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon Draw Settings")]
    public KeyCode drawKey = KeyCode.E; // Клавиша для доставания оружия
    public GameObject weapon; // Ссылка на объект оружия
    public GameObject crosshair; // Ссылка на объект прицела

    [Header("Bow Shooting Settings")]
    public KeyCode shootKey = KeyCode.Mouse0; // Клавиша для стрельбы (левая кнопка мыши)
    public string layerName = "BowLayer"; // Название слоя для анимаций лука
    public string drawAnimationName = "DrawBow"; // Имя анимации натяжения тетивы
    public string shootAnimationName = "ShootBow"; // Имя анимации выстрела
    public float transitionSpeed = 5f; // Скорость плавного перехода
    public GameObject arrowPrefab; // Префаб стрелы
    public Transform arrowSpawnPoint; // Точка вылета стрелы
    public float arrowForce = 10f; // Сила, с которой стрела вылетает

    private Animator animator;
    private bool isWeaponDrawn = false; // Флаг, указывающий, находится ли оружие в руках
    private bool isDrawing = false; // Флаг, указывающий, натянута ли тетива
    private bool isShooting = false; // Флаг, указывающий, происходит ли выстрел
    private bool canShoot = true; // Флаг, разрешающий выстрел
    private float targetWeight = 0f; // Целевой вес слоя (0 или 1)
    private int layerIndex; // Индекс слоя, который будет вычисляться по имени

    void Start()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();

        // Получаем индекс слоя по его имени
        layerIndex = animator.GetLayerIndex(layerName);

        // Проверяем, существует ли слой с таким именем
        if (layerIndex == -1)
        {
            Debug.LogError($"Слой с именем '{layerName}' не найден в Animator!");
            enabled = false; // Отключаем скрипт, если слой не найден
            return;
        }

        // Изначально выключаем слой
        animator.SetLayerWeight(layerIndex, 0f);
        animator.SetBool("IsDrawing", false);
        animator.SetBool("IsShooting", false);

        // Скрываем оружие и прицел в начале
        weapon.SetActive(false);
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    void Update()
    {
        // Обработка доставания/убирания оружия
        if (Input.GetKeyDown(drawKey))
        {
            if (isWeaponDrawn)
            {
                HideWeapon();
            }
            else
            {
                DrawWeaponAnimation();
            }
        }

        // Обработка стрельбы (если оружие в руках)
        if (isWeaponDrawn)
        {
            // Если нажата кнопка мыши и выстрел разрешен, начинаем натяжение тетивы
            if (Input.GetKeyDown(shootKey) && canShoot)
            {
                StartDrawing();
            }

            // Если кнопка мыши отпущена, тетива натянута, и анимация натяжения завершена, производим выстрел
            if (Input.GetKeyUp(shootKey) && isDrawing && IsAnimationComplete(drawAnimationName))
            {
                Shoot();
            }

            // Плавно изменяем вес слоя
            float currentWeight = animator.GetLayerWeight(layerIndex);
            float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * transitionSpeed);
            animator.SetLayerWeight(layerIndex, newWeight);

            // Если анимация выстрела завершена, возвращаемся в исходное состояние
            if (isShooting && IsAnimationComplete(shootAnimationName))
            {
                ResetState();
            }
        }
    }

    void DrawWeaponAnimation()
    {
        // Активируем триггер для воспроизведения анимации
        animator.SetTrigger("DrawWeapon");
        isWeaponDrawn = true;
    }

    // Метод, который будет вызываться через Animation Event
    void EnableWeapon()
    {
        weapon.SetActive(true);

        // Включаем прицел, если он назначен
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }

    void HideWeapon()
    {
        // Скрываем оружие
        weapon.SetActive(false);
        isWeaponDrawn = false;

        // Выключаем прицел, если он назначен
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    void StartDrawing()
    {
        // Включаем анимацию натяжения тетивы
        animator.SetBool("IsDrawing", true);
        isDrawing = true;
        targetWeight = 1f; // Плавно включаем слой
        canShoot = false; // Блокируем возможность стрельбы до завершения анимации
    }

    void Shoot()
    {
        // Включаем анимацию выстрела
        animator.SetBool("IsShooting", true);
        animator.SetBool("IsDrawing", false); // Отключаем анимацию натяжения
        isShooting = true;
        isDrawing = false;

        // Создаем стрелу и задаем ее движение
        SpawnArrow();
    }

    void SpawnArrow()
    {
        // Проверяем, есть ли префаб стрелы и точка вылета
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogError("Префаб стрелы или точка вылета не назначены!");
            return;
        }

        // Создаем стрелу на сцене
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        // Получаем компонент Rigidbody стрелы
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

        // Проверяем, есть ли Rigidbody
        if (arrowRigidbody == null)
        {
            Debug.LogError("У стрелы отсутствует компонент Rigidbody!");
            return;
        }

        // Задаем скорость стреле
        arrowRigidbody.velocity = arrowSpawnPoint.forward * arrowForce;

        // Уничтожаем стрелу через 5 секунд (чтобы не засорять сцену)
        Destroy(arrow, 5f);
    }

    void ResetState()
    {
        // Возвращаемся в исходное состояние
        animator.SetBool("IsShooting", false);
        animator.SetBool("IsDrawing", false);
        isShooting = false;
        targetWeight = 0f; // Плавно выключаем слой
        canShoot = true; // Разрешаем следующий выстрел
    }

    bool IsAnimationComplete(string animationName)
    {
        // Получаем информацию о текущем состоянии анимации
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        // Проверяем, завершилась ли анимация
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }
}
