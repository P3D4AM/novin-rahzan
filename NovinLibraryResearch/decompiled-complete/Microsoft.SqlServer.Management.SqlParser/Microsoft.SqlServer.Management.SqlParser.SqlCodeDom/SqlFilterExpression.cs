namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlFilterExpression : SqlCodeObject
{
	private readonly bool isValid = true;

	internal bool IsValid => isValid;

	internal SqlFilterExpression()
		: this(isValid: true)
	{
	}

	internal SqlFilterExpression(bool isValid)
	{
		this.isValid = isValid;
	}
}
