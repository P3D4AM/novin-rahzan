using System.Collections;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal class SoapMethodKey : SimpleObjectKey
{
	private string methodNamespace;

	internal static StringCollection soapMethodFields;

	public string Namespace
	{
		get
		{
			return methodNamespace;
		}
		set
		{
			methodNamespace = value;
		}
	}

	public override string UrnFilter
	{
		get
		{
			if (methodNamespace != null && methodNamespace.Length > 0)
			{
				return string.Format(SmoApplication.DefaultCulture, "@Name='{0}' and @Namespace='{1}'", Urn.EscapeString(name), Urn.EscapeString(methodNamespace));
			}
			return string.Format(SmoApplication.DefaultCulture, "@Name='{0}'", Urn.EscapeString(name));
		}
	}

	public override bool IsNull
	{
		get
		{
			if (name != null)
			{
				return methodNamespace == null;
			}
			return true;
		}
	}

	public SoapMethodKey(string name, string methodNamespace)
		: base(name)
	{
		this.methodNamespace = methodNamespace;
	}

	static SoapMethodKey()
	{
		soapMethodFields = new StringCollection();
		soapMethodFields.Add("Name");
		soapMethodFields.Add("Namespace");
	}

	public override StringCollection GetFieldNames()
	{
		return SimpleObjectKey.fields;
	}

	public override string ToString()
	{
		if (methodNamespace != null && methodNamespace.Length > 0)
		{
			return string.Format(SmoApplication.DefaultCulture, "{0}.{1}", SqlSmoObject.MakeSqlBraket(name), SqlSmoObject.MakeSqlBraket(methodNamespace));
		}
		return SqlSmoObject.MakeSqlBraket(name);
	}

	public override string GetExceptionName()
	{
		if (methodNamespace != null && methodNamespace.Length > 0)
		{
			return string.Format(SmoApplication.DefaultCulture, "{0}.{1}", name, methodNamespace);
		}
		return name;
	}

	public override ObjectKeyBase Clone()
	{
		return new SoapMethodKey(base.Name, Namespace);
	}

	public override ObjectComparerBase GetComparer(IComparer stringComparer)
	{
		return new SoapMethodComparer(stringComparer);
	}
}
