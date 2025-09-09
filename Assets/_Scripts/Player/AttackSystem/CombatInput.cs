using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CombatAction { Attack1 = 0, Attack2 = 1, Attack3 = 2 }
public enum InputPhase { Pressed, Released }

[Serializable]
public struct BufferedInputEvent
{
    public CombatAction action;
    public InputPhase phase;
    public float time;
    public bool alt;
}

public class CombatInput : MonoBehaviour
{
    [Header("Input Actions (Optional if using PlayerInput callbacks)")]
    public InputActionReference attack1;
    public InputActionReference attack2;
    public InputActionReference attack3;
    public InputActionReference alt;   

    [Header("Buffer Settings")]
    [Range(0.05f, 1.0f)] public float bufferWindow = 0.45f;
    [Range(1, 32)] public int maxBuffer = 8;

    private readonly List<BufferedInputEvent> _buffer = new();

    // When not using an Alt action, you can set this manually.
    [NonSerialized] public bool manualAlt;

    public bool AltPressed =>
        (alt != null && alt.action != null) ? alt.action.IsPressed() : manualAlt;

    void OnEnable()
    {
        Hook(attack1, OnA1Started, OnA1Canceled);
        Hook(attack2, OnA2Started, OnA2Canceled);
        Hook(attack3, OnA3Started, OnA3Canceled);
    }

    void OnDisable()
    {
        Unhook(attack1, OnA1Started, OnA1Canceled);
        Unhook(attack2, OnA2Started, OnA2Canceled);
        Unhook(attack3, OnA3Started, OnA3Canceled);
    }

    private void Hook(InputActionReference a,
        Action<InputAction.CallbackContext> started,
        Action<InputAction.CallbackContext> canceled)
    {
        if (a?.action == null) return;
        a.action.started += started;
        a.action.canceled += canceled;
        if (!a.action.enabled) a.action.Enable();
    }

    private void Unhook(InputActionReference a,
        Action<InputAction.CallbackContext> started,
        Action<InputAction.CallbackContext> canceled)
    {
        if (a?.action == null) return;
        a.action.started -= started;
        a.action.canceled -= canceled;
    }

    
    public void Push(CombatAction act, InputPhase phase)
    {
        if (_buffer.Count >= maxBuffer) _buffer.RemoveAt(0);
        _buffer.Add(new BufferedInputEvent
        {
            action = act,
            phase = phase,
            time = Time.time,
            alt = AltPressed
        });
    }

    // ——— Query/maintenance ———
    public bool TryPeekOldest(out BufferedInputEvent e)
    {
        CleanupExpired();
        if (_buffer.Count > 0) { e = _buffer[0]; return true; }
        e = default;
        return false;
    }

    public void ConsumeOldest()
    {
        if (_buffer.Count > 0) _buffer.RemoveAt(0);
    }

    public void CleanupExpired()
    {
        float cutoff = Time.time - bufferWindow;
        for (int i = _buffer.Count - 1; i >= 0; i--)
            if (_buffer[i].time < cutoff)
                _buffer.RemoveAt(i);
    }

    // ——— Default handlers (only if using InputActionReferences) ———
    private void OnA1Started(InputAction.CallbackContext _) => Push(CombatAction.Attack1, InputPhase.Pressed);
    private void OnA1Canceled(InputAction.CallbackContext _) => Push(CombatAction.Attack1, InputPhase.Released);
    private void OnA2Started(InputAction.CallbackContext _) => Push(CombatAction.Attack2, InputPhase.Pressed);
    private void OnA2Canceled(InputAction.CallbackContext _) => Push(CombatAction.Attack2, InputPhase.Released);
    private void OnA3Started(InputAction.CallbackContext _) => Push(CombatAction.Attack3, InputPhase.Pressed);
    private void OnA3Canceled(InputAction.CallbackContext _) => Push(CombatAction.Attack3, InputPhase.Released);
}
