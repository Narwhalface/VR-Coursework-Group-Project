using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightControl : MonoBehaviour
{
    public GameObject prompt;
    public GameObject flashlight;
    private float charge = 0f;

    private PlayerInput playerInput;
    private InputAction chargeLight;

    private int flickerAmount = 0;
    private int chargeDelay = 0;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        chargeLight = playerInput.actions["Flashlight Charge"];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight.GetComponent<Light>().intensity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float flicker = 0f;
        if (flickerAmount > 5 && charge > 0f)
        {
            flicker = Random.Range(-0.8f, 0.8f);

            flickerAmount = 0;
        }

        if (0.1f < charge && charge < 3f && !flashlight.GetComponents<AudioSource>()[1].isPlaying)
        {
            flashlight.GetComponents<AudioSource>()[1].Play();
        } else if (charge <= 0.1f)
        {
            flashlight.GetComponents<AudioSource>()[1].Stop();
        }

        if (chargeLight.WasPressedThisFrame() && chargeDelay <= 0)
        {
            chargeDelay = 250;
            flashlight.GetComponents<AudioSource>()[1].Stop();
            flashlight.GetComponents<AudioSource>()[0].Play();
            prompt.SetActive(false);
            charge += Time.deltaTime * 100f;
            charge = Mathf.Clamp(charge, 0f, 6);
            flashlight.GetComponent<Light>().intensity = Mathf.Clamp(charge + flicker, 0f, 3f)*1.5f;
        }
        else
        {
            charge = charge - Time.deltaTime * 0.2f;
            charge = Mathf.Clamp(charge, 0f, 6);
            flashlight.GetComponent<Light>().intensity = Mathf.Clamp(charge + flicker, 0f, 3f)*1.5f;
        }
        flickerAmount += 1;
        chargeDelay -= 1;
    }
}
