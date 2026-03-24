namespace Orders.ExternalServices.Interfaces
{
    public interface ICacheService 
    { 
        void CreateCache(Object value); 
        void DeleteCache(Object value);
    }
}
