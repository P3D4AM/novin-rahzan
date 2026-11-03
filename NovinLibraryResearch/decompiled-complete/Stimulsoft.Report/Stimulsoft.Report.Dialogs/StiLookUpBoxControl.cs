using System;
using System.ComponentModel;
using System.Drawing.Design;
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
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiComboBoxControl), "Stimulsoft.Report.Dialogs.Bmp.StiLookUpBoxControl.gif")]
[StiWpfPainter("Stimulsoft.Report.Painters.StiLookUpBoxControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiLookUpBoxControl : StiComboBoxControl
{
	internal class LookUpContainer
	{
		public string Value;

		public object Key;

		public override string ToString()
		{
			return Value;
		}

		public LookUpContainer(string value, object key)
		{
			Value = value;
			Key = key;
		}
	}

	private StiArrayList keys;

	private string keysBinding = "";

	private object selectedKey;

	private string selectedKeyBinding = "";

	public override StiComponentId ComponentId => StiComponentId.StiLookUpBoxControl;

	public override int ToolboxPosition => 9;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiLookUpBoxControl");

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor(typeof(StiStringCollectionEditor), typeof(UITypeEditor))]
	[Description("Gets an object representing the collection of the keys.")]
	[StiOrder(470)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiArrayList Keys
	{
		get
		{
			return keys;
		}
		set
		{
			keys = value;
			keys.CollectionChanged += OnKeysCollectionChanged;
			UpdateReportControl("Keys");
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the keys.")]
	[StiOrder(130)]
	public string KeysBinding
	{
		get
		{
			return keysBinding;
		}
		set
		{
			keysBinding = value;
		}
	}

	[Browsable(false)]
	public virtual object SelectedKey
	{
		get
		{
			return selectedKey;
		}
		set
		{
			selectedKey = value;
			UpdateReportControl("SelectedKey");
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the selected key.")]
	[StiOrder(200)]
	public string SelectedKeyBinding
	{
		get
		{
			return selectedKeyBinding;
		}
		set
		{
			selectedKeyBinding = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "KeysBinding", KeysBinding);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SelectedKeyBinding", SelectedKeyBinding);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "KeysBinding"))
			{
				if (text == "SelectedKeyBinding")
				{
					selectedKeyBinding = StiJsonReportObjectHelper.DeserializeString(item);
				}
			}
			else
			{
				keysBinding = StiJsonReportObjectHelper.DeserializeString(item);
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

	private void OnKeysCollectionChanged(object sender, EventArgs e)
	{
		UpdateReportControl("Keys");
	}

	public override StiComponent CreateNew()
	{
		return new StiLookUpBoxControl();
	}

	public StiLookUpBoxControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiLookUpBoxControl(RectangleD rect)
		: base(rect)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		base.Text = "LookUpBox";
		keys = new StiArrayList();
		keys.CollectionChanged += OnKeysCollectionChanged;
	}
}
