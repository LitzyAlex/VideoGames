using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject dialogPanel;

    void Start()
    {
        interactIcon.SetActive(false);
        dialogPanel.SetActive(false);
    }

    public void ShowIcon(bool show)
    {
        interactIcon.SetActive(show);
    }

    public void Interact()
    {
        dialogPanel.SetActive(true);
        Debug.Log("Diálogo abierto");
    }
}
