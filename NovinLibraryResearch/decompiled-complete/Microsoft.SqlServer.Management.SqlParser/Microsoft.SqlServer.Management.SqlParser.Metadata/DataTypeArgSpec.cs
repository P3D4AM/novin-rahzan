using System.Globalization;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class DataTypeArgSpec
{
	private static class DataTypeArgName
	{
		public const string Length = "Length";

		public const string Precision = "Precision";

		public const string Scale = "Scale";
	}

	public readonly string Name;

	public readonly int MinValue;

	public readonly int MaxValue;

	public readonly int DefaultValue;

	private readonly string objectStr;

	private static readonly DataTypeArgSpec numericPrecision = new DataTypeArgSpec(1, 38, 18, "Precision");

	private static readonly DataTypeArgSpec numericScale = new DataTypeArgSpec(0, 38, 0, "Scale");

	private static readonly DataTypeArgSpec timeScale = new DataTypeArgSpec(0, 7, 7, "Scale");

	private static readonly DataTypeArgSpec doubleByteLength = new DataTypeArgSpec(1, 4000, 1, "Length");

	private static readonly DataTypeArgSpec singleByteLength = new DataTypeArgSpec(1, 8000, 1, "Length");

	public static DataTypeArgSpec NumericPrecision => numericPrecision;

	public static DataTypeArgSpec NumericScale => numericScale;

	public static DataTypeArgSpec TimeScale => timeScale;

	public static DataTypeArgSpec DoubleByteLength => doubleByteLength;

	public static DataTypeArgSpec SingleByteLength => singleByteLength;

	private DataTypeArgSpec(int minValue, int maxValue, int defaultValue, string argName)
	{
		MinValue = minValue;
		MaxValue = maxValue;
		DefaultValue = defaultValue;
		Name = argName;
		objectStr = Name + " = " + DefaultValue.ToString(CultureInfo.InvariantCulture);
	}

	public override string ToString()
	{
		return objectStr;
	}
}
