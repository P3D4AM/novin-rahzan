using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[RefreshProperties(RefreshProperties.All)]
[JsonObject]
public class StiSolidBrush : StiBrush
{
	private static int defaultHashCode = "StiSolidBrush".GetHashCode();

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the color of this StiSolidBrush object.")]
	[RefreshProperties(RefreshProperties.All)]
	public Color Color { get; set; }

	public override StiBrushIdent Ident => StiBrushIdent.Solid;

	private bool ShouldSerializeColor()
	{
		return Color != Color.Transparent;
	}

	protected bool Equals(StiSolidBrush other)
	{
		return Color.Equals((object?)other.Color);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj.GetType() == GetType())
				{
					return Equals((StiSolidBrush)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (defaultHashCode * 397) ^ Color.GetHashCode();
	}

	public void LoadValuesFromJson(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Color")
			{
				Color = item.DeserializeColor();
			}
		}
	}

	public StiSolidBrush()
	{
		Color = Color.Transparent;
	}

	public StiSolidBrush(Color color)
	{
		Color = color;
	}

	public StiSolidBrush(string color)
	{
		Color = StiColor.Get(color);
	}
}
