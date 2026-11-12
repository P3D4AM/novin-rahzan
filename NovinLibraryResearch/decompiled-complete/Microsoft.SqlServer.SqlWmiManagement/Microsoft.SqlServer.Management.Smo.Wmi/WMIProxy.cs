using System;
using System.Data;
using System.Management;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class WMIProxy
{
	private Enumerator m_EnumProxy;

	private ManagementScope m_scope;

	internal WMIProxy(ManagementScope scope)
	{
		m_EnumProxy = new Enumerator();
		m_scope = scope;
	}

	internal DataSet ProcessRequest(Request req)
	{
		return (DataSet)ExecProcess(req);
	}

	private object ExecProcess(object request)
	{
		try
		{
			if (request is RequestObjectInfo)
			{
				return m_EnumProxy.Process(m_scope, (RequestObjectInfo)request);
			}
			return (DataSet)m_EnumProxy.Process(m_scope, (Request)request);
		}
		catch (Exception innerException)
		{
			throw new FailedOperationException(ExceptionTemplatesImpl.InnerWmiException, innerException).SetHelpContext("InnerWmiException");
		}
	}
}
