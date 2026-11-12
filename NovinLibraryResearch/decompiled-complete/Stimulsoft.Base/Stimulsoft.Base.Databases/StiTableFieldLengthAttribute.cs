using System;

namespace Stimulsoft.Base.Databases;

[AttributeUsage(AttributeTargets.Property)]
public class StiTableFieldLengthAttribute : Attribute
{
	public int Length { get; }

	public StiTableFieldLengthAttribute(int length)
	{
		Length = length;
	}
}
