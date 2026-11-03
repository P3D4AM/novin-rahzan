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
[StiServiceBitmap(typeof(StiCheckedListBoxControl), "Stimulsoft.Report.Dialogs.Bmp.StiCheckedListBoxControl.gif")]
[StiGdiPainter(typeof(StiCheckedListBoxControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiCheckedListBoxControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGuiMode(/*Could not decode attribute arguments.*/)]
public class StiCheckedListBoxControl : StiReportControl
{
	private static readonly object EventSelectedIndexChanged = new object();

	private CheckedListBox control;

	private string itemsBinding = "";

	private string selectedItemBinding = "";

	private string selectedValueBinding = "";

	private string selectedIndexBinding = "";

	private StiArrayList items;

	private int itemHeight = 13;

	private int selectedIndex = -1;

	private object selectedItem;

	private object selectedValue;

	private object[] checkedItems = new object[0];

	private SelectionMode selectionMode = (SelectionMode)1;

	private bool sorted;

	private bool checkOnClick;

	public override StiComponentId ComponentId => StiComponentId.StiCheckedListBoxControl;

	public override int ToolboxPosition => 10;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiCheckedListBoxControl");

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the SelectedIndex property has changed.")]
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
	public CheckedListBox Control
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
	[Description("Gets the data bindings for the selected index.")]
	[StiOrder(180)]
	public string SelectedIndexBinding
	{
		get
		{
			return selectedIndexBinding;
		}
		set
		{
			selectedIndexBinding = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor(typeof(StiStringCollectionEditor), typeof(UITypeEditor))]
	[Description("Gets the collection of items in this CheckedListBox.")]
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
	[DefaultValue(13)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets the height of the item area.")]
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

	[Browsable(false)]
	public virtual object[] CheckedItems
	{
		get
		{
			return checkedItems;
		}
		set
		{
			checkedItems = value;
			UpdateReportControl("CheckedItems");
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value specifying the selection mode.")]
	[StiOrder(650)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual SelectionMode SelectionMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return selectionMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			selectionMode = value;
			UpdateReportControl("SelectionMode");
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether the items in the ListBox are sorted alphabetically.")]
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

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether the check box should be toggled when an item is selected.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(190)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CheckOnClick
	{
		get
		{
			return checkOnClick;
		}
		set
		{
			checkOnClick = value;
			UpdateReportControl("CheckOnClick");
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 96.0, 96.0);

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
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "ForeColor");
		JsonExtensions.AddPropertyJObject(val, "SelectedIndexChangedEvent", SelectedIndexChangedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ItemsBinding", ItemsBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SelectedItemBinding", SelectedItemBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SelectedValueBinding", SelectedValueBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SelectedIndexBinding", SelectedIndexBinding);
		JsonExtensions.AddPropertyInt(val, "ItemHeight", ItemHeight, 13);
		JsonExtensions.AddPropertyEnum(val, "SelectionMode", (Enum)(object)SelectionMode, (Enum)(object)(SelectionMode)1);
		JsonExtensions.AddPropertyBool(val, "Sorted", Sorted, false, false);
		JsonExtensions.AddPropertyBool(val, "CheckOnClick", CheckOnClick, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
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
			case "ItemsBinding":
				itemsBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SelectedItemBinding":
				selectedItemBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SelectedValueBinding":
				selectedValueBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SelectedIndexBinding":
				selectedIndexBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ItemHeight":
				itemHeight = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "SelectionMode":
				selectionMode = StiJsonReportObjectHelper.DeserializeEnum<SelectionMode>(item);
				break;
			case "Sorted":
				sorted = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CheckOnClick":
				checkOnClick = StiJsonReportObjectHelper.DeserializeBool(item);
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
		return new StiCheckedListBoxControl();
	}

	public StiCheckedListBoxControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiCheckedListBoxControl(RectangleD rect)
		: base(rect)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		items = new StiArrayList();
		items.CollectionChanged += OnCollectionChanged;
		base.BackColor = SystemColors.Window;
		PlaceOnToolbox = true;
	}
}
