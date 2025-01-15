using Ardalis.SmartEnum;

namespace Domain.Enums;

public sealed class CustomerTypeEnum(string name, int value) : SmartEnum<CustomerTypeEnum>(name, value) {
	public static readonly CustomerTypeEnum Individual = new("Individual", 1);
	public static readonly CustomerTypeEnum Corporate  = new("Corporate", 2);
}