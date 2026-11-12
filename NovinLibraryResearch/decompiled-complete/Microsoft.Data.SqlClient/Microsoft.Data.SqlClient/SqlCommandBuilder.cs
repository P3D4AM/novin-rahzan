using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Data.Common;
using Microsoft.Data.Sql;

namespace Microsoft.Data.SqlClient;

[DesignerCategory("")]
public sealed class SqlCommandBuilder : DbCommandBuilder
{
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override CatalogLocation CatalogLocation
	{
		get
		{
			return CatalogLocation.Start;
		}
		set
		{
			if (CatalogLocation.Start != value)
			{
				throw ADP.SingleValuedProperty("CatalogLocation", "Start");
			}
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string CatalogSeparator
	{
		get
		{
			return ".";
		}
		set
		{
			if ("." != value)
			{
				throw ADP.SingleValuedProperty("CatalogSeparator", ".");
			}
		}
	}

	[DefaultValue(null)]
	[ResCategory("Update")]
	[ResDescription("The DataAdapter for which to automatically generate SqlCommands")]
	public new SqlDataAdapter DataAdapter
	{
		get
		{
			return (SqlDataAdapter)base.DataAdapter;
		}
		set
		{
			base.DataAdapter = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string QuotePrefix
	{
		get
		{
			return base.QuotePrefix;
		}
		set
		{
			if ("[" != value && "\"" != value)
			{
				throw ADP.DoubleValuedProperty("QuotePrefix", "[", "\"");
			}
			base.QuotePrefix = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string QuoteSuffix
	{
		get
		{
			return base.QuoteSuffix;
		}
		set
		{
			if ("]" != value && "\"" != value)
			{
				throw ADP.DoubleValuedProperty("QuoteSuffix", "]", "\"");
			}
			base.QuoteSuffix = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string SchemaSeparator
	{
		get
		{
			return ".";
		}
		set
		{
			if ("." != value)
			{
				throw ADP.SingleValuedProperty("SchemaSeparator", ".");
			}
		}
	}

	public SqlCommandBuilder()
	{
		GC.SuppressFinalize(this);
		base.QuotePrefix = "[";
		base.QuoteSuffix = "]";
	}

	public SqlCommandBuilder(SqlDataAdapter adapter)
		: this()
	{
		DataAdapter = adapter;
	}

	private void SqlRowUpdatingHandler(object sender, SqlRowUpdatingEventArgs ruevent)
	{
		RowUpdatingHandler(ruevent);
	}

	public new SqlCommand GetInsertCommand()
	{
		return (SqlCommand)base.GetInsertCommand();
	}

	public new SqlCommand GetInsertCommand(bool useColumnsForParameterNames)
	{
		return (SqlCommand)base.GetInsertCommand(useColumnsForParameterNames);
	}

	public new SqlCommand GetUpdateCommand()
	{
		return (SqlCommand)base.GetUpdateCommand();
	}

	public new SqlCommand GetUpdateCommand(bool useColumnsForParameterNames)
	{
		return (SqlCommand)base.GetUpdateCommand(useColumnsForParameterNames);
	}

	public new SqlCommand GetDeleteCommand()
	{
		return (SqlCommand)base.GetDeleteCommand();
	}

	public new SqlCommand GetDeleteCommand(bool useColumnsForParameterNames)
	{
		return (SqlCommand)base.GetDeleteCommand(useColumnsForParameterNames);
	}

	protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
	{
		SqlParameter sqlParameter = (SqlParameter)parameter;
		object obj = datarow[SchemaTableColumn.ProviderType];
		sqlParameter.SqlDbType = (SqlDbType)obj;
		sqlParameter.Offset = 0;
		if (sqlParameter.SqlDbType == SqlDbType.Udt && !sqlParameter.SourceColumnNullMapping)
		{
			sqlParameter.UdtTypeName = datarow["DataTypeName"] as string;
		}
		else
		{
			sqlParameter.UdtTypeName = string.Empty;
		}
		object obj2 = datarow[SchemaTableColumn.NumericPrecision];
		if (DBNull.Value != obj2)
		{
			byte b = (byte)(short)obj2;
			sqlParameter.PrecisionInternal = (byte)((byte.MaxValue != b) ? b : 0);
		}
		obj2 = datarow[SchemaTableColumn.NumericScale];
		if (DBNull.Value != obj2)
		{
			byte b2 = (byte)(short)obj2;
			sqlParameter.ScaleInternal = (byte)((byte.MaxValue != b2) ? b2 : 0);
		}
	}

	protected override string GetParameterName(int parameterOrdinal)
	{
		return "@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
	}

	protected override string GetParameterName(string parameterName)
	{
		return "@" + parameterName;
	}

	protected override string GetParameterPlaceholder(int parameterOrdinal)
	{
		return "@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
	}

	private void ConsistentQuoteDelimiters(string quotePrefix, string quoteSuffix)
	{
		if (("\"" == quotePrefix && "\"" != quoteSuffix) || ("[" == quotePrefix && "]" != quoteSuffix))
		{
			throw ADP.InvalidPrefixSuffix();
		}
	}

	public static void DeriveParameters(SqlCommand command)
	{
		SqlConnection.ExecutePermission.Demand();
		if (command == null)
		{
			throw ADP.ArgumentNull("command");
		}
		TdsParser target = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			target = SqlInternalConnection.GetBestEffortCleanupTarget(command.Connection);
			command.DeriveParameters();
		}
		catch (OutOfMemoryException e)
		{
			if (command != null && command.Connection != null)
			{
				command.Connection.Abort(e);
			}
			throw;
		}
		catch (StackOverflowException e2)
		{
			if (command != null && command.Connection != null)
			{
				command.Connection.Abort(e2);
			}
			throw;
		}
		catch (ThreadAbortException e3)
		{
			if (command != null && command.Connection != null)
			{
				command.Connection.Abort(e3);
			}
			SqlInternalConnection.BestEffortCleanup(target);
			throw;
		}
	}

	protected override DataTable GetSchemaTable(DbCommand srcCommand)
	{
		SqlCommand sqlCommand = srcCommand as SqlCommand;
		SqlNotificationRequest notification = sqlCommand.Notification;
		bool notificationAutoEnlist = sqlCommand.NotificationAutoEnlist;
		sqlCommand.Notification = null;
		sqlCommand.NotificationAutoEnlist = false;
		try
		{
			using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
			return sqlDataReader.GetSchemaTable();
		}
		finally
		{
			sqlCommand.Notification = notification;
			sqlCommand.NotificationAutoEnlist = notificationAutoEnlist;
		}
	}

	protected override DbCommand InitializeCommand(DbCommand command)
	{
		SqlCommand sqlCommand = (SqlCommand)base.InitializeCommand(command);
		sqlCommand.NotificationAutoEnlist = false;
		return sqlCommand;
	}

	public override string QuoteIdentifier(string unquotedIdentifier)
	{
		ADP.CheckArgumentNull(unquotedIdentifier, "unquotedIdentifier");
		string quoteSuffix = QuoteSuffix;
		string quotePrefix = QuotePrefix;
		ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
		return ADP.BuildQuotedString(quotePrefix, quoteSuffix, unquotedIdentifier);
	}

	protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
	{
		if (adapter == base.DataAdapter)
		{
			((SqlDataAdapter)adapter).RowUpdating -= SqlRowUpdatingHandler;
		}
		else
		{
			((SqlDataAdapter)adapter).RowUpdating += SqlRowUpdatingHandler;
		}
	}

	public override string UnquoteIdentifier(string quotedIdentifier)
	{
		ADP.CheckArgumentNull(quotedIdentifier, "quotedIdentifier");
		string quoteSuffix = QuoteSuffix;
		string quotePrefix = QuotePrefix;
		ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
		ADP.RemoveStringQuotes(quotePrefix, quoteSuffix, quotedIdentifier, out var unquotedString);
		return unquotedString;
	}
}
