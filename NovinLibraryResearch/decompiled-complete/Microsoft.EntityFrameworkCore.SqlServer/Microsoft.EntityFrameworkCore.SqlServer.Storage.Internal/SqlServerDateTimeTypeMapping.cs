using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerDateTimeTypeMapping : DateTimeTypeMapping
{
	private const string DateFormatConst = "'{0:yyyy-MM-dd}'";

	private const string SmallDateTimeFormatConst = "'{0:yyyy-MM-ddTHH:mm:ss}'";

	private const string DateTimeFormatConst = "'{0:yyyy-MM-ddTHH:mm:ss.fff}'";

	private readonly string[] _dateTime2Formats = new string[8] { "'{0:yyyy-MM-ddTHH:mm:ss}'", "'{0:yyyy-MM-ddTHH:mm:ss.fK}'", "'{0:yyyy-MM-ddTHH:mm:ss.ffK}'", "'{0:yyyy-MM-ddTHH:mm:ss.fffK}'", "'{0:yyyy-MM-ddTHH:mm:ss.ffffK}'", "'{0:yyyy-MM-ddTHH:mm:ss.fffffK}'", "'{0:yyyy-MM-ddTHH:mm:ss.ffffffK}'", "'{0:yyyy-MM-ddTHH:mm:ss.fffffffK}'" };

	protected override string SqlLiteralFormatString
	{
		get
		{
			switch (StoreType)
			{
			case "date":
				return "'{0:yyyy-MM-dd}'";
			case "datetime":
				return "'{0:yyyy-MM-ddTHH:mm:ss.fff}'";
			case "smalldatetime":
				return "'{0:yyyy-MM-ddTHH:mm:ss}'";
			default:
				if (Size.HasValue)
				{
					int value = Size.Value;
					switch (value)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
						return _dateTime2Formats[value];
					}
				}
				return _dateTime2Formats[7];
			}
		}
	}

	public SqlServerDateTimeTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, dbType)
	{
	}

	protected SqlServerDateTimeTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override void ConfigureParameter(DbParameter parameter)
	{
		base.ConfigureParameter(parameter);
		if (DbType == System.Data.DbType.Date)
		{
			((SqlParameter)parameter).SqlDbType = SqlDbType.Date;
		}
		if (Size.HasValue && Size.Value != -1)
		{
			parameter.Size = Size.Value;
		}
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerDateTimeTypeMapping(parameters);
	}
}
