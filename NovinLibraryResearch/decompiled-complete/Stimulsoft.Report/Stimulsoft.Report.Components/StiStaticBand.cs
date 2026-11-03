using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
public abstract class StiStaticBand : StiBand
{
	[StiNonSerialized]
	[Browsable(false)]
	public override bool CanBreak
	{
		get
		{
			return base.CanBreak;
		}
		set
		{
			base.CanBreak = value;
		}
	}

	public override StiComponentType ComponentType => StiComponentType.Static;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanBreak");
		return val;
	}

	public override bool Break(StiComponent dividedComponent, double devideFactor, ref double divLine)
	{
		return false;
	}

	public StiStaticBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiStaticBand(RectangleD rect)
		: base(rect)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)

}
