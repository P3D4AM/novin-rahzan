using System.Drawing;
using System.Drawing.Text;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public class StiStringFormatGeom : StiGeom
{
	public bool IsGeneric { get; set; }

	public StringAlignment Alignment { get; set; }

	public StringFormatFlags FormatFlags { get; set; }

	public HotkeyPrefix HotkeyPrefix { get; set; }

	public StringAlignment LineAlignment { get; set; }

	public StringTrimming Trimming { get; set; }

	public override StiGeomType Type => StiGeomType.StringFormat;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("IsGeneric", (object)IsGeneric));
		StringAlignment val2 = Alignment;
		((JContainer)val).Add((object)new JProperty("Alignment", (object)((object)(StringAlignment)(ref val2)).ToString()));
		StringFormatFlags formatFlags = FormatFlags;
		((JContainer)val).Add((object)new JProperty("FormatFlags", (object)((object)(StringFormatFlags)(ref formatFlags)).ToString()));
		HotkeyPrefix hotkeyPrefix = HotkeyPrefix;
		((JContainer)val).Add((object)new JProperty("HotkeyPrefix", (object)((object)(HotkeyPrefix)(ref hotkeyPrefix)).ToString()));
		val2 = LineAlignment;
		((JContainer)val).Add((object)new JProperty("LineAlignment", (object)((object)(StringAlignment)(ref val2)).ToString()));
		StringTrimming trimming = Trimming;
		((JContainer)val).Add((object)new JProperty("Trimming", (object)((object)(StringTrimming)(ref trimming)).ToString()));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiStringFormatGeom(StringFormat sf)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Alignment = sf.Alignment;
		FormatFlags = sf.FormatFlags;
		HotkeyPrefix = sf.HotkeyPrefix;
		LineAlignment = sf.LineAlignment;
		Trimming = sf.Trimming;
	}
}
