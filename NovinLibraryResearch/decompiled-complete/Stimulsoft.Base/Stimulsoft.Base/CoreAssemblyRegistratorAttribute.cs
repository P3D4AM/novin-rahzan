using System;

namespace Stimulsoft.Base;

[AttributeUsage(AttributeTargets.Assembly)]
public class CoreAssemblyRegistratorAttribute : Attribute
{
	public virtual void Register()
	{
	}
}
