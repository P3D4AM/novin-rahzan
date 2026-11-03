using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
[StiDesigner("Stimulsoft.Report.Components.Design.StiPointPrimitiveDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfPointPrimitiveDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiPointPrimitiveGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiPointPrimitiveWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiPointPrimitiveV1Builder))]
[StiV2Builder(typeof(StiPointPrimitiveV2Builder))]
public abstract class StiPointPrimitive : StiPrimitive, IStiHideFromReportTree, IStiComponentGuidReference
{
	[StiNonSerialized]
	public override bool IsSelected
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiSerializable]
	[Browsable(false)]
	public string ReferenceToGuid { get; set; }

	public override int ToolboxPosition => 0;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Shapes;

	public override string LocalizedName => "";

	public override double Width
	{
		get
		{
			return 0.0;
		}
		set
		{
		}
	}

	public override double Height
	{
		get
		{
			return 0.0;
		}
		set
		{
		}
	}

	[Browsable(false)]
	internal int StoredColumn { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "IsSelected");
		JsonExtensions.AddPropertyString(val, "ReferenceToGuid", ReferenceToGuid);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "ReferenceToGuid")
			{
				ReferenceToGuid = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public StiPointPrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiPointPrimitive(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
