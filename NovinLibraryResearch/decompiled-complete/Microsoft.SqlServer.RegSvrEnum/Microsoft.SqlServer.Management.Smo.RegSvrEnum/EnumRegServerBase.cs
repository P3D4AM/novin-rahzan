using System;
using System.Collections;
using System.Data;
using System.Globalization;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

internal abstract class EnumRegServerBase : EnumObject
{
	public override ResultType[] ResultTypes => new ResultType[1] { ResultType.DataTable };

	protected ParentRegistrationInfo RootServerTypeRegistration => ((base.ConnectionInfo as RegSvrConnectionInfo) ?? throw new InternalEnumeratorException(SRError.ConnectionInfoTypeIncorrect)).RootServerType;

	public override Request RetrieveParentRequest()
	{
		return null;
	}

	public override void PostProcess(EnumResult erChildren)
	{
	}

	protected ArrayList BuildRegistrationInfoList(ServerTypeRegistrationInfo svrTypeReg, string sPath, string sName, Type type)
	{
		ArrayList arrayList = new ArrayList();
		ParentRegistrationInfo registrationInfoFromPath = GetRegistrationInfoFromPath(svrTypeReg, Urn.UnEscapeString(sPath));
		if (registrationInfoFromPath != null)
		{
			foreach (RegistrationInfo child in registrationInfoFromPath.Children)
			{
				if (child.GetType() == type && (sName == null || sName.Length <= 0 || child.FriendlyName == sName))
				{
					arrayList.Add(child);
				}
			}
		}
		return arrayList;
	}

	protected ParentRegistrationInfo GetRegistrationInfoFromPath(ServerTypeRegistrationInfo svrTypeReg, Urn path)
	{
		if (path == null || ((string)path).Length <= 0)
		{
			return svrTypeReg;
		}
		ParentRegistrationInfo parentRegistrationInfo = svrTypeReg;
		for (int i = 0; i < path.XPathExpression.Length; i++)
		{
			string attributeFromFilter = path.XPathExpression[i].GetAttributeFromFilter("Name");
			parentRegistrationInfo = parentRegistrationInfo.Children[attributeFromFilter] as ParentRegistrationInfo;
			if (parentRegistrationInfo == null)
			{
				throw new InternalEnumeratorException(SRError.PathFilterInvalid(attributeFromFilter));
			}
		}
		return parentRegistrationInfo;
	}

	protected void GetFilterOptions(out string sPath, out string sName)
	{
		sPath = GetFixedStringProperty("Path", removeEscape: true);
		sName = GetFixedStringProperty("Name", removeEscape: true);
	}

	protected DataTable GetSchemaTable()
	{
		DataTable dataTable = new DataTable();
		dataTable.Locale = CultureInfo.InvariantCulture;
		string[] fields = base.Request.Fields;
		foreach (string name in fields)
		{
			ObjectProperty property = GetProperty(name, ObjectPropertyUsages.Request);
			dataTable.Columns.Add(new DataColumn(property.Name, Type.GetType(property.Type)));
		}
		return dataTable;
	}
}
