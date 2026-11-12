using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class SystemDataTypeLookup : SystemDataTypeLookupBase
{
	private static class Singleton
	{
		public static SystemDataTypeLookup Instance;

		static Singleton()
		{
			Instance = new SystemDataTypeLookup();
		}
	}

	public class IgnoreArgTypes
	{
		private readonly ISystemDataType m_binary;

		private readonly ISystemDataType m_char;

		private readonly ISystemDataType m_dateTime2;

		private readonly ISystemDataType m_dateTimeOffset;

		private readonly ISystemDataType m_decimal;

		private readonly ISystemDataType m_nchar;

		private readonly ISystemDataType m_numeric;

		private readonly ISystemDataType m_nvarChar;

		private readonly ISystemDataType m_time;

		private readonly ISystemDataType m_varBinary;

		private readonly ISystemDataType m_varChar;

		public ISystemDataType Binary => m_binary;

		public ISystemDataType Char => m_char;

		public ISystemDataType DateTime2 => m_dateTime2;

		public ISystemDataType DateTimeOffset => m_dateTimeOffset;

		public ISystemDataType Decimal => m_decimal;

		public ISystemDataType NChar => m_nchar;

		public ISystemDataType Numeric => m_numeric;

		public ISystemDataType NVarChar => m_nvarChar;

		public ISystemDataType Time => m_time;

		public ISystemDataType VarBinary => m_varBinary;

		public ISystemDataType VarChar => m_varChar;

		public IgnoreArgTypes()
		{
			m_binary = SystemDataType.SystemDataTypeFactory.Binary.GetDataType(ignoreArgs: true);
			m_char = SystemDataType.SystemDataTypeFactory.Char.GetDataType(ignoreArgs: true);
			m_dateTime2 = SystemDataType.SystemDataTypeFactory.DateTime2.GetDataType(ignoreArgs: true);
			m_dateTimeOffset = SystemDataType.SystemDataTypeFactory.DateTimeOffset.GetDataType(ignoreArgs: true);
			m_decimal = SystemDataType.SystemDataTypeFactory.Decimal.GetDataType(ignoreArgs: true);
			m_nchar = SystemDataType.SystemDataTypeFactory.NChar.GetDataType(ignoreArgs: true);
			m_numeric = SystemDataType.SystemDataTypeFactory.Numeric.GetDataType(ignoreArgs: true);
			m_nvarChar = SystemDataType.SystemDataTypeFactory.NVarChar.GetDataType(ignoreArgs: true);
			m_time = SystemDataType.SystemDataTypeFactory.Time.GetDataType(ignoreArgs: true);
			m_varBinary = SystemDataType.SystemDataTypeFactory.VarBinary.GetDataType(ignoreArgs: true);
			m_varChar = SystemDataType.SystemDataTypeFactory.VarChar.GetDataType(ignoreArgs: true);
		}
	}

	private readonly IgnoreArgTypes m_ignoreArgs;

	public static SystemDataTypeLookup Instance => Singleton.Instance;

	public IgnoreArgTypes IgnoreArgs => m_ignoreArgs;

	private SystemDataTypeLookup()
	{
		m_ignoreArgs = new IgnoreArgTypes();
	}
}
