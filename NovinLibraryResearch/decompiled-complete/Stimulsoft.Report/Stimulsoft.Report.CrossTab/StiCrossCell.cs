using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

public abstract class StiCrossCell : StiCrossField
{
	[StiSerializable]
	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a script of the event GetValueEvent.")]
	public StiGetCrossValueEvent GetCrossValueEvent { get; set; } = new StiGetCrossValueEvent();


	[Description("Gets or sets the expression that is used for calculation of a cell value.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual StiCrossValueExpression Value { get; set; } = new StiCrossValueExpression();


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

	public event StiGetCrossValueEventHandler GetCrossValue;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "GetCrossValueEvent", GetCrossValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Value", Value.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "GetCrossValueEvent"))
			{
				if (text == "Value")
				{
					StiCrossValueExpression stiCrossValueExpression = new StiCrossValueExpression();
					stiCrossValueExpression.LoadFromJsonObject((JObject)item.Value);
					Value = stiCrossValueExpression;
				}
			}
			else
			{
				StiGetCrossValueEvent stiGetCrossValueEvent = new StiGetCrossValueEvent();
				stiGetCrossValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetCrossValueEvent = stiGetCrossValueEvent;
			}
		}
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.DataEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetCrossValueEvent,
			StiPropertyEventId.ProcessCellEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetExcelValueEvent,
			StiPropertyEventId.GetValueEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetTagEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetBookmarkEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.BeforePrintEvent,
			StiPropertyEventId.AfterPrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[5]
		{
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		return stiEventCollection;
	}

	public override void Paint(StiPaintEventArgs e)
	{
		if (Parent is StiCrossTab)
		{
			SetTextInternal(CellText);
		}
		base.Paint(e);
	}

	protected virtual void OnGetCrossValue(StiGetCrossValueEventArgs e)
	{
	}

	public void InvokeGetCrossValue(StiGetCrossValueEventArgs e)
	{
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				OnGetCrossValue(e);
				this.GetCrossValue?.Invoke(this, e);
				return;
			}
			OnGetCrossValue(e);
			object obj = StiParser.ParseTextValue(Value, this);
			if (obj != null)
			{
				e.Value = obj;
			}
			this.GetCrossValue?.Invoke(this, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "Do GetCrossValue ... ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
		}
	}
}
