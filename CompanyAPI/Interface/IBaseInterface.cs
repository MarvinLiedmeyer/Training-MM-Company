using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompanyAPI.Interface
{
    public interface IBaseInterface<T,D>
    {
        Task<bool> Create(T data);
        Task<List<D>> Read();
        Task<bool> Update(T data, int id);
        Task<T> ReadId(int id);
        Task<bool> Delete(int id);
    }
}
