using System;
using System.Collections;
using System.Data;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

internal class EnumServerType : EnumRegServerBase
{
	internal EnumServerType()
	{
		AddProperty(new ObjectProperty
		{
			Name = "Name",
			Type = "System.String",
			ReadOnly = false,
			Usage = ObjectPropertyUsages.Request
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
			Usage = (ObjectPropertyUsages.Filter | ObjectPropertyUsages.Request)
		});
	}

	public override EnumResult GetData(EnumResult erParent)
	{
		string fixedStringProperty = GetFixedStringProperty("ServerType", removeEscape: true);
		if (base.Request == null)
		{
			if (fixedStringProperty == null)
			{
				throw new InternalEnumeratorException(SRError.ServerTypeNotSpecified);
			}
			Guid guid;
			try
			{
				guid = new Guid(fixedStringProperty);
			}
			catch (FormatException innerException)
			{
				throw new InternalEnumeratorException(SRError.ServerTypeMustBeGuid, innerException);
			}
			ParentRegistrationInfo rootServerTypeRegistration = base.RootServerTypeRegistration;
			ServerTypeRegistrationInfo serverTypeRegistrationInfo = null;
			foreach (RegistrationInfo child in rootServerTypeRegistration.Children)
			{
				serverTypeRegistrationInfo = child as ServerTypeRegistrationInfo;
				if (serverTypeRegistrationInfo != null && serverTypeRegistrationInfo.ServerType.CompareTo(guid) == 0)
				{
					break;
				}
			}
			if (serverTypeRegistrationInfo == null)
			{
				Guid sqlServerTypeGuid = RegSvrConnectionInfo.SqlServerTypeGuid;
				if (sqlServerTypeGuid.CompareTo(guid) != 0)
				{
					throw new InternalEnumeratorException(SRError.ServerTypeFilterInvalid);
				}
				serverTypeRegistrationInfo = ((base.ConnectionInfo as RegSvrConnectionInfo) ?? throw new InternalEnumeratorException(SRError.ConnectionInfoTypeIncorrect)).AddServerTypeNode(new ServerTypeRegistrationInfo
				{
					ServerType = guid
				}) as ServerTypeRegistrationInfo;
			}
			return new EnumResult(serverTypeRegistrationInfo, ResultType.Reserved1);
		}
		DataTable schemaTable = GetSchemaTable();
		foreach (RegistrationInfo child2 in base.RootServerTypeRegistration.Children)
		{
			ServerTypeRegistrationInfo serverTypeRegistrationInfo2 = child2 as ServerTypeRegistrationInfo;
			bool flag;
			try
			{
				flag = fixedStringProperty == null || serverTypeRegistrationInfo2.ServerType.CompareTo(new Guid(fixedStringProperty)) == 0;
			}
			catch (FormatException innerException2)
			{
				throw new InternalEnumeratorException(SRError.ServerTypeMustBeGuid, innerException2);
			}
			if (serverTypeRegistrationInfo2 != null && flag)
			{
				AddRow(serverTypeRegistrationInfo2, schemaTable);
			}
		}
		return new EnumResult(schemaTable, ResultType.DataTable);
	}

	private void AddRow(ServerTypeRegistrationInfo svrTypeReg, DataTable dt)
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
					dataRow["ServerType"] = svrTypeReg.ServerType;
					break;
				case "Name":
					dataRow["Name"] = svrTypeReg.FriendlyName;
					break;
				case "Description":
					dataRow["Description"] = svrTypeReg.Description;
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
