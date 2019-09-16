using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp.Model;

namespace CompanyAPI.Interface
{
    public interface IBaseInterface<T, D>
    {
        bool Create(T data);
        List<D> Read();
        bool Update(T data, int id);
        T ReadId(int id);
        bool Delete(int id);
    }
}
