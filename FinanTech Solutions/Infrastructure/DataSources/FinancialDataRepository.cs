using FinanTech_Solutions.Domain.Entities;

namespace FinanTech_Solutions.Infrastructure.DataSources;

public sealed class FinancialDataRepository
{
    private readonly IReadOnlyList<FinancialRecord> _records;

    public FinancialDataRepository()
    {
        var baseDate = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);
        _records =
        [
            new(Guid.Parse("a1b2c3d4-0001-0000-0000-000000000001"), baseDate.AddDays(0),  "ACC-001", "Q1 Revenue deposit",           48_500.00m, "USD", "Credit", "user-001", null),
            new(Guid.Parse("a1b2c3d4-0002-0000-0000-000000000002"), baseDate.AddDays(3),  "ACC-002", "Vendor payment - Office Inc",   12_300.00m, "USD", "Debit",  "user-002", "Approved by CFO"),
            new(Guid.Parse("a1b2c3d4-0003-0000-0000-000000000003"), baseDate.AddDays(5),  "ACC-001", "Client payment - Acme Corp",    22_000.00m, "USD", "Credit", "user-001", null),
            new(Guid.Parse("a1b2c3d4-0004-0000-0000-000000000004"), baseDate.AddDays(7),  "ACC-003", "Payroll disbursement",          35_750.00m, "USD", "Debit",  "user-003", "Monthly payroll"),
            new(Guid.Parse("a1b2c3d4-0005-0000-0000-000000000005"), baseDate.AddDays(10), "ACC-004", "Equipment lease payment",        8_400.00m, "USD", "Debit",  "user-002", null),
            new(Guid.Parse("a1b2c3d4-0006-0000-0000-000000000006"), baseDate.AddDays(12), "ACC-001", "Investment return Q4",          55_000.00m, "USD", "Credit", "user-004", "Dividend"),
            new(Guid.Parse("a1b2c3d4-0007-0000-0000-000000000007"), baseDate.AddDays(15), "ACC-005", "Consulting fee received",        9_800.00m, "USD", "Credit", "user-001", null),
            new(Guid.Parse("a1b2c3d4-0008-0000-0000-000000000008"), baseDate.AddDays(17), "ACC-002", "Software license renewal",       5_200.00m, "USD", "Debit",  "user-003", "Annual license"),
            new(Guid.Parse("a1b2c3d4-0009-0000-0000-000000000009"), baseDate.AddDays(20), "ACC-003", "Utility bills Q1",               3_100.00m, "USD", "Debit",  "user-002", null),
            new(Guid.Parse("a1b2c3d4-0010-0000-0000-000000000010"), baseDate.AddDays(22), "ACC-001", "Client advance payment",        27_500.00m, "USD", "Credit", "user-005", "Project Alpha"),
            new(Guid.Parse("a1b2c3d4-0011-0000-0000-000000000011"), baseDate.AddDays(25), "ACC-004", "Bank charges",                     850.00m, "USD", "Debit",  "user-002", null),
            new(Guid.Parse("a1b2c3d4-0012-0000-0000-000000000012"), baseDate.AddDays(28), "ACC-005", "Grant disbursement",            18_000.00m, "USD", "Credit", "user-004", "R&D Grant"),
            new(Guid.Parse("a1b2c3d4-0013-0000-0000-000000000013"), baseDate.AddDays(32), "ACC-002", "Insurance premium",              6_750.00m, "USD", "Debit",  "user-003", "Annual premium"),
            new(Guid.Parse("a1b2c3d4-0014-0000-0000-000000000014"), baseDate.AddDays(35), "ACC-001", "Q2 Revenue deposit",            62_000.00m, "USD", "Credit", "user-001", null),
            new(Guid.Parse("a1b2c3d4-0015-0000-0000-000000000015"), baseDate.AddDays(38), "ACC-003", "Office rent payment",           14_000.00m, "USD", "Debit",  "user-002", "Q2 rent"),
            new(Guid.Parse("a1b2c3d4-0016-0000-0000-000000000016"), baseDate.AddDays(42), "ACC-004", "Tax withholding remittance",    21_300.00m, "USD", "Debit",  "user-005", "IRS payment"),
            new(Guid.Parse("a1b2c3d4-0017-0000-0000-000000000017"), baseDate.AddDays(45), "ACC-005", "Product sales batch",           33_400.00m, "USD", "Credit", "user-001", null),
            new(Guid.Parse("a1b2c3d4-0018-0000-0000-000000000018"), baseDate.AddDays(48), "ACC-002", "Emergency IT repair",            4_500.00m, "USD", "Debit",  "user-003", "Server failure"),
            new(Guid.Parse("a1b2c3d4-0019-0000-0000-000000000019"), baseDate.AddDays(52), "ACC-001", "Acquisition deposit",           78_000.00m, "USD", "Credit", "user-004", "M&A prepayment"),
            new(Guid.Parse("a1b2c3d4-0020-0000-0000-000000000020"), baseDate.AddDays(55), "ACC-003", "Loan repayment installment",    16_500.00m, "USD", "Debit",  "user-005", "Bank loan inst.2"),
        ];
    }

    public Task<IEnumerable<FinancialRecord>> GetAllAsync()
        => Task.FromResult(_records.AsEnumerable());
}
