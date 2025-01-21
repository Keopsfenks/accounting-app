using Ardalis.SmartEnum;

namespace Domain.Enums;

public sealed class CurrencyTypeEnum(string name, int value) : SmartEnum<CurrencyTypeEnum>(name, value) {
	public static readonly CurrencyTypeEnum TL  = new("TRY", 1);
	public static readonly CurrencyTypeEnum USD = new("USD", 2);
	public static readonly CurrencyTypeEnum EUR = new("EURO", 3);
	public CurrencyTypeEnum() : this("", 1) { }
}