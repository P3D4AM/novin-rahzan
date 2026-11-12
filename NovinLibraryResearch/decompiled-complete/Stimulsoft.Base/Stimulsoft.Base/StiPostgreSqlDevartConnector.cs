using System;

namespace Stimulsoft.Base;

public class StiPostgreSqlDevartConnector : StiPostgreSqlConnector
{
	public override StiConnectionIdent ConnectionIdent => StiConnectionIdent.PostgreSqlDataSource;

	public override Type SqlType => typeof(StiDbType.Devart.PostgreSql);

	public override int DefaultSqlType => 25;

	public override string[] NuGetPackages => new string[1] { "dotConnect.Express.for.PostgreSQL" };

	public override string NuGetVersion => "7.8.862";

	public override int GetSqlType(Type type)
	{
		if (type == typeof(DateTime))
		{
			return 1082;
		}
		if (type == typeof(TimeSpan))
		{
			return 1186;
		}
		if (type == typeof(long))
		{
			return 20;
		}
		if (type == typeof(int))
		{
			return 23;
		}
		if (type == typeof(short))
		{
			return 21;
		}
		if (type == typeof(byte))
		{
			return 21;
		}
		if (type == typeof(ulong))
		{
			return 20;
		}
		if (type == typeof(uint))
		{
			return 23;
		}
		if (type == typeof(ushort))
		{
			return 21;
		}
		if (type == typeof(sbyte))
		{
			return 21;
		}
		if (type == typeof(float))
		{
			return 700;
		}
		if (type == typeof(double))
		{
			return 701;
		}
		if (type == typeof(decimal))
		{
			return 1700;
		}
		if (type == typeof(string))
		{
			return 25;
		}
		if (type == typeof(bool))
		{
			return 16;
		}
		if (type == typeof(char))
		{
			return 1042;
		}
		if (type == typeof(byte[]))
		{
			return 17;
		}
		if (type == typeof(Guid))
		{
			return 2950;
		}
		return 9;
	}

	public override Type GetNetType(int dbType)
	{
		switch ((StiDbType.Devart.PostgreSql)dbType)
		{
		case StiDbType.Devart.PostgreSql.BigInt:
		case StiDbType.Devart.PostgreSql.Int:
		case StiDbType.Devart.PostgreSql.Numeric:
			return typeof(long);
		case StiDbType.Devart.PostgreSql.SmallInt:
			return typeof(short);
		case StiDbType.Devart.PostgreSql.Money:
			return typeof(decimal);
		case StiDbType.Devart.PostgreSql.Real:
			return typeof(float);
		case StiDbType.Devart.PostgreSql.Double:
			return typeof(double);
		case StiDbType.Devart.PostgreSql.Date:
		case StiDbType.Devart.PostgreSql.TimeStamp:
		case StiDbType.Devart.PostgreSql.TimeStampTZ:
			return typeof(DateTime);
		case StiDbType.Devart.PostgreSql.Time:
		case StiDbType.Devart.PostgreSql.TimeTZ:
			return typeof(TimeSpan);
		case StiDbType.Devart.PostgreSql.Boolean:
			return typeof(bool);
		default:
			return typeof(string);
		}
	}

	public override string GetSampleConnectionString()
	{
		return "User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;" + Environment.NewLine + "Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;";
	}

	public StiPostgreSqlDevartConnector(string connectionString = null)
		: base(connectionString)
	{
		base.NameAssembly = "Devart.Data.PostgreSql.dll";
		base.TypeConnection = "Devart.Data.PostgreSql.PgSqlConnection";
		base.TypeDataAdapter = "Devart.Data.PostgreSql.PgSqlDataAdapter";
		base.TypeCommand = "Devart.Data.PostgreSql.PgSqlCommand";
		base.TypeParameter = "Devart.Data.PostgreSql.PgSqlParameter";
		base.TypeDbType = "Devart.Data.PostgreSql.PgSqlType";
		base.TypeCommandBuilder = "Devart.Data.PostgreSql.PgSqlCommandBuilder";
	}
}
