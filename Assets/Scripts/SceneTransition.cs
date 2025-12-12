using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private int sceneBuildIndex = -1;
    [SerializeField] private Vector3 detectionOffset = Vector3.zero;
    [SerializeField] private float detectionRadius = 0.4f;
    [SerializeField] private LayerMask playerMask = ~0;
    [SerializeField] private string requiredTag = "Player";
    [SerializeField] private float retriggerDelay = 0.5f;

    private float nextAvailableTime;
    private bool loading;

    private void Awake()
    {
        Collider plateCollider = GetComponent<Collider>();
        if (plateCollider != null && !plateCollider.isTrigger)
        {
            plateCollider.isTrigger = true;
        }
    }

    private void FixedUpdate()
    {
        if (loading || Time.time < nextAvailableTime || !HasTargetScene())
        {
            return;
        }

        Vector3 checkCenter = transform.TransformPoint(detectionOffset);
        Collider[] hits = Physics.OverlapSphere(checkCenter, detectionRadius, playerMask, QueryTriggerInteraction.Collide);
        for (int i = 0; i < hits.Length; i++)
        {
            CharacterMovement movement = hits[i].GetComponentInParent<CharacterMovement>();
            if (movement == null && !IsPlayerCollider(hits[i]))
            {
                continue;
            }

            BeginTransition();
            nextAvailableTime = Time.time + retriggerDelay;
            break;
        }
    }

    private bool IsPlayerCollider(Collider candidate)
    {
        if (candidate == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(requiredTag))
        {
            return true;
        }

        Transform root = candidate.transform.root;
        if (root != null && root.CompareTag(requiredTag))
        {
            return true;
        }

        return candidate.CompareTag(requiredTag);
    }

    private void BeginTransition()
    {
        loading = true;
        if (sceneBuildIndex >= 0)
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }

    private bool HasTargetScene()
    {
        return sceneBuildIndex >= 0 || !string.IsNullOrEmpty(sceneToLoad);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.TransformPoint(detectionOffset), detectionRadius);
    }
}
