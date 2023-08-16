using KillChain.Audio;
using UnityEditor;
using UnityEngine;

namespace KillChain.Editor
{
    [CustomEditor(typeof(AudioAsset), true)]
    [CanEditMultipleObjects]
    public class AudioAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            if (GUILayout.Button("Play"))
                ((IAudioAsset)target).PlayEditor();
        }
    }
}
