using System.Collections;
using MagmaLabs.Animation;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[AddComponentMenu("UI/Navigation Button")]
public class NavigationButton : Button
{
    public enum TransitionMode
    {
        Fade = 0,
        LoadingScreen = 1,
    }

    [Header("Canvases")]
    [Tooltip("If left empty and Auto Find From Canvas is enabled, this will be auto-populated from the button's parent Canvas.")]
    [SerializeField] private Canvas fromCanvas;
    [SerializeField] private Canvas toCanvas;
    [Tooltip("Optional. Used only when Transition Mode is LoadingScreen.")]
    [SerializeField] private Canvas loadingCanvas;

    [Header("Transition")]
    [SerializeField] private TransitionMode transitionMode = TransitionMode.Fade;
    [Min(0f)]
    [SerializeField] private float fadeDurationSeconds = 0.25f;
    [Min(0f)]
    [SerializeField] private float loadingHoldSeconds = 0.35f;
    [SerializeField] private bool useUnscaledTime = true;

    [Header("Behavior")]
    [SerializeField] private bool autoFindFromCanvas = true;
    [SerializeField] private bool disableUiInteractionDuringTransition = true;

    private bool isTransitioning;

    protected override void Awake()
    {
        base.Awake();
        AutoAssignFromCanvasIfNeeded();
        EnsureCanvasGroups();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        onClick.AddListener(HandleClick);
    }

    protected override void OnDisable()
    {
        onClick.RemoveListener(HandleClick);
        base.OnDisable();
    }

    protected void OnValidate()
    {
        AutoAssignFromCanvasIfNeeded();
        EnsureCanvasGroups();
    }

    private void Reset()
    {
        autoFindFromCanvas = true;
        transitionMode = TransitionMode.Fade;
        fadeDurationSeconds = 0.25f;
        loadingHoldSeconds = 0.35f;
        useUnscaledTime = true;
        disableUiInteractionDuringTransition = true;

        AutoAssignFromCanvasIfNeeded();
        EnsureCanvasGroups();
    }

    public void Navigate()
    {
        if (isTransitioning)
        {
            return;
        }

        if (toCanvas == null)
        {
            Debug.LogWarning($"{nameof(NavigationButton)} on '{name}' has no To Canvas assigned.", this);
            return;
        }

        if (fromCanvas == null)
        {
            Debug.LogWarning($"{nameof(NavigationButton)} on '{name}' has no From Canvas assigned.", this);
            return;
        }

        StartCoroutine(TransitionCoroutine());
    }

    private void HandleClick()
    {
        Navigate();
    }

    private IEnumerator TransitionCoroutine()
    {
        isTransitioning = true;

        switch (transitionMode)
        {
            case TransitionMode.Fade:
                yield return StartCoroutine(CrossFadeCoroutine(fromCanvas, toCanvas, fadeDurationSeconds, enableToInteractionAtEnd: true));
                break;

            case TransitionMode.LoadingScreen:
                if (loadingCanvas == null)
                {
                    Debug.LogWarning($"{nameof(NavigationButton)} on '{name}' is set to LoadingScreen but has no Loading Canvas assigned. Falling back to Fade.", this);
                    yield return StartCoroutine(CrossFadeCoroutine(fromCanvas, toCanvas, fadeDurationSeconds, enableToInteractionAtEnd: true));
                    break;
                }

                yield return StartCoroutine(CrossFadeCoroutine(fromCanvas, loadingCanvas, fadeDurationSeconds, enableToInteractionAtEnd: false, blockRaycastsAtEnd: true));
                yield return WaitSeconds(loadingHoldSeconds);
                yield return StartCoroutine(CrossFadeCoroutine(loadingCanvas, toCanvas, fadeDurationSeconds, enableToInteractionAtEnd: true));
                break;
        }

        isTransitioning = false;
    }

    private IEnumerator CrossFadeCoroutine(
        Canvas from,
        Canvas to,
        float durationSeconds,
        bool enableToInteractionAtEnd,
        bool blockRaycastsAtEnd = true)
    {
        if (from == null || to == null)
        {
            yield break;
        }

        if (ReferenceEquals(from, to))
        {
            // No-op.
            yield break;
        }

        CanvasGroup fromGroup = GetOrAddCanvasGroup(from);
        CanvasGroup toGroup = GetOrAddCanvasGroup(to);

        from.enabled = true;
        to.enabled = true;

        // Force deterministic start state.
        fromGroup.alpha = 1f;
        toGroup.alpha = 0f;

        if (disableUiInteractionDuringTransition)
        {
            SetInteraction(fromGroup, interactable: false, blocksRaycasts: true);
            SetInteraction(toGroup, interactable: false, blocksRaycasts: true);
        }

        if (durationSeconds <= 0f)
        {
            fromGroup.alpha = 0f;
            toGroup.alpha = 1f;
            from.enabled = false;
            ApplyEndInteraction(toGroup, enableToInteractionAtEnd, blockRaycastsAtEnd);
            yield break;
        }

        float t = 0f;
        while (t < durationSeconds)
        {
            t += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            float progress = Mathf.Clamp01(t / durationSeconds);
            float eased = Easing.EaseInOutCubic(progress);

            fromGroup.alpha = Mathf.Lerp(1f, 0f, eased);
            toGroup.alpha = Mathf.Lerp(0f, 1f, eased);
            yield return null;
        }

        fromGroup.alpha = 0f;
        toGroup.alpha = 1f;
        from.enabled = false;
        ApplyEndInteraction(toGroup, enableToInteractionAtEnd, blockRaycastsAtEnd);
    }

    private IEnumerator WaitSeconds(float seconds)
    {
        if (seconds <= 0f)
        {
            yield break;
        }

        if (!useUnscaledTime)
        {
            yield return new WaitForSeconds(seconds);
            yield break;
        }

        float end = Time.unscaledTime + seconds;
        while (Time.unscaledTime < end)
        {
            yield return null;
        }
    }

    private void AutoAssignFromCanvasIfNeeded()
    {
        if (!autoFindFromCanvas)
        {
            return;
        }

        if (fromCanvas != null)
        {
            return;
        }

        fromCanvas = GetComponentInParent<Canvas>();
    }

    private void EnsureCanvasGroups()
    {
        if (fromCanvas != null)
        {
            _ = GetOrAddCanvasGroup(fromCanvas);
        }

        if (toCanvas != null)
        {
            _ = GetOrAddCanvasGroup(toCanvas);
        }

        if (loadingCanvas != null)
        {
            _ = GetOrAddCanvasGroup(loadingCanvas);
        }
    }

    private static CanvasGroup GetOrAddCanvasGroup(Canvas canvas)
    {
        CanvasGroup group = canvas.GetComponent<CanvasGroup>();
        if (group != null)
        {
            return group;
        }

        return canvas.gameObject.AddComponent<CanvasGroup>();
    }

    private static void SetInteraction(CanvasGroup group, bool interactable, bool blocksRaycasts)
    {
        if (group == null)
        {
            return;
        }

        group.interactable = interactable;
        group.blocksRaycasts = blocksRaycasts;
    }

    private static void ApplyEndInteraction(CanvasGroup group, bool enableInteraction, bool blocksRaycasts)
    {
        if (group == null)
        {
            return;
        }

        group.interactable = enableInteraction;
        group.blocksRaycasts = blocksRaycasts;
    }
}
