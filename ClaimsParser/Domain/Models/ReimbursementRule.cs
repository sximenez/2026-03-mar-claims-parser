namespace ClaimsParser.Domain.Models
{
    internal class ReimbursementRule
    {
        public decimal Rate { get; init; } = decimal.Zero;
        public decimal MaxAmount { get; init; } = decimal.Zero;
    }
}
