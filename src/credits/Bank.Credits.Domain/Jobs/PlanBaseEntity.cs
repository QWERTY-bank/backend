namespace Bank.Credits.Domain.Jobs
{
    public class PlanBaseEntity 
    {
        public long Id { get; set; }
        public long FromPlanId { get; set; }
        public long ToPlanId { get; set; }
        public PlanStatusType Status { get; set; }
        public DateTime? StatusUpdatedAt { get; set; }
    }
}
