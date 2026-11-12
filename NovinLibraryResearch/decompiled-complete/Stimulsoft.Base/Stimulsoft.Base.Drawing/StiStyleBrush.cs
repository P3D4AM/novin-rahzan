using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Drawing;

[JsonObject]
public class StiStyleBrush : StiBrush
{
	public override StiBrushIdent Ident => StiBrushIdent.Style;

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				return obj.GetType() == GetType();
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return "StiStyleBrush".GetHashCode();
	}
}
