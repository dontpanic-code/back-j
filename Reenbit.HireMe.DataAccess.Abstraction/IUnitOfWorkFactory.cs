namespace Reenbit.HireMe.DataAccess.Abstraction
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
