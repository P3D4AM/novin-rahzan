using System;
using System.Data;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class EnumManagedComputer : EnumObject
{
	public override ResultType[] ResultTypes => new ResultType[2]
	{
		ResultType.DataSet,
		ResultType.DataTable
	};

	public EnumManagedComputer()
	{
		ObjectProperty objectProperty = null;
		AddProperty(new ObjectProperty
		{
			Name = "Name",
			Type = "System.String",
			Usage = ObjectPropertyUsages.Request
		});
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		string fixedStringProperty = GetFixedStringProperty("Name", removeEscape: true);
		if (fixedStringProperty == null)
		{
			throw new InternalEnumeratorException(StringWmiEnumerator.MachineNameMustBeSpecified);
		}
		if (base.Request is WmiRequest)
		{
			string text = "root/Microsoft/SqlServer/ComputerManagement" + 16.ToString(CultureInfo.InvariantCulture);
			if (string.Empty != fixedStringProperty)
			{
				text = "\\\\" + fixedStringProperty + "/" + text;
			}
			return new WmiEnumResult(text);
		}
		DataTable dataTable = new DataTable();
		dataTable.Locale = CultureInfo.InvariantCulture;
		dataTable.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
		DataRow dataRow = dataTable.NewRow();
		dataRow["Name"] = fixedStringProperty;
		dataTable.Rows.Add(dataRow);
		ResultType resultType = base.Request.ResultType;
		if (resultType == ResultType.Default)
		{
			resultType = ResultType.DataSet;
		}
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
}
