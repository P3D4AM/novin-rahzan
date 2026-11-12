using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Dmf;

internal class SupportedTypeAttributes
{
	internal TypeClass Class;

	internal List<OperatorType> SupportedOperators;

	internal SupportedTypeAttributes(TypeClass typeClass, List<OperatorType> operators)
	{
		Class = typeClass;
		SupportedOperators = operators;
	}
}
