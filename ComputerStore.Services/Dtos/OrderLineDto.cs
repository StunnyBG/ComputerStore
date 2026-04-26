namespace ComputerStore.Services.Dtos;

/// <summary>
/// Carries a single line of a cart into the service layer.
/// Keeps the Services project free of any WinForms / UI dependency.
/// </summary>
public record OrderLineDto(int PartId, string Name, decimal UnitPrice, int Quantity);
