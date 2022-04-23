using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CanEditMultipleObjects]
[CustomEditor(typeof(SimpleSpriteAnimator))]
public class SimpleSpriteAnimatorEditor : Editor
{
    SimpleSpriteAnimator myTarget;

    Sprite savedSprite;

    public override void OnInspectorGUI()
    {
        myTarget = (SimpleSpriteAnimator)target;

        if(myTarget.renderer == null && myTarget.image == null)
        {
            myTarget.Init();
            GUILayout.Label("Renderer is missing!");
            return;
        }

        //Properties
        DrawPropertyArray("presets");
        DrawPropertyArray("_frames");
        DrawPropertyArray("settings");
        myTarget.timeMultiplier = EditorGUILayout.FloatField("Time Multiplier", myTarget.timeMultiplier);

        //base.OnInspectorGUI();

        //Control Buttons
        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        if (myTarget.isPlaying)
        {
            if (GUILayout.Button(EditorGUIUtility.IconContent("PauseButton") ))
            {
                myTarget.Pause();
            }
        }
        else
        {
            if (GUILayout.Button(EditorGUIUtility.IconContent("PlayButton")))
            {
                myTarget.Play();
            }
        }

        if (GUILayout.Button(EditorGUIUtility.IconContent("Animation.PrevKey")))
        {
            myTarget.Stop();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(8);

        myTarget.timer = EditorGUILayout.FloatField("Animation Time", myTarget.timer);

        //Draw preview
        if (myTarget.settings.mode == SimpleSpriteAnimator.Mode.SingleRow)
        {
            DrawSingleRawOptions();
        }
        else if (myTarget.settings.mode == SimpleSpriteAnimator.Mode.Grid)
        {
            DrawGridOptions();
        }
        GUILayout.Space(8);



        //Update
        SetEditorDeltaTime();
        if (!Application.isPlaying && myTarget.isPlaying)
        {
            myTarget.Tick(editorDeltaTime);
        }
        

        if (GUI.changed)
        {
            EditorUtility.SetDirty(myTarget);
        }
            
    }

    //EDITOR Delta Time
    float editorDeltaTime = 0f;
    double lastTimeSinceStartup = 0f;

    void SetEditorDeltaTime()
    {
        if (lastTimeSinceStartup == 0f)
        {
            lastTimeSinceStartup = EditorApplication.timeSinceStartup;
        }
        editorDeltaTime = (float)(EditorApplication.timeSinceStartup - lastTimeSinceStartup);
        lastTimeSinceStartup = EditorApplication.timeSinceStartup;
    }


    void DrawSingleRawOptions()
    {
        

        //
        if (myTarget.frames == null) return;

        GUILayout.Space(16);
        myTarget.x = EditorGUILayout.IntField("X", myTarget.x);

        GUILayout.Space(32);

        if (Event.current.type != EventType.Layout && Event.current.type != EventType.Repaint) return;

        //SINGLE PREVIEW
        Rect rect = EditorGUILayout.BeginHorizontal();
        
        float scale = Mathf.Clamp(Mathf.Floor((Screen.width - 20) / spriteSize.x) / (myTarget.frames.Length), 0f, 1f); //Calculate sprite scale
        for (int x = 0; x < myTarget.frames.Length; x++)
        {
            DrawSprite(myTarget.frames[x], x == myTarget.curIndx, scale);
        }
        
        EditorGUILayout.EndHorizontal();

        
    }



    void DrawGridOptions()
    {
        GUILayout.Label("[GRID]");

        GUILayout.Space(16);

        myTarget.x = EditorGUILayout.IntField("X", myTarget.x);
        myTarget.y = EditorGUILayout.IntField("Y", myTarget.y);

        GUILayout.Space(32);

        //
        if (myTarget.frames == null) return;

        //GRID PREVIEW
        EditorGUILayout.BeginVertical();
        
        float scale = Mathf.Clamp(Mathf.Floor(Screen.width / spriteSize.x) / (myTarget.settings.rowLength), 0f, 1f); //Calculate sprite scale

        float yCount = (myTarget.frames!=null) ? (myTarget.frames.Length * 1f / myTarget.settings.rowLength) : 0;
        
        for (int y = 0; y < yCount; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < myTarget.settings.rowLength; x++)
            {
                int id = x + myTarget.settings.rowLength * y;
                //int curID = myTarget.curIndx + myTarget.gridRowLength * myTarget.gridY;
                Sprite s = (id >= myTarget.frames.Length) ? null : myTarget.frames[id];
                DrawSprite(s, id == myTarget.curIndx, scale);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }


    Vector2 spriteSize = new Vector2(32,32);

    void DrawSprite(Sprite s, bool highlight = false, float scale = 1f)
    {
        if (Event.current.type != EventType.Layout && Event.current.type != EventType.Repaint) return;

        if (!highlight)
            GUI.color = Color.gray;

        Rect rect = GUILayoutUtility.GetRect(spriteSize.x, spriteSize.y, GUILayout.Width(spriteSize.x * scale), GUILayout.Height(spriteSize.y * scale));
        GUI.Box(rect, "");

        if (s){
            GameUtility.GUIDrawSprite(rect, s);     
        }

        GUI.color = Color.white;
    }



    void DrawPropertyArray(string propertyName)
    {
        SerializedProperty sp = serializedObject.FindProperty(propertyName);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp, true);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }




    


}

