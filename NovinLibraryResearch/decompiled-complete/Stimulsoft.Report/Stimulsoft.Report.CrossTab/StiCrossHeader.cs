using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.CrossTab;

[StiGdiPainter(typeof(StiCrossHeaderGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiCrossHeaderWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public abstract class StiCrossHeader : StiCrossCell
{
	[StiSerializable]
	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a script of the event GetDisplayCrossValueEvent.")]
	public StiGetDisplayCrossValueEvent GetDisplayCrossValueEvent { get; set; } = new StiGetDisplayCrossValueEvent();


	[Browsable(false)]
	public override StiExpression Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	public override StiCrossValueExpression Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			if (base.Value.Value == DisplayValue.Value)
			{
				DisplayValue.Value = value.Value;
			}
			base.Value = value;
		}
	}

	[Description("Gets or sets the expression that is used for calculation of a cell value which will be output in the table.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual StiDisplayCrossValueExpression DisplayValue { get; set; } = new StiDisplayCrossValueExpression();


	[Browsable(false)]
	[StiNonSerialized]
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

	[StiNonSerialized]
	[Browsable(false)]
	public StiCrossTotal Total
	{
		get
		{
			if (TotalGuid == null || Parent == null)
			{
				return null;
			}
			foreach (StiComponent component in Parent.Components)
			{
				StiCrossTotal stiCrossTotal = component as StiCrossTotal;
				if (stiCrossTotal?.Guid == TotalGuid)
				{
					return stiCrossTotal;
				}
			}
			return null;
		}
		set
		{
			TotalGuid = value?.Guid;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public bool IsTotalVisible
	{
		get
		{
			if (ShowTotal && Total != null)
			{
				return Total.Enabled;
			}
			return false;
		}
	}

	[Browsable(false)]
	public int HeaderLevel { get; set; } = -1;


	[Browsable(false)]
	public string HeaderValue { get; set; }

	[StiSerializable]
	[Browsable(false)]
	public string TotalGuid { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value that indicates whether it is necessary to output totals or not.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(450)]
	public virtual bool ShowTotal { get; set; } = true;


	[DefaultValue(StiSortDirection.Asc)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the sorting direction.")]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiSortDirection SortDirection { get; set; }

	[Description("Gets or sets the type of sorting.")]
	[DefaultValue(StiSortType.ByDisplayValue)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiSortType SortType { get; set; } = StiSortType.ByDisplayValue;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(370)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the component is printed on all pages.")]
	public virtual bool PrintOnAllPages { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiDataTopN TopN { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public string MeterKey { get; set; }

	public event StiGetCrossValueEventHandler GetDisplayCrossValue;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "HideZeros");
		JsonExtensions.AddPropertyJObject(val, "GetDisplayCrossValueEvent", GetDisplayCrossValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DisplayValue", DisplayValue.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TotalGuid", TotalGuid);
		JsonExtensions.AddPropertyBool(val, "ShowTotal", ShowTotal, true, false);
		JsonExtensions.AddPropertyEnum(val, "SortDirection", (Enum)SortDirection, (Enum)StiSortDirection.Asc);
		JsonExtensions.AddPropertyEnum(val, "SortType", (Enum)SortType, (Enum)StiSortType.ByDisplayValue);
		JsonExtensions.AddPropertyBool(val, "PrintOnAllPages", PrintOnAllPages, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "GetDisplayCrossValueEvent":
			{
				StiGetDisplayCrossValueEvent stiGetDisplayCrossValueEvent = new StiGetDisplayCrossValueEvent();
				stiGetDisplayCrossValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetDisplayCrossValueEvent = stiGetDisplayCrossValueEvent;
				break;
			}
			case "DisplayValue":
			{
				StiDisplayCrossValueExpression stiDisplayCrossValueExpression = new StiDisplayCrossValueExpression();
				stiDisplayCrossValueExpression.LoadFromJsonObject((JObject)item.Value);
				DisplayValue = stiDisplayCrossValueExpression;
				break;
			}
			case "TotalGuid":
				TotalGuid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ShowTotal":
				ShowTotal = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SortDirection":
				SortDirection = StiJsonReportObjectHelper.DeserializeEnum<StiSortDirection>(item);
				break;
			case "SortType":
				SortType = StiJsonReportObjectHelper.DeserializeEnum<StiSortType>(item);
				break;
			case "PrintOnAllPages":
				PrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	protected virtual void OnGetDisplayCrossValue(StiGetCrossValueEventArgs e)
	{
	}

	public void InvokeGetDisplayCrossValue(StiGetCrossValueEventArgs e)
	{
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				OnGetDisplayCrossValue(e);
				this.GetDisplayCrossValue?.Invoke(this, e);
				return;
			}
			OnGetDisplayCrossValue(e);
			object obj = StiParser.ParseTextValue(DisplayValue, this);
			if (obj != null)
			{
				e.Value = obj;
			}
			this.GetDisplayCrossValue?.Invoke(this, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "Do GetDisplayCrossValue ... ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
		}
	}

	private bool ShouldSerializeTopN()
	{
		if (TopN != null)
		{
			return !TopN.IsDefault;
		}
		return true;
	}

	public StiCrossHeader()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		base.Brush = (StiBrush)new StiSolidBrush(Color.LightGray);
	}
}
