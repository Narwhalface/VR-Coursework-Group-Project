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

            if (dragAnchor == null)
            {
                dragAnchor = new GameObject("Drag Anchor");
                dragAnchor.transform.position = hit.point;
                dragAnchor.transform.parent = selectedDoor;
            }

            if (hitObject == null)
            {
                hitObject = new GameObject("Hit Object");
                hitObject.transform.position = hit.point;
                hitObject.transform.parent = selectedDoor;
            }

            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            dragAnchor.transform.position = ray.GetPoint(Mathf.Clamp(Vector3.Distance(hitObject.transform.position, transform.position), 0.5f, 2f));
            dragAnchor.transform.rotation = selectedDoor.rotation;

            float delta = Mathf.Pow(Vector3.Distance(dragAnchor.transform.position, hitObject.transform.position), 3);

            player.GetComponent<CharacterMovement>().MoveSpeed = Mathf.Clamp(5f/(Vector3.Distance(dragAnchor.transform.position, hitObject.transform.position)*5), 0f, 5f);

            float speedMultiplier = 500f;
            if (Mathf.Abs(selectedDoor.forward.z) > 0.5f)
            {
                if (dragAnchor.transform.position.x > hitObject.transform.position.x)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime;
                }
            }
            else
            {
                if (dragAnchor.transform.position.z > hitObject.transform.position.z)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime;
                }
            }
            hinge.motor = motor;

            if (interaction.ReadValue<float>() == 0 || Vector3.Distance(dragAnchor.transform.position, hitObject.transform.position) > maxDistance)
            {
                crosshair.SetActive(false);
                selectedDoor = null;
                motor.targetVelocity = 0;
                hinge.motor = motor;
                Destroy(dragAnchor);
                Destroy(hitObject);
                player.GetComponent<CharacterMovement>().MoveSpeed = 5f;
            }

            

            if (dragAnchor != null)
            {
                Debug.DrawLine(cam.transform.position, dragAnchor.transform.position, Color.green);
                Debug.DrawLine(hitObject.transform.position, dragAnchor.transform.position, Color.red);
            }
        }
    }
}
