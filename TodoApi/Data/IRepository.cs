namespace Controller_based_APIs.Data
{
    public interface IRepository<T>
    {
        T? Get(int id);
        IEnumerable<T> GetAll();
        void Add(T item);
        void Update(T item);
        void Delete(int id);
        bool Exists(int id);
    }
}
