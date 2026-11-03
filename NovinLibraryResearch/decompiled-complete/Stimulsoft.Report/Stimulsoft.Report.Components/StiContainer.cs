using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiContainer), "Stimulsoft.Report.Images.Components.StiContainer.png")]
[StiGdiPainter(typeof(StiContainerGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiContainerWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiContainerV1Builder))]
[StiV2Builder(typeof(StiContainerV2Builder))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiContainer : StiComponent, IStiBorder, IStiBrush, IStiSerializable, IStiBreakable, IStiIgnoryStyle, IStiGetFonts
{
	private static object PropertyCanBreak = new object();

	private StiContainerInfoV1 containerInfoV1;

	private StiContainerInfoV2 containerInfoV2;

	private StiComponentsCollection components;

	protected static object PropertyBlocked = new object();

	public override StiComponentId ComponentId => StiComponentId.StiContainer;

	public override string HelpUrl => null;

	[DefaultValue(false)]
	[Browsable(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CanBreak
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyCanBreak, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyCanBreak, value, false);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	public virtual StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[Browsable(false)]
	public StiContainerInfoV1 ContainerInfoV1 => containerInfoV1 ?? (containerInfoV1 = new StiContainerInfoV1());

	[Browsable(false)]
	public StiContainerInfoV2 ContainerInfoV2 => containerInfoV2 ?? (containerInfoV2 = new StiContainerInfoV2());

	public override int Priority => 0;

	public override int ToolboxPosition => 110;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override StiComponentType ComponentType
	{
		get
		{
			if (containerInfoV2 != null && containerInfoV2.ParentBand != null)
			{
				return containerInfoV2.ParentBand.ComponentType;
			}
			return StiComponentType.Simple;
		}
	}

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiContainer");

	[Browsable(false)]
	internal bool ParentComponentIsBand { get; set; }

	[Browsable(false)]
	internal bool ParentComponentIsCrossBand { get; set; }

	[Browsable(false)]
	public object CollapsedValue { get; set; }

	[Browsable(false)]
	public virtual int CollapsingIndex { get; set; }

	[Browsable(false)]
	public virtual string CollapsingTreePath { get; set; }

	[Browsable(false)]
	public bool HasSelected
	{
		get
		{
			lock (((ICollection)Components).SyncRoot)
			{
				foreach (StiComponent component in Components)
				{
					if (component.IsSelected)
					{
						return true;
					}
					if (component is StiContainer { HasSelected: not false })
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 100.0, 100.0);

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public virtual StiComponentsCollection Components
	{
		get
		{
			return components;
		}
		set
		{
			components = value;
		}
	}

	[Browsable(false)]
	protected internal virtual bool Blocked
	{
		get
		{
			return (bool)((StiService)this).Properties.Get(PropertyBlocked, (object)false);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyBlocked, (object)value, (object)false);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyJObject(val, "Components", Components.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CanBreak":
				CanBreak = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Components":
				Components.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Invalid comparison between Unknown and I4
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
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
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[4]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.Enabled()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[9]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[10]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
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

	public virtual bool Break(StiComponent dividedComponent, double devideFactor, ref double divideLine)
	{
		StiContainer stiContainer = StiComponentDivider.BreakContainer(Height, this);
		((StiContainer)dividedComponent).Components.Clear();
		((StiContainer)dividedComponent).Components.AddRange(stiContainer.Components);
		divideLine = Height;
		if (StiOptions.Engine.UsePrintOnAllPagesPropertyOfHeadersInSubreports)
		{
			List<StiBand> list = new List<StiBand>();
			List<StiContainer> list2 = new List<StiContainer>();
			List<StiBand> list3 = new List<StiBand>();
			foreach (StiComponent component in Components)
			{
				if (!(component is StiContainer))
				{
					continue;
				}
				StiBand parentBand = (component as StiContainer).ContainerInfoV2.ParentBand;
				if (!(parentBand is StiHeaderBand) || !(parentBand as IStiPrintOnAllPages).PrintOnAllPages)
				{
					continue;
				}
				int num = list.IndexOf(parentBand);
				if (num != -1)
				{
					list2[num] = ((StiService)component).Clone() as StiContainer;
					continue;
				}
				list.Add(parentBand as StiHeaderBand);
				list2.Add(((StiService)component).Clone() as StiContainer);
				if (parentBand.Parent != null)
				{
					list3.Add(StiHeaderBandV2Builder.GetMaster(parentBand as StiHeaderBand));
				}
				else
				{
					list3.Add(parentBand);
				}
			}
			if (list.Count > 0)
			{
				StiComponentsCollection stiComponentsCollection = ((StiContainer)dividedComponent).Components;
				List<StiComponent> list4 = new List<StiComponent>();
				int num2 = list.Count;
				foreach (StiComponent item in stiComponentsCollection)
				{
					if (!(item is StiContainer))
					{
						continue;
					}
					StiBand parentBand2 = (item as StiContainer).ContainerInfoV2.ParentBand;
					if (!(parentBand2 is StiDataBand stiDataBand))
					{
						continue;
					}
					StiBand stiBand = parentBand2;
					if (parentBand2 is StiTable && stiDataBand.DataBandInfoV2.Headers != null)
					{
						foreach (StiComponent header in stiDataBand.DataBandInfoV2.Headers)
						{
							if (header is StiHeaderBand && header.Name == parentBand2.Name.Substring(0, parentBand2.Name.Length - 3) + "_Hd")
							{
								stiBand = header as StiBand;
								break;
							}
						}
					}
					for (int num3 = list.Count - 1; num3 >= 0; num3--)
					{
						if (list[num3] != null && (parentBand2 == list3[num3] || stiBand == list3[num3]))
						{
							bool flag = true;
							for (int i = 0; i < list.Count; i++)
							{
								StiContainer stiContainer2 = null;
								if (i < stiComponentsCollection.Count)
								{
									stiContainer2 = stiComponentsCollection[i] as StiContainer;
								}
								if (stiContainer2 != null && stiContainer2.ContainerInfoV2.ParentBand == list[num3])
								{
									flag = false;
								}
							}
							if (flag)
							{
								list4.Add(list2[num3]);
							}
							list[num3] = null;
							num2--;
						}
					}
					if (num2 == 0)
					{
						break;
					}
				}
				if (list4.Count > 0)
				{
					foreach (StiComponent item2 in list4)
					{
						foreach (StiComponent item3 in stiComponentsCollection)
						{
							item3.Top += item2.Height;
						}
						item2.Top = 0.0;
						stiComponentsCollection.Insert(0, item2);
					}
				}
			}
		}
		return true;
	}

	private void Write(StiObjectStringConverter converter, string name, object obj, XmlTextWriter tw)
	{
		string text = converter.ObjectToString(obj);
		if (text != null)
		{
			tw.WriteAttributeString(name, text);
		}
	}

	public void Serialize(StiObjectStringConverter converter, XmlTextWriter tw)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		tw.WriteAttributeString("name", Name);
		if (!Printable)
		{
			tw.WriteAttributeString("pr", Printable.ToString());
		}
		if (base.Guid != null)
		{
			tw.WriteAttributeString("guid", base.Guid);
		}
		if (base.PointerValue != null)
		{
			tw.WriteAttributeString("pointer", base.PointerValue.ToString());
		}
		if (base.BookmarkValue != null)
		{
			tw.WriteAttributeString("bookmark", base.BookmarkValue.ToString());
		}
		if (base.HyperlinkValue != null)
		{
			tw.WriteAttributeString("hyperlink", base.HyperlinkValue.ToString());
		}
		if (base.TagValue != null)
		{
			tw.WriteAttributeString("tag", base.TagValue.ToString());
		}
		if (base.ToolTipValue != null)
		{
			tw.WriteAttributeString("toolTip", base.ToolTipValue.ToString());
		}
		if (!string.IsNullOrWhiteSpace(base.ComponentPlacement))
		{
			tw.WriteAttributeString("pl", base.ComponentPlacement);
		}
		Write(converter, "rc", ClientRectangle, tw);
		Write(converter, "bh", Brush, tw);
		Write(converter, "br", Border, tw);
	}

	public void Deserialize(StiObjectStringConverter converter, XmlTextReader tr)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		string attribute = tr.GetAttribute("name");
		if (attribute != null)
		{
			Name = attribute;
		}
		attribute = tr.GetAttribute("pr");
		if (attribute != null)
		{
			Printable = false;
		}
		base.Guid = tr.GetAttribute("guid");
		base.PointerValue = tr.GetAttribute("pointer");
		base.BookmarkValue = tr.GetAttribute("bookmark");
		base.HyperlinkValue = tr.GetAttribute("hyperlink");
		base.TagValue = tr.GetAttribute("tag");
		base.ToolTipValue = tr.GetAttribute("toolTip");
		attribute = tr.GetAttribute("pl");
		if (!string.IsNullOrWhiteSpace(attribute))
		{
			base.ComponentPlacement = attribute;
		}
		attribute = tr.GetAttribute("rc");
		if (attribute != null)
		{
			ClientRectangle = (RectangleD)converter.StringToObject(attribute, typeof(RectangleD));
		}
		attribute = tr.GetAttribute("bh");
		if (attribute != null)
		{
			object obj = converter.StringToObject(attribute, typeof(StiBrush));
			Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		}
		attribute = tr.GetAttribute("br");
		if (attribute != null)
		{
			object obj2 = converter.StringToObject(attribute, typeof(StiBorder));
			Border = (StiBorder)((obj2 is StiBorder) ? obj2 : null);
		}
	}

	public override object Clone(bool cloneProperties)
	{
		return Clone(cloneProperties, cloneComponents: true);
	}

	public virtual object Clone(bool cloneProperties, bool cloneComponents)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		StiContainer stiContainer = (StiContainer)base.Clone(cloneProperties);
		stiContainer.containerInfoV1 = ContainerInfoV1.Clone() as StiContainerInfoV1;
		stiContainer.containerInfoV2 = ContainerInfoV2.Clone() as StiContainerInfoV2;
		if (Border != null)
		{
			stiContainer.Border = (StiBorder)Border.Clone();
		}
		else
		{
			stiContainer.Border = null;
		}
		if (Brush != null)
		{
			stiContainer.Brush = (StiBrush)Brush.Clone();
		}
		else
		{
			stiContainer.Brush = null;
		}
		stiContainer.components = new StiComponentsCollection(stiContainer);
		if (cloneComponents)
		{
			lock (((ICollection)Components).SyncRoot)
			{
				foreach (StiComponent component in components)
				{
					stiContainer.components.Add((StiComponent)((StiService)component).Clone());
				}
			}
		}
		return stiContainer;
	}

	public void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot, bool convertComponents)
	{
		base.Convert(oldUnit, newUnit, isReportSnapshot);
		if (!convertComponents)
		{
			return;
		}
		lock (((ICollection)components).SyncRoot)
		{
			foreach (StiComponent component in components)
			{
				component.Convert(oldUnit, newUnit, isReportSnapshot);
			}
		}
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		Convert(oldUnit, newUnit, isReportSnapshot, convertComponents: true);
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.Transparent);
		}
		return true;
	}

	public override void SaveState(string stateName)
	{
		base.SaveState(stateName);
		base.States.Push(stateName, this, "CurrentComponent", ContainerInfoV1.CurrentComponent);
		Components.SaveState(stateName);
	}

	public override void RestoreState(string stateName)
	{
		base.RestoreState(stateName);
		if (base.States.IsExist(stateName, this))
		{
			ContainerInfoV1.CurrentComponent = base.States.Pop(stateName, this, "CurrentComponent") as StiComponent;
		}
		Components.RestoreState(stateName);
	}

	public override void ClearAllStates()
	{
		base.ClearAllStates();
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				component.ClearAllStates();
			}
		}
	}

	public override SizeD GetActualSize()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		bool needSecondPass = false;
		return GetActualSize(isFirstPass: false, ref needSecondPass);
	}

	internal SizeD GetActualSize(bool isFirstPass, ref bool needSecondPass)
	{
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		SizeD result = default(SizeD);
		((SizeD)(ref result))._002Ector(Width, Height);
		if (CanGrow || CanShrink)
		{
			double num = 0.0;
			double num2 = 0.0;
			if (!(this is StiPage))
			{
				lock (((ICollection)Components).SyncRoot)
				{
					SizeD actualSize = default(SizeD);
					foreach (StiComponent component in Components)
					{
						if (!component.Enabled)
						{
							continue;
						}
						if (component is StiPointPrimitive)
						{
							num = Math.Max(component.Top, num);
							num2 = Math.Max(component.Left, num2);
						}
						else
						{
							if ((component is StiPrimitive && !(component is StiHorizontalLinePrimitive)) || component.Width == 0.0 || component.Height == 0.0)
							{
								continue;
							}
							RectangleD clientRectangle = component.ClientRectangle;
							double num3 = ((RectangleD)(ref clientRectangle)).Width;
							clientRectangle = component.ClientRectangle;
							((SizeD)(ref actualSize))._002Ector(num3, ((RectangleD)(ref clientRectangle)).Height);
							if (component.DockStyle != StiDockStyle.None && component.Dockable)
							{
								if (!isFirstPass)
								{
									component.DockToContainer();
									if (StiOptions.Engine.AllowCacheForGetActualSize && component.Report != null && component.Report.Engine != null && component.Report.Engine.HashCheckSize != null)
									{
										component.Report.Engine.HashCheckSize[component] = null;
									}
									actualSize = component.GetActualSize();
								}
								needSecondPass = true;
							}
							num = ((!(component is StiHorizontalLinePrimitive)) ? Math.Max(component.Top + ((SizeD)(ref actualSize)).Height, num) : Math.Max(component.Top, num));
							num2 = Math.Max(component.Right, num2);
						}
					}
				}
				if (CanGrow)
				{
					((SizeD)(ref result)).Width = Math.Max(num2, ((SizeD)(ref result)).Width);
					((SizeD)(ref result)).Height = Math.Max(num, ((SizeD)(ref result)).Height);
				}
				if (CanShrink)
				{
					((SizeD)(ref result)).Width = Math.Min(num2, ((SizeD)(ref result)).Width);
					((SizeD)(ref result)).Height = Math.Min(num, ((SizeD)(ref result)).Height);
				}
			}
		}
		if (this is StiPage && (this as StiPage).UnlimitedHeight)
		{
			double num4 = 0.0;
			double num5 = 0.0;
			lock (((ICollection)Components).SyncRoot)
			{
				foreach (StiComponent component2 in Components)
				{
					if (component2.Enabled)
					{
						if (component2 is StiPointPrimitive)
						{
							num4 = Math.Max(component2.Top, num4);
							num5 = Math.Max(component2.Left, num5);
						}
						else if ((!(component2 is StiPrimitive) || component2 is StiHorizontalLinePrimitive) && component2.Width != 0.0 && component2.Height != 0.0)
						{
							num4 = ((!(component2 is StiHorizontalLinePrimitive)) ? Math.Max(component2.Bottom, num4) : Math.Max(component2.Top, num4));
							num5 = Math.Max(component2.Right, num5);
						}
					}
				}
			}
			((SizeD)(ref result)).Width = Math.Max(num5, ((SizeD)(ref result)).Width);
			((SizeD)(ref result)).Height = Math.Max(num4, ((SizeD)(ref result)).Height);
		}
		return result;
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		foreach (object component in Components)
		{
			if (component is IStiGetFonts)
			{
				fonts.AddRange(((IStiGetFonts)((component is IStiGetFonts) ? component : null)).GetFonts());
			}
		}
		return fonts.Distinct().ToList();
	}

	public virtual bool RenderContainer(ref StiComponent renderedComponent, StiContainer outContainer)
	{
		StiContainerV1Builder stiContainerV1Builder = StiV1Builder.GetBuilder(((object)this).GetType()) as StiContainerV1Builder;
		return stiContainerV1Builder.RenderContainer(this, ref renderedComponent, outContainer);
	}

	protected void CheckBlockedException(string propertyName)
	{
		if (!Blocked)
		{
			return;
		}
		throw new InvalidOperationException("You can't change property " + propertyName + " of " + Name + " component because since this property presently blocked by operation of the report rendering. ");
	}

	private void OnComponentAdded(object sender, EventArgs e)
	{
		OnComponentAdded(e);
	}

	protected virtual void OnComponentAdded(EventArgs e)
	{
	}

	private void OnComponentRemoved(object sender, EventArgs e)
	{
		OnComponentRemoved(e);
	}

	protected virtual void OnComponentRemoved(EventArgs e)
	{
	}

	protected internal void SetParentStylesToChilds()
	{
		StiBaseStyle stiBaseStyle = null;
		bool flag = true;
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.UseParentStyles)
				{
					if (flag)
					{
						stiBaseStyle = StiBaseStyle.GetStyle(this);
						flag = false;
					}
					stiBaseStyle?.SetStyleToComponent(component);
				}
			}
		}
	}

	protected internal void SetParentStylesToChilds(StiBaseStyle style)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.UseParentStyles)
				{
					style.SetStyleToComponent(component);
				}
			}
		}
	}

	public void OffsetLocation(double offsetX, double offsetY)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				component.Left += offsetX;
				component.Top += offsetY;
			}
		}
	}

	public void ChangePosition(RectangleD delta)
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		if (this is IStiForm stiForm && IsSelected)
		{
			stiForm.Size = new Size(Math.Max(stiForm.Size.Width + (int)((RectangleD)(ref delta)).Width, 112), Math.Max(stiForm.Size.Height + (int)((RectangleD)(ref delta)).Height, 28));
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && !(component is IStiIgnoreOffset))
				{
					component.DisplayRectangle = StiComponent.DoOffsetRect(component, component.DisplayRectangle, delta);
				}
				else if (component is StiContainer stiContainer)
				{
					stiContainer.ChangePosition(delta);
				}
			}
		}
	}

	public void Normalize()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					RectangleD clientRectangle = component.ClientRectangle;
					component.ClientRectangle = ((RectangleD)(ref clientRectangle)).Normalize();
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.Normalize();
				}
			}
		}
	}

	public void SetLink(bool linked)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					component.Linked = linked;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetLink(linked);
				}
			}
		}
	}

	public object GetLink()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && StiRestrictionsHelper.IsAllowChange(component))
				{
					return component.Linked;
				}
				if (component is StiContainer stiContainer)
				{
					object link = stiContainer.GetLink();
					if (link != null)
					{
						return link;
					}
				}
			}
		}
		return null;
	}

	public void SetLock(bool locked)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					component.Locked = locked;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetLock(locked);
				}
			}
		}
	}

	public object GetLock()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && StiRestrictionsHelper.IsAllowChange(component))
				{
					return component.Locked;
				}
				if (component is StiContainer stiContainer)
				{
					object @lock = stiContainer.GetLock();
					if (@lock != null)
					{
						return @lock;
					}
				}
			}
		}
		return null;
	}

	public void SetBorder(StiBorder border)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		if (IsSelected)
		{
			((IStiBorder)this).Border.Side = border.Side;
			((IStiBorder)this).Border.DropShadow = border.DropShadow;
			((IStiBorder)this).Border.Color = border.Color;
			((IStiBorder)this).Border.Style = border.Style;
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiSimpleBorder && !(component is IStiHideBorderFromDesigner))
				{
					IStiBorder stiBorder = (IStiBorder)component;
					stiBorder.Border.Side = border.Side;
					stiBorder.Border.DropShadow = border.DropShadow;
					stiBorder.Border.Color = border.Color;
					stiBorder.Border.Style = border.Style;
				}
				else if (component.IsSelected && component is IStiBorder && !(component is IStiHideBorderFromDesigner))
				{
					IStiBorder stiBorder2 = (IStiBorder)component;
					stiBorder2.Border.Side = border.Side;
					stiBorder2.Border.DropShadow = border.DropShadow;
					stiBorder2.Border.Color = border.Color;
					stiBorder2.Border.Style = border.Style;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetBorder(border);
				}
			}
		}
	}

	public object GetBorder()
	{
		if (IsSelected)
		{
			if (!(this is IStiDashboard))
			{
				return Border;
			}
			return null;
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				if (component.IsSelected && (component is IStiBorder || component is IStiSimpleBorder) && !(component is IStiHideBorderFromDesigner) && (stiStyle == null || !stiStyle.AllowUseBorderFormatting || !stiStyle.AllowUseBorderSides) && StiRestrictionsHelper.IsAllowChange(component))
				{
					if (component is StiGauge && ((StiGauge)component).AllowApplyStyle)
					{
						return null;
					}
					if (component is IStiSimpleBorder)
					{
						return ((IStiSimpleBorder)component).Border;
					}
					if (component is IStiBorder)
					{
						return ((IStiBorder)component).Border;
					}
				}
				if (component is StiContainer stiContainer)
				{
					object border = stiContainer.GetBorder();
					if (border != null)
					{
						return border;
					}
				}
			}
		}
		return null;
	}

	public object GetBorderColor()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiBorderColor stiBorderColor && StiRestrictionsHelper.IsAllowChange(component))
				{
					return stiBorderColor.BorderColor;
				}
				if (component is StiContainer stiContainer)
				{
					object borderColor = stiContainer.GetBorderColor();
					if (borderColor != null)
					{
						return borderColor;
					}
				}
			}
		}
		return null;
	}

	public object GetSimpleShadow()
	{
		if (IsSelected && this is IStiSimpleShadow)
		{
			return (this as IStiSimpleShadow)?.Shadow;
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiSimpleShadow stiSimpleShadow)
				{
					return stiSimpleShadow.Shadow;
				}
				if (component is StiContainer stiContainer)
				{
					object simpleShadow = stiContainer.GetSimpleShadow();
					if (simpleShadow != null)
					{
						return simpleShadow;
					}
				}
			}
		}
		return null;
	}

	public void SetVertAlign(StiVertAlignment vl)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiVertAlignment stiVertAlignment)
				{
					stiVertAlignment.VertAlignment = vl;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetVertAlign(vl);
				}
			}
		}
	}

	public object GetVertAlignment()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				if (component.IsSelected && component is IStiVertAlignment && (stiStyle == null || !stiStyle.AllowUseVertAlignment) && StiRestrictionsHelper.IsAllowChange(component))
				{
					return ((IStiVertAlignment)component).VertAlignment;
				}
				if (component is StiContainer stiContainer)
				{
					object vertAlignment = stiContainer.GetVertAlignment();
					if (vertAlignment != null)
					{
						return vertAlignment;
					}
				}
			}
		}
		return null;
	}

	public void SetHorAlign(StiHorAlignment hl)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					List<object> formatObjects = component.GetFormatObjects();
					if (formatObjects != null && formatObjects.Count > 0)
					{
						formatObjects.Where((object o) => o is IStiHorAlignment).Cast<IStiHorAlignment>().ToList()
							.ForEach(delegate(IStiHorAlignment o)
							{
								//IL_0002: Unknown result type (might be due to invalid IL or missing references)
								o.HorAlignment = hl;
							});
					}
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetHorAlign(hl);
				}
			}
		}
	}

	public object GetHorAlignment()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				if (component.IsSelected && (stiStyle == null || !stiStyle.AllowUseHorAlignment) && StiRestrictionsHelper.IsAllowChange(component) && component.GetFirstFormatObject() is IStiHorAlignment stiHorAlignment)
				{
					return stiHorAlignment.HorAlignment;
				}
				if (component is StiContainer stiContainer)
				{
					object horAlignment = stiContainer.GetHorAlignment();
					if (horAlignment != null)
					{
						return horAlignment;
					}
				}
			}
		}
		return null;
	}

	public void SetTextHorAlign(StiTextHorAlignment hl)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiTextHorAlignment)
				{
					((IStiTextHorAlignment)component).HorAlignment = hl;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetTextHorAlign(hl);
				}
			}
		}
	}

	public object GetTextHorAlignment()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				if (component.IsSelected && component is IStiTextHorAlignment && (stiStyle == null || !stiStyle.AllowUseHorAlignment) && StiRestrictionsHelper.IsAllowChange(component))
				{
					return ((IStiTextHorAlignment)component).HorAlignment;
				}
				if (component is StiContainer stiContainer)
				{
					object textHorAlignment = stiContainer.GetTextHorAlignment();
					if (textHorAlignment != null)
					{
						return textHorAlignment;
					}
				}
			}
		}
		return null;
	}

	public void SetCrossHorAlign(StiCrossHorAlignment vl)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is StiCrossTab)
				{
					((StiCrossTab)component).HorAlignment = vl;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetCrossHorAlign(vl);
				}
			}
		}
	}

	public object GetCrossHorAlignment()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				if (component.IsSelected && component is StiCrossTab && (stiStyle == null || !stiStyle.AllowUseHorAlignment) && StiRestrictionsHelper.IsAllowChange(component))
				{
					return ((StiCrossTab)component).HorAlignment;
				}
				if (component is StiContainer stiContainer)
				{
					object textHorAlignment = stiContainer.GetTextHorAlignment();
					if (textHorAlignment != null)
					{
						return textHorAlignment;
					}
				}
			}
		}
		return null;
	}

	public object GetWordWrap()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiTextOptions && StiRestrictionsHelper.IsAllowChange(component))
				{
					return ((IStiTextOptions)component).TextOptions.WordWrap;
				}
				if (component.IsSelected && component is StiCrossTab && StiRestrictionsHelper.IsAllowChange(component))
				{
					return ((StiCrossTab)component).Wrap;
				}
				if (component is StiContainer stiContainer)
				{
					object wordWrap = stiContainer.GetWordWrap();
					if (wordWrap != null)
					{
						return wordWrap;
					}
				}
			}
		}
		return null;
	}

	public void SetWordWrap(bool wordWrap)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiTextOptions && StiRestrictionsHelper.IsAllowChange(component))
				{
					((IStiTextOptions)component).TextOptions.WordWrap = wordWrap;
				}
				else if (component.IsSelected && component is StiCrossTab && StiRestrictionsHelper.IsAllowChange(component))
				{
					((StiCrossTab)component).Wrap = wordWrap;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetWordWrap(wordWrap);
				}
			}
		}
	}

	public void SetFont(Font font)
	{
		SetFont(font, setFontFamily: true, setSize: true, setStyleBold: true, setStyleItalic: true, setStyleUnderline: true, setStyleStrikeout: true, setUnit: true, setGdiCharSet: true, setGdiVerticalFont: true);
	}

	public void SetFont(Font font, bool setFontFamily, bool setSize, bool setStyleBold, bool setStyleItalic, bool setStyleUnderline, bool setStyleStrikeout, bool setUnit, bool setGdiCharSet, bool setGdiVerticalFont)
	{
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && (component is IStiFont || component is IStiTextFont))
				{
					Font val = ((component is IStiFont) ? ((IStiFont)component).Font : ((IStiTextFont)component).GetFont());
					FontFamily val2 = (setFontFamily ? font.FontFamily : val.FontFamily);
					float num = (setSize ? font.Size : val.Size);
					bool flag = (setStyleBold ? font.Bold : val.Bold);
					bool flag2 = (setStyleItalic ? font.Italic : val.Italic);
					bool flag3 = (setStyleUnderline ? font.Underline : val.Underline);
					bool flag4 = (setStyleStrikeout ? font.Strikeout : val.Strikeout);
					byte b = (setGdiCharSet ? font.GdiCharSet : val.GdiCharSet);
					bool flag5 = (setGdiVerticalFont ? font.GdiVerticalFont : val.GdiVerticalFont);
					GraphicsUnit val3 = (setUnit ? font.Unit : val.Unit);
					if (val.FontFamily != val2 || val.Size != num || val.Bold != flag || val.Italic != flag2 || val.Underline != flag3 || val.Strikeout != flag4 || val.GdiCharSet != b || val.GdiVerticalFont != flag5 || val.Unit != val3)
					{
						FontStyle val4 = (FontStyle)0;
						if (flag)
						{
							val4 = (FontStyle)(val4 | 1);
						}
						if (flag2)
						{
							val4 = (FontStyle)(val4 | 2);
						}
						if (flag3)
						{
							val4 = (FontStyle)(val4 | 4);
						}
						if (flag4)
						{
							val4 = (FontStyle)(val4 | 8);
						}
						if (component is IStiFont)
						{
							((IStiFont)component).Font = new Font(val2, num, StiFontUtils.CorrectStyle(val2.Name, val4), val3, b, flag5);
						}
						else
						{
							IStiTextFont stiTextFont = component as IStiTextFont;
							if (setFontFamily)
							{
								stiTextFont.SetFontName(val2.Name);
							}
							if (setSize)
							{
								stiTextFont.SetFontSize(num);
							}
							if (setStyleBold)
							{
								stiTextFont.SetFontBoldStyle(flag);
							}
							if (setStyleItalic)
							{
								stiTextFont.SetFontItalicStyle(flag2);
							}
							if (setStyleUnderline)
							{
								stiTextFont.SetFontUnderlineStyle(flag3);
							}
						}
					}
					if (component is IStiTextFont && val != null)
					{
						val.Dispose();
					}
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetFont(font, setFontFamily, setSize, setStyleBold, setStyleItalic, setStyleUnderline, setStyleStrikeout, setUnit, setGdiCharSet, setGdiVerticalFont);
				}
			}
		}
	}

	public object GetFont()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				if (component.IsSelected && (stiStyle == null || !stiStyle.AllowUseFont) && StiRestrictionsHelper.IsAllowChange(component))
				{
					if (component is IStiFont)
					{
						return ((IStiFont)component).Font;
					}
					if (component is IStiTextFont)
					{
						return ((IStiTextFont)component).GetFont();
					}
				}
				if (component is StiContainer stiContainer)
				{
					object font = stiContainer.GetFont();
					if (font != null)
					{
						return font;
					}
				}
			}
		}
		return null;
	}

	public void ChangeFontName(string fontName)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					if (component is IStiFont && component.IsSelected)
					{
						((IStiFont)component).Font = StiFontUtils.ChangeFontName(((IStiFont)component).Font, fontName);
					}
					if (component is IStiTextFont && component.IsSelected)
					{
						((IStiTextFont)component).SetFontName(fontName);
					}
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.ChangeFontName(fontName);
				}
			}
		}
	}

	public void ChangeFontSize(float fontSize)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component is IStiFont && component.IsSelected)
				{
					((IStiFont)component).Font = StiFontUtils.ChangeFontSize(((IStiFont)component).Font, fontSize);
				}
				if (component is IStiTextFont && component.IsSelected)
				{
					((IStiTextFont)component).SetFontSize(fontSize);
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.ChangeFontSize(fontSize);
				}
			}
		}
	}

	public void GrowFont()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component is IStiFont && component.IsSelected)
				{
					((IStiFont)component).Font = StiFontUtils.ChangeFontSize(((IStiFont)component).Font, ((IStiFont)component).Font.Size + 1f);
				}
				if (component is IStiTextFont && component.IsSelected)
				{
					((IStiTextFont)component).GrowFontSize();
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.GrowFont();
				}
			}
		}
	}

	public void ShrinkFont()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component is IStiFont && component.IsSelected && ((IStiFont)component).Font.Size - 1f > 0f)
				{
					((IStiFont)component).Font = StiFontUtils.ChangeFontSize(((IStiFont)component).Font, ((IStiFont)component).Font.Size - 1f);
				}
				if (component is IStiTextFont && component.IsSelected)
				{
					((IStiTextFont)component).ShrinkFontSize();
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.ShrinkFont();
				}
			}
		}
	}

	public void SetBrush(StiBrush brush)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		if (IsSelected)
		{
			Brush = (StiBrush)brush.Clone();
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiBrush)
				{
					IStiBrush stiBrush = (IStiBrush)component;
					stiBrush.Brush = (StiBrush)brush.Clone();
				}
				if (component.IsSelected && component is IStiBackColor)
				{
					IStiBackColor stiBackColor = (IStiBackColor)component;
					stiBackColor.BackColor = StiBrush.ToColor(brush);
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetBrush(brush);
				}
			}
		}
	}

	public object GetBrush()
	{
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		if (IsSelected)
		{
			return Brush;
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				if (component.IsSelected && (component is IStiBrush || component is IStiBackColor) && (stiStyle == null || !stiStyle.AllowUseBrush) && StiRestrictionsHelper.IsAllowChange(component))
				{
					if (component is StiGauge && ((StiGauge)component).AllowApplyStyle)
					{
						return null;
					}
					if (component is IStiBackColor)
					{
						return (object)new StiSolidBrush(((IStiBackColor)component).BackColor);
					}
					return ((IStiBrush)component).Brush;
				}
				if (component is StiContainer stiContainer)
				{
					object brush = stiContainer.GetBrush();
					if (brush != null)
					{
						return brush;
					}
				}
			}
		}
		return null;
	}

	public void SetTextFormat(StiFormatService format)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					List<object> formatObjects = component.GetFormatObjects();
					if (formatObjects != null && formatObjects.Count > 0)
					{
						formatObjects.Where((object o) => o is IStiTextFormat).Cast<IStiTextFormat>().ToList()
							.ForEach(delegate(IStiTextFormat o)
							{
								o.TextFormat = (StiFormatService)((StiService)format).Clone();
							});
					}
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetTextFormat(format);
				}
			}
		}
	}

	public StiFormatService GetTextFormat()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && StiRestrictionsHelper.IsAllowChange(component) && component.GetFirstFormatObject() is IStiTextFormat { TextFormat: var textFormat })
				{
					return textFormat;
				}
				if (component is StiContainer stiContainer)
				{
					StiFormatService textFormat2 = stiContainer.GetTextFormat();
					if (textFormat2 != null)
					{
						return textFormat2;
					}
				}
			}
		}
		return null;
	}

	public void SetTextBrush(StiBrush brush)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component is IStiTextBrush)
				{
					List<object> formatObjects = component.GetFormatObjects();
					if (formatObjects != null && formatObjects.Count > 0)
					{
						formatObjects.Where((object o) => o is IStiTextBrush).Cast<IStiTextBrush>().ToList()
							.ForEach(delegate(IStiTextBrush o)
							{
								o.TextBrush = brush;
							});
						continue;
					}
					IStiTextBrush stiTextBrush = (IStiTextBrush)component;
					stiTextBrush.TextBrush = (StiBrush)brush.Clone();
				}
				if (component.IsSelected && component is IStiForeColor)
				{
					List<object> formatObjects2 = component.GetFormatObjects();
					if (formatObjects2 != null && formatObjects2.Count > 0)
					{
						formatObjects2.Where((object o) => o is IStiForeColor).Cast<IStiForeColor>().ToList()
							.ForEach(delegate(IStiForeColor o)
							{
								o.ForeColor = StiBrush.ToColor(brush);
							});
						continue;
					}
					IStiForeColor stiForeColor = (IStiForeColor)component;
					stiForeColor.ForeColor = StiBrush.ToColor(brush);
				}
				IStiForeColor stiForeColor2 = component.GetFirstFormatObject() as IStiForeColor;
				if (component.IsSelected && stiForeColor2 != null)
				{
					stiForeColor2.ForeColor = StiBrush.ToColor(brush);
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetTextBrush(brush);
				}
			}
		}
	}

	public object GetTextBrush()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiStyle stiStyle = (StiOptions.Designer.PropertyGrid.ShowPropertiesWhichUsedFromStyles ? null : (component.GetComponentStyle() as StiStyle));
				IStiForeColor stiForeColor = component.GetFirstFormatObject() as IStiForeColor;
				if (component.IsSelected && (component is IStiTextBrush || component is IStiForeColor || stiForeColor != null) && (stiStyle == null || !stiStyle.AllowUseTextBrush) && StiRestrictionsHelper.IsAllowChange(component))
				{
					if (stiForeColor != null)
					{
						return (object)new StiSolidBrush(stiForeColor.ForeColor);
					}
					if (component is IStiForeColor)
					{
						return (object)new StiSolidBrush(((IStiForeColor)component).ForeColor);
					}
					return ((IStiTextBrush)component).TextBrush;
				}
				if (component is StiContainer stiContainer)
				{
					object textBrush = stiContainer.GetTextBrush();
					if (textBrush != null)
					{
						return textBrush;
					}
				}
			}
		}
		return null;
	}

	public void SetConditions(StiConditionsCollection conditions)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					component.Conditions = conditions.Clone() as StiConditionsCollection;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetConditions(conditions);
				}
			}
		}
	}

	public StiConditionsCollection GetConditions()
	{
		StiConditionsCollection stiConditionsCollection = new StiConditionsCollection();
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && component.Conditions.Count > 0 && StiRestrictionsHelper.IsAllowChange(component))
				{
					stiConditionsCollection.AddRange(component.Conditions, addOnlyNotEqual: true);
				}
				if (component is StiContainer stiContainer)
				{
					stiConditionsCollection.AddRange(stiContainer.GetConditions(), addOnlyNotEqual: true);
				}
			}
			return stiConditionsCollection;
		}
	}

	public void SetDockStyle(StiDockStyle dockStyle)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && !component.IsAutomaticDock)
				{
					component.DockStyle = dockStyle;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetDockStyle(dockStyle);
				}
			}
		}
	}

	public object GetDockStyle()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && !component.IsAutomaticDock && StiRestrictionsHelper.IsAllowChange(component))
				{
					return component.DockStyle;
				}
				if (component is StiContainer stiContainer)
				{
					object dockStyle = stiContainer.GetDockStyle();
					if (dockStyle != null)
					{
						return dockStyle;
					}
				}
			}
		}
		return null;
	}

	public RectangleD GetSelectedRectangleWithChilds()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		RectangleD result = RectangleD.Empty;
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					result = (((RectangleD)(ref result)).IsEmpty ? component.GetDisplayRectangle() : ((RectangleD)(ref result)).FitToRectangle(component.GetDisplayRectangle()));
				}
				if (component is StiContainer stiContainer)
				{
					RectangleD val = (component.IsSelected ? stiContainer.GetRectangle() : stiContainer.GetSelectedRectangleWithChilds());
					if (!((RectangleD)(ref val)).IsEmpty)
					{
						result = ((RectangleD)(ref result)).FitToRectangle(val);
					}
				}
			}
			return result;
		}
	}

	public RectangleD GetSelectedRectangle()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		RectangleD result = RectangleD.Empty;
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					result = (((RectangleD)(ref result)).IsEmpty ? component.GetDisplayRectangle() : ((RectangleD)(ref result)).FitToRectangle(component.GetDisplayRectangle()));
				}
				if (component is StiContainer stiContainer)
				{
					RectangleD selectedRectangle = stiContainer.GetSelectedRectangle();
					if (!((RectangleD)(ref selectedRectangle)).IsEmpty)
					{
						result = ((RectangleD)(ref result)).FitToRectangle(selectedRectangle);
					}
				}
			}
			return result;
		}
	}

	public RectangleD GetRectangle()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		RectangleD result = ComponentToPage(DisplayRectangle);
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				result = (((RectangleD)(ref result)).IsEmpty ? ContainerToPage(component.DisplayRectangle) : ((RectangleD)(ref result)).FitToRectangle(ContainerToPage(component.DisplayRectangle)));
				if (component is StiContainer stiContainer)
				{
					RectangleD rectangle = stiContainer.GetRectangle();
					if (!((RectangleD)(ref rectangle)).IsEmpty)
					{
						result = ((RectangleD)(ref result)).FitToRectangle(rectangle);
					}
				}
			}
			return result;
		}
	}

	public SizeD GetMaxSize()
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		SizeD result = default(SizeD);
		((SizeD)(ref result))._002Ector(0.0, 0.0);
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					if (((SizeD)(ref result)).Width < component.Width)
					{
						((SizeD)(ref result)).Width = component.Width;
					}
					if (((SizeD)(ref result)).Height < component.Height)
					{
						((SizeD)(ref result)).Height = component.Height;
					}
				}
				if (component is StiContainer stiContainer)
				{
					SizeD maxSize = stiContainer.GetMaxSize();
					if (((SizeD)(ref result)).Width < ((SizeD)(ref maxSize)).Width)
					{
						((SizeD)(ref result)).Width = ((SizeD)(ref maxSize)).Width;
					}
					if (((SizeD)(ref result)).Height < ((SizeD)(ref maxSize)).Height)
					{
						((SizeD)(ref result)).Height = ((SizeD)(ref maxSize)).Height;
					}
				}
			}
			return result;
		}
	}

	public void MakeSameSize(SizeD size)
	{
		StiComponentsCollection selectedComponents = GetSelectedComponents();
		lock (((ICollection)selectedComponents).SyncRoot)
		{
			foreach (StiComponent item in selectedComponents)
			{
				item.Width = ((SizeD)(ref size)).Width;
				item.Height = ((SizeD)(ref size)).Height;
			}
		}
	}

	public void MakeSameWidth(double width)
	{
		StiComponentsCollection selectedComponents = GetSelectedComponents();
		lock (((ICollection)selectedComponents).SyncRoot)
		{
			foreach (StiComponent item in selectedComponents)
			{
				item.Width = width;
			}
		}
	}

	public void MakeSameHeight(double height)
	{
		StiComponentsCollection selectedComponents = GetSelectedComponents();
		lock (((ICollection)selectedComponents).SyncRoot)
		{
			foreach (StiComponent item in selectedComponents)
			{
				item.Height = height;
			}
		}
	}

	public void MakeHorizontalSpacingEqual()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		StiComponentsCollection selectedComponents = GetSelectedComponents();
		RectangleD selectedRectangle = GetSelectedRectangle();
		double num = 0.0;
		lock (((ICollection)selectedComponents).SyncRoot)
		{
			foreach (StiComponent item in selectedComponents)
			{
				num += item.Width;
			}
		}
		double num2 = (((RectangleD)(ref selectedRectangle)).Width - num) / (double)Math.Max(selectedComponents.Count - 1, 1);
		selectedComponents.SortByLeftPosition();
		double num3 = selectedComponents[0].Right + num2;
		for (int i = 1; i < selectedComponents.Count - 1; i++)
		{
			selectedComponents[i].Left = num3;
			num3 = selectedComponents[i].Right + num2;
		}
	}

	public void MakeVerticalSpacingEqual()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		StiComponentsCollection selectedComponents = GetSelectedComponents();
		RectangleD selectedRectangle = GetSelectedRectangle();
		double num = 0.0;
		lock (((ICollection)selectedComponents).SyncRoot)
		{
			foreach (StiComponent item in selectedComponents)
			{
				num += item.Height;
			}
		}
		double num2 = (((RectangleD)(ref selectedRectangle)).Height - num) / (double)Math.Max(selectedComponents.Count - 1, 1);
		selectedComponents.SortByTopPosition();
		double num3 = selectedComponents[0].Bottom + num2;
		for (int i = 1; i < selectedComponents.Count - 1; i++)
		{
			selectedComponents[i].Top = num3;
			num3 = selectedComponents[i].Bottom + num2;
		}
	}

	public void SetCenterHorizontally()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					component.Left = (component.Parent.Width - component.Width) / 2.0;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetCenterHorizontally();
				}
			}
		}
	}

	public void SetCenterVertically()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					component.Top = (component.Parent.Height - component.Height) / 2.0;
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.SetCenterVertically();
				}
			}
		}
	}

	public void AlignToGrid()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected && !component.Locked && !component.Inherited && StiRestrictionsHelper.IsAllowChangePosition(component))
				{
					RectangleD clientRectangle = component.ClientRectangle;
					component.ClientRectangle = ((RectangleD)(ref clientRectangle)).AlignToGrid(Page.GridSize, true);
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.AlignToGrid();
				}
			}
		}
	}

	public void AlignToGrid(StiComponent comp)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (comp.IsSelected && !comp.Locked && !comp.Inherited && StiRestrictionsHelper.IsAllowChangePosition(comp))
		{
			RectangleD clientRectangle = comp.ClientRectangle;
			comp.ClientRectangle = ((RectangleD)(ref clientRectangle)).AlignToGrid(Page.GridSize, true);
		}
		if (comp is StiContainer stiContainer)
		{
			stiContainer.AlignToGrid();
		}
	}

	public void SortByPriority()
	{
		Components.SortByPriority();
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component is StiContainer stiContainer)
				{
					stiContainer.SortByPriority();
				}
			}
		}
	}

	public void AlignTo(StiAligning aligning)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		StiComponentsCollection selectedComponents = GetSelectedComponents();
		bool flag = true;
		for (int i = 0; i < selectedComponents.Count; i++)
		{
			for (int j = 0; j < selectedComponents.Count; j++)
			{
				if (i != j && selectedComponents[i].SelectionTick == selectedComponents[j].SelectionTick)
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		StiComponentsCollection stiComponentsCollection = (StiComponentsCollection)selectedComponents.Clone();
		stiComponentsCollection.SortBySelectionTick();
		RectangleD val = (flag ? stiComponentsCollection[0].ClientRectangle : GetSelectedRectangle());
		switch (aligning)
		{
		case StiAligning.Left:
		{
			selectedComponents.SortByLeftPosition();
			for (int n = 0; n < selectedComponents.Count; n++)
			{
				selectedComponents[n].Left = ((RectangleD)(ref val)).Left;
			}
			break;
		}
		case StiAligning.Center:
		{
			for (int num2 = 0; num2 < selectedComponents.Count; num2++)
			{
				selectedComponents[num2].Left = ((RectangleD)(ref val)).Left + (((RectangleD)(ref val)).Width - selectedComponents[num2].Width) / 2.0;
			}
			break;
		}
		case StiAligning.Right:
		{
			selectedComponents.SortByRightPosition();
			for (int l = 0; l < selectedComponents.Count; l++)
			{
				selectedComponents[l].Left = ((RectangleD)(ref val)).Right - selectedComponents[l].Width;
			}
			break;
		}
		case StiAligning.Top:
		{
			selectedComponents.SortByTopPosition();
			for (int num = 0; num < selectedComponents.Count; num++)
			{
				selectedComponents[num].Top = ((RectangleD)(ref val)).Top;
			}
			break;
		}
		case StiAligning.Middle:
		{
			for (int m = 0; m < selectedComponents.Count; m++)
			{
				selectedComponents[m].Top = ((RectangleD)(ref val)).Top + (((RectangleD)(ref val)).Height - selectedComponents[m].Height) / 2.0;
			}
			break;
		}
		case StiAligning.Bottom:
		{
			selectedComponents.SortByBottomPosition();
			for (int k = 0; k < selectedComponents.Count; k++)
			{
				selectedComponents[k].Top = ((RectangleD)(ref val)).Bottom - selectedComponents[k].Height;
			}
			break;
		}
		}
	}

	public void BringToFront()
	{
		int num = Components.Count;
		int num2 = 0;
		while (num2 < num)
		{
			StiComponent stiComponent = Components[num2];
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.BringToFront();
			}
			if (stiComponent.IsSelected)
			{
				Components.Remove(stiComponent);
				Components.Add(stiComponent);
				num--;
			}
			else
			{
				num2++;
			}
		}
		SortByPriority();
	}

	public void SendToBack()
	{
		int count = Components.Count;
		for (int i = 0; i < count; i++)
		{
			StiComponent stiComponent = Components[i];
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.SendToBack();
			}
			if (stiComponent.IsSelected)
			{
				Components.Remove(stiComponent);
				Components.Insert(0, stiComponent);
			}
		}
		SortByPriority();
	}

	public void MoveForward()
	{
		for (int num = Components.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = Components[num];
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.MoveForward();
			}
			if (stiComponent.IsSelected)
			{
				int num2 = Components.IndexOf(stiComponent);
				if (num2 < Components.Count - 1)
				{
					Components.Remove(stiComponent);
					Components.Insert(num2 + 1, stiComponent);
				}
			}
		}
		SortByPriority();
	}

	public void MoveBackward()
	{
		int count = Components.Count;
		for (int i = 0; i < count; i++)
		{
			StiComponent stiComponent = Components[i];
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.MoveBackward();
			}
			if (stiComponent.IsSelected)
			{
				int num = Components.IndexOf(stiComponent);
				if (num > 0)
				{
					Components.Remove(stiComponent);
					Components.Insert(num - 1, stiComponent);
				}
			}
		}
		SortByPriority();
	}

	private void ControlInPoint(ref StiComponent obj, ref StiAction action, double x, double y, StiGuiMode guiMode, bool forGetDesigner = false)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected I4, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		PointD val = this.PageToContainer(new PointD(x, y));
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(null, ((object)component).GetType(), guiMode);
				if (componentDesigner != null)
				{
					StiAction val2 = (forGetDesigner ? componentDesigner.GetActionFromPoint(((PointD)(ref val)).X, ((PointD)(ref val)).Y, component, forGetDesigner: true) : componentDesigner.GetActionFromPoint(((PointD)(ref val)).X, ((PointD)(ref val)).Y, component));
					if ((int)val2 != 0 && ((int)val2 != 1 || ((int)val2 == 1 && (int)action == 1) || (int)action == 0))
					{
						action = (StiAction)(int)val2;
						obj = component;
					}
					if (component is StiContainer stiContainer)
					{
						stiContainer.ControlInPoint(ref obj, ref action, x, y, guiMode, forGetDesigner);
					}
				}
			}
		}
	}

	public StiComponent GetComponentInControl(double x, double y, bool usePage)
	{
		return GetComponentInControl(x, y, usePage, (StiGuiMode)0, forGetDesigner: false);
	}

	public StiComponent GetComponentInControl(double x, double y, bool usePage, StiGuiMode guiMode)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return GetComponentInControl(x, y, usePage, guiMode, forGetDesigner: false);
	}

	public StiComponent GetComponentInControl(double x, double y, bool usePage, StiGuiMode guiMode, bool forGetDesigner = false)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		StiComponent obj = null;
		StiAction action = (StiAction)0;
		ControlInPoint(ref obj, ref action, x, y, guiMode, forGetDesigner);
		if (obj == null && usePage)
		{
			return Page;
		}
		return obj;
	}

	public StiComponent GetComponentInPoint(double x, double y)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		StiComponent result = null;
		PointD val = this.PageToContainer(new PointD(x, y));
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (StiActionUtils.PointInRect(((PointD)(ref val)).X, ((PointD)(ref val)).Y, component.ClientRectangle))
				{
					result = component;
				}
				if (component is StiContainer stiContainer)
				{
					StiComponent componentInPoint = stiContainer.GetComponentInPoint(x, y);
					if (componentInPoint != null)
					{
						result = componentInPoint;
					}
				}
			}
			return result;
		}
	}

	private StiContainer getContainerInRect(RectangleD rect, StiComponent component)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = null;
		RectangleD val = PageToContainer(rect);
		StiComponentsCollection stiComponentsCollection = GetComponents();
		for (int num = stiComponentsCollection.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = stiComponentsCollection[num];
			if (stiComponent != component && !stiComponent.CheckForParentComponent(component) && stiComponent is StiContainer stiContainer2 && component.CanContainIn(stiContainer2))
			{
				RectangleD val2 = PageToContainer(stiComponent.ComponentToPage(stiComponent.ClientRectangle));
				if (((RectangleD)(ref val2)).IntersectsWith(val) && component.IsCross)
				{
					stiContainer = stiContainer2;
				}
				if (stiComponent is StiBand && !stiComponent.IsCross)
				{
					double num2 = Page.Unit.ConvertFromHInches(1.0);
					if (component is StiPrimitive)
					{
						((RectangleD)(ref val2)).Width = ((RectangleD)(ref val2)).Width + num2;
					}
					if (Report != null && Report.Info.ShowHeaders)
					{
						((RectangleD)(ref val2)).Height = ((RectangleD)(ref val2)).Height + num2;
						double num3 = Page.Unit.ConvertFromHInches((stiComponent as StiBand).HeaderSize);
						val2 = ((RectangleD)(ref val2)).OffsetRect(new RectangleD(0.0, num3, 0.0, num3));
					}
				}
				if (component is StiEndPointPrimitive || component is StiStartPointPrimitive)
				{
					decimal num4 = Math.Round((decimal)((RectangleD)(ref val)).X, 2);
					decimal num5 = Math.Round((decimal)((RectangleD)(ref val)).Y, 2);
					decimal num6 = Math.Round((decimal)((RectangleD)(ref val2)).Left, 2);
					decimal num7 = Math.Round((decimal)((RectangleD)(ref val2)).Top, 2);
					decimal num8 = Math.Round((decimal)((RectangleD)(ref val2)).Right, 2);
					decimal num9 = Math.Round((decimal)((RectangleD)(ref val2)).Bottom, 2);
					if (num7 <= num5 && num9 >= num5 && num6 <= num4 && num8 >= num4)
					{
						stiContainer = stiContainer2;
					}
				}
				if (StiActionUtils.PointInRect(((RectangleD)(ref val)).X, ((RectangleD)(ref val)).Y, val2))
				{
					stiContainer = stiContainer2;
				}
				StiContainer containerInRect = stiContainer2.getContainerInRect(rect, component);
				if (containerInRect != null)
				{
					stiContainer = containerInRect;
				}
			}
		}
		if (component.IsCross)
		{
			return stiContainer;
		}
		if (component is StiTable && stiContainer is StiTable)
		{
			return null;
		}
		if (component is StiTable && stiContainer is StiDataBand)
		{
			return stiContainer;
		}
		if (!(component is IStiTableCell) && stiContainer is StiTable)
		{
			return null;
		}
		if (component is IStiTableCell && !(stiContainer is StiTable))
		{
			return null;
		}
		if (component is StiBand && stiContainer is StiBand)
		{
			return null;
		}
		return stiContainer;
	}

	public StiContainer GetContainerInRect(RectangleD rect, StiComponent component)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (component.Page != null && component is StiBand)
		{
			double num = component.Page.Unit.ConvertFromHInches((component as StiBand).HeaderSize);
			rect = ((RectangleD)(ref rect)).OffsetRect(new RectangleD(0.0, num, 0.0, num));
		}
		StiContainer containerInRect = getContainerInRect(rect, component);
		if (containerInRect == null)
		{
			return Page;
		}
		if (containerInRect.CheckForParentComponent(component))
		{
			return Page;
		}
		return containerInRect;
	}

	public StiComponentsCollection GetIncorrect()
	{
		return GetIncorrect(onlySelect: false);
	}

	private Hashtable GetSizesTable(StiComponent component)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Hashtable hashtable = new Hashtable();
		RectangleD baseRect = default(RectangleD);
		GetNodeSize(hashtable, component, ref baseRect);
		return hashtable;
	}

	private void GetNodeSize(Hashtable hash, StiComponent component, ref RectangleD baseRect)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		RectangleD baseRect2 = ContainerToPage(component.ClientRectangle);
		if (component.Page != null && component is StiBand && !component.IsCross)
		{
			double num = component.Page.Unit.ConvertFromHInches((component as StiBand).HeaderSize);
			baseRect2 = ((RectangleD)(ref baseRect2)).OffsetRect(new RectangleD(0.0, num, 0.0, num));
			double num2 = Page.Unit.ConvertFromHInches(1.0);
			((RectangleD)(ref baseRect2)).Width = ((RectangleD)(ref baseRect2)).Width + num2;
			if (Report != null && Report.Info.ShowHeaders)
			{
				((RectangleD)(ref baseRect2)).Height = ((RectangleD)(ref baseRect2)).Height + num2;
			}
		}
		if (component is StiContainer stiContainer)
		{
			foreach (StiComponent component2 in stiContainer.Components)
			{
				stiContainer.GetNodeSize(hash, component2, ref baseRect2);
			}
		}
		if (((RectangleD)(ref baseRect2)).X < ((RectangleD)(ref baseRect)).X)
		{
			((RectangleD)(ref baseRect)).Width = ((RectangleD)(ref baseRect)).Width + (((RectangleD)(ref baseRect)).X - ((RectangleD)(ref baseRect2)).X);
			((RectangleD)(ref baseRect)).X = ((RectangleD)(ref baseRect2)).X;
		}
		if (((RectangleD)(ref baseRect2)).Y < ((RectangleD)(ref baseRect)).Y)
		{
			((RectangleD)(ref baseRect)).Height = ((RectangleD)(ref baseRect)).Height + (((RectangleD)(ref baseRect)).Y - ((RectangleD)(ref baseRect2)).Y);
			((RectangleD)(ref baseRect)).Y = ((RectangleD)(ref baseRect2)).Y;
		}
		if (((RectangleD)(ref baseRect2)).X + ((RectangleD)(ref baseRect2)).Width > ((RectangleD)(ref baseRect)).X + ((RectangleD)(ref baseRect)).Width)
		{
			((RectangleD)(ref baseRect)).Width = ((RectangleD)(ref baseRect2)).X + ((RectangleD)(ref baseRect2)).Width - ((RectangleD)(ref baseRect)).X;
		}
		if (((RectangleD)(ref baseRect2)).Y + ((RectangleD)(ref baseRect2)).Height > ((RectangleD)(ref baseRect)).Y + ((RectangleD)(ref baseRect)).Height)
		{
			((RectangleD)(ref baseRect)).Height = ((RectangleD)(ref baseRect2)).Y + ((RectangleD)(ref baseRect2)).Height - ((RectangleD)(ref baseRect)).Y;
		}
		hash[component] = baseRect2;
	}

	private StiContainer getContainerInRect2(RectangleD rect, StiComponent component, Hashtable hash)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = null;
		RectangleD val = (RectangleD)hash[this];
		if (!StiActionUtils.PointInRect(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, val))
		{
			return null;
		}
		if (this == component || CheckForParentComponent(component))
		{
			return null;
		}
		StiComponentsCollection stiComponentsCollection = Components;
		for (int num = stiComponentsCollection.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = stiComponentsCollection[num];
			if (stiComponent != component && ((stiComponent.Width != 0.0 && stiComponent.Height != 0.0) || num >= stiComponentsCollection.Count - 1 || stiComponentsCollection[num + 1].Left != stiComponent.Left || stiComponentsCollection[num + 1].Top != stiComponent.Top) && stiComponent is StiContainer stiContainer2)
			{
				RectangleD val2 = (RectangleD)hash[stiComponent];
				if (StiActionUtils.PointInRect(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, val2))
				{
					StiContainer containerInRect = stiContainer2.getContainerInRect2(rect, component, hash);
					if (containerInRect != null)
					{
						stiContainer = containerInRect;
						break;
					}
				}
			}
		}
		if (stiContainer == null && Parent != null && component.CanContainIn(this))
		{
			val = Parent.ContainerToPage(ClientRectangle);
			if (Page != null && this is StiBand && !IsCross)
			{
				double num2 = Page.Unit.ConvertFromHInches((this as StiBand).HeaderSize);
				val = ((RectangleD)(ref val)).OffsetRect(new RectangleD(0.0, num2, 0.0, num2));
				double num3 = Page.Unit.ConvertFromHInches(1.0);
				if (component is StiPrimitive)
				{
					((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width + num3;
				}
				if (Report != null && Report.Info.ShowHeaders)
				{
					((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height + num3;
				}
			}
			if (StiActionUtils.PointInRect(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, val))
			{
				stiContainer = this;
			}
		}
		if (component.IsCross)
		{
			return stiContainer;
		}
		if (component is StiTable && stiContainer is StiTable)
		{
			return null;
		}
		if (component is StiTable && stiContainer is StiDataBand)
		{
			return stiContainer;
		}
		if (!(component is IStiTableCell) && stiContainer is StiTable)
		{
			return null;
		}
		if (component is IStiTableCell && !(stiContainer is StiTable))
		{
			return null;
		}
		if (component is StiBand && stiContainer is StiBand)
		{
			return null;
		}
		return stiContainer;
	}

	public StiContainer GetContainerInRect2(RectangleD rect, StiComponent component, Hashtable hash)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		bool flag = component is StiCrossDataBand || component is StiCrossHeaderBand || component is StiCrossFooterBand || component is StiCrossGroupHeaderBand || component is StiCrossGroupFooterBand;
		if (component.Page != null && component is StiBand && !flag)
		{
			double num = component.Page.Unit.ConvertFromHInches((component as StiBand).HeaderSize);
			rect = ((RectangleD)(ref rect)).OffsetRect(new RectangleD(0.0, num, 0.0, num));
		}
		StiContainer containerInRect = getContainerInRect2(rect, component, hash);
		if (containerInRect == null)
		{
			return Page;
		}
		if (containerInRect.CheckForParentComponent(component))
		{
			return Page;
		}
		return containerInRect;
	}

	public StiComponentsCollection GetIncorrect2(bool onlySelect)
	{
		return GetIncorrect2(onlySelect, null);
	}

	private StiComponentsCollection GetIncorrect2(bool onlySelect, Hashtable hash)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		if (hash == null)
		{
			hash = GetSizesTable(this);
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (!component.Linked && !component.Inherited && StiRestrictionsHelper.IsAllowChangePosition(component) && (!onlySelect || component.IsSelected))
				{
					RectangleD rect = ContainerToPage(component.ClientRectangle);
					StiComponent containerInRect = Page.GetContainerInRect2(rect, component, hash);
					if (containerInRect != component.Parent && !containerInRect.CheckForParentComponent(component))
					{
						RectangleD val = ContainerToPage(containerInRect.ClientRectangle);
						RectangleD val2 = ContainerToPage(component.ClientRectangle);
						if (component.Left != 0.0 || ((RectangleD)(ref val)).Left != ((RectangleD)(ref val2)).Left || component.DockStyle == StiDockStyle.None)
						{
							stiComponentsCollection.Add(component);
						}
					}
					else if (component.IsCross && !component.CanContainIn(component.Parent))
					{
						stiComponentsCollection.Add(component);
					}
				}
				if (component is StiContainer stiContainer)
				{
					stiComponentsCollection.AddRange(stiContainer.GetIncorrect2(onlySelect, hash));
				}
			}
			return stiComponentsCollection;
		}
	}

	public void Correct2(bool onlySelect)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		Hashtable sizesTable = GetSizesTable(this);
		StiComponentsCollection incorrect = GetIncorrect2(onlySelect, sizesTable);
		lock (((ICollection)incorrect).SyncRoot)
		{
			foreach (StiComponent item in incorrect)
			{
				if (item.Parent == null)
				{
					continue;
				}
				item.ClientRectangle = item.ComponentToPage(item.ClientRectangle);
				StiContainer parent = item.Parent;
				item.Parent.Components.Remove(item);
				StiContainer stiContainer = GetContainerInRect2(item.ClientRectangle, item, sizesTable);
				if (stiContainer == null)
				{
					stiContainer = Page;
				}
				if (item.CanContainIn(stiContainer))
				{
					item.ClientRectangle = stiContainer.PageToContainer(item.ClientRectangle);
					stiContainer.Components.Add(item);
					continue;
				}
				if (!(parent is StiPage))
				{
					item.ClientRectangle = item.PageToComponent(item.ClientRectangle);
					parent.Components.Add(item);
					continue;
				}
				StiLogService.Write(((object)this).GetType(), string.Format(StiLocalization.Get("Errors", "ContainerIsNotValidForComponent"), stiContainer.Name, item.Name));
				if (!StiOptions.Engine.HideMessages)
				{
					MessageBox.Show(string.Format(StiLocalization.Get("Errors", "ContainerIsNotValidForComponent"), stiContainer.Name, item.Name));
				}
			}
		}
		DockToContainer();
		SortByPriority();
	}

	public StiComponentsCollection GetIncorrect(bool onlySelect)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (!component.Linked && !component.Inherited && StiRestrictionsHelper.IsAllowChangePosition(component) && (!onlySelect || component.IsSelected))
				{
					RectangleD rect = ContainerToPage(component.ClientRectangle);
					StiContainer containerInRect = Page.GetContainerInRect(rect, component);
					if (containerInRect != component.Parent && !containerInRect.CheckForParentComponent(component))
					{
						RectangleD val = ContainerToPage(containerInRect.ClientRectangle);
						RectangleD val2 = ContainerToPage(component.ClientRectangle);
						if (component.Left != 0.0 || ((RectangleD)(ref val)).Left != ((RectangleD)(ref val2)).Left || component.DockStyle == StiDockStyle.None)
						{
							stiComponentsCollection.Add(component);
						}
					}
					else if (component.IsCross && !component.CanContainIn(component.Parent))
					{
						stiComponentsCollection.Add(component);
					}
				}
				if (component is StiContainer stiContainer)
				{
					stiComponentsCollection.AddRange(stiContainer.GetIncorrect(onlySelect));
				}
			}
			return stiComponentsCollection;
		}
	}

	public void Correct()
	{
		Correct(onlySelect: false);
	}

	public void Correct(bool onlySelect)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		if (StiOptions.Designer.UseComponentPlacementOptimization)
		{
			Correct2(onlySelect);
			CheckLargeHeight();
			return;
		}
		StiComponentsCollection incorrect = GetIncorrect(onlySelect);
		lock (((ICollection)incorrect).SyncRoot)
		{
			foreach (StiComponent item in incorrect)
			{
				if (item.Parent == null)
				{
					continue;
				}
				item.ClientRectangle = item.ComponentToPage(item.ClientRectangle);
				StiContainer parent = item.Parent;
				item.Parent.Components.Remove(item);
				StiContainer stiContainer = GetContainerInRect(item.ClientRectangle, item);
				if (stiContainer == null)
				{
					stiContainer = Page;
				}
				if (item.CanContainIn(stiContainer))
				{
					item.ClientRectangle = stiContainer.PageToContainer(item.ClientRectangle);
					stiContainer.Components.Add(item);
					continue;
				}
				if (!(parent is StiPage))
				{
					item.ClientRectangle = item.PageToComponent(item.ClientRectangle);
					parent.Components.Add(item);
					continue;
				}
				StiLogService.Write(((object)this).GetType(), string.Format(StiLocalization.Get("Errors", "ContainerIsNotValidForComponent"), stiContainer.Name, item.Name));
				if (!StiOptions.Engine.HideMessages)
				{
					MessageBox.Show(string.Format(StiLocalization.Get("Errors", "ContainerIsNotValidForComponent"), stiContainer.Name, item.Name));
				}
			}
		}
		DockToContainer();
		SortByPriority();
		CheckLargeHeight();
	}

	public void CheckLargeHeight(bool needFullCalculation = false)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (!(this is StiPage { LargeHeight: false } stiPage) || !StiOptions.Designer.AutoLargeHeight || !base.IsDesigning)
		{
			return;
		}
		double num = 0.0;
		double num2 = stiPage.Height;
		if (num2 <= 0.0)
		{
			return;
		}
		RectangleD displayRectangle;
		if (needFullCalculation)
		{
			foreach (StiComponent component in stiPage.Components)
			{
				if (!(component is StiBand) || component.IsCross)
				{
					continue;
				}
				if (component is StiPageFooterBand)
				{
					double num3 = num2;
					displayRectangle = component.DisplayRectangle;
					num2 = num3 - ((RectangleD)(ref displayRectangle)).Height;
					continue;
				}
				displayRectangle = component.DisplayRectangle;
				double bottom = ((RectangleD)(ref displayRectangle)).Height;
				if (component is StiContainer stiContainer)
				{
					foreach (StiComponent component2 in stiContainer.Components)
					{
						displayRectangle = component2.DisplayRectangle;
						if (((RectangleD)(ref displayRectangle)).Bottom > bottom)
						{
							displayRectangle = component2.DisplayRectangle;
							bottom = ((RectangleD)(ref displayRectangle)).Bottom;
						}
					}
				}
				num += bottom;
			}
		}
		else
		{
			foreach (StiComponent component3 in stiPage.Components)
			{
				if (!(component3 is StiBand) || component3.IsCross)
				{
					continue;
				}
				if (component3 is StiPageFooterBand)
				{
					num2 = Math.Min(num2, component3.Top);
					continue;
				}
				num = Math.Max(num, component3.Bottom);
				if (!(component3 is StiContainer stiContainer2))
				{
					continue;
				}
				displayRectangle = component3.DisplayRectangle;
				double num4 = ((RectangleD)(ref displayRectangle)).Top;
				foreach (StiComponent component4 in stiContainer2.Components)
				{
					displayRectangle = component4.DisplayRectangle;
					if (num4 + ((RectangleD)(ref displayRectangle)).Bottom > num)
					{
						displayRectangle = component4.DisplayRectangle;
						num = num4 + ((RectangleD)(ref displayRectangle)).Bottom;
					}
				}
			}
		}
		double num5 = 0.2;
		for (double num6 = stiPage.Unit.ConvertFromHInches(90.0); stiPage.PageHeight * num5 < num6; num5 += 0.2)
		{
		}
		double largeHeightAutoFactor = stiPage.LargeHeightAutoFactor;
		while ((decimal)stiPage.LargeHeightAutoFactor < 20m && num2 - num < stiPage.PageHeight * num5)
		{
			double num7 = stiPage.Height;
			double largeHeightAutoFactor2 = stiPage.LargeHeightAutoFactor;
			stiPage.LargeHeightAutoFactor += num5;
			if (stiPage.LargeHeightAutoFactor == largeHeightAutoFactor2)
			{
				break;
			}
			num2 += stiPage.Height - num7;
		}
		while (num2 - num > stiPage.PageHeight * (num5 * 2.0) && (decimal)stiPage.LargeHeightAutoFactor > 1m)
		{
			double num8 = stiPage.Height;
			double largeHeightAutoFactor3 = stiPage.LargeHeightAutoFactor;
			stiPage.LargeHeightAutoFactor -= num5;
			if (stiPage.LargeHeightAutoFactor == largeHeightAutoFactor3)
			{
				break;
			}
			num2 -= num8 - stiPage.Height;
		}
		if (stiPage.LargeHeightAutoFactor != largeHeightAutoFactor)
		{
			StiOptions.Engine.GlobalEvents.InvokeAutoLargeHeightChanged(stiPage.Report, new StiAutoLargeHeightEventArgs(stiPage.Report, stiPage));
		}
	}

	public StiComponent GetDockableComponent()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.DockStyle != StiDockStyle.None && component.IsSelected)
				{
					return component;
				}
			}
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component2 in Components)
			{
				if (component2 is StiContainer stiContainer)
				{
					StiComponent dockableComponent = stiContainer.GetDockableComponent();
					if (dockableComponent != null)
					{
						return dockableComponent;
					}
				}
			}
		}
		return null;
	}

	public StiComponent GetFirstSelectableCompanent()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					return component;
				}
			}
		}
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component2 in Components)
			{
				if (component2 is StiContainer stiContainer)
				{
					StiComponent firstSelectableCompanent = stiContainer.GetFirstSelectableCompanent();
					if (firstSelectableCompanent != null)
					{
						return firstSelectableCompanent;
					}
				}
			}
		}
		return null;
	}

	public void ResetSelection()
	{
		Reset();
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				component.Reset();
				if (component is StiContainer stiContainer)
				{
					stiContainer.ResetSelection();
				}
			}
		}
	}

	public StiComponentsCollection GetSelectedComponents()
	{
		StiComponentsCollection comps = new StiComponentsCollection();
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					comps.Add(component);
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.GetSelectedComponents(ref comps);
				}
			}
			return comps;
		}
	}

	public void GetSelectedComponents(ref StiComponentsCollection comps)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					comps.Add(component);
				}
				if (component is StiContainer stiContainer)
				{
					stiContainer.GetSelectedComponents(ref comps);
				}
			}
		}
	}

	public StiComponentsCollection GetSelectedComponentsWithChilds()
	{
		StiComponentsCollection comps = new StiComponentsCollection();
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					comps.Add(component);
				}
				if (component is StiContainer stiContainer)
				{
					if (component.IsSelected)
					{
						comps.AddRange(stiContainer.GetComponents());
					}
					else
					{
						stiContainer.GetSelectedComponentsWithChilds(ref comps);
					}
				}
			}
			return comps;
		}
	}

	private void GetSelectedComponentsWithChilds(ref StiComponentsCollection comps)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component.IsSelected)
				{
					comps.Add(component);
				}
				if (component is StiContainer stiContainer)
				{
					if (component.IsSelected)
					{
						stiContainer.GetComponents(ref comps);
					}
					else
					{
						stiContainer.GetSelectedComponentsWithChilds(ref comps);
					}
				}
			}
		}
	}

	public void SelectInRectangleCheckBands(RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		SelectInRectangle(rect);
		StiComponentsCollection selectedComponents = GetSelectedComponents();
		int num = 0;
		lock (((ICollection)selectedComponents).SyncRoot)
		{
			foreach (StiComponent item in selectedComponents)
			{
				if (item is StiBand)
				{
					num++;
				}
			}
		}
		if (num <= 0 || num >= selectedComponents.Count)
		{
			return;
		}
		lock (((ICollection)selectedComponents).SyncRoot)
		{
			foreach (StiComponent item2 in selectedComponents)
			{
				if (item2 is StiBand)
				{
					item2.Reset();
				}
			}
		}
	}

	public void SelectInRectangle(RectangleD rect)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		rect = ((RectangleD)(ref rect)).Normalize();
		for (int num = Components.Count - 1; num >= 0; num--)
		{
			StiComponent stiComponent = Components[num];
			RectangleD val = stiComponent.ComponentToPage(stiComponent.SelectRectangle);
			if (((RectangleD)(ref rect)).IntersectsWith(val))
			{
				stiComponent.Select();
			}
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.SelectInRectangle(rect);
			}
		}
	}

	public void SelectAll()
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				component.Select();
				if (component is StiContainer stiContainer)
				{
					stiContainer.SelectAll();
				}
			}
		}
	}

	public void SelectComponent(StiComponent component)
	{
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component2 in Components)
			{
				if (component2 == component)
				{
					component2.Select();
					break;
				}
				if (component2 is StiContainer stiContainer)
				{
					stiContainer.SelectComponent(component);
				}
			}
		}
	}

	public PointD ContainerToPage(PointD point)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		for (StiContainer stiContainer = this; stiContainer != null; stiContainer = stiContainer.Parent)
		{
			RectangleD rect = stiContainer.ClientRectangle;
			if (stiContainer.IsSelected)
			{
				rect = StiComponent.DoOffsetRect(stiContainer, rect, Page.OffsetRectangle);
			}
			((PointD)(ref point)).X = ((PointD)(ref point)).X + ((RectangleD)(ref rect)).Left;
			((PointD)(ref point)).Y = ((PointD)(ref point)).Y + ((RectangleD)(ref rect)).Top;
		}
		return point;
	}

	public PointD PageToContainer(PointD point)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		for (StiComponent stiComponent = this; stiComponent != null; stiComponent = stiComponent.Parent)
		{
			RectangleD rect = stiComponent.ClientRectangle;
			if (stiComponent.IsSelected)
			{
				rect = StiComponent.DoOffsetRect(stiComponent, rect, Page.OffsetRectangle);
			}
			((PointD)(ref point)).X = ((PointD)(ref point)).X - ((RectangleD)(ref rect)).Left;
			((PointD)(ref point)).Y = ((PointD)(ref point)).Y - ((RectangleD)(ref rect)).Top;
		}
		return point;
	}

	public RectangleD ContainerToPage(RectangleD rect)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		StiComponent stiComponent = this;
		while (stiComponent != null && !(stiComponent is StiPage))
		{
			RectangleD rect2 = stiComponent.ClientRectangle;
			if (stiComponent.IsSelected)
			{
				rect2 = StiComponent.DoOffsetRect(stiComponent, rect2, Page.OffsetRectangle);
			}
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + ((RectangleD)(ref rect2)).Left;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect2)).Top;
			stiComponent = stiComponent.Parent;
		}
		return rect;
	}

	public RectangleD PageToContainer(RectangleD rect)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		for (StiComponent stiComponent = this; stiComponent != null; stiComponent = stiComponent.Parent)
		{
			RectangleD rect2 = stiComponent.ClientRectangle;
			if (stiComponent.IsSelected)
			{
				rect2 = StiComponent.DoOffsetRect(stiComponent, rect2, Page.OffsetRectangle);
			}
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X - ((RectangleD)(ref rect2)).Left;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y - ((RectangleD)(ref rect2)).Top;
		}
		return rect;
	}

	public StiComponentsCollection GetComponents()
	{
		StiComponentsCollection comps = new StiComponentsCollection();
		if (this is StiClone)
		{
			return comps;
		}
		comps.AddRange(Components);
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component is StiContainer stiContainer)
				{
					stiContainer.GetComponents(ref comps);
				}
			}
			return comps;
		}
	}

	public void GetComponents(ref StiComponentsCollection comps)
	{
		if (this is StiClone)
		{
			return;
		}
		comps.AddRange(Components);
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component is StiContainer stiContainer)
				{
					stiContainer.GetComponents(ref comps);
				}
			}
		}
	}

	public List<StiComponent> GetComponentsList()
	{
		List<StiComponent> list = new List<StiComponent>();
		if (this is StiClone)
		{
			return list;
		}
		foreach (StiComponent component in Components)
		{
			list.Add(component);
			if (component is StiContainer stiContainer)
			{
				list.AddRange(stiContainer.GetComponentsList());
			}
		}
		return list;
	}

	public virtual int GetComponentsCount()
	{
		int num = 0;
		if (this is StiClone)
		{
			return 1;
		}
		num += Components.Count;
		lock (((ICollection)Components).SyncRoot)
		{
			foreach (StiComponent component in Components)
			{
				if (component is StiContainer stiContainer)
				{
					num += stiContainer.GetComponentsCount();
				}
			}
			return num;
		}
	}

	public virtual void RemoveAllSelected()
	{
		int num = 0;
		while (num < components.Count)
		{
			StiComponent stiComponent = components[num];
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.RemoveAllSelected();
			}
			if (stiComponent.IsSelected && stiComponent.AllowDelete)
			{
				StiComponent stiComponent2 = components[num];
				stiComponent2.OnRemoveComponent();
				if (stiComponent2.Report != null && stiComponent2.Report.Designer != null)
				{
					stiComponent2.Report.GlobalizationStrings.RemoveComponent(stiComponent2);
				}
				components.Remove(stiComponent2);
				StiOptions.Engine.GlobalEvents.InvokeComponentRemoved(stiComponent2.Report.Designer, new StiComponentRemovingEventArgs(stiComponent2.Report.Designer, stiComponent2));
			}
			else
			{
				num++;
			}
		}
	}

	public void RemoveAllSelected(ref List<StiComponent> removeComps)
	{
		int num = 0;
		while (num < components.Count)
		{
			StiComponent stiComponent = components[num];
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.RemoveAllSelected(ref removeComps);
			}
			if (stiComponent.IsSelected && stiComponent.AllowDelete)
			{
				StiComponent stiComponent2 = components[num];
				stiComponent2.OnRemoveComponent();
				if (stiComponent2.Report != null && stiComponent2.Report.Designer != null)
				{
					stiComponent2.Report.GlobalizationStrings.RemoveComponent(stiComponent2);
				}
				components.Remove(stiComponent2);
				removeComps.Add(stiComponent2);
				StiOptions.Engine.GlobalEvents.InvokeComponentRemoved(stiComponent2.Report.Designer, new StiComponentRemovingEventArgs(stiComponent2.Report.Designer, stiComponent2));
			}
			else
			{
				num++;
			}
		}
	}

	public void MoveComponentsToPage()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		List<StiComponent> componentsList = GetComponentsList();
		foreach (StiComponent item in componentsList)
		{
			if (item.Parent != item.Page)
			{
				item.SetDirectDisplayRectangle(item.ComponentToPage(item.DisplayRectangle));
				item.Page.Components.Add(item);
			}
		}
		foreach (StiComponent item2 in componentsList)
		{
			if (item2 is StiContainer stiContainer && stiContainer.Components.Count > 0)
			{
				stiContainer.Components.Clear();
			}
		}
	}

	public void ConvertSelectedToPage()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		while (num < components.Count)
		{
			StiComponent stiComponent = components[num];
			if (stiComponent is StiContainer stiContainer)
			{
				stiContainer.MoveComponentsToPage();
			}
			if (!(stiComponent.Parent is StiPage) && stiComponent.IsSelected)
			{
				stiComponent.DisplayRectangle = ContainerToPage(stiComponent.DisplayRectangle);
				stiComponent.Parent.components.Remove(stiComponent);
				stiComponent.Page.Components.Add(stiComponent);
			}
			else
			{
				num++;
			}
		}
	}

	public void ConvertToPage()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		foreach (StiComponent component in Components)
		{
			component.ClientRectangle = component.ComponentToPage(component.ClientRectangle);
			if (component is StiContainer stiContainer)
			{
				stiContainer.ConvertToPage();
			}
		}
	}

	public void InvertComponentsPosition()
	{
		foreach (StiComponent component in Components)
		{
			component.Left = Width - component.Left - component.Width;
			if (component is StiContainer stiContainer)
			{
				stiContainer.InvertComponentsPosition();
			}
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiContainer();
	}

	public StiContainer()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiContainer(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		components = new StiComponentsCollection(this);
		components.ComponentAdded += OnComponentAdded;
		components.ComponentRemoved += OnComponentRemoved;
		PlaceOnToolbox = true;
	}
}
