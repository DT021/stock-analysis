using System.Collections.Generic;
using System.Threading.Tasks;

namespace core.Account
{
    public interface IAccountStorage
    {
         Task RecordLoginAsync(LoginLogEntry entry);

         Task<IEnumerable<LoginLogEntry>> GetLogins();
    }
}