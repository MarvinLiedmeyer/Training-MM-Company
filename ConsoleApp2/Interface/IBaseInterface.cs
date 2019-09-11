using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp.Model;

namespace ConsoleApp.Interface
{
    public interface IBaseInterface<T>
    {
        T Create(T data);
        List<T> Read();
        T Update(T data);
        bool Delete(int id);
    }
}
