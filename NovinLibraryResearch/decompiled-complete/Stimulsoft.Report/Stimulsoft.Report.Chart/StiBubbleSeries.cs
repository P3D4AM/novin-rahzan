using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiBubbleSeries : StiScatterSeries, IStiFontIconsSeries, IStiBubbleSeries, IStiScatterSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries, IStiSeriesBorderThickness
{
	private Color borderColor = Color.Gray;

	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	private double[] weights = new double[3] { 1.0, 3.0, 2.0 };

	private string weightDataColumn = string.Empty;

	private StiGetValueEvent getWeightEvent = new StiGetValueEvent();

	private StiGetListOfWeightsEvent getListOfWeightsEvent = new StiGetListOfWeightsEvent();

	private StiExpression weight = new StiExpression();

	private StiListOfWeightsExpression listOfWeights = new StiListOfWeightsExpression();

	public override StiComponentId ComponentId => StiComponentId.StiBubbleSeries;

	[Browsable(false)]
	[StiNonSerialized]
	public override IStiLineMarker LineMarker
	{
		get
		{
			return base.LineMarker;
		}
		set
		{
			base.LineMarker = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override IStiMarker Marker
	{
		get
		{
			return base.Marker;
		}
		set
		{
			base.Marker = value;
		}
	}

	[Browsable(false)]
	public override Color LineColor
	{
		get
		{
			return base.LineColor;
		}
		set
		{
			base.LineColor = value;
		}
	}

	[Browsable(false)]
	public override Color LineColorNegative
	{
		get
		{
			return base.LineColorNegative;
		}
		set
		{
			base.LineColorNegative = value;
		}
	}

	[Browsable(false)]
	public override bool AllowApplyColorNegative
	{
		get
		{
			return base.AllowApplyColorNegative;
		}
		set
		{
			base.AllowApplyColorNegative = value;
		}
	}

	[StiSerializable]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border color of series bar.")]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			borderColor = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border thickness of series bar.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int BorderThickness { get; set; } = 1;


	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will used to fill bar area.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			brush = value;
		}
	}

	[Browsable(false)]
	public double[] Weights
	{
		get
		{
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || !base.Chart.IsDesigning || base.IsDashboard)
			{
				return weights;
			}
			if (!string.IsNullOrEmpty(ListOfWeights.Value))
			{
				return StiSeries.GetValuesFromString(ListOfWeights.Value);
			}
			int num = base.Chart.Series.IndexOf(this);
			if (num == 0)
			{
				return weights;
			}
			return new double[3]
			{
				3 + num * 3,
				5 + num * 4,
				9 + num * 3
			};
		}
		set
		{
			weights = value;
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string WeightsString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			double[] array = weights;
			foreach (double num in array)
			{
				if (flag)
				{
					stringBuilder.AppendFormat("{0}", num);
				}
				else
				{
					stringBuilder.AppendFormat(";{0}", num);
				}
				flag = false;
			}
			return stringBuilder.ToString();
		}
		set
		{
			if (value == null || value.Trim().Length == 0)
			{
				weights = new double[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			weights = new double[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				weights[num++] = double.Parse(s);
			}
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the weight value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string WeightDataColumn
	{
		get
		{
			return weightDataColumn;
		}
		set
		{
			weightDataColumn = value;
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFontIcons? Icon { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetWeightEvent.")]
	public StiGetValueEvent GetWeightEvent
	{
		get
		{
			return getWeightEvent;
		}
		set
		{
			getWeightEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetListOfWeightsEvent.")]
	public StiGetListOfWeightsEvent GetListOfWeightsEvent
	{
		get
		{
			return getListOfWeightsEvent;
		}
		set
		{
			getListOfWeightsEvent = value;
		}
	}

	[StiOrder(110)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets expression for weight calculating. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExpression Weight
	{
		get
		{
			return weight;
		}
		set
		{
			weight = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of weights.  Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfWeightsExpression ListOfWeights
	{
		get
		{
			return listOfWeights;
		}
		set
		{
			listOfWeights = value;
		}
	}

	public event StiGetValueEventHandler GetWeight;

	public event StiGetValueEventHandler GetListOfWeights;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "LineMarker");
		JsonExtensions.RemoveProperty(val, "Marker");
		JsonExtensions.AddPropertyColor(val, "BorderColor", borderColor, Color.Gray);
		JsonExtensions.AddPropertyInt(val, "BorderThickness", BorderThickness, 1);
		JsonExtensions.AddPropertyBrush(val, "Brush", brush);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "WeightDataColumn", weightDataColumn);
		JsonExtensions.AddPropertyJObject(val, "GetWeightEvent", getWeightEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfWeightsEvent", getListOfWeightsEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Weight", Weight.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfWeights", ListOfWeights.SaveToJsonObject(mode));
		if (Icon.HasValue)
		{
			JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "BorderColor":
				borderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderThickness":
				BorderThickness = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Brush":
				brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "WeightDataColumn":
				weightDataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "GetWeightEvent":
				getWeightEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetListOfWeightsEvent":
				getListOfWeightsEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Weight":
				Weight.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ListOfWeights":
				ListOfWeights.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiFontIcons>(item);
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
			propertiesHelper.ArgumentDataColumn(),
			propertiesHelper.Argument(),
			propertiesHelper.ListOfArguments()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Argument, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.WeightDataColumn(),
			propertiesHelper.Weight(),
			propertiesHelper.ListOfWeights()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Weight, value);
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
		value = new StiPropertyObject[4]
		{
			propertiesHelper.LabelsOffset(),
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowNulls(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		return base.Clone() as IStiBubbleSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiBubbleArea);
	}

	protected virtual void OnGetWeight(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetWeight(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetWeight(e);
			if (this.GetWeight != null)
			{
				this.GetWeight(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(((StiChart)base.Chart).Report, sender, GetWeightEvent, e);
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series InvokeGetWeight...ERROR");
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series " + ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "Series InvokeGetWeight...ERROR");
		}
	}

	protected virtual void OnGetListOfWeights(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfWeights(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfWeights(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText component = new StiText
				{
					Name = "**ChartSeriesListOfWeights**",
					Page = sender.Report.Pages[0]
				};
				object obj = StiParser.ParseTextValue(ListOfWeights, component);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetListOfWeights != null)
			{
				this.GetListOfWeights(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "InvokeGetListOfWeights...Warning");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "InvokeGetListOfWeights...ERROR");
		}
	}

	public override StiSeries CreateNew()
	{
		return new StiBubbleSeries();
	}

	public StiBubbleSeries()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		base.Core = new StiBubbleSeriesCoreXF(this);
	}
}
