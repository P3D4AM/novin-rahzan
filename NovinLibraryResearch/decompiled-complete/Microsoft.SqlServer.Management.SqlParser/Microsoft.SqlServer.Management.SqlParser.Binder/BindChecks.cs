using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal static class BindChecks
{
	internal class MetadataObjectValidator
	{
		private readonly Dictionary<object, bool> VisitedObjects = new Dictionary<object, bool>();

		private readonly Dictionary<Type, List<MemberInfo>> TypeMembers = new Dictionary<Type, List<MemberInfo>>();

		private List<MemberInfo> GetTypeMembers(Type type)
		{
			return null;
		}

		private static object GetMemberInfoValue(MemberInfo memberInfo, object obj)
		{
			return null;
		}

		[Conditional("DEBUG")]
		private void ValidateProxyMetadataObject(IProxyMetadataObject obj)
		{
			if (obj is SchemaEx)
			{
				_ = ((SchemaEx)obj).IsEmptySchema;
			}
		}

		[Conditional("DEBUG")]
		public void ValidateMetadataObject(object obj)
		{
			if (obj is string)
			{
				return;
			}
			_ = obj is IProxyMetadataObject;
			if ((!(obj is IMutableMetadataObject) && !(obj is IEnumerable)) || VisitedObjects.ContainsKey(obj))
			{
				return;
			}
			VisitedObjects[obj] = true;
			if (obj is IEnumerable)
			{
				foreach (object item in (IEnumerable)obj)
				{
					_ = item is IMetadataObject;
				}
			}
			if (!(obj is IMetadataObject))
			{
				return;
			}
			foreach (MemberInfo typeMember in GetTypeMembers(obj.GetType()))
			{
				object memberInfoValue = GetMemberInfoValue(typeMember, obj);
				if (!(memberInfoValue is IMetadataObject))
				{
					_ = memberInfoValue is IEnumerable;
				}
			}
		}
	}

	[Conditional("DEBUG")]
	public static void ValidateMetadataObject(object obj)
	{
		new MetadataObjectValidator();
	}
}
