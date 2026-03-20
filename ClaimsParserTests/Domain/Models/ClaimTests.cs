using ClaimsParser.Domain.Models;

namespace ClaimsParserTests.Domain.Models
{
    public class ClaimTests
    {
        [Fact]
        public void Create_ValidInput_ReturnsClaim()
        {
            // Arrange
            string claimId = "C123";
            string memberId = "M456";
            string date = "2024-01-01";
            string amount = "100,50";
            string category = "Medical";
            string provider = "Provider A";

            // Act
            Claim claim = Claim.Create(claimId, memberId, date, amount, category, provider);

            // Assert
            Assert.Equal(claimId, claim.ClaimId);
            Assert.Equal(memberId, claim.MemberId);
            Assert.Equal(DateTime.Parse("2024-01-01"), claim.Date);
            Assert.Equal(100.50m, claim.Amount);
            Assert.Equal(category, claim.Category);
            Assert.Equal(provider, claim.Provider);
        }
    }
}
