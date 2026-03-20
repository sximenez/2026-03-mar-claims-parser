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

        private Claim() { }

        public static Claim Create(string claimId, string memberId, string date, string amount, string category, string provider)
        {
            return new Claim()
            {
                ClaimId = ValidateClaimId(claimId),
                MemberId = ValidateMemberId(memberId),
                Date = ValidateDate(date),
                Amount = ValidateAmount(amount),
                Category = ValidateCategory(category),
                Provider = ValidateProvider(provider)
            };
        }

        private static string ValidateClaimId(string claimId)
        {
            if (string.IsNullOrEmpty(claimId))
            {
                throw new ArgumentException("ClaimId error.", nameof(claimId));
            }

            return claimId;
        }

        private static string ValidateMemberId(string memberId)
        {
            if (string.IsNullOrEmpty(memberId))
            {
                throw new ArgumentException("MemberId error.", nameof(memberId));
            }

            return memberId;
        }

        private static DateTime ValidateDate(string date)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateResult))
            {
                throw new ArgumentException("Date error.", nameof(date));
            }
            else if (dateResult > DateTime.Now)
            {
                throw new ArgumentException("Date cannot be in the future.", nameof(date));
            }

            return dateResult;
        }

        private static decimal ValidateAmount(string amount)
        {
            if (!decimal.TryParse(amount, out decimal amountResult))
            {
                throw new ArgumentException("Amount error.", nameof(amount));
            }
            else if (amountResult < 0)
            {
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            }

            return amountResult;
        }

        private static string ValidateCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentException("Category error.", nameof(category));
            }

            return category;
        }

        private static string ValidateProvider(string provider)
        {
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentException("Provider error.", nameof(provider));
            }

            return provider;
        }
    }
}
