using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiGanttSeries : StiClusteredBarSeries, IStiGanttSeries, IStiClusteredBarSeries, IStiClusteredColumnSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyBrushNegative, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiShowZerosSeries, IStiRangeSeries
{
	private double?[] valuesEnd = new double?[3] { 3.0, 5.0, 9.0 };

	private string valueDataColumnEnd = string.Empty;

	private StiGetValueEndEvent getValueEndEvent = new StiGetValueEndEvent();

	private StiGetListOfValuesEndEvent getListOfValuesEndEvent = new StiGetListOfValuesEndEvent();

	private StiValueEndExpression valueObjEnd = new StiValueEndExpression();

	private StiListOfValuesEndExpression listOfValuesEnd = new StiListOfValuesEndExpression();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiGanttSeries;

	[Browsable(false)]
	[StiNonSerialized]
	public override IStiSeriesTopN TopN
	{
		get
		{
			return base.TopN;
		}
		set
		{
			base.TopN = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool TopNAllowed => false;

	[Browsable(false)]
	[StiNonSerialized]
	public override StiTrendLinesCollection TrendLines
	{
		get
		{
			return base.TrendLines;
		}
		set
		{
			base.TrendLines = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool TrendLineAllowed => false;

	[Browsable(false)]
	public override StiBrush BrushNegative
	{
		get
		{
			return base.BrushNegative;
		}
		set
		{
			base.BrushNegative = value;
		}
	}

	[Browsable(false)]
	public override bool AllowApplyBrushNegative
	{
		get
		{
			return base.AllowApplyBrushNegative;
		}
		set
		{
			base.AllowApplyBrushNegative = value;
		}
	}

	[Browsable(false)]
	public override bool ShowZeros
	{
		get
		{
			return base.ShowZeros;
		}
		set
		{
			base.ShowZeros = value;
		}
	}

	[Browsable(false)]
	public double?[] ValuesEnd
	{
		get
		{
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || !base.Chart.IsDesigning || base.IsDashboard)
			{
				return valuesEnd;
			}
			if (!string.IsNullOrEmpty(ListOfValuesEnd.Value))
			{
				return StiSeries.GetNullableValuesFromString(this, ListOfValuesEnd.Value);
			}
			int num = base.Chart.Series.IndexOf(this);
			if (num == 0)
			{
				return valuesEnd;
			}
			return new double?[3]
			{
				3 + num * 3,
				5 + num * 4,
				9 + num * 3
			};
		}
		set
		{
			valuesEnd = value;
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ValuesStringEnd
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			double?[] array = valuesEnd;
			for (int i = 0; i < array.Length; i++)
			{
				double value = array[i].Value;
				if (flag)
				{
					stringBuilder.AppendFormat("{0}", value);
				}
				else
				{
					stringBuilder.AppendFormat(";{0}", value);
				}
				flag = false;
			}
			return stringBuilder.ToString();
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				valuesEnd = new double?[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			valuesEnd = new double?[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				valuesEnd[num++] = double.Parse(s);
			}
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the start value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override string ValueDataColumn
	{
		get
		{
			return base.ValueDataColumn;
		}
		set
		{
			base.ValueDataColumn = value;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the end value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string ValueDataColumnEnd
	{
		get
		{
			return valueDataColumnEnd;
		}
		set
		{
			valueDataColumnEnd = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetValueEnd.")]
	public StiGetValueEndEvent GetValueEndEvent
	{
		get
		{
			return getValueEndEvent;
		}
		set
		{
			getValueEndEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetListOfValuesEnd.")]
	public StiGetListOfValuesEndEvent GetListOfValuesEndEvent
	{
		get
		{
			return getListOfValuesEndEvent;
		}
		set
		{
			getListOfValuesEndEvent = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[Description("Gets or sets start value expression. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiExpression Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			base.Value = value;
		}
	}

	[StiOrder(110)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets end value expression. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiValueEndExpression ValueEnd
	{
		get
		{
			return valueObjEnd;
		}
		set
		{
			valueObjEnd = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of start values.  Example: 1;2;3")]
	public override StiListOfValuesExpression ListOfValues
	{
		get
		{
			return base.ListOfValues;
		}
		set
		{
			base.ListOfValues = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of end values.  Example: 1;2;3")]
	public virtual StiListOfValuesEndExpression ListOfValuesEnd
	{
		get
		{
			return listOfValuesEnd;
		}
		set
		{
			listOfValuesEnd = value;
		}
	}

	public event StiGetValueEventHandler GetValueEnd;

	public event StiGetValueEventHandler GetListOfValuesEnd;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TopN");
		JsonExtensions.RemoveProperty(val, "TrendLine");
		JsonExtensions.AddPropertyString(val, "ValueDataColumnEnd", valueDataColumnEnd);
		JsonExtensions.AddPropertyJObject(val, "GetValueEndEvent", getValueEndEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfValuesEndEvent", getListOfValuesEndEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ValueEnd", ValueEnd.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfValuesEnd", ListOfValuesEnd.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ValueDataColumnEnd":
				valueDataColumnEnd = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "GetValueEndEvent":
				getValueEndEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetListOfValuesEndEvent":
				getListOfValuesEndEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ValueEnd":
				ValueEnd.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ListOfValuesEnd":
				ListOfValuesEnd.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumn(),
			propertiesHelper.Value(),
			propertiesHelper.ListOfValues()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumnEnd(),
			propertiesHelper.ValueEnd(),
			propertiesHelper.ListOfValuesEnd()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueEnd, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ArgumentDataColumn(),
			propertiesHelper.Argument(),
			propertiesHelper.ListOfArguments()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Argument, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Format(),
			propertiesHelper.SortBy(),
			propertiesHelper.SortDirection(),
			propertiesHelper.AutoSeriesKeyDataColumn(),
			propertiesHelper.AutoSeriesColorDataColumn(),
			propertiesHelper.AutoSeriesTitleDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis(),
			propertiesHelper.fWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		return base.Clone() as IStiGanttSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiGanttArea);
	}

	protected virtual void OnGetValueEnd(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValueEnd(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValueEnd(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesValueEnd**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(ValueEnd.Value, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetValueEnd != null)
			{
				this.GetValueEnd(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series InvokeGetValueEnd...ERROR");
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series " + ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "Series InvokeGetValueEnd...ERROR");
		}
	}

	protected virtual void OnGetListOfValuesEnd(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfValuesEnd(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfValuesEnd(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesListOfValuesEnd**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(listOfValuesEnd, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetListOfValuesEnd != null)
			{
				this.GetListOfValuesEnd(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "InvokeGetListOfValuesEnd...Warning");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "InvokeGetListOfValuesEnd...ERROR");
		}
	}

	public override StiSeries CreateNew()
	{
		return new StiGanttSeries();
	}

	public StiGanttSeries()
	{
		base.Core = new StiGanttSeriesCoreXF(this);
	}
}
