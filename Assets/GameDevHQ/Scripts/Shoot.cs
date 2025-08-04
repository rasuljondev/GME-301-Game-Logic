using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _Bullets;
    [SerializeField] private LayerMask shootableMask; // Assign "AI" + "Barrier" layers in Inspector

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo, 100f, shootableMask))
            {
                // Optional: Visual bullet
                Vector3 direction = (hitInfo.point - transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction);
                Instantiate(_Bullets, hitInfo.point, rotation);

                // Try killing AI
                AI ai = hitInfo.collider.GetComponentInParent<AI>();
                if (ai != null)
                {
                    ai.TriggerDeath();
                }

                Debug.Log("Hit: " + hitInfo.collider.name);
            }
        }
    }
}
