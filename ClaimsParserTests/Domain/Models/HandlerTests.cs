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
                ""category"": ""Medical"",
                ""provider"": ""Provider A""
            }";

            // Act
            Result<Claim> claimResult = Handler.Handle(json);

            // Assert
            Assert.True(claimResult.IsSuccess);
            Assert.Equal("C123", claimResult.Value!.ClaimId);
            Assert.Equal("M456", claimResult.Value!.MemberId);
            Assert.Equal(DateTime.Parse("2024-01-01"), claimResult.Value!.Date);
            Assert.Equal(100.50m, claimResult.Value!.Amount);
            Assert.Equal("Medical", claimResult.Value!.Category);
            Assert.Equal("Provider A", claimResult.Value!.Provider);
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
    }
}
