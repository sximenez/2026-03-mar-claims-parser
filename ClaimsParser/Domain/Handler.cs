using System.Text.Json;
using ClaimsParser.Domain.Models;

namespace ClaimsParser.Domain
{
	internal class Handler
	{
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

			return claim;
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