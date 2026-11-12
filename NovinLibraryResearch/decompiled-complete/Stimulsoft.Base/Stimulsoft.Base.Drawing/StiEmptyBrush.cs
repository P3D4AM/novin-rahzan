using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Drawing;

[JsonObject]
public class StiEmptyBrush : StiBrush
{
	public override StiBrushIdent Ident => StiBrushIdent.Empty;

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
		return "StiEmptyBrush".GetHashCode();
	}
}
