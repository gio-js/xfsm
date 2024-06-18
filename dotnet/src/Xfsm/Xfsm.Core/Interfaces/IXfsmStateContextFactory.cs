namespace Xfsm.Core.Interfaces
{
    public interface IXfsmStateContextFactory<T>
    {
        IXfsmStateContext Create(IXfsmElement<T> element);
    }
}
