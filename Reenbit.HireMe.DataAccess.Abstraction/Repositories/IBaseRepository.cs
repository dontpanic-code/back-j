using Microsoft.EntityFrameworkCore;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IBaseRepository
    {
        void SetContext(DbContext context);
    }
}
