namespace Serilog.Exceptions.Reflection;

internal class ReflectionInfo
{
	public ReflectionPropertyInfo[] Properties { get; }

	public ReflectionPropertyInfo[] PropertiesExceptBaseOnes { get; }

	public ReflectionInfo(ReflectionPropertyInfo[] properties, ReflectionPropertyInfo[] propertiesExceptBaseOnes)
	{
		Properties = properties;
		PropertiesExceptBaseOnes = propertiesExceptBaseOnes;
	}
}
