using UnityEngine;

public class InteractObjectsPlayer : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField] private Transform controladorInteract;
    [SerializeField] private Vector2 tamaño = new Vector2(1f, 1f);

    private IInteractable currentInteractable;

    void Awake()
    {
        if (controladorInteract == null)
        {
            controladorInteract = transform.Find("ControladorInteract");

            if (controladorInteract == null)
            {
                Debug.LogError("No se encontró ControladorInteract en " + gameObject.name);
            }
        }
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        DetectInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void DetectInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            controladorInteract.position,
            tamaño,
            0
        );

        IInteractable nearest = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IInteractable interactable))
            {
                nearest = interactable;
                break;
            }
        }

        if (nearest != currentInteractable)
        {
            if (currentInteractable != null)
                currentInteractable.ShowIcon(false);

            currentInteractable = nearest;

            if (currentInteractable != null)
                currentInteractable.ShowIcon(true);
        }
    }

    void OnDisable()
    {
        // Apaga icono si cambias de forma
        if (currentInteractable != null)
            currentInteractable.ShowIcon(false);

        currentInteractable = null;
    }

    void OnDrawGizmos()
    {
        if (controladorInteract == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(controladorInteract.position, tamaño);
    }
}


