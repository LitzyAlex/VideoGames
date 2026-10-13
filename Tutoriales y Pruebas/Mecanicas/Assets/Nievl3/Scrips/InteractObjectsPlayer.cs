using UnityEngine;

public class InteractObjectsPlayer : MonoBehaviour
{
    [SerializeField] private Transform controladorInteract;
    [SerializeField] private Vector2 tamaño;

    private IInteractable currentInteractable; //Guarda el objeto que se puede interactuar cercano

    void Update()
    {
        DetectInteractable();

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void DetectInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(controladorInteract.position,tamaño,0);  //Caja invisible que detecta collisions

        IInteractable nearest = null; //Guardarlo

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out IInteractable interactable)) //Revisa si el componente es interactuable y agarra el primero que hay
            {
                nearest = interactable;
                break;
            }
        }

        if (nearest != currentInteractable)
        {
            if (currentInteractable != null)
                currentInteractable.ShowIcon(false);

            currentInteractable = nearest; //actualiza

            if (currentInteractable != null)
                currentInteractable.ShowIcon(true);
        }
    }

    void OnDrawGizmos()
    {
        if (controladorInteract == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(controladorInteract.position, tamaño);
    }
}
