using Ardalis.SmartEnum;

namespace Domain.Enums;

public sealed class CurrencyTypeEnum(string name, int value) : SmartEnum<CurrencyTypeEnum>(name, value) {
	public static readonly CurrencyTypeEnum TL  = new("TL", 1);
	public static readonly CurrencyTypeEnum USD = new("USD", 2);
	public static readonly CurrencyTypeEnum EUR = new("Euro", 3);
}