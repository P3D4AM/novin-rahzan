using System;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.Data.Common;

internal static class GreenMethods
{
	private const string ExtensionAssemblyRef = "System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

	private const string SystemDataCommonDbProviderServices_TypeName = "System.Data.Common.DbProviderServices, System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

	internal static Type SystemDataCommonDbProviderServices_Type = Type.GetType("System.Data.Common.DbProviderServices, System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", throwOnError: false);

	private const string MicrosoftDataSqlClientSqlProviderServices_TypeName = "Microsoft.Data.SqlClient.SQLProviderServices, System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

	private static FieldInfo MicrosoftDataSqlClientSqlProviderServices_Instance_FieldInfo;

	internal static object MicrosoftDataSqlClientSqlProviderServices_Instance()
	{
		if (null == MicrosoftDataSqlClientSqlProviderServices_Instance_FieldInfo)
		{
			Type type = Type.GetType("Microsoft.Data.SqlClient.SQLProviderServices, System.Data.Entity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", throwOnError: false);
			if (null != type)
			{
				MicrosoftDataSqlClientSqlProviderServices_Instance_FieldInfo = type.GetField("Instance", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			}
		}
		return MicrosoftDataSqlClientSqlProviderServices_Instance_GetValue();
	}

	[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
	private static object MicrosoftDataSqlClientSqlProviderServices_Instance_GetValue()
	{
		object result = null;
		if (null != MicrosoftDataSqlClientSqlProviderServices_Instance_FieldInfo)
		{
			result = MicrosoftDataSqlClientSqlProviderServices_Instance_FieldInfo.GetValue(null);
		}
		return result;
	}
}
