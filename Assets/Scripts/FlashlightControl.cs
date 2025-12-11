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
        
        if (chargeLight.WasPressedThisFrame())
        {
            prompt.SetActive(false);
            charge += Time.deltaTime * 50f;
            charge = Mathf.Clamp(charge, 0f, 5);
            flashlight.GetComponent<Light>().intensity = Mathf.Clamp(charge + flicker, 0f, 2f);
        }
        else
        {
            charge = charge - Time.deltaTime * 0.2f;
            charge = Mathf.Clamp(charge, 0f, 5);
            flashlight.GetComponent<Light>().intensity = Mathf.Clamp(charge + flicker, 0f, 2f);
        }
        flickerAmount += 1;
    }
}
