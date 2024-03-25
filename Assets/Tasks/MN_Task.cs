using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MN_Task : CL_Manager
{
    private List<CL_Task>[] tasks = new List<CL_Task>[5]; //one for each type

    private void Awake()
    {
        tasks[0] = GetComponents<CL_Task>().ToList();
    }

    private void Start()
    {
        TidyTasks();
    }


    /// <summary>
    /// put all task inside of task[0] list in to other list that correspong to their type
    /// </summary>
    public void TidyTasks()
    {
        //once all task has been referenced , set them all inside there atributed list
        foreach(CL_Task _task in tasks[0])
        {
            switch(_task.taskType)
            {
                case CL_Task.Type.military:
                    tasks[1].Add(_task);
                    tasks[0].Remove(_task);
                    break;

                case CL_Task.Type.engeniering:
                    tasks[2].Add(_task);
                    tasks[0].Remove(_task);
                    break;

                case CL_Task.Type.doctor:
                    tasks[3].Add(_task);
                    tasks[0].Remove(_task);
                    break;

                case CL_Task.Type.craft:
                    tasks[4].Add(_task);
                    tasks[0].Remove(_task);
                    break;

                case CL_Task.Type.tidy:
                    tasks[5].Add(_task);
                    tasks[0].Remove(_task);
                    break;
            }
        }

        SortClassTasks();
    }


    /// <summary>
    /// sort all lists of classes
    /// </summary>
    public void SortClassTasks()
    {
        SortByPriority(tasks[1]);
        SortByPriority(tasks[2]);
        SortByPriority(tasks[3]);
        SortByPriority(tasks[4]);
        SortByPriority(tasks[5]);
    }


    /// <summary>
    /// sort given list of task by taksks innate priority level
    /// </summary>
    /// <param name="_listOfTask"></param>
    /// <returns></returns>
    private List<CL_Task> SortByPriority(List<CL_Task> _listOfTask)
    {
        for (int i = 0; i < _listOfTask.Count - 1; i++)
        {
            for (int j = 0; j < _listOfTask.Count - i - 1; j++)
            {
                if (_listOfTask[j].innatePriority > _listOfTask[j + 1].innatePriority)
                {
                    // Échange les éléments
                    CL_Task temp = _listOfTask[j];
                    _listOfTask[j] = _listOfTask[j + 1];
                    _listOfTask[j + 1] = temp;
                }
            }
        }

        return (_listOfTask);
    }
}
