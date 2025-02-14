using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.E; // Клавиша для доставания/убирания оружия
    public GameObject weapon; // Ссылка на объект оружия

    private Animator animator;
    private bool isWeaponDrawn = false; // Флаг, указывающий, находится ли оружие в руках


    void Start()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();

        // Скрываем оружие в начале
        weapon.SetActive(false);
    }

    void Update()
    {
        // Проверяем, нажата ли клавиша
        if (Input.GetKeyDown(toggleKey))
        {
            // Переключаем состояние оружия
            if (isWeaponDrawn)
            {
                // Убираем оружие
                HolsterWeapon();
            }
            else
            {
                // Достаем оружие
                DrawWeapon();
            }
        }
    }

    void DrawWeapon()
    {
        // Включаем анимацию доставания оружия
        animator.SetBool("IsWeaponDrawn", true);
        isWeaponDrawn = true;
    }

    void HolsterWeapon()
    {
        // Включаем анимацию убирания оружия
        animator.SetBool("IsWeaponDrawn", false);
        isWeaponDrawn = false;
    }

    // Метод, который будет вызываться через Animation Event в анимации DrawWeapon
    void EnableWeapon()
    {
        weapon.SetActive(true);
    }

    // Метод, который будет вызываться через Animation Event в анимации HolsterWeapon
    void DisableWeapon()
    {
        weapon.SetActive(false);
    }
}
