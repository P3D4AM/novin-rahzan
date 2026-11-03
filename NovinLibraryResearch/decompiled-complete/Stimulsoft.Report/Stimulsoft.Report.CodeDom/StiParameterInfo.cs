using System;

namespace Stimulsoft.Report.CodeDom;

public class StiParameterInfo
{
	public Type Type { get; set; }

	public string Name { get; set; }

	public StiParameterInfo(Type type, string name)
	{
		Type = type;
		Name = name;
	}
}
