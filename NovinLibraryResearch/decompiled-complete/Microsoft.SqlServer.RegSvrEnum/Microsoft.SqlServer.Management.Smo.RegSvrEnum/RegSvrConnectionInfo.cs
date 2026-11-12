using System;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public sealed class RegSvrConnectionInfo
{
	public static readonly Guid SqlServerTypeGuid = new Guid("8c91a03d-f9b4-46c0-a305-b5dcc79ff907");

	private ParentRegistrationInfo m_RootServerType;

	public ParentRegistrationInfo RootServerType => m_RootServerType;

	public RegSvrConnectionInfo()
	{
		m_RootServerType = RegistrationProvider.RootNode;
	}

	public ParentRegistrationInfo GetRegistrationInfoFromPath(Urn path)
	{
		ServerTypeRegistrationInfo serverTypeRegistrationInfo = null;
		foreach (RegistrationInfo child in m_RootServerType.Children)
		{
			if (child is ServerTypeRegistrationInfo { ServerType: var serverType } serverTypeRegistrationInfo2 && serverType.CompareTo(SqlServerTypeGuid) == 0)
			{
				serverTypeRegistrationInfo = serverTypeRegistrationInfo2;
				break;
			}
		}
		if (serverTypeRegistrationInfo == null)
		{
			throw new InternalEnumeratorException(SRError.SqlServerTypeNotFound(SqlServerTypeGuid.ToString()));
		}
		if (path == null || ((string)path).Length <= 0)
		{
			return serverTypeRegistrationInfo;
		}
		ParentRegistrationInfo parentRegistrationInfo = serverTypeRegistrationInfo;
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

	public RegistrationInfo AddServerTypeNode(ServerTypeRegistrationInfo node)
	{
		return RegistrationProvider.AddServerTypeNode(node);
	}

	public void AddNode(RegistrationInfo reg, ParentRegistrationInfo parent)
	{
		RegistrationProvider.AddNode(reg, parent, RegistrationAddBehavior.CreateCopy);
	}

	public void Save(RegistrationInfo node)
	{
		RegistrationProvider.MarkNodeModified(node);
	}

	public void RemoveNode(RegistrationInfo reg, ParentRegistrationInfo parent)
	{
		RegistrationProvider.RemoveNode(reg, parent);
	}
}
