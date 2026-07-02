namespace MiniBillMart.Models;

public sealed class MonthlyPurchase
{
    public MonthlyPurchase(
        string itemCode,
        string productName,
        string pack,
        int lastQty,
        int suggestedQty,
        DateTime lastBillDate,
        decimal currentRate,
        decimal mrp,
        decimal gstPercent,
        string reason,
        string availability)
    {
        ItemCode = itemCode;
        ProductName = productName;
        Pack = pack;
        LastQty = lastQty;
        SuggestedQty = suggestedQty;
        LastBillDate = lastBillDate;
        CurrentRate = currentRate;
        Mrp = mrp;
        GstPercent = gstPercent;
        Reason = reason;
        Availability = availability;
    }

    public string ItemCode { get; }

    public string ProductName { get; }

    public string Pack { get; }

    public int LastQty { get; }

    public int SuggestedQty { get; }

    public DateTime LastBillDate { get; }

    public decimal CurrentRate { get; }

    public decimal Mrp { get; }

    public decimal GstPercent { get; }

    public string Reason { get; }

    public string Availability { get; }

    public bool AddedToBill { get; set; }

    public bool SkippedThisMonth { get; set; }

    public decimal SuggestedAmount => SuggestedQty * CurrentRate;
}
