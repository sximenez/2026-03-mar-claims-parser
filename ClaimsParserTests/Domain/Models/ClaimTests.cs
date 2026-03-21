using ClaimsParser.Domain.Models;

namespace ClaimsParserTests.Domain.Models
{
    public class ClaimTests
    {
        [Fact]
        public void Create_ValidInput_ReturnsClaim()
        {
            // Arrange
            Dictionary<string, string> jsonDictionary = new Dictionary<string, string>
            {
                { "claimId", "C123" },
                { "memberId", "M456" },
                { "date", "2024-01-01" },
                { "amount", "100.50" },
                { "category", "Medical" },
                { "provider", "Provider A" }
            };

            // Act
            Result<Claim> claim = Claim.Create(jsonDictionary);

            // Assert
            Assert.Equal(jsonDictionary["claimId"], claim.Value!.ClaimId);
            Assert.Equal(jsonDictionary["memberId"], claim.Value!.MemberId);
            Assert.Equal(DateTime.Parse("2024-01-01"), claim.Value!.Date);
            Assert.Equal(100.50m, claim.Value!.Amount);
            Assert.Equal(jsonDictionary["category"], claim.Value!.Category);
            Assert.Equal(jsonDictionary["provider"], claim.Value!.Provider);
        }

        [Fact]
        public void Create_MissingMemberId_ReturnsFail()
        {
            // Arrange
            Dictionary<string, string> jsonDictionary = new Dictionary<string, string>
            {
                { "claimId", "C123" },
                { "date", "2024-01-01" },
                { "amount", "100.50" },
                { "category", "Medical" },
                { "provider", "Provider A" }
            };

            // Act
            Result<Claim> claim = Claim.Create(jsonDictionary);

            // Assert
            Assert.False(claim.IsSuccess);
            Assert.Contains("MemberId", claim.Error!);
        }

        [Fact]
        public void Create_NegativeAmount_ReturnsFail()
        {
            // Arrange
            Dictionary<string, string> jsonDictionary = new Dictionary<string, string>
            {
                { "claimId", "C123" },
                { "memberId", "M456" },
                { "date", "2024-01-01" },
                { "amount", "-100.50" },
                { "category", "Medical" },
                { "provider", "Provider A" }
            };

            // Act
            Result<Claim> claim = Claim.Create(jsonDictionary);

            // Assert
            Assert.False(claim.IsSuccess);
        }

        [Fact]
        public void Create_FutureDate_ReturnsFail()
        {
            // Arrange
            Dictionary<string, string> jsonDictionary = new Dictionary<string, string>
            {
                { "claimId", "C123" },
                { "memberId", "M456" },
                { "date", "2100-01-01" },
                { "amount", "100.50" },
                { "category", "Medical" },
                { "provider", "Provider A" }
            };

            // Act
            Result<Claim> claim = Claim.Create(jsonDictionary);

            // Assert
            Assert.False(claim.IsSuccess);
        }
    }
}
