using Ardalis.SmartEnum;

namespace Domain.Enums;

public sealed class OperationTypeEnum(string name, int value) : SmartEnum<OperationTypeEnum>(name, value) {
	public static readonly OperationTypeEnum CashProceeds = new("Cash Proceeds", 1);
	public static readonly OperationTypeEnum Sales        = new("Sales", 2);
}