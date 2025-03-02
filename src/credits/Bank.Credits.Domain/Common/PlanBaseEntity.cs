namespace Bank.Credits.Domain.Common
{
    public class PlanBaseEntity 
    {
        public long Id { get; set; }
        public long FromPlanId { get; set; }
        public long ToPlanId { get; set; }
    }
}
