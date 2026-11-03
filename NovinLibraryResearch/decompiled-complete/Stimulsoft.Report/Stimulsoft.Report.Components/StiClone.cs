using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiClone), "Stimulsoft.Report.Images.Components.StiClone.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiCloneDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfCloneDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiCloneGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiCloneWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiCloneV1Builder))]
[StiV2Builder(typeof(StiCloneV2Builder))]
[StiContextTool(typeof(IStiClone))]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiClone : StiContainer, IStiClone
{
	internal string jsonContainerValueTemp;

	private StiCloneInfoV1 cloneInfoV1;

	public override StiComponentId ComponentId => StiComponentId.StiClone;

	public override string HelpUrl => "user-manual/index.html?report_internals_panels_cloning.htm";

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiAnchorMode Anchor
	{
		get
		{
			return base.Anchor;
		}
		set
		{
			base.Anchor = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiCloneConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiCloneEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a clone container.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public StiContainer Container { get; set; }

	[Browsable(false)]
	public StiCloneInfoV1 CloneInfoV1 => cloneInfoV1 ?? (cloneInfoV1 = new StiCloneInfoV1());

	public override bool IsRendered
	{
		get
		{
			if (Container != null)
			{
				return Container.IsRendered;
			}
			return true;
		}
		set
		{
			if (Container != null)
			{
				Container.IsRendered = value;
			}
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
			base.Conditions = value;
		}
	}

	public override int ToolboxPosition => 112;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	[Browsable(false)]
	public override bool ForceRedrawAll => true;

	public override string LocalizedCategory => Loc.Get("Report", "Components");

	public override string LocalizedName => Loc.Get("Components", "StiClone");

	[Browsable(false)]
	[StiNonSerialized]
	public override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
		}
	}

	public override StiComponentType ComponentType
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Report == null || (int)Report.EngineVersion != 1)
			{
				return StiComponentType.Simple;
			}
			return StiComponentType.Detail;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiComponentsCollection Components
	{
		get
		{
			if (Container == null || base.IsDesigning)
			{
				return base.Components;
			}
			return Container.Components;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[DefaultValue(false)]
	[StiSerializable]
	[Description("Gets or sets value which indicates that contents of the container will be shrunk or grown.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public bool ScaleHor { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "Conditions");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "Components");
		JsonExtensions.AddPropertyBool(val, "ScaleHor", ScaleHor, false, false);
		if (Container != null)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "Container", Container.Name);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "ScaleHor"))
			{
				if (text == "Container")
				{
					jsonContainerValueTemp = StiJsonReportObjectHelper.DeserializeString(item);
					Report.jsonLoaderHelper.Clones.Add(this);
				}
			}
			else
			{
				ScaleHor = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Invalid comparison between Unknown and I4
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.CloneEditor() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[4]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[6]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height(),
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[3]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[1] { propertiesHelper.Enabled() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.Printable(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetBookmarkEvent,
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetPointerEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.AfterPrintEvent,
			StiPropertyEventId.BeforePrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override void SaveState(string stateName)
	{
		StiContainer container = Container;
		Container = null;
		base.SaveState(stateName);
		Container = container;
	}

	public override void RestoreState(string stateName)
	{
		StiContainer container = Container;
		Container = null;
		base.RestoreState(stateName);
		Container = container;
	}

	private bool ShouldSerializeContainer()
	{
		return Container != null;
	}

	public override object Clone()
	{
		StiClone stiClone = MemberwiseClone() as StiClone;
		stiClone.cloneInfoV1 = CloneInfoV1.Clone() as StiCloneInfoV1;
		stiClone.Components = new StiComponentsCollection(stiClone);
		return stiClone;
	}

	public override bool CanContainIn(StiComponent component)
	{
		if (component is IStiReportControl)
		{
			return false;
		}
		if (component is StiClone)
		{
			return false;
		}
		if (component is StiContainer)
		{
			return true;
		}
		return false;
	}

	protected override void OnBeforePrint(EventArgs e)
	{
		base.OnBeforePrint(e);
		Container?.InvokeBeforePrint(this, e);
	}

	protected override void OnAfterPrint(EventArgs e)
	{
		base.OnAfterPrint(e);
		Container?.InvokeAfterPrint(this, e);
	}

	public override StiComponent CreateNew()
	{
		return new StiClone();
	}

	public StiClone()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiClone(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
