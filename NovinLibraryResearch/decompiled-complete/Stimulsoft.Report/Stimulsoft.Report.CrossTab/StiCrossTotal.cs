using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossTotal : StiCrossField
{
	[StiNonSerialized]
	[Browsable(false)]
	public override StiFormatService TextFormat
	{
		get
		{
			return base.TextFormat;
		}
		set
		{
			base.TextFormat = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool HideZeros
	{
		get
		{
			return base.HideZeros;
		}
		set
		{
			base.HideZeros = value;
		}
	}

	[Browsable(true)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	public override string CellText => Text;

	public override StiComponentId ComponentId => StiComponentId.StiCrossTotal;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TextFormat");
		JsonExtensions.RemoveProperty(val, "HideZeros");
		return val;
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossTotal();
	}

	public StiCrossTotal()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		base.Brush = (StiBrush)new StiSolidBrush(Color.WhiteSmoke);
	}
}
