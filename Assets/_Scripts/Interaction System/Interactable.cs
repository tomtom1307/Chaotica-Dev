using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    [Header("Outline")]
    public bool DoOutline = true;

    [Header("Gaze")]
    [Tooltip("Seconds of continuous gaze required before OnGazeHold() fires.")]
    public float gazeThreshold = 0.5f;

    [HideInInspector] public GameObject interactor;
    public UnityEvent onInteraction = new UnityEvent();

    protected Outline outline;
    public string message;
    // Gaze state
    bool seenThisFrame;
    bool isFocused;
    bool fired;
    float gazeTime;

    public virtual void Start()
    {
        outline = GetComponent<Outline>();
        if (outline) outline.enabled = false;
    }

    void Update()
    {
        if (seenThisFrame)
        {
            if (!isFocused)
            {
                isFocused = true;
                gazeTime = 0f;
                fired = false;
                if (DoOutline && outline) outline.enabled = true;
                OnGazeStart();
            }

            gazeTime += Time.deltaTime;

            if (!fired && gazeTime >= gazeThreshold)
            {
                fired = true;
                OnGazeHold();
            }
        }
        else if (isFocused)
        {
            // Lost focus this frame
            if (DoOutline && outline) outline.enabled = false;
            OnGazeStop();
            isFocused = false;
            fired = false;
            gazeTime = 0f;
        }
    }

    void LateUpdate()
    {
        // Heartbeat reset – if LookedAt() isn’t called next frame, we auto-stop
        seenThisFrame = false;
    }

    public virtual void Interact(GameObject player)
    {
        interactor = player;
        onInteraction.Invoke();
    }

    // Call this every frame the player is looking at the object
    public virtual void LookedAt()
    {
        seenThisFrame = true; // that’s it; timing happens in Update()
    }

    // Optional explicit stop (e.g., when your ray hits something else)
    public virtual void StoppedLookingAt()
    {
        seenThisFrame = false; // Update() will pick this up
    }

    // Hooks for subclasses
    protected virtual void OnGazeStart() { }
    protected virtual void OnGazeHold() { }   // fires once per focus session
    protected virtual void OnGazeStop() { }
}
