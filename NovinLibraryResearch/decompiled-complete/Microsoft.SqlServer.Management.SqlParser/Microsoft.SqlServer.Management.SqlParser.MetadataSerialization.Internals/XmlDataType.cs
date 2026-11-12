using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class XmlDataType : IXmlDataType, ISystemDataType, IScalarDataType, IDataType, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly bool isXmlNode;

	private readonly int length;

	private readonly int numericPrecision;

	private readonly int numericScale;

	private readonly DataTypeSpec typeSpec;

	bool IXmlDataType.IsXmlNode => isXmlNode;

	IMetadataCollection<IXmlDataTypeMethod> IXmlDataType.Methods => model.GetXmlDataTypeMethodCollection();

	IMetadataCollection<IXmlDataTypeTableMethod> IXmlDataType.TableMethods => model.GetXmlDataTypeTableMethodCollection();

	DataTypeSpec ISystemDataType.TypeSpec => typeSpec;

	int ISystemDataType.Length => length;

	int ISystemDataType.NumericPrecision => numericPrecision;

	int ISystemDataType.NumericScale => numericScale;

	bool IScalarDataType.IsSystem => this != null;

	bool IScalarDataType.IsClr => this is IClrDataType;

	bool IScalarDataType.IsXml => this != null;

	bool IScalarDataType.IsVoid => this is IVoidDataType;

	ISystemDataType IScalarDataType.BaseSystemDataType => this;

	bool IDataType.IsScalar => this != null;

	bool IDataType.IsTable => this is ITableDataType;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => false;

	string IMetadataObject.Name => ((ISystemDataType)this).TypeSpec.Name;

	public XmlDataType(DeserializationModel model, bool isXmlNode, int length, int numericPrecision, int numericScale, DataTypeSpec typeSpec)
	{
		this.model = model;
		this.isXmlNode = isXmlNode;
		this.length = length;
		this.numericPrecision = numericPrecision;
		this.numericScale = numericScale;
		this.typeSpec = typeSpec;
	}

	public XmlDataType(DeserializationModel model, object[] values)
		: this(model, (bool)values[0], (int)values[1], (int)values[2], (int)values[3], (DataTypeSpec)values[4])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
