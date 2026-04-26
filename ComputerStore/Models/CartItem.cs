namespace ComputerStore;

/// <summary>One line in the in-memory shopping cart.</summary>
public class CartItem
{
    public int     PartId    { get; set; }
    public string  Name      { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int     Quantity  { get; set; }
    public decimal Total     => UnitPrice * Quantity;
}
