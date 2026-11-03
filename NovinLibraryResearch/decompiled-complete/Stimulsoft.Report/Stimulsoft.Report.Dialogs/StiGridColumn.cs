using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dialogs.Design;

namespace Stimulsoft.Report.Dialogs;

[TypeConverter(typeof(StiGridColumnConverter))]
[StiSerializable]
public class StiGridColumn : ICloneable, IStiJsonReportObject
{
	private string dataTextField;

	private bool visible = true;

	private int width;

	private HorizontalAlignment alignment;

	private string headerText = "";

	private string nullText = "";

	private StiGridColumnsCollection gridColumnCollection;

	[Editor("Stimulsoft.Report.Dialogs.Design.StiGridDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets name of data column to be shown.")]
	[StiOrder(240)]
	public string DataTextField
	{
		get
		{
			return dataTextField;
		}
		set
		{
			dataTextField = value;
		}
	}

	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets visiblity of column.")]
	[StiOrder(790)]
	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
		}
	}

	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets width of column.")]
	[StiOrder(800)]
	public int Width
	{
		get
		{
			return width;
		}
		set
		{
			width = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets horizontal alignment of column content.")]
	[StiOrder(120)]
	public HorizontalAlignment Alignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return alignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			alignment = value;
		}
	}

	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets header text of column.")]
	[StiOrder(410)]
	public string HeaderText
	{
		get
		{
			return headerText;
		}
		set
		{
			headerText = value;
		}
	}

	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets text which will be shown instead null values.")]
	[StiOrder(560)]
	public string NullText
	{
		get
		{
			return nullText;
		}
		set
		{
			nullText = value;
		}
	}

	[Browsable(false)]
	public StiGridColumnsCollection GridColumnCollection
	{
		get
		{
			return gridColumnCollection;
		}
		set
		{
			gridColumnCollection = value;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataTextField", DataTextField);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		JsonExtensions.AddPropertyInt(val, "Width", Width, 0);
		JsonExtensions.AddPropertyEnum(val, "Alignment", (Enum)(object)Alignment, (Enum)(object)(HorizontalAlignment)0);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "HeaderText", HeaderText);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "NullText", NullText);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "DataTextField":
				dataTextField = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Visible":
				visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Width":
				width = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Alignment":
				alignment = StiJsonReportObjectHelper.DeserializeEnum<HorizontalAlignment>(item);
				break;
			case "HeaderText":
				headerText = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "NullText":
				nullText = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		if (gridColumnCollection == null)
		{
			return "Column";
		}
		return "Column" + (gridColumnCollection.IndexOf(this) + 1);
	}

	public StiGridColumn()
		: this(string.Empty, visible: true, 0, (HorizontalAlignment)0, "", "")
	{
	}

	public StiGridColumn(string dataTextField, bool visible, int width, HorizontalAlignment alignment, string headerText, string nullText)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		this.dataTextField = dataTextField;
		this.visible = visible;
		this.width = width;
		this.alignment = alignment;
		this.headerText = headerText;
		this.nullText = nullText;
	}
}
