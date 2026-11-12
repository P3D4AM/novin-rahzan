namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableClrDataType : IClrDataType, IScalarDataType, IDataType, IMetadataObject, IMutableScalarDataType, IMutableDataType, IMutableMetadataObject
{
	new string AssemblyName { get; set; }

	new string ClassName { get; set; }

	new bool IsBinaryOrdered { get; set; }

	new bool IsComVisible { get; set; }

	new bool IsNullable { get; set; }

	new IMutableMetadataCollection<IUdtMethod> Methods { get; }

	new IMutableMetadataCollection<IUdtDataMember> DataMembers { get; }
}
