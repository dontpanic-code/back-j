using System.Collections.Generic;

namespace Reenbit.HireMe.Infrastructure
{
    public interface IConfigurationManager
    {
        string DatabaseConnectionString { get; }

        List<string> Admins { get; }
    }
}
