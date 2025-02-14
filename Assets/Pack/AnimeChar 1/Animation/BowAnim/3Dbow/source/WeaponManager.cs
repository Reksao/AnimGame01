using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.E; // ������� ��� ����������/�������� ������
    public GameObject weapon; // ������ �� ������ ������

    private Animator animator;
    private bool isWeaponDrawn = false; // ����, �����������, ��������� �� ������ � �����


    void Start()
    {
        // �������� ��������� Animator
        animator = GetComponent<Animator>();

        // �������� ������ � ������
        weapon.SetActive(false);
    }

    void Update()
    {
        // ���������, ������ �� �������
        if (Input.GetKeyDown(toggleKey))
        {
            // ����������� ��������� ������
            if (isWeaponDrawn)
            {
                // ������� ������
                HolsterWeapon();
            }
            else
            {
                // ������� ������
                DrawWeapon();
            }
        }
    }

    void DrawWeapon()
    {
        // �������� �������� ���������� ������
        animator.SetBool("IsWeaponDrawn", true);
        isWeaponDrawn = true;
    }

    void HolsterWeapon()
    {
        // �������� �������� �������� ������
        animator.SetBool("IsWeaponDrawn", false);
        isWeaponDrawn = false;
    }

    // �����, ������� ����� ���������� ����� Animation Event � �������� DrawWeapon
    void EnableWeapon()
    {
        weapon.SetActive(true);
    }

    // �����, ������� ����� ���������� ����� Animation Event � �������� HolsterWeapon
    void DisableWeapon()
    {
        weapon.SetActive(false);
    }
}
