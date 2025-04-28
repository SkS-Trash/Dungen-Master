using System;
using Services.Progress;
using Services.SaveLoadData;
using Subscribers.EventBusSystem;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SaveLoadWindow : EditorWindow
{
    private static IProgressService ProgressService => Application.isPlaying
        ? FindAnyObjectByType<LifetimeScope>().Container.Resolve<IProgressService>()
        : new ProgressInLocalStorageService(new SaveLoadLocalDataService());

    private readonly Color headerColor = new Color(0.15f, 0.15f, 0.15f);
    private readonly Color btnSaveColor = new Color(0.2f, 0.6f, 0.2f);
    private readonly Color btnLoadColor = new Color(0.2f, 0.2f, 0.6f);
    private readonly Color btnResetColor = new Color(0.6f, 0.2f, 0.2f);

    private enum Tab
    {
        Save,
        Load,
        Reset
    }

    private Tab _currentTab;

    [MenuItem("Tools/Save|Load Progress")]
    public static void ShowWindow()
    {
        var window = GetWindow<SaveLoadWindow>();
        window.titleContent = new GUIContent("🔄 Progress");
        window.minSize = new Vector2(300, 200);
    }

    private void OnGUI()
    {
        DrawHeader();
        DrawTabs();

        GUILayout.Space(8);

        switch (_currentTab)
        {
            case Tab.Save: DrawSaveSection(); break;
            case Tab.Load: DrawLoadSection(); break;
            case Tab.Reset: DrawResetSection(); break;
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("✖ Закрыть", GUILayout.Height(24)))
            Close();
    }

    private void DrawHeader()
    {
        var rect = EditorGUILayout.GetControlRect(false, 30);
        EditorGUI.DrawRect(rect, headerColor);
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white }
        };
        EditorGUI.LabelField(rect, "Save & Load Progress", style);
    }

    private void DrawTabs()
    {
        GUILayout.BeginHorizontal();
        DrawTabButton("💾 Save", Tab.Save);
        DrawTabButton("📂 Load", Tab.Load);
        DrawTabButton("🗑️ Reset", Tab.Reset);
        GUILayout.EndHorizontal();
    }

    private void DrawTabButton(string label, Tab tab)
    {
        var isActive = _currentTab == tab;
        var bg = GUI.backgroundColor;
        GUI.backgroundColor = isActive ? Color.gray : Color.black;
        if (GUILayout.Button(label, GUILayout.Height(22)))
            _currentTab = tab;
        GUI.backgroundColor = bg;
    }

    private void DrawSaveSection()
    {
        DrawBox(() =>
        {
            DrawColoredButton("Сохранить глобальный", btnSaveColor, SaveGlobalProgress, "SaveAs Icon");
            DrawColoredButton("Сохранить уровень", btnSaveColor, SaveLevelProgress, "SaveIcon");
            DrawColoredButton("Сохранить всё", btnSaveColor, SaveAllProgress, "d_SaveAs");
        });
    }

    private void DrawLoadSection()
    {
        DrawBox(() =>
        {
            DrawColoredButton("Загрузить прогресс", btnLoadColor, LoadProgress, "d_UnityEditor.AnimationWindow");
            DrawColoredButton("Оповестить об изменении", btnLoadColor, NotifyProgressChanged,
                "d_UnityEditor.SceneView");
        });
    }

    private void DrawResetSection()
    {
        DrawBox(() => { DrawColoredButton("Сбросить прогресс", btnResetColor, ResetProgress, "d_Animation.Warning"); });
    }

    private void DrawBox(Action content)
    {
        var boxStyle = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(8, 8, 8, 8),
            margin = new RectOffset(4, 4, 4, 4)
        };
        EditorGUILayout.BeginVertical(boxStyle);
        content.Invoke();
        EditorGUILayout.EndVertical();
    }

    private void DrawColoredButton(string label, Color col, Action action, string iconName = null)
    {
        var prevColor = GUI.backgroundColor;
        GUI.backgroundColor = col;

        Texture icon = iconName != null ? EditorGUIUtility.IconContent(iconName).image : null;
        GUIContent content = icon != null ? new GUIContent(label, icon) : new GUIContent(label);
        if (GUILayout.Button(content, GUILayout.Height(26)))
            action.Invoke();
        GUI.backgroundColor = prevColor;
    }

    private void SaveGlobalProgress()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Нужно быть в Play Mode.");
            return;
        }

        ProgressService.SaveGlobal();
        Debug.Log("Глобальный прогресс сохранён.");
    }

    private void SaveLevelProgress()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Нужно быть в Play Mode.");
            return;
        }

        ProgressService.SaveLevel();
        Debug.Log("Прогресс уровня сохранён.");
    }

    private void SaveAllProgress()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Нужно быть в Play Mode.");
            return;
        }

        ProgressService.SaveGlobal();
        ProgressService.SaveLevel();
        Debug.Log("Весь прогресс сохранён.");
    }

    private void LoadProgress()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Нужно быть в Play Mode.");
            return;
        }

        ProgressService.LoadProgress();
        Debug.Log("Прогресс загружен.");
    }

    private void NotifyProgressChanged()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Нужно быть в Play Mode.");
            return;
        }

        EventBus.RaiseEvent<ILevelProgressLoadSubscriber>(x => x.OnProgressLoaded(ProgressService.LevelProgress));
        EventBus.RaiseEvent<IGlobalProgressLoadSubscriber>(x => x.OnProgressLoaded(ProgressService.GlobalProgress));
        Debug.Log("Подписчики оповещены о прогрессе.");
    }

    private void ResetProgress()
    {
        ProgressService.ResetProgress();
        Debug.Log("Прогресс сброшен.");
    }
}