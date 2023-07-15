using System.Collections.Generic;
namespace Logic
{
    internal interface ICRUD<T, IDType>
    {
        T Get(IDType id);
        List<T> GetAll();
        bool Finded(IDType id);
        void Add(T crud);
        void Update(T crud);
        void Delete(IDType id);
    }
}
