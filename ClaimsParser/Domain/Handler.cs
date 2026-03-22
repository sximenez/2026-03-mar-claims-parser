using System.Text.Json;
using ClaimsParser.Domain.Models;

namespace ClaimsParser.Domain
{
    internal class Handler
    {
        private static readonly Dictionary<string, ReimbursementRule> ReimbursementRules = GetReimbursementRules();

        public static Result<Claim> Handle(string json)
        {
            Result<Dictionary<string, string>> deserialized = JsonToDictionary(json);
            if (!deserialized.IsSuccess)
            {
                return Result<Claim>.Fail($"{deserialized.Error}");
            }

            Result<Claim> claim = Claim.Create(deserialized.Value!);
            if (!claim.IsSuccess)
            {
                return Result<Claim>.Fail(claim.Error!);
            }

            return CalculateReimbursement(claim.Value!);
        }

        internal static Dictionary<string, ReimbursementRule> GetReimbursementRules()
        {
            return new Dictionary<string, ReimbursementRule>
            {
                { "consultation", new ReimbursementRule { Rate = 0.70m, MaxAmount = 25.00m } },
                { "specialist", new ReimbursementRule { Rate = 0.80m, MaxAmount = 50.00m } },
                { "hospital", new ReimbursementRule { Rate = 1.00m, MaxAmount = 500.00m } },
                { "pharmacy", new ReimbursementRule { Rate = 0.65m, MaxAmount = 30.00m } }
            };
        }

        internal static Result<Claim> CalculateReimbursement(Claim claim)
        {
            string claimCategory = claim.Category.ToLower();
            if (!ReimbursementRules.TryGetValue(claimCategory, out ReimbursementRule? rule))
            {
                return Result<Claim>.Fail($"Unknown category: {claimCategory}");
            }

            decimal reimbursementAmount = Math.Min(claim.Amount * rule.Rate, rule.MaxAmount);
            return Result<Claim>.Ok(claim with
            {
                ReimbursementAmount = reimbursementAmount,
                ReimbursementRule = claimCategory,
                ReimbursementStatus = reimbursementAmount > 0 ? "Approved" : "Denied"
            });
        }

        private static Result<Dictionary<string, string>> JsonToDictionary(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return Result<Dictionary<string, string>>.Fail("JSON input is empty.");
            }

            try
            {
                Dictionary<string, string>? dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (dictionary == null)
                {
                    return Result<Dictionary<string, string>>.Fail("JSON deserialization to dictionary failed.");
                }

                return Result<Dictionary<string, string>>.Ok(dictionary);
            }
            catch (JsonException ex)
            {
                return Result<Dictionary<string, string>>.Fail($"JSON deserialization error: {ex.Message}");
            }

        }
    }
}