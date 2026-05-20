using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(NavigationButton))]
[CanEditMultipleObjects]
public class NavigationButtonEditor : ButtonEditor
{
    private SerializedProperty fromCanvasProp;
    private SerializedProperty toCanvasProp;
    private SerializedProperty loadingCanvasProp;

    private SerializedProperty transitionModeProp;
    private SerializedProperty fadeDurationSecondsProp;
    private SerializedProperty loadingHoldSecondsProp;
    private SerializedProperty useUnscaledTimeProp;

    private SerializedProperty autoFindFromCanvasProp;
    private SerializedProperty disableUiInteractionDuringTransitionProp;

    protected override void OnEnable()
    {
        base.OnEnable();

        fromCanvasProp = serializedObject.FindProperty("fromCanvas");
        toCanvasProp = serializedObject.FindProperty("toCanvas");
        loadingCanvasProp = serializedObject.FindProperty("loadingCanvas");

        transitionModeProp = serializedObject.FindProperty("transitionMode");
        fadeDurationSecondsProp = serializedObject.FindProperty("fadeDurationSeconds");
        loadingHoldSecondsProp = serializedObject.FindProperty("loadingHoldSeconds");
        useUnscaledTimeProp = serializedObject.FindProperty("useUnscaledTime");

        autoFindFromCanvasProp = serializedObject.FindProperty("autoFindFromCanvas");
        disableUiInteractionDuringTransitionProp = serializedObject.FindProperty("disableUiInteractionDuringTransition");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Navigation Button", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(transitionModeProp);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(autoFindFromCanvasProp);
        EditorGUILayout.PropertyField(fromCanvasProp);
        EditorGUILayout.PropertyField(toCanvasProp);

        bool showLoading = transitionModeProp.hasMultipleDifferentValues ||
                           transitionModeProp.enumValueIndex == (int)NavigationButton.TransitionMode.LoadingScreen;
        if (showLoading)
        {
            EditorGUILayout.PropertyField(loadingCanvasProp);
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(fadeDurationSecondsProp);
        if (showLoading)
        {
            EditorGUILayout.PropertyField(loadingHoldSecondsProp);
        }
        EditorGUILayout.PropertyField(useUnscaledTimeProp);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(disableUiInteractionDuringTransitionProp);

        serializedObject.ApplyModifiedProperties();
    }
}

