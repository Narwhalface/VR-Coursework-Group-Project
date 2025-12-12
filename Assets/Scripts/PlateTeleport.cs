using UnityEngine;

public class PlateTeleport : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private bool matchDestinationRotation = true;
    [SerializeField] private Vector3 detectionOffset = Vector3.zero;
    [SerializeField] private float detectionRadius = 0.4f;
    [SerializeField] private LayerMask playerMask = ~0;
    [SerializeField] private float retriggerDelay = 0.5f;

    public GameObject soundObject;

    private float nextAvailableTime;

    private void Awake()
    {
        if (destination == null)
        {
            destination = transform;
        }

        Collider plateCollider = GetComponent<Collider>();
        if (plateCollider != null && !plateCollider.isTrigger)
        {
            plateCollider.isTrigger = true;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time < nextAvailableTime)
        {
            return;
        }

        Vector3 checkCenter = transform.TransformPoint(detectionOffset);
        Collider[] hits = Physics.OverlapSphere(checkCenter, detectionRadius, playerMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < hits.Length; i++)
        {
            CharacterMovement movement = hits[i].GetComponentInParent<CharacterMovement>();
            if (movement == null)
            {
                continue;
            }

            Teleport(movement);
            nextAvailableTime = Time.time + retriggerDelay;
            break;
        }
    }

    private void Teleport(CharacterMovement movement)
    {
        soundObject.GetComponents<AudioSource>()[1].Play();
        soundObject.GetComponents<AudioSource>()[0].Play();
        Transform playerTransform = movement.transform;
        CharacterController controller = movement.controller != null
            ? movement.controller
            : playerTransform.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        playerTransform.SetPositionAndRotation(destination.position,
            matchDestinationRotation ? destination.rotation : playerTransform.rotation);

        Debug.Log($"PlateTeleport on {name} moved {playerTransform.name} to {destination.position}");

        if (controller != null)
        {
            controller.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.TransformPoint(detectionOffset), detectionRadius);
    }
}
