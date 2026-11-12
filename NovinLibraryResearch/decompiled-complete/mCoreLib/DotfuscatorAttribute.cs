using System;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class DotfuscatorAttribute : Attribute
{
	private string a;

	public string A => a;

	public DotfuscatorAttribute(string a)
	{
		this.a = a;
	}
}
