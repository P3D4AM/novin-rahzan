using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Drawing;

[JsonObject]
public class StiDefaultBrush : StiBrush
{
	public override StiBrushIdent Ident => StiBrushIdent.Default;

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
		return "StiDefaultBrush".GetHashCode();
	}
}
