using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public GameObject crosshair;
    public Sprite ready;
    public Sprite grab;
    public GameObject player;
    private PlayerInput playerInput;
    private InputAction interaction;
    public float maxDistance = 3f;
    public Camera cam;
    Transform selectedDoor;
    GameObject dragAnchor;

    GameObject hitObject;
    public LayerMask doorLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crosshair.SetActive(false);
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        interaction = playerInput.actions["Interact"];
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, doorLayer) && selectedDoor == null)
        {
            crosshair.SetActive(true);
            crosshair.GetComponent<UnityEngine.UI.Image>().sprite = ready;

            if (interaction.ReadValue<float>() == 1)
            {
                crosshair.GetComponent<UnityEngine.UI.Image>().sprite = grab;
                selectedDoor = hit.collider.gameObject.transform;
            }
        }
        else if (selectedDoor == null)
        {
            crosshair.SetActive(false);
        }


        if (selectedDoor != null)
        {
            HingeJoint hinge = selectedDoor.GetComponent<HingeJoint>();
            JointMotor motor = hinge.motor; 

            hinge.useMotor = true;
            motor.force = 10;
            motor.targetVelocity = 50;

            hinge.motor = motor;;
            if (interaction.ReadValue<float>() == 0)
            {
                crosshair.SetActive(false);
                selectedDoor = null;
                hinge.useMotor = false;
                motor.targetVelocity = 0;
                motor.force = 0;
                hinge.motor = motor;
                player.GetComponent<CharacterMovement>().MoveSpeed = 5f;
            }

        }
    }
}
