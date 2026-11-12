using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Threading;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

[DefaultEvent("RowUpdated")]
[ToolboxItem("Microsoft.VSDesigner.Data.VS.SqlDataAdapterToolboxItem, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[Designer("Microsoft.VSDesigner.Data.VS.SqlDataAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[DesignerCategory("")]
public sealed class SqlDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
{
	private static readonly object EventRowUpdated = new object();

	private static readonly object EventRowUpdating = new object();

	private SqlCommand _deleteCommand;

	private SqlCommand _insertCommand;

	private SqlCommand _selectCommand;

	private SqlCommand _updateCommand;

	private SqlCommandSet _commandSet;

	private int _updateBatchSize = 1;

	private static int _objectTypeCount;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	internal int ObjectID => _objectID;

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ResCategory("Update")]
	[ResDescription("Used during Update for deleted rows in DataSet.")]
	public new SqlCommand DeleteCommand
	{
		get
		{
			return _deleteCommand;
		}
		set
		{
			_deleteCommand = value;
		}
	}

	IDbCommand IDbDataAdapter.DeleteCommand
	{
		get
		{
			return _deleteCommand;
		}
		set
		{
			_deleteCommand = (SqlCommand)value;
		}
	}

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ResCategory("Update")]
	[ResDescription("Used during Update for new rows in DataSet.")]
	public new SqlCommand InsertCommand
	{
		get
		{
			return _insertCommand;
		}
		set
		{
			_insertCommand = value;
		}
	}

	IDbCommand IDbDataAdapter.InsertCommand
	{
		get
		{
			return _insertCommand;
		}
		set
		{
			_insertCommand = (SqlCommand)value;
		}
	}

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ResCategory("Fill")]
	[ResDescription("Used during Fill/FillSchema.")]
	public new SqlCommand SelectCommand
	{
		get
		{
			return _selectCommand;
		}
		set
		{
			_selectCommand = value;
		}
	}

	IDbCommand IDbDataAdapter.SelectCommand
	{
		get
		{
			return _selectCommand;
		}
		set
		{
			_selectCommand = (SqlCommand)value;
		}
	}

	public override int UpdateBatchSize
	{
		get
		{
			return _updateBatchSize;
		}
		set
		{
			if (0 > value)
			{
				throw ADP.ArgumentOutOfRange("UpdateBatchSize");
			}
			_updateBatchSize = value;
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDataAdapter.set_UpdateBatchSize|API> {0}, {1}", ObjectID, value);
		}
	}

	[DefaultValue(null)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ResCategory("Update")]
	[ResDescription("Used during Update for modified rows in DataSet.")]
	public new SqlCommand UpdateCommand
	{
		get
		{
			return _updateCommand;
		}
		set
		{
			_updateCommand = value;
		}
	}

	IDbCommand IDbDataAdapter.UpdateCommand
	{
		get
		{
			return _updateCommand;
		}
		set
		{
			_updateCommand = (SqlCommand)value;
		}
	}

	[ResCategory("Update")]
	[ResDescription("Event triggered before every DataRow during Update.")]
	public event SqlRowUpdatedEventHandler RowUpdated
	{
		add
		{
			base.Events.AddHandler(EventRowUpdated, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventRowUpdated, value);
		}
	}

	[ResCategory("Update")]
	[ResDescription("Event triggered after every DataRow during Update.")]
	public event SqlRowUpdatingEventHandler RowUpdating
	{
		add
		{
			SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler = (SqlRowUpdatingEventHandler)base.Events[EventRowUpdating];
			if (sqlRowUpdatingEventHandler != null && value.Target is DbCommandBuilder)
			{
				SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler2 = (SqlRowUpdatingEventHandler)ADP.FindBuilder(sqlRowUpdatingEventHandler);
				if (sqlRowUpdatingEventHandler2 != null)
				{
					base.Events.RemoveHandler(EventRowUpdating, sqlRowUpdatingEventHandler2);
				}
			}
			base.Events.AddHandler(EventRowUpdating, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventRowUpdating, value);
		}
	}

	public SqlDataAdapter()
	{
		GC.SuppressFinalize(this);
	}

	public SqlDataAdapter(SqlCommand selectCommand)
		: this()
	{
		SelectCommand = selectCommand;
	}

	public SqlDataAdapter(string selectCommandText, string selectConnectionString)
		: this()
	{
		SqlConnection connection = new SqlConnection(selectConnectionString);
		SelectCommand = new SqlCommand(selectCommandText, connection);
	}

	public SqlDataAdapter(string selectCommandText, SqlConnection selectConnection)
		: this()
	{
		SelectCommand = new SqlCommand(selectCommandText, selectConnection);
	}

	private SqlDataAdapter(SqlDataAdapter from)
		: base(from)
	{
		GC.SuppressFinalize(this);
	}

	protected override int AddToBatch(IDbCommand command)
	{
		int commandCount = _commandSet.CommandCount;
		_commandSet.Append((SqlCommand)command);
		return commandCount;
	}

	protected override void ClearBatch()
	{
		_commandSet.Clear();
	}

	object ICloneable.Clone()
	{
		return new SqlDataAdapter(this);
	}

	protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
	{
		return new SqlRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
	}

	protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
	{
		return new SqlRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
	}

	protected override int ExecuteBatch()
	{
		SqlClientEventSource.Log.TryCorrelationTraceEvent("<sc.SqlDataAdapter.ExecuteBatch|Info|Correlation> ObjectID {0}, ActivityID {1}", ObjectID, ActivityCorrelator.Current);
		return _commandSet.ExecuteNonQuery();
	}

	protected override IDataParameter GetBatchedParameter(int commandIdentifier, int parameterIndex)
	{
		return _commandSet.GetParameter(commandIdentifier, parameterIndex);
	}

	protected override bool GetBatchedRecordsAffected(int commandIdentifier, out int recordsAffected, out Exception error)
	{
		return _commandSet.GetBatchedAffected(commandIdentifier, out recordsAffected, out error);
	}

	protected override void InitializeBatching()
	{
		SqlClientEventSource.Log.TryTraceEvent("<sc.SqlDataAdapter.InitializeBatching|API> {0}", ObjectID);
		_commandSet = new SqlCommandSet();
		SqlCommand sqlCommand = SelectCommand;
		if (sqlCommand == null)
		{
			sqlCommand = InsertCommand;
			if (sqlCommand == null)
			{
				sqlCommand = UpdateCommand;
				if (sqlCommand == null)
				{
					sqlCommand = DeleteCommand;
				}
			}
		}
		if (sqlCommand != null)
		{
			_commandSet.Connection = sqlCommand.Connection;
			_commandSet.Transaction = sqlCommand.Transaction;
			_commandSet.CommandTimeout = sqlCommand.CommandTimeout;
		}
	}

	protected override void OnRowUpdated(RowUpdatedEventArgs value)
	{
		SqlRowUpdatedEventHandler sqlRowUpdatedEventHandler = (SqlRowUpdatedEventHandler)base.Events[EventRowUpdated];
		if (sqlRowUpdatedEventHandler != null && value is SqlRowUpdatedEventArgs)
		{
			sqlRowUpdatedEventHandler(this, (SqlRowUpdatedEventArgs)value);
		}
		base.OnRowUpdated(value);
	}

	protected override void OnRowUpdating(RowUpdatingEventArgs value)
	{
		SqlRowUpdatingEventHandler sqlRowUpdatingEventHandler = (SqlRowUpdatingEventHandler)base.Events[EventRowUpdating];
		if (sqlRowUpdatingEventHandler != null && value is SqlRowUpdatingEventArgs)
		{
			sqlRowUpdatingEventHandler(this, (SqlRowUpdatingEventArgs)value);
		}
		base.OnRowUpdating(value);
	}

	protected override void TerminateBatching()
	{
		if (_commandSet != null)
		{
			_commandSet.Dispose();
			_commandSet = null;
		}
	}
}
