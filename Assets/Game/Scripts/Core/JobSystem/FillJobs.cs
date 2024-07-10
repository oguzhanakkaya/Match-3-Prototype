using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillJobs 
{
    public List<int> tasks = new List<int> ();

    public void Add(int task)
    {
        tasks.Add(task);
    }
    public async UniTask ExecuteJob()
    {
      //  await UniTask.WhenAll(tasks);
    }
}
