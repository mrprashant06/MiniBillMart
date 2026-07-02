using MiniBillMart.Models;

namespace MiniBillMart.Services;

public sealed class SampleDataService
{
    public IReadOnlyList<Party> GetParties()
    {
        return new List<Party>
        {
            new("3811", "DELHI FRESH SUPERMARKET", "LAJPAT NAGAR", "DELHI"),
            new("1042", "GURUGRAM DAILY MART", "SECTOR 29, GURUGRAM", "HARYANA"),
            new("2208", "NCR FAMILY GROCER", "DWARKA EXPRESSWAY, GURUGRAM", "HARYANA")
        };
    }

    public IReadOnlyList<MonthlyPurchase> GetMonthlyPurchases(string partyCode)
    {
        return partyCode switch
        {
            "3811" => new List<MonthlyPurchase>
            {
                new("628", "BASMATI RICE PREMIUM 5KG", "5KG", 4, 4, new DateTime(2026, 6, 1), 520.00m, 599.00m, 5m, "Monthly item", "Available"),
                new("130", "WHOLE WHEAT ATTA 10KG", "10KG", 3, 3, new DateTime(2026, 6, 2), 410.00m, 465.00m, 5m, "30 day repeat", "Limited"),
                new("015", "REFINED SUNFLOWER OIL 1L", "1L", 12, 12, new DateTime(2026, 5, 25), 142.00m, 165.00m, 5m, "Not billed yet", "Available"),
                new("452", "TOOR DAL 1KG", "1KG", 10, 10, new DateTime(2026, 6, 5), 138.00m, 159.00m, 5m, "Monthly item", "Available")
            },
            "1042" => new List<MonthlyPurchase>
            {
                new("501", "MIXED FRUIT JAM 500GM", "500GM", 6, 6, new DateTime(2026, 6, 4), 145.00m, 175.00m, 12m, "Monthly item", "Available"),
                new("702", "CORN FLAKES 875GM", "875GM", 5, 5, new DateTime(2026, 6, 8), 255.00m, 299.00m, 18m, "30 day repeat", "Limited"),
                new("728", "INSTANT NOODLES FAMILY PACK", "12 PCS", 8, 8, new DateTime(2026, 5, 30), 132.00m, 156.00m, 12m, "Not billed yet", "Available")
            },
            "2208" => new List<MonthlyPurchase>
            {
                new("810", "UHT TONED MILK 1L", "1L", 24, 24, new DateTime(2026, 6, 7), 62.00m, 68.00m, 5m, "Monthly item", "Available"),
                new("811", "BROWN BREAD 400GM", "400GM", 18, 18, new DateTime(2026, 6, 7), 38.00m, 45.00m, 5m, "Same basket", "Available"),
                new("920", "FARM FRESH EGGS 12 PCS", "12 PCS", 10, 10, new DateTime(2026, 5, 28), 86.00m, 99.00m, 5m, "Not billed yet", "On Order")
            },
            _ => new List<MonthlyPurchase>()
        };
    }
}
