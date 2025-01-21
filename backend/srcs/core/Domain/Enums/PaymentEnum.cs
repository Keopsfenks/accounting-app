using Ardalis.SmartEnum;

namespace Domain.Enums;

public sealed class PaymentTypeEnum(string name, int value) : SmartEnum<PaymentTypeEnum>(name, value) {
	public static readonly PaymentTypeEnum Cash = new PaymentTypeEnum("Cash", 1);
	public static readonly PaymentTypeEnum CreditCard = new PaymentTypeEnum("Credit Card", 2);
	public static readonly PaymentTypeEnum BankTransfer = new PaymentTypeEnum("Bank Transfer", 3);
	public static readonly PaymentTypeEnum Cheque = new PaymentTypeEnum("Cheque", 4);
}

public sealed class PaymentEnum(string name, int value) : SmartEnum<PaymentEnum>(name, value) {
	public static readonly PaymentEnum Future = new PaymentEnum("Future", 1);
	public static readonly PaymentEnum InCash = new PaymentEnum("In Cash", 2);

}