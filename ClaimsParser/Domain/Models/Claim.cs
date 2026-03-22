using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ClaimsParserTests")]
namespace ClaimsParser.Domain.Models
{
    internal record Claim
    {
        public string ClaimId { get; private set; } = string.Empty;
        public string MemberId { get; private set; } = string.Empty;
        public DateTime Date { get; private set; } = DateTime.MinValue;
        public decimal Amount { get; private set; } = 0m;
        public string Category { get; private set; } = string.Empty;
        public string Provider { get; private set; } = string.Empty;

        public decimal ReimbursementAmount { get; init; } = decimal.Zero;
        public string ReimbursementRule { get; init; } = string.Empty;
        public string ReimbursementStatus { get; init; } = string.Empty;

        private Claim() { }

        public static Result<Claim> Create(Dictionary<string, string> json)
        {
            if (!json.TryGetValue("claimId", out string? claimId) || string.IsNullOrEmpty(claimId))
            {
                return Result<Claim>.Fail("ClaimId error.");
            }

            if (!json.TryGetValue("memberId", out string? memberId) || string.IsNullOrEmpty(memberId))
            {
                return Result<Claim>.Fail("MemberId error.");
            }

            if (!json.TryGetValue("date", out string? date) ||
                !DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateResult))
            {
                return Result<Claim>.Fail("Date error.");
            }
            else if (dateResult > DateTime.Now)
            {
                return Result<Claim>.Fail("Date cannot be in the future.");
            }

            if (!json.TryGetValue("amount", out string? amount) || !decimal.TryParse(amount, out decimal amountResult))
            {
                return Result<Claim>.Fail("Amount error.");
            }
            else if (amountResult < 0)
            {
                return Result<Claim>.Fail("Amount cannot be negative.");
            }

            if (!json.TryGetValue("category", out string? category) || string.IsNullOrEmpty(category))
            {
                return Result<Claim>.Fail("Category error.");
            }

            if (!json.TryGetValue("provider", out string? provider) || string.IsNullOrEmpty(provider))
            {
                return Result<Claim>.Fail("Provider error.");
            }

            return Result<Claim>.Ok(new Claim()
            {
                ClaimId = claimId,
                MemberId = memberId,
                Date = dateResult,
                Amount = amountResult,
                Category = category,
                Provider = provider
            });
        }
    }
}
