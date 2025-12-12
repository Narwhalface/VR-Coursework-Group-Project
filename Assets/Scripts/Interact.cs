using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public GameObject crosshair;
    public Sprite ready;
    public Sprite grab;
    public GameObject player;
    public GameObject bookKeyTeleporter;
    private PlayerInput playerInput;
    private InputAction interaction;
    public float maxDistance = 3f;
    public Camera cam;
    Transform selectedObject;
    GameObject dragAnchor;
    GameObject hitObject;
    public LayerMask interactLayer;
    bool bookKeyActivated;

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
        if (bookKeyTeleporter != null)
        {
            bookKeyTeleporter.SetActive(bookKeyActivated);
        }

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, interactLayer) && selectedObject == null)
        {
            crosshair.SetActive(true);
            crosshair.GetComponent<UnityEngine.UI.Image>().sprite = ready;

            if (interaction.ReadValue<float>() == 1)
            {
                crosshair.GetComponent<UnityEngine.UI.Image>().sprite = grab;
                selectedObject = hit.collider.gameObject.transform;
            }
        }else if (selectedObject == null)
        {
            crosshair.SetActive(false);
        }



        if (selectedObject != null)
        {
            if (selectedObject.CompareTag("Door") == true)
            {
                HingeJoint hinge = selectedObject.GetComponent<HingeJoint>();
                JointMotor motor = hinge.motor; 

                hinge.useMotor = true;
                motor.force = 10;
                motor.targetVelocity = 50;

                hinge.motor = motor;;
                if (interaction.ReadValue<float>() == 0)
                {
                    crosshair.SetActive(false);
                    selectedObject = null;
                    hinge.useMotor = false;
                    motor.targetVelocity = 0;
                    motor.force = 0;
                    hinge.motor = motor;
                    player.GetComponent<CharacterMovement>().MoveSpeed = 5f;
                }
            }else if (selectedObject.CompareTag("Candle") == true)
            {
                
                //selectedObject.GetChild(0).GetComponent<Light>().enabled = !selectedObject.GetChild(0).GetComponent<Light>().enabled;
                // Prevent toggling when key held by only toggling on key press (rising edge)
                if (interaction.WasPressedThisFrame())
                {
                    ParticleSystem.EmissionModule em = selectedObject.GetChild(0).GetComponent<ParticleSystem>().emission;
                    em.enabled = !em.enabled;
                }


            }else if (selectedObject.CompareTag("BookKey") == true || selectedObject.root.CompareTag("BookKey") == true)
            {
                if (interaction.WasPressedThisFrame())
                {
                    Transform book = selectedObject.CompareTag("BookKey") ? selectedObject : selectedObject.root;

                    if (bookKeyTeleporter != null)
                    {
                        bookKeyActivated = true;
                        bookKeyTeleporter.SetActive(true);
                        book.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("Interact: Book key teleporter reference is missing.");
                    }

                    selectedObject = null;
                    crosshair.SetActive(false);
                }


            }

            if (interaction.ReadValue<float>() == 0)
            {
                selectedObject = null;
                crosshair.SetActive(false);
            }
        }
    }
}
