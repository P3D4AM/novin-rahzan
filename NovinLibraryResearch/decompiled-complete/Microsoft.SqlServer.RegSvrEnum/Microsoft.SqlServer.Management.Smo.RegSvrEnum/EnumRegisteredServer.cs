using System;
using System.Collections;
using System.Data;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

internal class EnumRegisteredServer : EnumRegServerBase
{
	internal EnumRegisteredServer()
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
			Name = "Description",
			Type = "System.String",
			ReadOnly = false,
			Usage = ObjectPropertyUsages.Request
		});
		AddProperty(new ObjectProperty
		{
			Name = "Login",
			Type = "System.String",
			ReadOnly = false,
			Usage = ObjectPropertyUsages.Request
		});
		AddProperty(new ObjectProperty
		{
			Name = "EncryptedPassword",
			Type = "System.String",
			ReadOnly = true,
			Usage = ObjectPropertyUsages.Request
		});
		AddProperty(new ObjectProperty
		{
			Name = "AuthenticationType",
			Type = "System.Int32",
			ReadOnly = false,
			Usage = ObjectPropertyUsages.Request
		});
		AddProperty(new ObjectProperty
		{
			Name = "ServerInstance",
			Type = "System.String",
			ReadOnly = false,
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
		ArrayList arrayList = BuildRegistrationInfoList(serverTypeRegistrationInfo, sPath, sName, typeof(ServerInstanceRegistrationInfo));
		if (sPath == null)
		{
			sPath = string.Empty;
		}
		DataTable schemaTable = GetSchemaTable();
		foreach (ServerInstanceRegistrationInfo item in arrayList)
		{
			AddRow(sPath, item, schemaTable);
		}
		return new EnumResult(schemaTable, ResultType.DataTable);
	}

	private void AddRow(string sPath, ServerInstanceRegistrationInfo regInst, DataTable dt)
	{
		DataRow dataRow = dt.NewRow();
		UIConnectionInfo connectionInfo = regInst.ConnectionInfo;
		IEnumerator enumerator = dt.Columns.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				switch (((DataColumn)enumerator.Current).Caption)
				{
				case "Name":
					dataRow["Name"] = regInst.FriendlyName;
					break;
				case "Description":
					dataRow["Description"] = regInst.Description;
					break;
				case "Login":
					dataRow["Login"] = connectionInfo.UserName;
					break;
				case "EncryptedPassword":
					dataRow["EncryptedPassword"] = connectionInfo.InMemoryPassword;
					break;
				case "AuthenticationType":
					dataRow["AuthenticationType"] = connectionInfo.AuthenticationType;
					break;
				case "ServerInstance":
					dataRow["ServerInstance"] = connectionInfo.ServerName;
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
