using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public AudioClip fireClip;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Only shoot on button press (once)
        if (Input.GetButtonDown("Fire1")) // Use your actual shoot input here
        {
            FireWeapon();
        }
    }

    void FireWeapon()
    {
        // Your shooting logic goes here

        // Play the fire sound once
        if (fireClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireClip);
        }
    }
}
