using System;
using System.Drawing;

namespace Stimulsoft.Base;

public struct StiFont : IEquatable<StiFont>
{
	public string Name { get; set; }

	public float Size { get; set; }

	public FontStyle Style { get; set; }

	public bool Equals(StiFont other)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (string.Equals(Name, other.Name) && Size == other.Size)
		{
			return Style == other.Style;
		}
		return false;
	}

	public StiFont(Font font)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Name = font.Name;
		Size = font.Size;
		Style = font.Style;
	}
}
