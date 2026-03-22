using ClaimsParser.Domain;
using ClaimsParser.Domain.Models;

namespace ClaimsParserTests.Domain.Models
{
    public class HandlerTests
    {
        [Fact]
        public void Handle_ValidJson_ReturnsClaim()
        {
            // Arrange
            string json = @"{
                ""claimId"": ""C123"",
                ""memberId"": ""M456"",
                ""date"": ""2024-01-01"",
                ""amount"": ""100.50"",
                ""category"": ""Specialist"",
                ""provider"": ""Provider A""
            }";

            // Act
            Result<Claim> result = Handler.Handle(json);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("C123", result.Value!.ClaimId);
            Assert.Equal("M456", result.Value!.MemberId);
            Assert.Equal(DateTime.Parse("2024-01-01"), result.Value!.Date);
            Assert.Equal(100.50m, result.Value!.Amount);
            Assert.Equal("Specialist", result.Value!.Category);
            Assert.Equal("Provider A", result.Value!.Provider);
        }

        [Fact]
        public void Handle_MalformedJson_ReturnsError()
        {
            // Arrange
            string json = @"{
                ""claimId"": ""C123"",
                ""memberId"": ""M456"",
                ""date"": ""2024-01-01"",
                ""amount"": ""100.50"",
                ""category"": ""Medical"",
                ""provider"": ""Provider A"""; // Missing closing brace

            // Act
            Result<Claim> claimResult = Handler.Handle(json);

            // Assert
            Assert.False(claimResult.IsSuccess);
            Assert.Contains("JSON deserialization error", claimResult.Error!);
        }

        [Theory]
        [InlineData("consultation")]
        [InlineData("specialist")]
        [InlineData("hospital")]
        [InlineData("pharmacy")]
        public void GetReimbursementRules_ReturnsAmountBelowMax(string ruleStr)
        {
            // Arrange
            Dictionary<string, ReimbursementRule> rules = Handler.GetReimbursementRules();

            // Act
            ReimbursementRule rule = rules[ruleStr];
            decimal reimbursementAmount = Math.Min(100.00m * rule.Rate, rule.MaxAmount);

            // Assert
            Assert.True(reimbursementAmount <= rule.MaxAmount);
        }

        [Theory]
        [InlineData("consultation")]
        [InlineData("specialist")]
        [InlineData("hospital")]
        [InlineData("pharmacy")]
        public void GetReimbursementRules_ReturnsAmountAboveMax(string ruleStr)
        {
            // Arrange
            Dictionary<string, ReimbursementRule> rules = Handler.GetReimbursementRules();

            // Act
            ReimbursementRule rule = rules[ruleStr];
            decimal reimbursementAmount = Math.Min(1000.00m * rule.Rate, rule.MaxAmount);

            // Assert
            Assert.Equal(rule.MaxAmount, reimbursementAmount);
        }

        [Fact]
        public void CalculateReimbursement_ReturnsFail()
        {
            // Arrange
            Dictionary<string, string> json = new Dictionary<string, string>
            {
                { "claimId", "C123" },
                { "memberId", "M456" },
                { "date", "2024-01-01" },
                { "amount", "100.50" },
                { "category", "Dental" },
                { "provider", "Provider A" }
            };

            Claim claim = Claim.Create(json).Value!;

            // Act
            Result<Claim> result = Handler.CalculateReimbursement(claim);

            // Assert
            Assert.False(result.IsSuccess);
        }
    }
}
