using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX; // ��������� ������������ ���� ��� ������ � Visual Effect

public class ArrowBehaviour : MonoBehaviour
{
    public float stickForce = 10f; // ����, � ������� ������ ��������� � ������
    public VisualEffect hitEffectPrefab; // ������ Visual Effect ��� ������������

    private Rigidbody rb;
    private bool hasStuck = false; // ����, �����������, ��������� �� ������

    void Start()
    {
        // �������� ��������� Rigidbody
        rb = GetComponent<Rigidbody>();

        // ��������, ��� Rigidbody ����������
        if (rb == null)
        {
            Debug.LogError("� ������ ����������� ��������� Rigidbody!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // ���� ������ ��� ���������, �������
        if (hasStuck) return;

        // ������������� ������ ������
        rb.isKinematic = true;

        // ��������� ���������, ����� ������ �� ����������������� � ������� ���������
        GetComponent<Collider>().enabled = false;

        // ����������� ������ � �������, � ������� ��� ������
        transform.SetParent(collision.transform);

        // ��������� ��������� ����, ����� ������ "���������" � ������
        Vector3 stickDirection = collision.contacts[0].normal; // ������� �����������
        rb.AddForce(stickDirection * stickForce, ForceMode.Impulse);

        // ������������� ����, ��� ������ ���������
        hasStuck = true;

        // ������������� Visual Effect �� ����� ������������
        if (hitEffectPrefab != null)
        {
            // ������� ������ �� ����� ������������
            VisualEffect hitEffect = Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);

            // ���������� ������ �� ������� �����������
            hitEffect.transform.forward = stickDirection;

            // ���������� ������ ����� 2 ������� (����� �� �������� �����)
            Destroy(hitEffect.gameObject, 2f);
        }

        // ���������� ������ ����� 10 ������ (�����������)
        Destroy(gameObject, 10f);
    }
}
