using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiSubReport), "Stimulsoft.Report.Images.Components.StiSubReport.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiSubReportDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfSubReportDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiSubReportGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiSubReportWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiSubReportV1Builder))]
[StiV2Builder(typeof(StiSubReportV2Builder))]
[StiToolbox(true)]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiSubReport : StiContainer
{
	private StiSubReportInfoV1 subReportInfoV1;

	private static readonly object EventFillParameters = new object();

	public override StiComponentId ComponentId => StiComponentId.StiSubReport;

	public override string HelpUrl => "user-manual/index.html?report_internals_sub-reports.htm";

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
			base.CanShrink = value;
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
			base.CanGrow = value;
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

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public override StiShiftMode ShiftMode
	{
		get
		{
			return base.ShiftMode;
		}
		set
		{
			base.ShiftMode = value;
		}
	}

	public override int ToolboxPosition => 114;

	public override StiToolboxCategory ToolboxCategory { get; } = StiToolboxCategory.Components;


	public override StiComponentType ComponentType => StiComponentType.Simple;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiSubReport");

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

	public override int Priority
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Report != null && (int)Report.EngineVersion == 1)
			{
				return 1500;
			}
			return 0;
		}
	}

	public override double Width
	{
		get
		{
			return base.Width;
		}
		set
		{
			base.Width = value;
			UpdateSubReportPageWidth();
		}
	}

	public override RectangleD ClientRectangle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.ClientRectangle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.ClientRectangle = value;
			UpdateSubReportPageWidth();
		}
	}

	[Browsable(false)]
	public StiSubReportInfoV1 SubReportInfoV1 => subReportInfoV1 ?? (subReportInfoV1 = new StiSubReportInfoV1());

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the FillParameters is calculated.")]
	public StiFillParametersEvent FillParametersEvent
	{
		get
		{
			return new StiFillParametersEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[StiBrowsable(false)]
	[Obsolete("StiSubReport.UseExternalReport property is obsolete.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool UseExternalReport
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(270)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that the SubReport must to be kept together with DataBand on what it is placed.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	public virtual bool KeepSubReportTogether { get; set; }

	[StiNonSerialized]
	[TypeConverter(typeof(StiSubReportConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiSubReportEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets a page that contains SubReport.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public StiPage SubReportPage
	{
		get
		{
			if (Report == null)
			{
				return null;
			}
			foreach (StiPage page in Report.Pages)
			{
				if (page.Guid == SubReportPageGuid)
				{
					return page;
				}
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				SubReportPageGuid = null;
				return;
			}
			if (value.Guid == null)
			{
				value.Guid = StiGuidUtils.NewGuid();
			}
			SubReportPageGuid = value.Guid;
			UpdateSubReportPageWidth();
		}
	}

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(null)]
	public string SubReportPageGuid { get; set; }

	[StiSerializable]
	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a URL that contains the SubReport.")]
	[DefaultValue(null)]
	public string SubReportUrl { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(120)]
	[Description("Gets or sets a parameter collection.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public StiParametersCollection Parameters { get; set; } = new StiParametersCollection();


	public event StiFillParametersEventHandler FillParameters
	{
		add
		{
			base.Events.AddHandler(EventFillParameters, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventFillParameters, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.AddPropertyJObject(val, "FillParametersEvent", FillParametersEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "KeepSubReportTogether", KeepSubReportTogether, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SubReportPageGuid", SubReportPageGuid);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "SubReportUrl", SubReportUrl);
		if ((int)mode == 0)
		{
			JsonExtensions.AddPropertyJObject(val, "Parameters", Parameters.SaveToJsonObject(mode));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "FillParametersEvent":
			{
				StiFillParametersEvent stiFillParametersEvent = new StiFillParametersEvent();
				stiFillParametersEvent.LoadFromJsonObject((JObject)item.Value);
				FillParametersEvent = stiFillParametersEvent;
				break;
			}
			case "KeepSubReportTogether":
				KeepSubReportTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "SubReportPageGuid":
				SubReportPageGuid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SubReportUrl":
				SubReportUrl = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Parameters":
				Parameters.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Invalid comparison between Unknown and I4
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.SubReportDesigner() });
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.Enabled(),
				propertiesHelper.KeepSubReportTogether()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepSubReportTogether(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.KeepSubReportTogether(),
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.FillParametersEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override object Clone(bool cloneProperties)
	{
		return Clone(cloneProperties, cloneComponents: false);
	}

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiSubReport stiSubReport = (StiSubReport)base.Clone(cloneProperties, cloneComponents: false);
		stiSubReport.subReportInfoV1 = SubReportInfoV1.Clone() as StiSubReportInfoV1;
		return stiSubReport;
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (FillParametersEvent != null)
		{
			stiEventsCollection.Add(FillParametersEvent);
		}
		return stiEventsCollection;
	}

	public override bool CanContainIn(StiComponent component)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			if (component is IStiReportControl)
			{
				return false;
			}
			if (component is StiPage)
			{
				return true;
			}
			if (component is StiDataBand)
			{
				return true;
			}
		}
		else
		{
			if (component is StiReportControl)
			{
				return false;
			}
			if (component is StiContainer && !(component is StiSubReport) && !(component is StiClone))
			{
				return true;
			}
		}
		return false;
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		Left = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(Left));
		Top = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(Top));
		base.Width = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(Width));
		Height = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(Height));
		if (!isReportSnapshot)
		{
			MinSize = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(MinSize));
			MaxSize = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(MaxSize));
		}
	}

	private void UpdateSubReportPageWidth()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if (SubReportPage != null && Report != null && (int)Report.EngineVersion == 2)
		{
			if (Page != null && SubReportPage.Orientation != Page.Orientation)
			{
				double pageHeight = SubReportPage.PageHeight;
				SubReportPage.Orientation = Page.Orientation;
				SubReportPage.PageHeight = pageHeight;
				SubReportPage.Margins = new StiMargins(SubReportPage.Margins.Top, SubReportPage.Margins.Bottom, SubReportPage.Margins.Left, SubReportPage.Margins.Right);
			}
			SubReportPage.Width = Width;
		}
	}

	public StiReport GetExternalSubReport()
	{
		StiReport stiReport = GetSubReportFromUrl(SubReportUrl);
		if (stiReport == null)
		{
			stiReport = GetSubReportFromFile(SubReportUrl);
		}
		if (stiReport == null)
		{
			StiGetSubReportEventArgs stiGetSubReportEventArgs = new StiGetSubReportEventArgs(Name);
			Report.InvokeGetSubReport(stiGetSubReportEventArgs);
			stiReport = stiGetSubReportEventArgs.Report;
		}
		if (stiReport != null && stiReport.ReportUnit != Report.ReportUnit)
		{
			if (stiReport.IsDocument)
			{
				foreach (StiPage renderedPage in stiReport.RenderedPages)
				{
					renderedPage.Convert(stiReport.Unit, Report.Unit);
				}
			}
			else
			{
				stiReport.ReportUnit = Report.ReportUnit;
			}
		}
		if (stiReport != null && StiOptions.Engine.ForceInterpretationMode)
		{
			stiReport.CalculationMode = StiCalculationMode.Interpretation;
		}
		return stiReport;
	}

	protected StiReport GetSubReportFromUrl(string url)
	{
		try
		{
			if (!string.IsNullOrEmpty(url))
			{
				if (StiHyperlinkProcessor.IsServerHyperlink(url))
				{
					return StiStimulsoftServerResource.GetSubReport(this, StiHyperlinkProcessor.GetServerNameFromHyperlink(SubReportPageGuid));
				}
				StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
				string resourceName = StiHyperlinkProcessor.GetResourceNameFromHyperlink(url);
				if (resourceName != null)
				{
					resourceName = resourceName.ToLowerInvariant().Trim();
					StiResource stiResource = Report.Dictionary.Resources.ToList().FirstOrDefault((StiResource r) => r.Name != null && r.Name.ToLowerInvariant().Trim() == resourceName);
					if (stiResource != null)
					{
						switch (stiResource.Type)
						{
						case StiResourceType.Report:
							stiReport.Load(stiResource.Content);
							break;
						case StiResourceType.ReportSnapshot:
							stiReport.LoadDocument(stiResource.Content);
							break;
						}
					}
				}
				else if (StringExt.EndsWithInvariantIgnoreCase(url, ".mdc") || StringExt.EndsWithInvariantIgnoreCase(url, ".mdz"))
				{
					stiReport.LoadDocumentFromUrl(url);
				}
				else
				{
					stiReport.LoadFromUrl(url);
				}
				return stiReport;
			}
		}
		catch (Exception ex)
		{
			string text = "SubReport can't be loaded from URL '" + url + "' in subreport component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
		return null;
	}

	protected virtual StiReport GetSubReportFromFile(string url)
	{
		string fileNameFromHyperlink = StiHyperlinkProcessor.GetFileNameFromHyperlink(url);
		if (fileNameFromHyperlink == null)
		{
			return null;
		}
		if (File.Exists(fileNameFromHyperlink))
		{
			try
			{
				StiReport stiReport = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
				string extension = Path.GetExtension(fileNameFromHyperlink);
				if (extension == ".mdc" || extension == ".mdz")
				{
					stiReport.LoadDocumentFromUrl(url);
				}
				else
				{
					stiReport.LoadFromUrl(url);
				}
				return stiReport;
			}
			catch (Exception ex)
			{
				string text = "SubReport can't be loaded from file '" + fileNameFromHyperlink + "' in subreport component " + Name + "!";
				StiLogService.Write(((object)this).GetType(), text);
				StiLogService.Write(((object)this).GetType(), ex.Message);
				Report.WriteToReportRenderingMessages(text);
			}
		}
		else
		{
			string text2 = "File '" + fileNameFromHyperlink + "' does not exist in subreport component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text2);
			Report.WriteToReportRenderingMessages(text2);
		}
		return null;
	}

	protected virtual void OnFillParameters(StiFillParametersEventArgs e)
	{
	}

	public virtual void InvokeFillParameters(StiComponent sender, StiFillParametersEventArgs e)
	{
		try
		{
			OnFillParameters(e);
			if (base.Events[EventFillParameters] is StiFillParametersEventHandler stiFillParametersEventHandler)
			{
				stiFillParametersEventHandler(sender, e);
			}
			if (Report == null || Report.CalculationMode != StiCalculationMode.Interpretation || Parameters == null || Parameters.Count <= 0)
			{
				return;
			}
			foreach (StiParameter parameter in Parameters)
			{
				if (!string.IsNullOrEmpty(parameter.Name) && parameter.Expression != null && !string.IsNullOrEmpty(parameter.Expression.Value))
				{
					object value = StiParser.ParseTextValue("{" + parameter.Expression.Value + "}", this);
					e.Value.Add(parameter.Name, value);
				}
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, FillParametersEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in parameters of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiSubReport();
	}

	public static StiSubReport GetSubReportForPage(StiPage page)
	{
		if (page == null)
		{
			return null;
		}
		StiComponentsCollection stiComponentsCollection = page.Report.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (item is StiSubReport && ((StiSubReport)item).SubReportPage == page)
			{
				return item as StiSubReport;
			}
		}
		return null;
	}

	public StiSubReport()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiSubReport(RectangleD rect)
		: base(rect)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
