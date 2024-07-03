namespace Game.Scripts.Core.Interfaces
{
    public interface IContext
    {
        T Resolve<T>();
    }
}
