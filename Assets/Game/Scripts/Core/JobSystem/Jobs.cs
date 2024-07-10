using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class Jobs 
{
    public List<UniTask> tasks = new List<UniTask>();

    public void Add(UniTask task)
    {
        tasks.Add(task);
    }
    public async UniTask ExecuteJob()
    {
        await UniTask.WhenAll(tasks);
    }
}
