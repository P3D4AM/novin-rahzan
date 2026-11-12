using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public class UIConnectionGroupInfo : List<UIConnectionInfo>
{
	private string name = string.Empty;

	public string Name
	{
		get
		{
			string displayName = name;
			if (string.IsNullOrEmpty(name) && base.Count == 1)
			{
				displayName = base[0].DisplayName;
			}
			return displayName;
		}
		set
		{
			name = ((value != null) ? value : string.Empty);
		}
	}

	public UIConnectionGroupInfo()
	{
	}

	public UIConnectionGroupInfo(IEnumerable<UIConnectionInfo> collection)
		: base(collection)
	{
	}

	public UIConnectionGroupInfo(int capacity)
		: base(capacity)
	{
	}

	public UIConnectionGroupInfo(UIConnectionGroupInfo other)
		: base((IEnumerable<UIConnectionInfo>)other)
	{
		name = other.name;
	}

	public UIConnectionGroupInfo(UIConnectionInfo connectionInfo)
		: base(1)
	{
		if (connectionInfo != null)
		{
			Add(connectionInfo);
			name = connectionInfo.DisplayName;
		}
	}

	public UIConnectionGroupInfo ShallowCopy()
	{
		return new UIConnectionGroupInfo(this);
	}

	public UIConnectionGroupInfo DeepCopy(bool withNewConnectionIds)
	{
		UIConnectionGroupInfo uIConnectionGroupInfo = new UIConnectionGroupInfo(base.Count);
		uIConnectionGroupInfo.Name = Name;
		using Enumerator enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			UIConnectionInfo item = new UIConnectionInfo(enumerator.Current, withNewConnectionIds);
			uIConnectionGroupInfo.Add(item);
		}
		return uIConnectionGroupInfo;
	}
}
