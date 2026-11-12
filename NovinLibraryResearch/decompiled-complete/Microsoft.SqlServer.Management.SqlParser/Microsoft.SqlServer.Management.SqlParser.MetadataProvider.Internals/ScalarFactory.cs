using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class ScalarFactory : ObjectFactoryBase<ScalarFactory>, IScalarFactory
{
	private class ScalarExpression : MetadataObjectBase, IScalarExpression, IScalar, IMetadataObject
	{
		private readonly IScalarDataType m_dataType;

		private readonly bool m_nullable;

		private readonly ScalarType m_type;

		public ScalarType ScalarType => m_type;

		public IScalarDataType DataType => m_dataType;

		public bool Nullable => m_nullable;

		public ScalarExpression(IScalarDataType dataType, bool nullable, ScalarType type)
			: base(string.Empty)
		{
			m_dataType = dataType;
			m_nullable = nullable;
			m_type = type;
		}

		public static ScalarExpression Literal(IScalarDataType dataType)
		{
			return new ScalarExpression(dataType, nullable: false, ScalarType.Literal);
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

	private sealed class LiteralExpressionFactory : ILiteralScalarFactory
	{
		private readonly ScalarExpression m_binary;

		private readonly ScalarExpression m_default;

		private readonly ScalarExpression m_identifier;

		private readonly ScalarExpression m_image;

		private readonly ScalarExpression m_integer;

		private readonly ScalarExpression m_money;

		private readonly ScalarExpression m_null;

		private readonly ScalarExpression m_numeric;

		private readonly ScalarExpression m_real;

		private readonly ScalarExpression m_string;

		private readonly ScalarExpression m_unicodeString;

		public IScalarExpression Binary => m_binary;

		public IScalarExpression Default => m_default;

		public IScalarExpression Identifier => m_identifier;

		public IScalarExpression Image => m_image;

		public IScalarExpression Integer => m_integer;

		public IScalarExpression Money => m_money;

		public IScalarExpression Null => m_null;

		public IScalarExpression Numeric => m_numeric;

		public IScalarExpression Real => m_real;

		public IScalarExpression String => m_string;

		public IScalarExpression UnicodeString => m_unicodeString;

		public LiteralExpressionFactory()
		{
			SystemDataTypeLookup instance = SystemDataTypeLookup.Instance;
			m_binary = ScalarExpression.Literal(instance.Binary());
			m_default = ScalarExpression.Literal(ObjectFactoryBase<DataTypeFactory>.Instance.UnknownScalar);
			m_identifier = ScalarExpression.Literal(instance.NVarChar());
			m_image = ScalarExpression.Literal(instance.Image);
			m_integer = ScalarExpression.Literal(instance.Int);
			m_money = ScalarExpression.Literal(instance.Money);
			m_null = ScalarExpression.Literal(ObjectFactoryBase<DataTypeFactory>.Instance.Void);
			m_numeric = ScalarExpression.Literal(instance.Numeric());
			m_real = ScalarExpression.Literal(instance.Float);
			m_string = ScalarExpression.Literal(instance.VarChar());
			m_unicodeString = ScalarExpression.Literal(instance.NVarChar());
		}
	}

	private readonly LiteralExpressionFactory m_literalExpressionFactory;

	private readonly ScalarExpression m_expression;

	private readonly ScalarExpression m_null;

	public ILiteralScalarFactory Literal => m_literalExpressionFactory;

	public IScalar Null => m_null;

	public ScalarFactory()
	{
		m_literalExpressionFactory = new LiteralExpressionFactory();
		IScalarDataType unknownScalar = ObjectFactoryBase<DataTypeFactory>.Instance.UnknownScalar;
		m_expression = new ScalarExpression(unknownScalar, nullable: true, ScalarType.Expression);
		m_null = new ScalarExpression(unknownScalar, nullable: true, ScalarType.Null);
	}

	public IScalarExpression CreateScalarExpression(IScalarDataType dataType, bool nullable)
	{
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		if (dataType.IsUnknown)
		{
			return m_expression;
		}
		return new ScalarExpression(dataType, nullable, ScalarType.Expression);
	}
}
