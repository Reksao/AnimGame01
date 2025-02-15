using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon Draw Settings")]
    public KeyCode drawKey = KeyCode.E; // ������� ��� ���������� ������
    public GameObject weapon; // ������ �� ������ ������
    public GameObject crosshair; // ������ �� ������ �������

    [Header("Bow Shooting Settings")]
    public KeyCode shootKey = KeyCode.Mouse0; // ������� ��� �������� (����� ������ ����)
    public string layerName = "BowLayer"; // �������� ���� ��� �������� ����
    public string drawAnimationName = "DrawBow"; // ��� �������� ��������� ������
    public string shootAnimationName = "ShootBow"; // ��� �������� ��������
    public float transitionSpeed = 5f; // �������� �������� ��������
    public GameObject arrowPrefab; // ������ ������
    public Transform arrowSpawnPoint; // ����� ������ ������
    public float arrowForce = 10f; // ����, � ������� ������ ��������

    private Animator animator;
    private bool isWeaponDrawn = false; // ����, �����������, ��������� �� ������ � �����
    private bool isDrawing = false; // ����, �����������, �������� �� ������
    private bool isShooting = false; // ����, �����������, ���������� �� �������
    private bool canShoot = true; // ����, ����������� �������
    private float targetWeight = 0f; // ������� ��� ���� (0 ��� 1)
    private int layerIndex; // ������ ����, ������� ����� ����������� �� �����
    public bool _isAiming = false;

    void Start()
    {
        // �������� ��������� Animator
        animator = GetComponent<Animator>();

        // �������� ������ ���� �� ��� �����
        layerIndex = animator.GetLayerIndex(layerName);

        // ���������, ���������� �� ���� � ����� ������
        if (layerIndex == -1)
        {
            Debug.LogError($"���� � ������ '{layerName}' �� ������ � Animator!");
            enabled = false; // ��������� ������, ���� ���� �� ������
            return;
        }

        // ���������� ��������� ����
        animator.SetLayerWeight(layerIndex, 0f);
        animator.SetBool("IsDrawing", false);
        animator.SetBool("IsShooting", false);

        // �������� ������ � ������ � ������
        weapon.SetActive(false);
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    void Update()
    {
        // ��������� ����������/�������� ������
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

        // ��������� �������� (���� ������ � �����)
        if (isWeaponDrawn)
        {
            // ���� ������ ������ ���� � ������� ��������, �������� ��������� ������
            if (Input.GetKeyDown(shootKey) && canShoot)
            {
                StartDrawing();
                _isAiming = true;
            }

            // ���� ������ ���� ��������, ������ ��������, � �������� ��������� ���������, ���������� �������
            if (Input.GetKeyUp(shootKey) && isDrawing && IsAnimationComplete(drawAnimationName))
            {
                Shoot();
            }

            // ������ �������� ��� ����
            float currentWeight = animator.GetLayerWeight(layerIndex);
            float newWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * transitionSpeed);
            animator.SetLayerWeight(layerIndex, newWeight);

            // ���� �������� �������� ���������, ������������ � �������� ���������
            if (isShooting && IsAnimationComplete(shootAnimationName))
            {
                ResetState();
            }
        }
    }

    void DrawWeaponAnimation()
    {
        // ���������� ������� ��� ��������������� ��������
        animator.SetTrigger("DrawWeapon");
        isWeaponDrawn = true;
    }

    // �����, ������� ����� ���������� ����� Animation Event
    void EnableWeapon()
    {
        weapon.SetActive(true);

        // �������� ������, ���� �� ��������
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
    }

    void HideWeapon()
    {
        // �������� ������
        weapon.SetActive(false);
        isWeaponDrawn = false;

        // ��������� ������, ���� �� ��������
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    void StartDrawing()
    {
        // �������� �������� ��������� ������
        animator.SetBool("IsDrawing", true);
        isDrawing = true;
        targetWeight = 1f; // ������ �������� ����
        canShoot = false; // ��������� ����������� �������� �� ���������� ��������
    }

    void Shoot()
    {
        // �������� �������� ��������
        animator.SetBool("IsShooting", true);
        animator.SetBool("IsDrawing", false); // ��������� �������� ���������
        isShooting = true;
        isDrawing = false;

        // ������� ������ � ������ �� ��������
        SpawnArrow();
        _isAiming = false;
    }

    void SpawnArrow()
    {
        // ���������, ���� �� ������ ������ � ����� ������
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogError("������ ������ ��� ����� ������ �� ���������!");
            return;
        }

        // ������� ������ �� �����
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        // �������� ��������� Rigidbody ������
        Rigidbody arrowRigidbody = arrow.GetComponent<Rigidbody>();

        // ���������, ���� �� Rigidbody
        if (arrowRigidbody == null)
        {
            Debug.LogError("� ������ ����������� ��������� Rigidbody!");
            return;
        }

        // ������ �������� ������
        arrowRigidbody.velocity = arrowSpawnPoint.forward * arrowForce;

        // ���������� ������ ����� 5 ������ (����� �� �������� �����)
        Destroy(arrow, 5f);
    }

    void ResetState()
    {
        // ������������ � �������� ���������
        animator.SetBool("IsShooting", false);
        animator.SetBool("IsDrawing", false);
        isShooting = false;
        targetWeight = 0f; // ������ ��������� ����
        canShoot = true; // ��������� ��������� �������
    }

    bool IsAnimationComplete(string animationName)
    {
        // �������� ���������� � ������� ��������� ��������
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

        // ���������, ����������� �� ��������
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }
}
