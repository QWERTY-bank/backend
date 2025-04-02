using Bank.Credits.Domain.Jobs;

namespace Bank.Credits.Quartz.Helpers
{
    internal static class PlanFabric
    {
        /// <summary>
        /// Вычисление отрезка
        /// </summary>
        /// <param name="startFrom">Откуда начинать самый первый отрезок</param>
        /// <param name="segmentLength">Длина отрезка</param>
        /// <param name="segmentIndex">Номер отрезка (начало с 0)</param>
        /// <returns></returns>
        public static (long from, long to) GetSegment(long startFrom, long endAt, int segmentLength, long segmentIndex)
        {
            long to = startFrom + (segmentIndex + 1) * segmentLength - 1;

            return (
                startFrom + segmentIndex * segmentLength,
                endAt < to ? endAt : to
            );
        }

        public static TPlan CreatePlan<TPlan>(long startFrom, long endAt, int segmentLength, long segmentIndex) where TPlan : PlanBaseEntity, new()
        {
            (long from, long to) = GetSegment(startFrom, endAt, segmentLength, segmentIndex);
            return new TPlan() { FromPlanId = from, ToPlanId = to, Status = PlanStatusType.Wait };
        }
    }
}
