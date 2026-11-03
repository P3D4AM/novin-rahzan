using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
public abstract class StiDynamicBand : StiBand, IStiPageBreak, IStiPrintAtBottom
{
	private bool printAtBottom;

	private bool skipFirst = true;

	private float breakIfLessThan = 100f;

	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(320)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer is printed at bottom of page.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintAtBottom
	{
		get
		{
			return printAtBottom;
		}
		set
		{
			if (printAtBottom != value)
			{
				CheckBlockedException("PrintAtBottom");
				printAtBottom = value;
			}
		}
	}

	[Browsable(true)]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
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

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("If the value of this property is true, then, before output of a band, a new page will be generated. Output of a band will be continued on the next page.")]
	[StiOrder(100)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool NewPageBefore { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("If the value of this property is true, then, after output of a band, a new page will be generated.")]
	[StiOrder(110)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool NewPageAfter { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("If the value of this property is true, then, before output of a band, a new column will be generated. Output of a band will be continued on the next column.")]
	[StiOrder(120)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool NewColumnBefore { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("If the value of this property is true, then, after output of a band, a new column will be generated.")]
	[StiOrder(130)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool NewColumnAfter { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("If the value of this property is true, then, a new page/column will be generated only starting from the second case.")]
	[StiOrder(150)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool SkipFirst
	{
		get
		{
			return skipFirst;
		}
		set
		{
			if (skipFirst != value)
			{
				CheckBlockedException("SkipFirst");
				skipFirst = value;
			}
		}
	}

	[DefaultValue(100f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[StiOrder(140)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates how much free space is on a page (in per cent) should be reserved for formation of a new page or a new column. The value should be set in the range from 0 to 100. If the value is 100 then, in any case, a new page or a new column will be formed. This property is used together with NewPageBefore, NewPageAfter, NewColumnBefore, NewColumnAfter properties.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float BreakIfLessThan
	{
		get
		{
			return breakIfLessThan;
		}
		set
		{
			if (breakIfLessThan != value)
			{
				if (value < 0f || value > 100f)
				{
					throw new ArgumentOutOfRangeException("BreakIfLessThan", $"Value of '{value}' is not valid for 'BreakIfLessThan'. 'BreakIfLessThan' should be between 0% and 100%.");
				}
				if (value >= 0f && value <= 100f)
				{
					breakIfLessThan = value;
				}
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "PrintAtBottom", PrintAtBottom, false, false);
		JsonExtensions.AddPropertyBool(val, "NewPageBefore", NewPageBefore, false, false);
		JsonExtensions.AddPropertyBool(val, "NewPageAfter", NewPageAfter, false, false);
		JsonExtensions.AddPropertyBool(val, "NewColumnBefore", NewColumnBefore, false, false);
		JsonExtensions.AddPropertyBool(val, "NewColumnAfter", NewColumnAfter, false, false);
		JsonExtensions.AddPropertyBool(val, "SkipFirst", SkipFirst, true, false);
		JsonExtensions.AddPropertyFloat(val, "BreakIfLessThan", BreakIfLessThan, 100f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "PrintAtBottom":
				printAtBottom = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "NewPageBefore":
				NewPageBefore = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "NewPageAfter":
				NewPageAfter = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "NewColumnBefore":
				NewColumnBefore = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "NewColumnAfter":
				NewColumnAfter = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SkipFirst":
				skipFirst = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BreakIfLessThan":
				breakIfLessThan = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public override bool Break(StiComponent dividedComponent, double devideFactor, ref double divLine)
	{
		return false;
	}

	public StiDynamicBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiDynamicBand(RectangleD rect)
		: base(rect)
	{
	}//IL_0013: Unknown result type (might be due to invalid IL or missing references)

}
