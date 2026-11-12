using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IBuiltInFunctionLookup
{
	IMetadataCollection<IBuiltInFunction> BuiltInFunctions { get; }

	IMetadataCollection<IBuiltInFunction> GlobalVariables { get; }

	IMetadataCollection<IBuiltInFunction> AllBuiltIns { get; }

	IMetadataCollection<IDatePart> DateParts { get; }

	IBuiltInFunction GetMultiTypeBuiltInFunction(string name, ISystemDataType dataType);
}
