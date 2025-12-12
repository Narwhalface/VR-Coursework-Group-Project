using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightControl : MonoBehaviour
{

    public GameObject flashlight;
    private float charge = 0f;

    private PlayerInput playerInput;
    private InputAction chargeLight;

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
        if (chargeLight.WasPressedThisFrame())
        {
            charge += Time.deltaTime * 25f;
            charge = Mathf.Clamp(charge, 0f, 150f);
            flashlight.GetComponent<Light>().intensity = Mathf.Clamp(charge, 0f, 65f);
        }
        else
        {
            charge = charge - Time.deltaTime * 0.2f;
            flashlight.GetComponent<Light>().intensity = charge;
        }
    }
}
