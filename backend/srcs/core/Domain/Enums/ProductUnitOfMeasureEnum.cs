using Ardalis.SmartEnum;

namespace Domain.Enums;

public sealed class ProductUnitOfMeasureEnum(string name, int value)
	: SmartEnum<ProductUnitOfMeasureEnum>(name, value)
{
	public static readonly ProductUnitOfMeasureEnum Kilogram  = new("Kilogram", 1);
	public static readonly ProductUnitOfMeasureEnum Gram      = new("Gram", 2);
	public static readonly ProductUnitOfMeasureEnum Milligram = new("Milligram", 3);
	public static readonly ProductUnitOfMeasureEnum Ton       = new("Ton", 4);

	public static readonly ProductUnitOfMeasureEnum Liter      = new("Litre", 5);
	public static readonly ProductUnitOfMeasureEnum Milliliter = new("Millilitre", 6);
	public static readonly ProductUnitOfMeasureEnum CubicMeter = new("Cubic Meter", 7);

	public static readonly ProductUnitOfMeasureEnum Unit   = new("Unit", 8);
	public static readonly ProductUnitOfMeasureEnum Box    = new("Box", 9);
	public static readonly ProductUnitOfMeasureEnum Packet = new("Packet", 10);
	public static readonly ProductUnitOfMeasureEnum Piece  = new("Piece", 11);

	public static readonly ProductUnitOfMeasureEnum Meter      = new("Meter", 12);
	public static readonly ProductUnitOfMeasureEnum Centimeter = new("Centimeter", 13);
	public static readonly ProductUnitOfMeasureEnum Millimeter = new("Millimeter", 14);
	public static readonly ProductUnitOfMeasureEnum Kilometer  = new("Kilometer", 15);

	public static readonly ProductUnitOfMeasureEnum SquareMeter     = new("Square Meter", 16);
	public static readonly ProductUnitOfMeasureEnum SquareKilometer = new("Square Kilometer", 17);
	public static readonly ProductUnitOfMeasureEnum Hectare         = new("Hectare", 18);
	public static readonly ProductUnitOfMeasureEnum Acre            = new("Acre", 19);

	public static readonly ProductUnitOfMeasureEnum Hour   = new("Hour", 20);
	public static readonly ProductUnitOfMeasureEnum Minute = new("Minute", 21);
	public static readonly ProductUnitOfMeasureEnum Second = new("Second", 22);
}