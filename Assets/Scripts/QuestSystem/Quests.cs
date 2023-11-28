using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Quests : ScriptableObject
{
    [System.Serializable]

    public struct Info {
        public string Name;
        public Sprite Icon;
        public string Description;
    }

    [Header("Info")] public Info Information;

    [System.Serializable]
    public struct Stat {
        public int Currency;
        public int XP;
    }

    [Header("Reward")] public Stat Reward = new Stat{Currency = 10, XP = 10};

    public bool Completed { get; private set; }
    public QuestCompletedEvent QuestCompleted;

    public abstract class QuestGoal : ScriptableObject
    {
        protected string Description;
        public int CurrentAmount { get; protected set; }
        public int RequiredAmount = 1;

        public bool Completed { get; protected set; }
        [HideInInspector] public UnityEvent GoalCompleted;

        public virtual string GetDescription() {
            return Description;
        }

        public virtual void Initialize() {
            Completed = false;
            GoalCompleted = new UnityEvent();
        }

        protected void Evaluate() {
            if (CurrentAmount >= RequiredAmount) {
                Complete();
            }
        }

        private void Complete() {
            Completed = true;
            GoalCompleted.Invoke();
            GoalCompleted.RemoveAllListeners();
        }

        public void Skip() {
            // Charge the player some game currency to skip quest?
            Complete();
        }
    }

    public List<QuestGoal> Goals;

    public void Initialize() {
        Completed = false;
        QuestCompleted = new QuestCompletedEvent();

        foreach (var goal in Goals) {
            goal.Initialize();
            goal.GoalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }

    private void CheckGoals() {
        // Completed = Goals.Add(g => g.Completed);
        if (Completed) {
            // Give Reward
            QuestCompleted.Invoke(this);
            QuestCompleted.RemoveAllListeners();
        }
    }
}

public class QuestCompletedEvent : UnityEvent<Quests> { }

// Shows Information about a Quest
#if Unity_EDITOR
[CustomEditor(typeof(Quests))]
public class QuestEditor : Editor 
{
    SerializedProperty m_QuestInfoProperty;
    SerializedProperty m_QuestStatProperty;

    List<string> m_QuestGoalType;;
    SerializedProperty m_QuestGoalListProperty;

    [MenuItem("Assets/Quest", priority = 0)]
    public static void CreateQuest() {
        var newQuest = CreateInstance<Quests>();

        ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
    }

    void onEnable() {
        m_QuestInfoProperty = serializedObject.FindProperty(nameof(Quests.Information));
        m_QuestStatProperty = serializedObject.FindProperty(nameof(Quests.Reward));

        m_QuestGoalListProperty = serializedObject.FindProperty(nameof(Quests.Goals));

        var lookup = typeof(Quests.QuestGoal);
        m_QuestGoalType = System.AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(assembly => assembly.GetTypes())
        .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
        .Select(type => type.Name)
        .ToList();
    }

    public override void OnInspectorGUI()
    {
        var child = m_QuestInfoProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest info", EditorStyles.boldlabel);
        while (child.depth > depth) {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        child = m_QuestStatProperty.Copy();
        depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest Reward", EditorStyles.boldlabel);
        while (child.depth > depth) {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false;)
        }

        // Creates a drop down menu with all the types of Quest Goals
        int choice = EditorGUILayout.Popup("Add new Quest Goal", -1, m_QuestGoalType.ToArray());

        if (choice != -1) {
            var newInstance = ScriptableObject.CreateInstance(m_QuestGoalType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_QuestGoalListProperty.InsertArrayElementAtIndex(m_QuestGoalListProperty.arraySize);
            m_QuestGoalListProperty.GetArrayElementAtIndex(m_QuestGoalListProperty.arraySize - 1)
            .objectReferenceValue = newInstance;
        }

        // Display the whole list of goals
        Editor ed = null;
        int toDelete = -1;
        for (int i = 0; i < m_QuestGoalListProperty.arraySize; i++) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_QuestGoalListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
        }
    }
}
#endif