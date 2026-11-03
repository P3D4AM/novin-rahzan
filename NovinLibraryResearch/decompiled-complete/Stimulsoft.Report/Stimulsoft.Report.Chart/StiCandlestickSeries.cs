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
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiCandlestickSeries : StiSeries, IStiCandlestickSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiFinancialSeries
{
	private double?[] valuesOpen = new double?[3] { 4.0, 7.0, 5.0 };

	private double?[] valuesClose = new double?[3] { 2.0, 3.0, 5.0 };

	private double?[] valuesHigh = new double?[3] { 6.0, 9.0, 7.0 };

	private double?[] valuesLow = new double?[3] { 1.0, 3.0, 4.0 };

	private string valueDataColumnOpen = string.Empty;

	private string valueDataColumnClose = string.Empty;

	private string valueDataColumnHigh = string.Empty;

	private string valueDataColumnLow = string.Empty;

	private Color borderColor = Color.Gray;

	private Color borderColorNegative = Color.Gray;

	private float borderWidth = 2f;

	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	private StiBrush brushNegative = (StiBrush)new StiSolidBrush(Color.Transparent);

	private StiGetValueOpenEvent getValueOpenEvent = new StiGetValueOpenEvent();

	private StiGetListOfValuesOpenEvent getListOfValuesOpenEvent = new StiGetListOfValuesOpenEvent();

	private StiGetValueCloseEvent getValueCloseEvent = new StiGetValueCloseEvent();

	private StiGetListOfValuesCloseEvent getListOfValuesCloseEvent = new StiGetListOfValuesCloseEvent();

	private StiGetValueHighEvent getValueHighEvent = new StiGetValueHighEvent();

	private StiGetListOfValuesHighEvent getListOfValuesHighEvent = new StiGetListOfValuesHighEvent();

	private StiGetValueLowEvent getValueLowEvent = new StiGetValueLowEvent();

	private StiGetListOfValuesLowEvent getListOfValuesLowEvent = new StiGetListOfValuesLowEvent();

	private StiValueOpenExpression valueObjOpen = new StiValueOpenExpression();

	private StiListOfValuesOpenExpression listOfValuesOpen = new StiListOfValuesOpenExpression();

	private StiValueCloseExpression valueObjClose = new StiValueCloseExpression();

	private StiListOfValuesCloseExpression listOfValuesClose = new StiListOfValuesCloseExpression();

	private StiValueHighExpression valueObjHigh = new StiValueHighExpression();

	private StiListOfValuesHighExpression listOfValuesHigh = new StiListOfValuesHighExpression();

	private StiValueLowExpression valueObjLow = new StiValueLowExpression();

	private StiListOfValuesLowExpression listOfValuesLow = new StiListOfValuesLowExpression();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCandlestickSeries;

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
	[StiNonSerialized]
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

	[Browsable(false)]
	[StiNonSerialized]
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

	[Browsable(false)]
	[StiNonSerialized]
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

	[Browsable(false)]
	public double?[] ValuesOpen
	{
		get
		{
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || !base.Chart.IsDesigning || base.IsDashboard)
			{
				return valuesOpen;
			}
			if (!string.IsNullOrEmpty(ListOfValuesOpen.Value))
			{
				return StiSeries.GetNullableValuesFromString(this, ListOfValuesOpen.Value);
			}
			int num = base.Chart.Series.IndexOf(this);
			double offsetForValues = GetOffsetForValues();
			return new double?[3]
			{
				offsetForValues + 4.0 + (double)(num * 7),
				offsetForValues + 7.0 + (double)(num * 7),
				offsetForValues + 5.0 + (double)(num * 7)
			};
		}
		set
		{
			valuesOpen = value;
		}
	}

	[Browsable(false)]
	public double?[] ValuesClose
	{
		get
		{
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || !base.Chart.IsDesigning || base.IsDashboard)
			{
				return valuesClose;
			}
			if (!string.IsNullOrEmpty(ListOfValuesClose.Value))
			{
				return StiSeries.GetNullableValuesFromString(this, ListOfValuesClose.Value);
			}
			int num = base.Chart.Series.IndexOf(this);
			double offsetForValues = GetOffsetForValues();
			return new double?[3]
			{
				offsetForValues + 2.0 + (double)(num * 7),
				offsetForValues + 3.0 + (double)(num * 7),
				offsetForValues + 5.0 + (double)(num * 7)
			};
		}
		set
		{
			valuesClose = value;
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ValuesStringOpen
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			double?[] array = valuesOpen;
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
				valuesOpen = new double?[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			valuesOpen = new double?[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				valuesOpen[num++] = double.Parse(s);
			}
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ValuesStringClose
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			double?[] array = valuesClose;
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
				valuesClose = new double?[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			valuesClose = new double?[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				valuesClose[num++] = double.Parse(s);
			}
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ValuesStringHigh
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			double?[] array = valuesHigh;
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
				valuesHigh = new double?[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			valuesHigh = new double?[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				valuesHigh[num++] = double.Parse(s);
			}
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ValuesStringLow
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			double?[] array = valuesLow;
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
				valuesLow = new double?[0];
				return;
			}
			string[] array = value.Split(new char[1] { ';' });
			valuesLow = new double?[array.Length];
			int num = 0;
			string[] array2 = array;
			foreach (string s in array2)
			{
				valuesLow[num++] = double.Parse(s);
			}
		}
	}

	[Browsable(false)]
	public double?[] ValuesHigh
	{
		get
		{
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || !base.Chart.IsDesigning || base.IsDashboard)
			{
				return valuesHigh;
			}
			if (!string.IsNullOrEmpty(ListOfValuesHigh.Value))
			{
				return StiSeries.GetNullableValuesFromString(this, ListOfValuesHigh.Value);
			}
			int num = base.Chart.Series.IndexOf(this);
			double offsetForValues = GetOffsetForValues();
			return new double?[3]
			{
				offsetForValues + 6.0 + (double)(num * 7),
				offsetForValues + 9.0 + (double)(num * 7),
				offsetForValues + 7.0 + (double)(num * 7)
			};
		}
		set
		{
			valuesHigh = value;
		}
	}

	[Browsable(false)]
	public double?[] ValuesLow
	{
		get
		{
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || !base.Chart.IsDesigning || base.IsDashboard)
			{
				return valuesLow;
			}
			if (!string.IsNullOrEmpty(ListOfValuesLow.Value))
			{
				return StiSeries.GetNullableValuesFromString(this, ListOfValuesLow.Value);
			}
			int num = base.Chart.Series.IndexOf(this);
			double offsetForValues = GetOffsetForValues();
			return new double?[3]
			{
				offsetForValues + 1.0 + (double)(num * 7),
				offsetForValues + 3.0 + (double)(num * 7),
				offsetForValues + 4.0 + (double)(num * 7)
			};
		}
		set
		{
			valuesLow = value;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(1)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the open value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string ValueDataColumnOpen
	{
		get
		{
			return valueDataColumnOpen;
		}
		set
		{
			valueDataColumnOpen = value;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the close value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string ValueDataColumnClose
	{
		get
		{
			return valueDataColumnClose;
		}
		set
		{
			valueDataColumnClose = value;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the high value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string ValueDataColumnHigh
	{
		get
		{
			return valueDataColumnHigh;
		}
		set
		{
			valueDataColumnHigh = value;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a name of the column that contains the low value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string ValueDataColumnLow
	{
		get
		{
			return valueDataColumnLow;
		}
		set
		{
			valueDataColumnLow = value;
		}
	}

	[StiSerializable]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border color of series bar.")]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color BorderColor
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
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border color of series bar.")]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color BorderColorNegative
	{
		get
		{
			return borderColorNegative;
		}
		set
		{
			borderColorNegative = value;
		}
	}

	[DefaultValue(2f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border width of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float BorderWidth
	{
		get
		{
			return borderWidth;
		}
		set
		{
			if (value > 0f)
			{
				borderWidth = value;
			}
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will used to fill bar area.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiBrush Brush
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

	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a brush which will be used to fill negative values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiBrush BrushNegative
	{
		get
		{
			return brushNegative;
		}
		set
		{
			brushNegative = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetValueOpen.")]
	public StiGetValueOpenEvent GetValueOpenEvent
	{
		get
		{
			return getValueOpenEvent;
		}
		set
		{
			getValueOpenEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetListOfValuesOpen.")]
	public StiGetListOfValuesOpenEvent GetListOfValuesOpenEvent
	{
		get
		{
			return getListOfValuesOpenEvent;
		}
		set
		{
			getListOfValuesOpenEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetValueClose.")]
	public StiGetValueCloseEvent GetValueCloseEvent
	{
		get
		{
			return getValueCloseEvent;
		}
		set
		{
			getValueCloseEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetListOfValuesClose.")]
	public StiGetListOfValuesCloseEvent GetListOfValuesCloseEvent
	{
		get
		{
			return getListOfValuesCloseEvent;
		}
		set
		{
			getListOfValuesCloseEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetValueHigh.")]
	public StiGetValueHighEvent GetValueHighEvent
	{
		get
		{
			return getValueHighEvent;
		}
		set
		{
			getValueHighEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetListOfValuesHigh.")]
	public StiGetListOfValuesHighEvent GetListOfValuesHighEvent
	{
		get
		{
			return getListOfValuesHighEvent;
		}
		set
		{
			getListOfValuesHighEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetValueLow.")]
	public StiGetValueLowEvent GetValueLowEvent
	{
		get
		{
			return getValueLowEvent;
		}
		set
		{
			getValueLowEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetListOfValuesLow.")]
	public StiGetListOfValuesLowEvent GetListOfValuesLowEvent
	{
		get
		{
			return getListOfValuesLowEvent;
		}
		set
		{
			getListOfValuesLowEvent = value;
		}
	}

	[StiOrder(2)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets open value expression. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiValueOpenExpression ValueOpen
	{
		get
		{
			return valueObjOpen;
		}
		set
		{
			valueObjOpen = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(3)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of open values.  Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfValuesOpenExpression ListOfValuesOpen
	{
		get
		{
			return listOfValuesOpen;
		}
		set
		{
			listOfValuesOpen = value;
		}
	}

	[StiOrder(110)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets close value expression. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiValueCloseExpression ValueClose
	{
		get
		{
			return valueObjClose;
		}
		set
		{
			valueObjClose = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of close values.  Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfValuesCloseExpression ListOfValuesClose
	{
		get
		{
			return listOfValuesClose;
		}
		set
		{
			listOfValuesClose = value;
		}
	}

	[StiOrder(110)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets high value expression. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiValueHighExpression ValueHigh
	{
		get
		{
			return valueObjHigh;
		}
		set
		{
			valueObjHigh = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of high values.  Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfValuesHighExpression ListOfValuesHigh
	{
		get
		{
			return listOfValuesHigh;
		}
		set
		{
			listOfValuesHigh = value;
		}
	}

	[StiOrder(110)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets low value expression. Example: {Order.Value}")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiValueLowExpression ValueLow
	{
		get
		{
			return valueObjLow;
		}
		set
		{
			valueObjLow = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of low values.  Example: 1;2;3")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiListOfValuesLowExpression ListOfValuesLow
	{
		get
		{
			return listOfValuesLow;
		}
		set
		{
			listOfValuesLow = value;
		}
	}

	public event StiGetValueEventHandler GetValueOpen;

	public event StiGetValueEventHandler GetListOfValuesOpen;

	public event StiGetValueEventHandler GetValueClose;

	public event StiGetValueEventHandler GetListOfValuesClose;

	public event StiGetValueEventHandler GetValueHigh;

	public event StiGetValueEventHandler GetListOfValuesHigh;

	public event StiGetValueEventHandler GetValueLow;

	public event StiGetValueEventHandler GetListOfValuesLow;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Invalid comparison between Unknown and I4
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TopN");
		JsonExtensions.RemoveProperty(val, "TrendLine");
		JsonExtensions.RemoveProperty(val, "ValueDataColumn");
		JsonExtensions.RemoveProperty(val, "Value");
		JsonExtensions.RemoveProperty(val, "ListOfValues");
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueDataColumnOpen", valueDataColumnOpen);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueDataColumnClose", valueDataColumnClose);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueDataColumnHigh", valueDataColumnHigh);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueDataColumnLow", valueDataColumnLow);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Gray);
		JsonExtensions.AddPropertyColor(val, "BorderColorNegative", BorderColorNegative, Color.Gray);
		JsonExtensions.AddPropertyFloat(val, "BorderWidth", BorderWidth, 2f);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBrush(val, "BrushNegative", BrushNegative);
		JsonExtensions.AddPropertyJObject(val, "GetValueOpenEvent", getValueOpenEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfValuesOpenEvent", getListOfValuesOpenEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetValueCloseEvent", getValueCloseEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfValuesCloseEvent", getListOfValuesCloseEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetValueHighEvent", getValueHighEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfValuesHighEvent", getListOfValuesHighEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetValueLowEvent", getValueLowEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetListOfValuesLowEvent", getListOfValuesLowEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ValueOpen", ValueOpen.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfValuesOpen", ListOfValuesOpen.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ValueClose", ValueClose.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfValuesClose", ListOfValuesClose.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ValueHigh", ValueHigh.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfValuesHigh", ListOfValuesHigh.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ValueLow", ValueLow.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ListOfValuesLow", ListOfValuesLow.SaveToJsonObject(mode));
		if ((int)mode == 1)
		{
			JsonExtensions.RemoveProperty(val, "ValuesString");
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValuesStringClose", ValuesStringClose);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValuesStringHigh", ValuesStringHigh);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValuesStringLow", ValuesStringLow);
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValuesStringOpen", ValuesStringOpen);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Expected O, but got Unknown
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Expected O, but got Unknown
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Expected O, but got Unknown
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Expected O, but got Unknown
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Expected O, but got Unknown
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ValuesStringClose":
				ValuesStringClose = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValuesStringHigh":
				ValuesStringHigh = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValuesStringLow":
				ValuesStringLow = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValuesStringOpen":
				ValuesStringOpen = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValueDataColumnOpen":
				valueDataColumnOpen = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValueDataColumnClose":
				valueDataColumnClose = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValueDataColumnHigh":
				valueDataColumnHigh = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ValueDataColumnLow":
				valueDataColumnLow = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderColorNegative":
				BorderColorNegative = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderWidth":
				BorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BrushNegative":
				BrushNegative = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "GetValueOpenEvent":
				getValueOpenEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetListOfValuesOpenEvent":
				getListOfValuesOpenEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetValueCloseEvent":
				getValueCloseEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetListOfValuesCloseEvent":
				getListOfValuesCloseEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetValueHighEvent":
				getValueHighEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetListOfValuesHighEvent":
				getListOfValuesHighEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetValueLowEvent":
				getValueLowEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetListOfValuesLowEvent":
				getListOfValuesLowEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ValueOpen":
				ValueOpen.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ListOfValuesOpen":
				ListOfValuesOpen.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ValueClose":
				ValueClose.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ListOfValuesClose":
				ListOfValuesClose.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ValueHigh":
				ValueHigh.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ListOfValuesHigh":
				ListOfValuesHigh.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ValueLow":
				ValueLow.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ListOfValuesLow":
				ListOfValuesLow.LoadFromJsonObject((JObject)item.Value);
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
			propertiesHelper.ValueDataColumnOpen(),
			propertiesHelper.ValueOpen(),
			propertiesHelper.ListOfValuesOpen()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueOpen, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumnClose(),
			propertiesHelper.ValueClose(),
			propertiesHelper.ListOfValuesClose()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueClose, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumnHigh(),
			propertiesHelper.ValueHigh(),
			propertiesHelper.ListOfValuesHigh()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueHigh, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumnLow(),
			propertiesHelper.ValueLow(),
			propertiesHelper.ListOfValuesLow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueLow, value);
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
		value = new StiPropertyObject[5]
		{
			propertiesHelper.BorderWidth(),
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.BrushNegative(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		return base.Clone() as IStiCandlestickSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiCandlestickArea);
	}

	protected virtual void OnGetValueOpen(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValueOpen(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValueOpen(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesValueOpen**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(ValueOpen.Value, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetValueOpen != null)
			{
				this.GetValueOpen(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series InvokeGetValueOpen...ERROR");
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series " + ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "Series InvokeGetValueOpen...ERROR");
		}
	}

	protected virtual void OnGetListOfValuesOpen(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfValuesOpen(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfValuesOpen(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesListOfValuesOpen**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(listOfValuesOpen, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetListOfValuesOpen != null)
			{
				this.GetListOfValuesOpen(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "InvokeGetListOfValuesOpen...Warning");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "InvokeGetListOfValuesOpen...ERROR");
		}
	}

	protected virtual void OnGetValueClose(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValueClose(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValueClose(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesValueClose**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(ValueClose.Value, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetValueClose != null)
			{
				this.GetValueClose(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series InvokeGetValueClose...ERROR");
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series " + ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "Series InvokeGetValueClose...ERROR");
		}
	}

	protected virtual void OnGetListOfValuesClose(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfValuesClose(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfValuesClose(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesListOfValuesClose**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(listOfValuesClose, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetListOfValuesClose != null)
			{
				this.GetListOfValuesClose(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "InvokeGetListOfValuesClose...Warning");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "InvokeGetListOfValuesClose...ERROR");
		}
	}

	protected virtual void OnGetValueHigh(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValueHigh(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValueHigh(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesValueHigh**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(ValueHigh.Value, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetValueHigh != null)
			{
				this.GetValueHigh(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series InvokeGetValueHigh...ERROR");
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series " + ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "Series InvokeGetValueHigh...ERROR");
		}
	}

	protected virtual void OnGetListOfValuesHigh(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfValuesHigh(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfValuesHigh(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesListOfValuesHigh**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(listOfValuesHigh, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetListOfValuesHigh != null)
			{
				this.GetListOfValuesHigh(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "InvokeGetListOfValuesHigh...Warning");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "InvokeGetListOfValuesHigh...ERROR");
		}
	}

	protected virtual void OnGetValueLow(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValueLow(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValueLow(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesValueLow**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(ValueLow.Value, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetValueLow != null)
			{
				this.GetValueLow(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series InvokeGetValueLow...ERROR");
			StiLogService.Write(((object)this).GetType(), ((StiChart)base.Chart).Name + "Series " + ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "Series InvokeGetValueLow...ERROR");
		}
	}

	protected virtual void OnGetListOfValuesLow(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetListOfValuesLow(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetListOfValuesLow(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartGanttSeriesListOfValuesLow**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(listOfValuesLow, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetListOfValuesLow != null)
			{
				this.GetListOfValuesLow(sender, e);
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "InvokeGetListOfValuesLow...Warning");
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(((StiChart)base.Chart).Name + "InvokeGetListOfValuesLow...ERROR");
		}
	}

	public override StiSeries CreateNew()
	{
		return new StiCandlestickSeries();
	}

	public StiCandlestickSeries()
	{
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		base.Core = new StiCandlestickSeriesCoreXF(this);
	}
}
