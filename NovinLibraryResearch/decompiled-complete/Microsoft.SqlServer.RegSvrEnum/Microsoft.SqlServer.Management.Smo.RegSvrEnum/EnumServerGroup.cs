using System;
using System.Collections;
using System.Data;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

internal class EnumServerGroup : EnumRegServerBase
{
	internal EnumServerGroup()
	{
		AddProperty(new ObjectProperty
		{
			Name = "Name",
			Type = "System.String",
			ReadOnly = false,
			Usage = (ObjectPropertyUsages.Filter | ObjectPropertyUsages.Request)
		});
		AddProperty(new ObjectProperty
		{
			Name = "Path",
			Type = "System.String",
			ReadOnly = true,
			Usage = (ObjectPropertyUsages.Filter | ObjectPropertyUsages.Request)
		});
		AddProperty(new ObjectProperty
		{
			Name = "Description",
			Type = "System.String",
			ReadOnly = false,
			Usage = ObjectPropertyUsages.Request
		});
		AddProperty(new ObjectProperty
		{
			Name = "ServerType",
			Type = "System.Guid",
			ReadOnly = true,
			Usage = ObjectPropertyUsages.Request
		});
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		GetFilterOptions(out var sPath, out var sName);
		ServerTypeRegistrationInfo serverTypeRegistrationInfo = erParent.Data as ServerTypeRegistrationInfo;
		if (erParent.Type != ResultType.Reserved1 || serverTypeRegistrationInfo == null)
		{
			throw new InternalEnumeratorException(SRError.InvalidParentResultType);
		}
		ArrayList arrayList = BuildRegistrationInfoList(serverTypeRegistrationInfo, sPath, sName, typeof(GroupRegistrationInfo));
		if (sPath == null)
		{
			sPath = string.Empty;
		}
		DataTable schemaTable = GetSchemaTable();
		foreach (GroupRegistrationInfo item in arrayList)
		{
			AddRow(sPath, item, schemaTable);
		}
		return new EnumResult(schemaTable, ResultType.DataTable);
	}

	private void AddRow(string sPath, GroupRegistrationInfo gri, DataTable dt)
	{
		DataRow dataRow = dt.NewRow();
		IEnumerator enumerator = dt.Columns.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				switch (((DataColumn)enumerator.Current).Caption)
				{
				case "ServerType":
					dataRow["ServerType"] = gri.ServerType;
					break;
				case "Path":
					dataRow["Path"] = sPath;
					break;
				case "Name":
					dataRow["Name"] = gri.FriendlyName;
					break;
				case "Description":
					dataRow["Description"] = gri.Description;
					break;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		dt.Rows.Add(dataRow);
	}
}
