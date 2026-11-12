using System;

namespace Microsoft.Data.SqlClient.Server;

[Serializable]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SqlFunctionAttribute : Attribute
{
	private bool _isDeterministic;

	private DataAccessKind _dataAccess;

	private SystemDataAccessKind _systemDataAccess;

	private bool _isPrecise;

	private string _name;

	private string _tableDefinition;

	private string _fillRowMethodName;

	public bool IsDeterministic
	{
		get
		{
			return _isDeterministic;
		}
		set
		{
			_isDeterministic = value;
		}
	}

	public DataAccessKind DataAccess
	{
		get
		{
			return _dataAccess;
		}
		set
		{
			_dataAccess = value;
		}
	}

	public SystemDataAccessKind SystemDataAccess
	{
		get
		{
			return _systemDataAccess;
		}
		set
		{
			_systemDataAccess = value;
		}
	}

	public bool IsPrecise
	{
		get
		{
			return _isPrecise;
		}
		set
		{
			_isPrecise = value;
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public string TableDefinition
	{
		get
		{
			return _tableDefinition;
		}
		set
		{
			_tableDefinition = value;
		}
	}

	public string FillRowMethodName
	{
		get
		{
			return _fillRowMethodName;
		}
		set
		{
			_fillRowMethodName = value;
		}
	}

	public SqlFunctionAttribute()
	{
		_isDeterministic = false;
		_dataAccess = DataAccessKind.None;
		_systemDataAccess = SystemDataAccessKind.None;
		_isPrecise = false;
		_name = null;
		_tableDefinition = null;
		_fillRowMethodName = null;
	}
}
