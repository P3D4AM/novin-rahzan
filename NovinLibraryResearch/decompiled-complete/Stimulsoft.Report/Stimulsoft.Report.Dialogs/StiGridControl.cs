using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiGridControl), "Stimulsoft.Report.Dialogs.Bmp.StiGridControl.gif")]
[StiGdiPainter(typeof(StiGridControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiGridControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGuiMode(/*Could not decode attribute arguments.*/)]
public class StiGridControl : StiReportControl, IStiDataSource, IStiEnumerator
{
	private Color alternatingBackColor = SystemColors.Window;

	private Color backgroundColor = SystemColors.AppWorkspace;

	private Color gridLineColor = SystemColors.Control;

	private Color headerBackColor = SystemColors.Control;

	private Color headerForeColor = SystemColors.ControlText;

	private Color selectionBackColor = SystemColors.ActiveCaption;

	private Color selectionForeColor = SystemColors.ActiveCaptionText;

	private string filter = "";

	private Font headerFont = new Font("Microsoft Sans Serif", 8f);

	private bool columnHeadersVisible = true;

	private bool rowHeadersVisible = true;

	private DataGridLineStyle gridLineStyle = (DataGridLineStyle)1;

	private int preferredColumnWidth = 75;

	private int preferredRowHeight = 16;

	private int rowHeaderWidth = 35;

	private StiGridColumnsCollection columns;

	internal bool isEofValue;

	internal bool isBofValue;

	private string dataSourceName = "";

	private static readonly object EventPositionChanged = new object();

	public override StiComponentId ComponentId => StiComponentId.StiGridControl;

	public override int ToolboxPosition => 14;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiGridControl");

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the foreground color (typically the color of the text) property.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(340)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override Color ForeColor
	{
		get
		{
			return base.ForeColor;
		}
		set
		{
			base.ForeColor = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the background color of the grid.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override Color BackColor
	{
		get
		{
			return base.BackColor;
		}
		set
		{
			base.BackColor = value;
		}
	}

	[Browsable(false)]
	public virtual int Position
	{
		get
		{
			if (Control == null)
			{
				return -1;
			}
			Form val = ((Control)Control).FindForm();
			if (val == null)
			{
				return -1;
			}
			BindingManagerBase obj = ((Control)val).BindingContext[Control.DataSource, Control.DataMember];
			CurrencyManager val2 = (CurrencyManager)(object)((obj is CurrencyManager) ? obj : null);
			if (val2 == null)
			{
				return -1;
			}
			return ((BindingManagerBase)val2).Position;
		}
		set
		{
			if (Control == null)
			{
				return;
			}
			Form val = ((Control)Control).FindForm();
			if (val != null)
			{
				BindingManagerBase obj = ((Control)val).BindingContext[Control.DataSource, Control.DataMember];
				CurrencyManager val2 = (CurrencyManager)(object)((obj is CurrencyManager) ? obj : null);
				if (val2 != null)
				{
					((BindingManagerBase)val2).Position = value;
				}
			}
		}
	}

	[Browsable(false)]
	public virtual int Count
	{
		get
		{
			if (Control == null)
			{
				return -1;
			}
			Form val = ((Control)Control).FindForm();
			if (val == null)
			{
				return -1;
			}
			BindingManagerBase obj = ((Control)val).BindingContext[Control.DataSource, Control.DataMember];
			CurrencyManager val2 = (CurrencyManager)(object)((obj is CurrencyManager) ? obj : null);
			if (val2 == null)
			{
				return -1;
			}
			return ((BindingManagerBase)val2).Count;
		}
	}

	[Browsable(false)]
	public virtual DataRow Current
	{
		get
		{
			if (Control == null)
			{
				return null;
			}
			Form val = ((Control)Control).FindForm();
			if (val == null)
			{
				return null;
			}
			BindingManagerBase obj = ((Control)val).BindingContext[Control.DataSource, Control.DataMember];
			CurrencyManager val2 = (CurrencyManager)(object)((obj is CurrencyManager) ? obj : null);
			if (val2 == null)
			{
				return null;
			}
			DataRowView dataRowView = ((BindingManagerBase)val2).Current as DataRowView;
			return dataRowView.Row;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the background color of alternating rows for a ledger appearance.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color AlternatingBackColor
	{
		get
		{
			return alternatingBackColor;
		}
		set
		{
			alternatingBackColor = value;
			UpdateReportControl("AlternatingBackColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the color of the non-row area of the grid.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color BackgroundColor
	{
		get
		{
			return backgroundColor;
		}
		set
		{
			backgroundColor = value;
			UpdateReportControl("BackgroundColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the color of the grid lines.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(360)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color GridLineColor
	{
		get
		{
			return gridLineColor;
		}
		set
		{
			gridLineColor = value;
			UpdateReportControl("GridLineColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the background color of all row and column headers.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(380)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color HeaderBackColor
	{
		get
		{
			return headerBackColor;
		}
		set
		{
			headerBackColor = value;
			UpdateReportControl("HeaderBackColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the foreground color of headers.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(400)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color HeaderForeColor
	{
		get
		{
			return headerForeColor;
		}
		set
		{
			headerForeColor = value;
			UpdateReportControl("HeaderForeColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the background color of selected rows.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(630)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color SelectionBackColor
	{
		get
		{
			return selectionBackColor;
		}
		set
		{
			selectionBackColor = value;
			UpdateReportControl("SelectionBackColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or set the foreground color of selected rows.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(640)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color SelectionForeColor
	{
		get
		{
			return selectionForeColor;
		}
		set
		{
			selectionForeColor = value;
			UpdateReportControl("SelectionForeColor");
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets filter string.")]
	[StiOrder(320)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string Filter
	{
		get
		{
			return filter;
		}
		set
		{
			filter = value;
			UpdateReportControl("Filter");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the font used for column headers.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(390)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Font HeaderFont
	{
		get
		{
			return headerFont;
		}
		set
		{
			headerFont = value;
			UpdateReportControl("HeaderFont");
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether the column headers a table are visible.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ColumnHeadersVisible
	{
		get
		{
			return columnHeadersVisible;
		}
		set
		{
			columnHeadersVisible = value;
			UpdateReportControl("ColumnHeadersVisible");
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value that specifies whether row headers are visible.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(610)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool RowHeadersVisible
	{
		get
		{
			return rowHeadersVisible;
		}
		set
		{
			rowHeadersVisible = value;
			UpdateReportControl("RowHeadersVisible");
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the line style of the grid.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(370)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual DataGridLineStyle GridLineStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return gridLineStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			gridLineStyle = value;
			UpdateReportControl("GridLineStyle");
		}
	}

	[StiSerializable]
	[DefaultValue(75)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the default width of the grid columns in pixels.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(580)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int PreferredColumnWidth
	{
		get
		{
			return preferredColumnWidth;
		}
		set
		{
			preferredColumnWidth = value;
			UpdateReportControl("PreferredColumnWidth");
		}
	}

	[StiSerializable]
	[DefaultValue(16)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the preferred row height.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(590)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int PreferredRowHeight
	{
		get
		{
			return preferredRowHeight;
		}
		set
		{
			preferredRowHeight = value;
			UpdateReportControl("PreferredRowHeight");
		}
	}

	[StiSerializable]
	[DefaultValue(35)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the width of row headers.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(620)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int RowHeaderWidth
	{
		get
		{
			return rowHeaderWidth;
		}
		set
		{
			rowHeaderWidth = value;
			UpdateReportControl("RowHeaderWidth");
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Dialogs.Design.StiGridColumnsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the column collection.")]
	[StiOrder(210)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiGridColumnsCollection Columns
	{
		get
		{
			return columns;
		}
		set
		{
			columns = value;
		}
	}

	[Browsable(false)]
	public DataGridView Control { get; set; }

	[Browsable(false)]
	public virtual bool IsEof
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.IsEof;
			}
			return true;
		}
		set
		{
			if (DataSource != null)
			{
				DataSource.IsEof = value;
			}
			else
			{
				isEofValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsBof
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.IsBof;
			}
			return false;
		}
		set
		{
			if (DataSource != null)
			{
				DataSource.IsBof = value;
			}
			else
			{
				isBofValue = value;
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsEmpty
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.IsEmpty;
			}
			return false;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataSourceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiDataSourceConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets data source.")]
	[StiOrder(235)]
	public virtual StiDataSource DataSource
	{
		get
		{
			if (Page == null || Report == null || Report.DataSources == null || DataSourceName == null || DataSourceName.Length == 0)
			{
				return null;
			}
			return Report.DataSources[DataSourceName];
		}
	}

	[Browsable(false)]
	[StiSerializable]
	public string DataSourceName
	{
		get
		{
			return dataSourceName;
		}
		set
		{
			if (dataSourceName != value)
			{
				dataSourceName = value;
				StiOptions.Engine.GlobalEvents.InvokeDataSourceAssigned(this, EventArgs.Empty);
			}
		}
	}

	[Browsable(false)]
	public bool IsDataSourceEmpty
	{
		get
		{
			if (!string.IsNullOrEmpty(DataSourceName))
			{
				return DataSource == null;
			}
			return true;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event PositionChanged.")]
	public virtual StiPositionChangedEvent PositionChangedEvent
	{
		get
		{
			return new StiPositionChangedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public override RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.RightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.RightToLeft = value;
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 192.0, 192.0);

	public event EventHandler PositionChanged
	{
		add
		{
			base.Events.AddHandler(EventPositionChanged, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventPositionChanged, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "AlternatingBackColor", AlternatingBackColor, SystemColors.Window);
		JsonExtensions.AddPropertyColor(val, "BackgroundColor", BackgroundColor, SystemColors.AppWorkspace);
		JsonExtensions.AddPropertyColor(val, "GridLineColor", GridLineColor, SystemColors.Control);
		JsonExtensions.AddPropertyColor(val, "HeaderBackColor", HeaderBackColor, SystemColors.Control);
		JsonExtensions.AddPropertyColor(val, "HeaderForeColor", HeaderForeColor, SystemColors.ControlText);
		JsonExtensions.AddPropertyColor(val, "SelectionBackColor", SelectionBackColor, SystemColors.ActiveCaption);
		JsonExtensions.AddPropertyColor(val, "SelectionForeColor", SelectionForeColor, SystemColors.ActiveCaptionText);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Filter", Filter);
		JsonExtensions.AddPropertyBool(val, "ColumnHeadersVisible", ColumnHeadersVisible, true, false);
		JsonExtensions.AddPropertyBool(val, "RowHeadersVisible", RowHeadersVisible, true, false);
		JsonExtensions.AddPropertyEnum(val, "GridLineStyle", (Enum)(object)GridLineStyle, (Enum)(object)(DataGridLineStyle)1);
		JsonExtensions.AddPropertyInt(val, "PreferredColumnWidth", PreferredColumnWidth, 75);
		JsonExtensions.AddPropertyInt(val, "PreferredRowHeight", PreferredRowHeight, 16);
		JsonExtensions.AddPropertyInt(val, "RowHeaderWidth", RowHeaderWidth, 35);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataSourceName", DataSourceName);
		JsonExtensions.AddPropertyJObject(val, "PositionChangedEvent", PositionChangedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Columns", Columns.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AlternatingBackColor":
				alternatingBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BackgroundColor":
				backgroundColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "GridLineColor":
				gridLineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HeaderBackColor":
				headerBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HeaderForeColor":
				headerForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectionBackColor":
				selectionBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectionForeColor":
				selectionForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Filter":
				filter = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ColumnHeadersVisible":
				columnHeadersVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RowHeadersVisible":
				rowHeadersVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "GridLineStyle":
				gridLineStyle = StiJsonReportObjectHelper.DeserializeEnum<DataGridLineStyle>(item);
				break;
			case "PreferredColumnWidth":
				preferredColumnWidth = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "PreferredRowHeight":
				preferredRowHeight = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "RowHeaderWidth":
				rowHeaderWidth = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "DataSourceName":
				dataSourceName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "PositionChangedEvent":
			{
				StiPositionChangedEvent stiPositionChangedEvent = new StiPositionChangedEvent();
				stiPositionChangedEvent.LoadFromJsonObject((JObject)item.Value);
				PositionChangedEvent = stiPositionChangedEvent;
				break;
			}
			case "Columns":
				columns.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override List<StiFont> GetFonts()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(HeaderFont));
		return fonts.Distinct().ToList();
	}

	private bool ShouldSerializeAlternatingBackColor()
	{
		return alternatingBackColor != SystemColors.Window;
	}

	private bool ShouldSerializeBackgroundColor()
	{
		return backgroundColor != SystemColors.AppWorkspace;
	}

	private bool ShouldSerializeGridLineColor()
	{
		return gridLineColor != SystemColors.Control;
	}

	private bool ShouldSerializeHeaderBackColor()
	{
		return headerBackColor != SystemColors.Control;
	}

	private bool ShouldSerializeHeaderForeColor()
	{
		return headerForeColor != SystemColors.ControlText;
	}

	private bool ShouldSerializeSelectionBackColor()
	{
		return selectionBackColor != SystemColors.ActiveCaption;
	}

	private bool ShouldSerializeSelectionForeColor()
	{
		return selectionForeColor != SystemColors.ActiveCaptionText;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != SystemColors.Control;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != SystemColors.ControlText;
	}

	public virtual void MoveNext()
	{
		Next();
	}

	public virtual void First()
	{
		if (DataSource != null)
		{
			DataSource.First();
		}
	}

	public virtual void Prior()
	{
		if (DataSource != null)
		{
			DataSource.Prior();
		}
	}

	public virtual void Next()
	{
		if (DataSource != null)
		{
			DataSource.Next();
		}
	}

	public virtual void Last()
	{
		if (DataSource != null)
		{
			DataSource.Last();
		}
	}

	public void InvokePositionChanged(EventArgs e)
	{
		if (base.Events[EventPositionChanged] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, PositionChangedEvent);
	}

	public override StiEvent GetDefaultEvent()
	{
		return MouseDownEvent;
	}

	public override StiComponent CreateNew()
	{
		return new StiGridControl();
	}

	public StiGridControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiGridControl(RectangleD rect)
		: base(rect)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		columns = new StiGridColumnsCollection(this);
		PlaceOnToolbox = true;
	}
}
