namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal static class Numeric
{
	public static decimal BitAnd(decimal x, decimal y)
	{
		int num = (int)x;
		int num2 = (int)y;
		return num & num2;
	}

	public static decimal BitOr(decimal x, decimal y)
	{
		int num = (int)x;
		int num2 = (int)y;
		return num | num2;
	}

	public static decimal BitXor(decimal x, decimal y)
	{
		int num = (int)x;
		int num2 = (int)y;
		return num ^ num2;
	}
}
