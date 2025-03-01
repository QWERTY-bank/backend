namespace Bank.Credits.Domain.Common
{
    public abstract class SoftDeleteBaseEntity : BaseEntity
    {
        public DateTime? DeleteDateTime { get; set; }
    }
}
