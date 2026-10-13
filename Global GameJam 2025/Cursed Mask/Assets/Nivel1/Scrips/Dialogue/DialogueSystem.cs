using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSystem", menuName = "DialogueSystem")]
public class DialogueSystem : ScriptableObject
{
    public string npcName;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 1f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
}
