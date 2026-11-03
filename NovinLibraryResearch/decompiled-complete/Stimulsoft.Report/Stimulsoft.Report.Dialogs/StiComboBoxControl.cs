using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiComboBoxControl), "Stimulsoft.Report.Dialogs.Bmp.StiComboBoxControl.gif")]
[StiGdiPainter(typeof(StiComboBoxControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiComboBoxControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiComboBoxControl : StiReportControl
{
	private static readonly object EventSelectedIndexChanged = new object();

	private ComboBox control;

	private string textBinding = "";

	private string selectedItemBinding = "";

	private string selectedValueBinding = "";

	private string itemsBinding = "";

	private string text = "ComboBox";

	private StiArrayList items;

	private ComboBoxStyle dropDownStyle = (ComboBoxStyle)1;

	private int itemHeight = 14;

	private int selectedIndex = -1;

	private object selectedItem;

	private object selectedValue;

	private int maxLength;

	private int maxDropDownItems = 8;

	private int dropDownWidth = 121;

	private bool sorted;

	private Image buttonBitmap;

	public override StiComponentId ComponentId => StiComponentId.StiComboBoxControl;

	public override int ToolboxPosition => 8;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiComboBoxControl");

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event SelectedIndexChanged.")]
	public virtual StiSelectedIndexChangedEvent SelectedIndexChangedEvent
	{
		get
		{
			return new StiSelectedIndexChangedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public ComboBox Control
	{
		get
		{
			return control;
		}
		set
		{
			control = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the text.")]
	[StiOrder(220)]
	public string TextBinding
	{
		get
		{
			return textBinding;
		}
		set
		{
			textBinding = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the selected item.")]
	[StiOrder(190)]
	public string SelectedItemBinding
	{
		get
		{
			return selectedItemBinding;
		}
		set
		{
			selectedItemBinding = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the selected value.")]
	[StiOrder(210)]
	public string SelectedValueBinding
	{
		get
		{
			return selectedValueBinding;
		}
		set
		{
			selectedValueBinding = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the items.")]
	[StiOrder(120)]
	public string ItemsBinding
	{
		get
		{
			return itemsBinding;
		}
		set
		{
			itemsBinding = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the text associated with this control.")]
	[StiOrder(730)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			UpdateReportControl("Text");
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override double Height
	{
		get
		{
			return 20.0;
		}
		set
		{
			base.Height = 20.0;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor(typeof(StiStringCollectionEditor), typeof(UITypeEditor))]
	[Description("Gets an object representing the collection of the items.")]
	[StiOrder(460)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiArrayList Items
	{
		get
		{
			return items;
		}
		set
		{
			items = value;
			items.CollectionChanged += OnCollectionChanged;
			UpdateReportControl("Items");
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value specifying the style.")]
	[StiOrder(290)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return dropDownStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			dropDownStyle = value;
			UpdateReportControl("DropDownStyle");
		}
	}

	[StiSerializable]
	[DefaultValue(14)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the height of an item.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(450)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int ItemHeight
	{
		get
		{
			return itemHeight;
		}
		set
		{
			itemHeight = value;
			UpdateReportControl("ItemHeight");
		}
	}

	[Browsable(false)]
	public virtual int SelectedIndex
	{
		get
		{
			return selectedIndex;
		}
		set
		{
			selectedIndex = value;
			UpdateReportControl("SelectedIndex");
		}
	}

	[Browsable(false)]
	public virtual object SelectedItem
	{
		get
		{
			return selectedItem;
		}
		set
		{
			selectedItem = value;
			UpdateReportControl("SelectedItem");
		}
	}

	[Browsable(false)]
	public virtual object SelectedValue
	{
		get
		{
			return selectedValue;
		}
		set
		{
			selectedValue = value;
			UpdateReportControl("SelectedValue");
		}
	}

	[StiSerializable]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the maximum number of characters allowed.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(520)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int MaxLength
	{
		get
		{
			return maxLength;
		}
		set
		{
			maxLength = value;
			UpdateReportControl("MaxLength");
		}
	}

	[StiSerializable]
	[DefaultValue(8)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the maximum number.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(500)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int MaxDropDownItems
	{
		get
		{
			return maxDropDownItems;
		}
		set
		{
			maxDropDownItems = value;
			UpdateReportControl("MaxDropDownItems");
		}
	}

	[StiSerializable]
	[DefaultValue(121)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the width of the of the drop-down portion.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(300)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int DropDownWidth
	{
		get
		{
			return dropDownWidth;
		}
		set
		{
			dropDownWidth = value;
			UpdateReportControl("DropDownWidth");
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether the items are sorted.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(690)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Sorted
	{
		get
		{
			return sorted;
		}
		set
		{
			sorted = value;
			UpdateReportControl("Sorted");
		}
	}

	[Browsable(false)]
	public Image ButtonBitmap
	{
		get
		{
			if (buttonBitmap == null)
			{
				buttonBitmap = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Images.Dialogs.ComboDown.bmp");
			}
			return buttonBitmap;
		}
	}

	public event EventHandler SelectedIndexChanged
	{
		add
		{
			base.Events.AddHandler(EventSelectedIndexChanged, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventSelectedIndexChanged, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "SelectedIndexChangedEvent", SelectedIndexChangedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBinding", TextBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SelectedItemBinding", SelectedItemBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SelectedValueBinding", SelectedValueBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ItemsBinding", ItemsBinding);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		JsonExtensions.AddPropertyEnum(val, "DropDownStyle", (Enum)(object)DropDownStyle, (Enum)(object)(ComboBoxStyle)1);
		JsonExtensions.AddPropertyInt(val, "ItemHeight", ItemHeight, 14);
		JsonExtensions.AddPropertyInt(val, "MaxLength", MaxLength, 0);
		JsonExtensions.AddPropertyInt(val, "MaxDropDownItems", MaxDropDownItems, 8);
		JsonExtensions.AddPropertyInt(val, "DropDownWidth", DropDownWidth, 121);
		JsonExtensions.AddPropertyBool(val, "Sorted", Sorted, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "SelectedIndexChangedEvent":
			{
				StiSelectedIndexChangedEvent stiSelectedIndexChangedEvent = new StiSelectedIndexChangedEvent();
				stiSelectedIndexChangedEvent.LoadFromJsonObject((JObject)item.Value);
				SelectedIndexChangedEvent = stiSelectedIndexChangedEvent;
				break;
			}
			case "TextBinding":
				textBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SelectedItemBinding":
				selectedItemBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SelectedValueBinding":
				selectedValueBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ItemsBinding":
				itemsBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Text":
				text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DropDownStyle":
				dropDownStyle = StiJsonReportObjectHelper.DeserializeEnum<ComboBoxStyle>(item);
				break;
			case "ItemHeight":
				itemHeight = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MaxLength":
				maxLength = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "MaxDropDownItems":
				maxDropDownItems = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "DropDownWidth":
				dropDownWidth = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Sorted":
				sorted = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (SelectedIndexChangedEvent != null)
		{
			stiEventsCollection.Add(SelectedIndexChangedEvent);
		}
		return stiEventsCollection;
	}

	public void InvokeSelectedIndexChanged(EventArgs e)
	{
		if (base.Events[EventSelectedIndexChanged] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, SelectedIndexChangedEvent);
	}

	private void OnCollectionChanged(object sender, EventArgs e)
	{
		UpdateReportControl("Items");
	}

	public override StiEvent GetDefaultEvent()
	{
		return SelectedIndexChangedEvent;
	}

	public override StiComponent CreateNew()
	{
		return new StiComboBoxControl();
	}

	public StiComboBoxControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiComboBoxControl(RectangleD rect)
		: base(rect)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		items = new StiArrayList();
		items.CollectionChanged += OnCollectionChanged;
		base.BackColor = SystemColors.Window;
		PlaceOnToolbox = true;
		Text = LocalizedName;
	}
}
