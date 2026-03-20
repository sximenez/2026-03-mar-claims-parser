# Claims Parser

## TOC

## Stages

**Structures used**: `Result<T>`, `Dictionary<K,V>`, `List<T>`

**Core skill**: Parse structured input, apply business rules, return structured output, handle errors, write tests

### Stage 1 - Parse a claim record

**Input**: a JSON string representing a single health claim.
```json
{
  "claim_id": "CLM-001",
  "member_id": "MBR-123",
  "date": "2026-03-01",
  "amount": 85.50,
  "category": "consultation",
  "provider": "Dr. Martin"
}
```

1. Parse it into a strongly-typed Claim record:



2. Validate: all fields present, amount > 0, date not in the future.


3. Return Result<Claim> — not exceptions.
- A missing field returns Result<Claim>.Fail("Missing field: member_id").
- A negative amount returns Result<Claim>.Fail("Amount must be positive").

Tests to write:

Valid input → IsSuccess = true, claim fields populated correctly
Missing member_id → IsSuccess = false, error names the field
Negative amount → IsSuccess = false
Future date → IsSuccess = false
Malformed JSON → IsSuccess = false, does not throw

Why Result<T> here:
The caller (a batch processor) will handle thousands of claims.
One bad record must not stop the batch.
Result<T> lets the caller decide what to do — not the parser.