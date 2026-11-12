using System;
using System.Data;
using System.Globalization;
using System.Management;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class WmiBase : EnumObject, ISqlFilterDecoderCallback
{
	private string m_ClassName;

	private StatementBuilder m_sb;

	protected StatementBuilder StatementBuilder => m_sb;

	public override ResultType[] ResultTypes => new ResultType[2]
	{
		ResultType.DataSet,
		ResultType.DataTable
	};

	bool ISqlFilterDecoderCallback.SupportsParameterization => false;

	private bool UseReservedFields
	{
		get
		{
			if (base.Request is WmiRequest { Fields: not null })
			{
				return true;
			}
			return false;
		}
	}

	public WmiBase(string className)
	{
		m_ClassName = className;
	}

	private WmiProperty GetPropertyForRequest(string propName)
	{
		WmiProperty wmiProperty = null;
		if (UseReservedFields)
		{
			return (WmiProperty)GetProperty(propName, ObjectPropertyUsages.Reserved1);
		}
		return (WmiProperty)GetProperty(propName, ObjectPropertyUsages.Request);
	}

	private string GetPhysicalNameForRequest(string propName)
	{
		return GetPropertyForRequest(propName).PhysicalName;
	}

	private void AddPropertyForRequest(string propName)
	{
		m_sb.AddFields(GetPhysicalNameForRequest(propName));
	}

	string ISqlFilterDecoderCallback.AddPropertyForFilter(string propName)
	{
		WmiProperty wmiProperty = null;
		wmiProperty = ((!UseReservedFields) ? ((WmiProperty)GetProperty(propName, ObjectPropertyUsages.Filter)) : ((WmiProperty)GetProperty(propName, ObjectPropertyUsages.Reserved1)));
		return wmiProperty.PhysicalName;
	}

	string ISqlFilterDecoderCallback.AddConstantForFilter(string constantValue)
	{
		return constantValue.Replace("\\", "\\\\").Replace("'", "\\'");
	}

	protected string GetXpathFilter()
	{
		return new FilterDecoder(this)
		{
			StringPrefix = string.Empty
		}.GetSql(base.Filter);
	}

	private void AddXpathFilter()
	{
		string xpathFilter = GetXpathFilter();
		if (xpathFilter.Length != 0)
		{
			m_sb.AddWhere(xpathFilter);
		}
	}

	protected virtual void BuildStatementBuilder()
	{
		string[] fields = base.Request.Fields;
		foreach (string propName in fields)
		{
			AddPropertyForRequest(propName);
		}
		m_sb.AddFrom(m_ClassName);
		AddXpathFilter();
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		m_sb = new StatementBuilder();
		BuildStatementBuilder();
		ManagementObjectSearcher val = new ManagementObjectSearcher();
		if (base.ConnectionInfo is WmiMgmtScopeConnection { ManagementScope: not null } wmiMgmtScopeConnection)
		{
			val.Scope = wmiMgmtScopeConnection.ManagementScope;
		}
		else
		{
			_003F val2 = val;
			object connectionInfo = base.ConnectionInfo;
			((ManagementObjectSearcher)val2).Scope = (ManagementScope)((connectionInfo is ManagementScope) ? connectionInfo : null);
		}
		if (val.Scope == null)
		{
			val.Scope = new ManagementScope(((WmiEnumResult)erParent).Scope);
		}
		val.Query = (ObjectQuery)new WqlObjectQuery(m_sb.SqlStatement);
		return BuildResult(val.Get());
	}

	protected virtual object GetTranslatedValue(ManagementObject mo, string propPhysName)
	{
		return ((ManagementBaseObject)mo)[propPhysName];
	}

	protected DataTable BuildDataTable(ManagementObjectCollection listManagementObject)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		DataTable dataTable = new DataTable();
		dataTable.Locale = CultureInfo.InvariantCulture;
		string[] fields = base.Request.Fields;
		foreach (string text in fields)
		{
			dataTable.Columns.Add(new DataColumn(text, Type.GetType(GetPropertyForRequest(text).BaseType)));
		}
		ManagementObjectEnumerator enumerator = listManagementObject.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ManagementObject mo = (ManagementObject)enumerator.Current;
				DataRow dataRow = dataTable.NewRow();
				fields = base.Request.Fields;
				foreach (string text2 in fields)
				{
					object translatedValue = GetTranslatedValue(mo, GetPhysicalNameForRequest(text2));
					dataRow[text2] = ((translatedValue != null) ? translatedValue : DBNull.Value);
				}
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
		finally
		{
			((IDisposable)enumerator)?.Dispose();
		}
	}

	private EnumResult BuildResult(ManagementObjectCollection listManagementObject)
	{
		ResultType resultType = base.Request.ResultType;
		if (resultType == ResultType.Default)
		{
			resultType = ResultType.DataSet;
		}
		DataTable dataTable = BuildDataTable(listManagementObject);
		if (ResultType.DataSet == resultType)
		{
			return new EnumResult(new DataSet
			{
				Locale = CultureInfo.InvariantCulture,
				Tables = { dataTable }
			}, resultType);
		}
		if (ResultType.DataTable == resultType)
		{
			return new EnumResult(dataTable, resultType);
		}
		throw new ResultTypeNotSupportedEnumeratorException(resultType);
	}

	public override Request RetrieveParentRequest()
	{
		return new WmiRequest();
	}
}
