using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    [System.Serializable]
    public class Task
    {
        public string title;
        public string description;
    }

    [Header("Task List")]
    public List<Task> tasks = new List<Task>();

    [Header("UI Components")]
    public TextMeshProUGUI titleText;        // 用于显示任务标题
    public TextMeshProUGUI descriptionText;  // 用于显示任务描述

    private int currentTaskIndex = 0;

    void Start()
    {
        if (tasks.Count > 0)
            ShowTask(currentTaskIndex);
        else
            Debug.LogWarning("Task list is empty!");
    }

    void OnGUI()
    {
        // 用 OnGUI 捕获空格键，确保 Editor 里也能触发
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            NextTask();
        }
    }

    void ShowTask(int index)
    {
        if (index >= 0 && index < tasks.Count)
        {
            if (titleText != null)
                titleText.text = tasks[index].title;

            if (descriptionText != null)
                descriptionText.text = tasks[index].description;
        }
    }

    void NextTask()
    {
        if (currentTaskIndex < tasks.Count - 1)
        {
            currentTaskIndex++;
            ShowTask(currentTaskIndex);
        }
        else
        {
            Debug.Log("All tasks completed!");
        }
    }
}
