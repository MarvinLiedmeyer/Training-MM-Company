using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyAPI.Interface
{
    public interface IBaseInterface<T,TD>
    {
        Task<bool> Create(T data);
        Task<List<TD>> Read();
        Task<bool> Update(T data, int id);
        Task<T> ReadId(int id);
        Task<bool> Delete(int id);
    }
}
