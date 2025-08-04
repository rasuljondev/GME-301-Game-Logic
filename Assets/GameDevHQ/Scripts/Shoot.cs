using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _Bullets;
    [SerializeField] private LayerMask shootableMask;
    [SerializeField] private AudioClip barrierHitSound; // ✅ New

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
                Vector3 direction = (hitInfo.point - transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction);
                Instantiate(_Bullets, hitInfo.point, rotation);

                AI ai = hitInfo.collider.GetComponentInParent<AI>();
                if (ai != null)
                {
                    ai.TriggerDeath();
                }

                // ✅ Play barrier hit sound
                if (hitInfo.collider.CompareTag("Barrier"))
                {
                    if (barrierHitSound != null)
                    {
                        GameObject tempAudio = new GameObject("TempBarrierHitSound");
                        AudioSource source = tempAudio.AddComponent<AudioSource>();
                        source.clip = barrierHitSound;
                        source.spatialBlend = 0f;
                        source.volume = 1f;
                        source.Play();
                        Destroy(tempAudio, barrierHitSound.length + 0.1f);
                    }

                    Debug.Log("Hit a barrier, played sound.");
                }

                Debug.Log("Hit: " + hitInfo.collider.name);
            }
        }
    }
}

