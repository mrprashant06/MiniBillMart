namespace MiniBillMart.Models;

public sealed class InvoiceLine
{
    public InvoiceLine(string itemCode, string productName, string pack, int qty, decimal rate, decimal gstPercent)
    {
        ItemCode = itemCode;
        ProductName = productName;
        Pack = pack;
        Qty = qty;
        Rate = rate;
        GstPercent = gstPercent;
    }

    public string ItemCode { get; }

    public string ProductName { get; }

    public string Pack { get; }

    public int Qty { get; private set; }

    public decimal Rate { get; }

    public decimal GstPercent { get; }

    public decimal Amount => Qty * Rate;

    public void AddQty(int qty)
    {
        Qty += qty;
    }
}
