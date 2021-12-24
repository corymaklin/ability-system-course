using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatSystem.Editor
{
    public class StatDatabaseEditorWindow : EditorWindow
    {
        private StatDatabase m_Database;
        private StatCollectionEditor m_Current;
        
        [MenuItem("Window/StatSystem/StatDatabase")]
        public static void ShowWindow()
        {
            StatDatabaseEditorWindow window = GetWindow<StatDatabaseEditorWindow>();
            window.minSize = new Vector2(800, 600);
            window.titleContent = new GUIContent("StatDatabase");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is StatDatabase)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        private void OnSelectionChange()
        {
            m_Database = Selection.activeObject as StatDatabase;
        }

        public void CreateGUI()
        {
            OnSelectionChange();
            
            VisualElement root = rootVisualElement;
            
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/StatSystem/Scripts/Editor/StatDatabaseEditorWindow.uxml");
            visualTree.CloneTree(root);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StatSystem/Scripts/Editor/StatDatabaseEditorWindow.uss");
            root.styleSheets.Add(styleSheet);

            StatCollectionEditor stats = root.Q<StatCollectionEditor>("stats");
            stats.Initialize(m_Database, m_Database.stats);
            Button statsTab = root.Q<Button>("stats-tab");
            statsTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                stats.style.display = DisplayStyle.Flex;
                m_Current = stats;
            };

            StatCollectionEditor primaryStats = root.Q<StatCollectionEditor>("primary-stats");
            primaryStats.Initialize(m_Database, m_Database.primaryStats);
            Button primaryStatsTab = root.Q<Button>("primary-stats-tab");
            primaryStatsTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                primaryStats.style.display = DisplayStyle.Flex;
                m_Current = primaryStats;
            };

            StatCollectionEditor attributes = root.Q<StatCollectionEditor>("attributes");
            attributes.Initialize(m_Database, m_Database.attributes);
            Button attributesTab = root.Q<Button>("attributes-tab");
            attributesTab.clicked += () =>
            {
                m_Current.style.display = DisplayStyle.None;
                attributes.style.display = DisplayStyle.Flex;
                m_Current = attributes;
            };

            m_Current = stats;
        }
    }
}