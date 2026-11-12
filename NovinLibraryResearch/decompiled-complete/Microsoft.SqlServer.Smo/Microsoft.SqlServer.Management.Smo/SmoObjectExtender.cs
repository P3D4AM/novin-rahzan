using System;
using System.Collections.Specialized;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
public class SmoObjectExtender<T> : SfcObjectExtender<T> where T : SqlSmoObject, new()
{
	public SmoObjectExtender()
	{
		Initialize();
	}

	public SmoObjectExtender(T obj)
		: base(obj)
	{
		Initialize();
	}

	private void Initialize()
	{
		try
		{
			RegisterParentProperty(typeof(T).GetProperty("Name"));
		}
		catch (Exception)
		{
		}
	}

	protected void PropagateAlterToChildren(StringCollection script, bool scriptParent = false)
	{
		ScriptingPreferences sp = new ScriptingPreferences(base.Parent);
		if (scriptParent)
		{
			base.Parent.ScriptAlter(script, sp);
		}
		base.Parent.PropagateScript(script, sp, SqlSmoObject.PropagateAction.Alter);
	}

	protected override ISfcPropertySet GetParentSfcPropertySet()
	{
		if (base.Parent != null)
		{
			try
			{
				base.Parent.CheckPendingState();
			}
			catch
			{
				return new SqlPropertyCollection(base.Parent, base.Parent.GetPropertyMetadataProvider());
			}
		}
		return base.GetParentSfcPropertySet();
	}
}
