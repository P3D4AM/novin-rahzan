namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public abstract class ParentRegistrationInfo : RegistrationInfo
{
	private RegistrationInfoCollection children;

	public RegistrationInfoCollection Children => children;

	protected ParentRegistrationInfo()
	{
		children = new RegistrationInfoCollection();
	}

	internal bool HasChildWithName(string name)
	{
		return children.Contains(name);
	}

	internal string GenerateUniqueName(string oldName)
	{
		string text = oldName;
		int num = 1;
		while (children.Contains(text))
		{
			text = SRError.UniqueRegisteredNamePattern(num++, oldName);
		}
		return text;
	}

	internal void RemoveChild(RegistrationInfo reg)
	{
		Children.Remove(reg);
		if (reg.Parent == this)
		{
			reg.Parent = null;
		}
	}

	internal void AddChild(RegistrationInfo reg)
	{
		reg.Parent = this;
		Children.Add(reg);
	}
}
