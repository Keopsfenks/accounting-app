using Ardalis.SmartEnum;

namespace Domain.Enums;

public sealed class StatusEnum(string name, int value) : SmartEnum<StatusEnum>(name, value) {
	public static readonly StatusEnum Draft     = new ("Draft", 1);     // Taslak
	public static readonly StatusEnum Sent      = new ("Sent", 2);      // Gönderilmiş
	public static readonly StatusEnum Paid      = new ("Paid", 3);      // Ödenmiş
	public static readonly StatusEnum Cancelled = new ("Cancelled", 4); // İptal Edilmiş

}