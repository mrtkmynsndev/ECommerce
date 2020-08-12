namespace ECommerce.Core.Entities
{
    public class BaseEntity <T>
    {
        public BaseEntity()
        {
            
        }

        public BaseEntity(T id)
        {
            Id = id;
        }

        public T Id { get; set; }
    }
}