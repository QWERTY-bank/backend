using Bank.Credits.Domain.Common;
using Bank.Credits.Domain.Credits;

namespace Bank.Credits.Domain.User
{
    public class UserEntity : BaseEntity
    {
        public double Rating { get; set; }
        public bool RatingIsActual { get; set; }

        public List<Credit> Credits { get; set; } = null!;
    }
}
