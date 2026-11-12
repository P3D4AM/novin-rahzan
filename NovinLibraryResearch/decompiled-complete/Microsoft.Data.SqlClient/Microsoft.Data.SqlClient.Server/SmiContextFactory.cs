using System;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.Data.SqlClient.Server;

internal sealed class SmiContextFactory
{
	internal enum ContextKey
	{
		Connection,
		SqlContext
	}

	public static readonly SmiContextFactory Instance = new SmiContextFactory();

	private readonly SmiLink _smiLink;

	private readonly ulong _negotiatedSmiVersion;

	private readonly byte _majorVersion;

	private readonly byte _minorVersion;

	private readonly short _buildNum;

	private readonly string _serverVersion;

	private readonly SmiEventSink_Default _eventSinkForGetCurrentContext;

	internal const ulong YukonVersion = 100uL;

	internal const ulong KatmaiVersion = 210uL;

	internal const ulong LatestVersion = 210uL;

	private readonly ulong[] __supportedSmiVersions = new ulong[2] { 100uL, 210uL };

	internal ulong NegotiatedSmiVersion
	{
		get
		{
			if (_smiLink == null)
			{
				throw SQL.ContextUnavailableOutOfProc();
			}
			return _negotiatedSmiVersion;
		}
	}

	internal string ServerVersion
	{
		get
		{
			if (_smiLink == null)
			{
				throw SQL.ContextUnavailableOutOfProc();
			}
			return _serverVersion;
		}
	}

	private SmiContextFactory()
	{
		if (!InOutOfProcHelper.InProc)
		{
			return;
		}
		Type type = Type.GetType("Microsoft.SqlServer.Server.InProcLink, SqlAccess, PublicKeyToken=89845dcd8080cc91");
		if (null == type)
		{
			throw SQL.ContextUnavailableOutOfProc();
		}
		FieldInfo staticField = GetStaticField(type, "Instance");
		if (staticField != null)
		{
			_smiLink = (SmiLink)GetValue(staticField);
			FieldInfo staticField2 = GetStaticField(type, "BuildVersion");
			if (staticField2 != null)
			{
				uint num = (uint)GetValue(staticField2);
				_majorVersion = (byte)(num >> 24);
				_minorVersion = (byte)((num >> 16) & 0xFFu);
				_buildNum = (short)(num & 0xFFFF);
				_serverVersion = string.Format(null, "{0:00}.{1:00}.{2:0000}", _majorVersion, (short)_minorVersion, _buildNum);
			}
			else
			{
				_serverVersion = string.Empty;
			}
			_negotiatedSmiVersion = _smiLink.NegotiateVersion(210uL);
			bool flag = false;
			int num2 = 0;
			while (!flag && num2 < __supportedSmiVersions.Length)
			{
				if (__supportedSmiVersions[num2] == _negotiatedSmiVersion)
				{
					flag = true;
				}
				num2++;
			}
			if (!flag)
			{
				_smiLink = null;
			}
			_eventSinkForGetCurrentContext = new SmiEventSink_Default();
			return;
		}
		throw SQL.ContextUnavailableOutOfProc();
	}

	internal SmiContext GetCurrentContext()
	{
		if (_smiLink == null)
		{
			throw SQL.ContextUnavailableOutOfProc();
		}
		object currentContext = _smiLink.GetCurrentContext(_eventSinkForGetCurrentContext);
		_eventSinkForGetCurrentContext.ProcessMessagesAndThrow();
		if (currentContext == null)
		{
			throw SQL.ContextUnavailableWhileInProc();
		}
		return (SmiContext)currentContext;
	}

	[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
	private object GetValue(FieldInfo fieldInfo)
	{
		return fieldInfo.GetValue(null);
	}

	[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
	private FieldInfo GetStaticField(Type aType, string fieldName)
	{
		return aType.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
	}
}
