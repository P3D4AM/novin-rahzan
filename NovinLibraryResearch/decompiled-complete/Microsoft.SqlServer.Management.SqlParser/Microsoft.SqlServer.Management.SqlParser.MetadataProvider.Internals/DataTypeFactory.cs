using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class DataTypeFactory : ObjectFactoryBase<DataTypeFactory>, IDataTypeFactory
{
	private abstract class DataTypeBase : MetadataObjectBase, IDataType, IMetadataObject
	{
		public abstract bool IsUnknown { get; }

		public abstract bool IsScalar { get; }

		public abstract bool IsTable { get; }

		public abstract bool IsCursor { get; }

		public abstract IScalarDataType AsScalarDataType { get; }

		public abstract ITableDataType AsTableDataType { get; }

		public abstract IUserDefinedType AsUserDefinedType { get; }

		protected DataTypeBase(string name)
			: base(name)
		{
		}
	}

	private sealed class CursorDataType : DataTypeBase, ICursorDataType, IDataType, IMetadataObject
	{
		public override bool IsScalar => false;

		public override bool IsTable => false;

		public override bool IsCursor => true;

		public override bool IsUnknown => false;

		public override IScalarDataType AsScalarDataType => null;

		public override ITableDataType AsTableDataType => null;

		public override IUserDefinedType AsUserDefinedType => null;

		public CursorDataType()
			: base("cursor")
		{
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	private sealed class VoidDataType : DataTypeBase, IVoidDataType, IScalarDataType, IDataType, IMetadataObject
	{
		public override bool IsScalar => true;

		public override bool IsTable => false;

		public override bool IsCursor => false;

		public override bool IsUnknown => false;

		public override IScalarDataType AsScalarDataType => this;

		public override ITableDataType AsTableDataType => null;

		public override IUserDefinedType AsUserDefinedType => null;

		public bool IsSystem => false;

		public bool IsClr => false;

		public bool IsXml => false;

		public bool IsVoid => true;

		public ISystemDataType BaseSystemDataType => null;

		public IClrDataType AsClrDataType => null;

		public VoidDataType()
			: base("void")
		{
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	private abstract class UnknownDataType : DataTypeBase, IDataType, IMetadataObject
	{
		public sealed override bool IsUnknown => true;

		public sealed override IUserDefinedType AsUserDefinedType => null;

		protected UnknownDataType()
			: base(string.Empty)
		{
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}

	private sealed class UnknownScalarDataType : UnknownDataType, IScalarDataType, IDataType, IMetadataObject
	{
		private readonly string objString;

		public override bool IsScalar => true;

		public override bool IsTable => false;

		public override bool IsCursor => false;

		public override IScalarDataType AsScalarDataType => this;

		public override ITableDataType AsTableDataType => null;

		public bool IsSystem => false;

		public bool IsClr => false;

		public bool IsXml => false;

		public bool IsVoid => false;

		public ISystemDataType BaseSystemDataType => null;

		public IClrDataType AsClrDataType => null;

		public UnknownScalarDataType()
			: this(string.Empty)
		{
		}

		public UnknownScalarDataType(string objString)
		{
			this.objString = objString;
		}

		public override string ToString()
		{
			return objString;
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	private sealed class UnknownTableDataType : UnknownDataType, ITableDataType, IDataType, IMetadataObject, IDatabaseTable, ITabular
	{
		private readonly IMetadataOrderedCollection<IColumn> m_columns;

		private readonly IMetadataOrderedCollection<IConstraint> m_constraints;

		private readonly IMetadataOrderedCollection<IIndex> m_indexes;

		private readonly IMetadataOrderedCollection<IStatistics> m_statistics;

		public override bool IsScalar => false;

		public override bool IsTable => true;

		public override bool IsCursor => false;

		public override IScalarDataType AsScalarDataType => null;

		public override ITableDataType AsTableDataType => this;

		public CollationInfo CollationInfo => CollationInfo.Default;

		public IMetadataCollection<IConstraint> Constraints => m_constraints;

		public IMetadataCollection<IIndex> Indexes => m_indexes;

		public IMetadataCollection<IStatistics> Statistics => m_statistics;

		public TabularType TabularType => TabularType.TableDataType;

		public IMetadataOrderedCollection<IColumn> Columns => m_columns;

		public ITabular Unaliased => this;

		public UnknownTableDataType()
		{
			m_columns = Collection<IColumn>.EmptyOrdered;
			m_constraints = Collection<IConstraint>.EmptyOrdered;
			m_indexes = Collection<IIndex>.EmptyOrdered;
			m_statistics = Collection<IStatistics>.EmptyOrdered;
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	private sealed class UnknownClrDataType : UnknownDataType, IClrDataType, IScalarDataType, IDataType, IMetadataObject
	{
		private readonly IMetadataCollection<IUdtMethod> m_methods;

		private readonly IMetadataCollection<IUdtDataMember> m_dataMembers;

		public override bool IsScalar => true;

		public override bool IsTable => false;

		public override bool IsCursor => false;

		public override IScalarDataType AsScalarDataType => this;

		public override ITableDataType AsTableDataType => null;

		public string AssemblyName => null;

		public string ClassName => null;

		public bool IsNullable => false;

		public bool IsBinaryOrdered => false;

		public bool IsComVisible => false;

		public IMetadataCollection<IUdtMethod> Methods => m_methods;

		public IMetadataCollection<IUdtDataMember> DataMembers => m_dataMembers;

		public bool IsSystem => false;

		public bool IsClr => true;

		public bool IsXml => false;

		public bool IsVoid => false;

		public ISystemDataType BaseSystemDataType => null;

		public IClrDataType AsClrDataType => this;

		public UnknownClrDataType()
		{
			m_methods = Collection<IUdtMethod>.Empty;
			m_dataMembers = Collection<IUdtDataMember>.Empty;
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	private sealed class XmlNodeDataType : SystemDataType, IXmlDataType, ISystemDataType, IScalarDataType, IDataType, IMetadataObject
	{
		public bool IsXmlNode => true;

		public IMetadataCollection<IXmlDataTypeMethod> Methods => XmlDataTypeMethodCollection.Instance;

		public IMetadataCollection<IXmlDataTypeTableMethod> TableMethods => XmlDataTypeTableMethodCollection.Instance;

		public XmlNodeDataType()
			: base(DataTypeSpec.Xml)
		{
		}

		public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}
			return visitor.Visit(this);
		}
	}

	internal readonly IScalarDataType AnyType = new UnknownScalarDataType("<any type>");

	internal readonly IScalarDataType DataTypeName = new UnknownScalarDataType("<data type>");

	internal readonly IScalarDataType DatePart = new UnknownScalarDataType("<datepart>");

	internal readonly IScalarDataType ExpressionType = new UnknownScalarDataType("<expression type>");

	internal readonly IScalarDataType NumericType = new UnknownScalarDataType("<numeric type>");

	internal readonly IScalarDataType IntegerType = new UnknownScalarDataType("<integer type>");

	internal readonly IScalarDataType String = new UnknownScalarDataType("<string>");

	private readonly ICursorDataType m_cursor;

	private readonly IScalarDataType m_void;

	private readonly IScalarDataType m_unknownScalar;

	private readonly ITableDataType m_unknownTable;

	private readonly IClrDataType m_unknownClr;

	private readonly IXmlDataType m_xmlNode;

	public ICursorDataType Cursor => m_cursor;

	public IClrDataType UnknownClr => m_unknownClr;

	public IScalarDataType UnknownScalar => m_unknownScalar;

	public ITableDataType UnknownTable => m_unknownTable;

	public IScalarDataType Void => m_void;

	public IXmlDataType XmlNode => m_xmlNode;

	public DataTypeFactory()
	{
		m_cursor = new CursorDataType();
		m_void = new VoidDataType();
		m_unknownScalar = new UnknownScalarDataType();
		m_unknownTable = new UnknownTableDataType();
		m_unknownClr = new UnknownClrDataType();
		m_xmlNode = new XmlNodeDataType();
	}

	public IMutableTableDataType CreateTableDataType(string name, CollationInfo collationInfo)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (collationInfo == null)
		{
			throw new ArgumentNullException("collationInfo");
		}
		return new TableDataType(name, collationInfo);
	}
}
