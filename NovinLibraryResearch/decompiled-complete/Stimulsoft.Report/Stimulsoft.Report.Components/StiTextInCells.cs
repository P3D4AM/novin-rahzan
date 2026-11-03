using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiTextInCells), "Stimulsoft.Report.Images.Components.StiTextInCells.png")]
[StiToolbox(true)]
[StiGdiPainter(typeof(StiTextInCellsGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiTextInCellsWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiV1Builder(typeof(StiTextInCellsV1Builder))]
[StiV2Builder(typeof(StiTextInCellsV2Builder))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiTextQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfTextQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiTextInCells : StiText
{
	protected static object PropertyCellWidth = new object();

	protected static object PropertyCellHeight = new object();

	protected static object PropertyHorSpacing = new object();

	protected static object PropertyVertSpacing = new object();

	protected static object PropertyContinuousText = new object();

	public override StiComponentId ComponentId => StiComponentId.StiTextInCells;

	public override string HelpUrl => "user-manual/right_to_left_text_in_cells_component.htm?zoom_highlightsub=Text";

	[StiNonSerialized]
	[Browsable(false)]
	public override StiVertAlignment VertAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.VertAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.VertAlignment = value;
		}
	}

	[Browsable(false)]
	public override StiTextOptions TextOptions
	{
		get
		{
			return base.TextOptions;
		}
		set
		{
			base.TextOptions = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool AutoWidth
	{
		get
		{
			return base.AutoWidth;
		}
		set
		{
			base.AutoWidth = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiNonSerialized]
	[Browsable(false)]
	public override bool AllowHtmlTags
	{
		get
		{
			return base.AllowHtmlTags;
		}
		set
		{
			AllowHtmlTags = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool ShrinkFontToFit
	{
		get
		{
			return base.ShrinkFontToFit;
		}
		set
		{
			base.ShrinkFontToFit = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override float ShrinkFontToFitMinimumSize
	{
		get
		{
			return base.ShrinkFontToFitMinimumSize;
		}
		set
		{
			base.ShrinkFontToFitMinimumSize = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override float Angle
	{
		get
		{
			return base.Angle;
		}
		set
		{
			base.Angle = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiPenStyle LinesOfUnderline
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.LinesOfUnderline;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.LinesOfUnderline = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override int MaxNumberOfLines
	{
		get
		{
			return base.MaxNumberOfLines;
		}
		set
		{
			base.MaxNumberOfLines = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiProcessingDuplicatesType ProcessingDuplicates
	{
		get
		{
			return base.ProcessingDuplicates;
		}
		set
		{
			base.ProcessingDuplicates = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override string RenderTo
	{
		get
		{
			return base.RenderTo;
		}
		set
		{
			base.RenderTo = value;
		}
	}

	public override int ToolboxPosition => 102;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiTextInCells");

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets width of the cell.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float CellWidth
	{
		get
		{
			return ((StiService)this).Properties.GetFloat(PropertyCellWidth, 0f);
		}
		set
		{
			if (value < 0f)
			{
				((StiService)this).Properties.SetFloat(PropertyCellWidth, 0f, 0f);
			}
			else
			{
				((StiService)this).Properties.SetFloat(PropertyCellWidth, value, 0f);
			}
		}
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Gets or sets height of the cell.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float CellHeight
	{
		get
		{
			return ((StiService)this).Properties.GetFloat(PropertyCellHeight, 0f);
		}
		set
		{
			if (value < 0f)
			{
				((StiService)this).Properties.SetFloat(PropertyCellHeight, 0f, 0f);
			}
			else
			{
				((StiService)this).Properties.SetFloat(PropertyCellHeight, value, 0f);
			}
		}
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[Description("Gets or sets horizontal spacing between cells.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float HorSpacing
	{
		get
		{
			return ((StiService)this).Properties.GetFloat(PropertyHorSpacing, 0f);
		}
		set
		{
			if (value < 0f)
			{
				((StiService)this).Properties.SetFloat(PropertyHorSpacing, 0f, 0f);
			}
			else
			{
				((StiService)this).Properties.SetFloat(PropertyHorSpacing, value, 0f);
			}
		}
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets vertical spacing between cells.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float VertSpacing
	{
		get
		{
			return ((StiService)this).Properties.GetFloat(PropertyVertSpacing, 0f);
		}
		set
		{
			if (value < 0f)
			{
				((StiService)this).Properties.SetFloat(PropertyVertSpacing, 0f, 0f);
			}
			else
			{
				((StiService)this).Properties.SetFloat(PropertyVertSpacing, value, 0f);
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiTextQuality TextQuality
	{
		get
		{
			return base.TextQuality;
		}
		set
		{
			base.TextQuality = value;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(310)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool WordWrap
	{
		get
		{
			return TextOptions.WordWrap;
		}
		set
		{
			TextOptions.WordWrap = value;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(250)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets horizontal output direction.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool RightToLeft
	{
		get
		{
			return TextOptions.RightToLeft;
		}
		set
		{
			TextOptions.RightToLeft = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(115)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets continuous text flag.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ContinuousText
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyContinuousText, true);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyContinuousText, value, true);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiExcelValueExpression ExcelValue
	{
		get
		{
			return base.ExcelValue;
		}
		set
		{
			base.ExcelValue = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "VertAlignment");
		JsonExtensions.RemoveProperty(val, "AutoWidth");
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "AllowHtmlTags");
		JsonExtensions.RemoveProperty(val, "ShrinkFontToFit");
		JsonExtensions.RemoveProperty(val, "ShrinkFontToFitMinimumSize");
		JsonExtensions.RemoveProperty(val, "Angle");
		JsonExtensions.RemoveProperty(val, "LinesOfUnderline");
		JsonExtensions.RemoveProperty(val, "MaxNumberOfLines");
		JsonExtensions.RemoveProperty(val, "ProcessingDuplicates");
		JsonExtensions.RemoveProperty(val, "RenderTo");
		JsonExtensions.RemoveProperty(val, "TextQuality");
		JsonExtensions.RemoveProperty(val, "ExcelValue");
		JsonExtensions.AddPropertyFloat(val, "CellWidth", CellWidth, 0f);
		JsonExtensions.AddPropertyFloat(val, "CellHeight", CellHeight, 0f);
		JsonExtensions.AddPropertyFloat(val, "HorSpacing", HorSpacing, 0f);
		JsonExtensions.AddPropertyFloat(val, "VertSpacing", VertSpacing, 0f);
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, false, false);
		JsonExtensions.AddPropertyBool(val, "RightToLeft", RightToLeft, false, false);
		JsonExtensions.AddPropertyBool(val, "ContinuousText", ContinuousText, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CellWidth":
				CellWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "CellHeight":
				CellHeight = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "HorSpacing":
				HorSpacing = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "VertSpacing":
				VertSpacing = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "WordWrap":
				WordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RightToLeft":
				RightToLeft = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ContinuousText":
				ContinuousText = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Invalid comparison between Unknown and I4
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Invalid comparison between Unknown and I4
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[3]
			{
				propertiesHelper.Text(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.TextFormat()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[7]
			{
				propertiesHelper.Text(),
				propertiesHelper.CellWidth(),
				propertiesHelper.CellHeight(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.HorSpacing(),
				propertiesHelper.VertSpacing(),
				propertiesHelper.TextFormat()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[4]
			{
				propertiesHelper.ContinuousText(),
				propertiesHelper.HideZeros(),
				propertiesHelper.LineSpacing(),
				propertiesHelper.WordWrap()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[7]
			{
				propertiesHelper.ContinuousText(),
				propertiesHelper.Editable(),
				propertiesHelper.HideZeros(),
				propertiesHelper.LineSpacing(),
				propertiesHelper.Margins(),
				propertiesHelper.OnlyText(),
				propertiesHelper.WordWrap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[8]
			{
				propertiesHelper.ContinuousText(),
				propertiesHelper.Editable(),
				propertiesHelper.HideZeros(),
				propertiesHelper.LineSpacing(),
				propertiesHelper.Margins(),
				propertiesHelper.OnlyText(),
				propertiesHelper.ProcessAt(),
				propertiesHelper.WordWrap()
			});
		}
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
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.Font(),
				propertiesHelper.TextBrush()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.Font(),
				propertiesHelper.TextBrush(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[4]
			{
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[9]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[10]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
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
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[6]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.GlobalizedName(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		if ((int)level == 2)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Export, new StiPropertyObject[1] { propertiesHelper.ExportAsImage() });
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetExcelValueEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetValueEvent
		});
		return stiEventCollection;
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		base.Convert(oldUnit, newUnit, isReportSnapshot);
		CellWidth = (float)newUnit.ConvertFromHInches(oldUnit.ConvertToHInches((double)CellWidth));
		CellHeight = (float)newUnit.ConvertFromHInches(oldUnit.ConvertToHInches((double)CellHeight));
		HorSpacing = (float)newUnit.ConvertFromHInches(oldUnit.ConvertToHInches((double)HorSpacing));
		VertSpacing = (float)newUnit.ConvertFromHInches(oldUnit.ConvertToHInches((double)VertSpacing));
	}

	public override SizeD GetActualSize()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new SizeD(Width, Height);
	}

	internal static StiContainer SplitByCells(StiTextInCells masterTextInCells, StiComponent renderedComponent, string textString)
	{
		return SplitByCells(masterTextInCells, renderedComponent, textString, measure: false);
	}

	internal static StiContainer SplitByCells(StiTextInCells masterTextInCells, StiComponent renderedComponent, string textString, bool measure)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Invalid comparison between Unknown and I4
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Invalid comparison between Unknown and I4
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = new StiContainer();
		stiContainer.ClientRectangle = renderedComponent.ClientRectangle;
		stiContainer.Name = renderedComponent.Name;
		stiContainer.CanGrow = renderedComponent.CanGrow;
		RectangleD clientRectangle = stiContainer.ClientRectangle;
		((RectangleD)(ref clientRectangle)).X = 0.0;
		((RectangleD)(ref clientRectangle)).Y = 0.0;
		StiUnit unit = masterTextInCells.Page.Unit;
		double num = unit.ConvertFromHInches(masterTextInCells.Border.Size / 2.0 * masterTextInCells.Page.Zoom);
		double num2 = masterTextInCells.HorSpacing;
		double num3 = masterTextInCells.VertSpacing;
		double num4 = masterTextInCells.CellWidth;
		double num5 = masterTextInCells.CellHeight;
		if (masterTextInCells.CellWidth == 0f)
		{
			num4 = unit.ConvertFromHInches((double)(masterTextInCells.Font.GetHeight() * 1.5f) * StiDpiHelper.DeviceCapsScale);
		}
		if (masterTextInCells.CellHeight == 0f)
		{
			num5 = unit.ConvertFromHInches((double)(masterTextInCells.Font.GetHeight() * 1.5f) * StiDpiHelper.DeviceCapsScale);
		}
		if (!masterTextInCells.ContinuousText)
		{
			Size size = new Size(1, 1);
			double num6 = (float)(((RectangleD)(ref clientRectangle)).X + num + num4);
			while (num6 + num2 + num4 < ((RectangleD)(ref clientRectangle)).Right)
			{
				num6 += num2 + num4;
				size.Width++;
			}
			double num7 = (float)(((RectangleD)(ref clientRectangle)).Y + num + num5);
			while (num7 + num3 + num5 < ((RectangleD)(ref clientRectangle)).Bottom)
			{
				num7 += num3 + num5;
				size.Height++;
			}
			if (!masterTextInCells.WordWrap)
			{
				size.Height = 1;
			}
			List<string> list = new List<string>();
			string text = string.Empty;
			if (textString != null)
			{
				for (int i = 0; i < textString.Length; i++)
				{
					char c = textString[i];
					if (char.IsControl(c))
					{
						if (c == '\n')
						{
							list.Add(StiTextInCellsHelper.TrimEndWhiteSpace(text));
							text = string.Empty;
						}
					}
					else
					{
						text += c;
					}
				}
			}
			if (text != string.Empty)
			{
				list.Add(StiTextInCellsHelper.TrimEndWhiteSpace(text));
			}
			if (list.Count == 0)
			{
				list.Add(text);
			}
			if (masterTextInCells.WordWrap)
			{
				for (int j = 0; j < list.Count; j++)
				{
					string text2 = list[j];
					if (text2.Length <= size.Width)
					{
						continue;
					}
					int[] array = new int[text2.Length];
					int num8 = 0;
					int k;
					for (k = 0; k < text2.Length && char.IsWhiteSpace(text2[k]); k++)
					{
						array[k] = num8;
					}
					for (int l = k; l < text2.Length; l++)
					{
						if (char.IsWhiteSpace(text2[l]))
						{
							num8++;
						}
						array[l] = num8;
					}
					int m = size.Width;
					int num9 = m - 1;
					if (array[m] > 0)
					{
						if (array[m] != array[num9])
						{
							for (; char.IsWhiteSpace(text2[m]); m++)
							{
							}
						}
						else
						{
							while (!char.IsWhiteSpace(text2[m]))
							{
								m--;
							}
							num9 = m++;
							while (char.IsWhiteSpace(text2[num9]))
							{
								num9--;
							}
						}
					}
					list[j] = text2.Substring(0, num9 + 1);
					list.Insert(j + 1, text2.Substring(m, text2.Length - m));
				}
			}
			if (measure && masterTextInCells.CanGrow)
			{
				size.Height = list.Count;
			}
			num7 = (float)(((RectangleD)(ref clientRectangle)).Y + num);
			RectangleD clientRectangle2 = default(RectangleD);
			for (int n = 0; n < size.Height; n++)
			{
				string text3 = ((n < list.Count) ? list[n] : string.Empty);
				int num10 = 0;
				if ((int)masterTextInCells.HorAlignment == 1)
				{
					num10 = (size.Width - text3.Length) / 2;
				}
				if ((int)masterTextInCells.HorAlignment == 2)
				{
					num10 = size.Width - text3.Length;
				}
				if (num10 > 0)
				{
					text3 = new string(' ', num10) + text3;
				}
				num6 = (float)(((RectangleD)(ref clientRectangle)).X + num);
				for (int num11 = 0; num11 < size.Width; num11++)
				{
					double num12 = Math.Round(num6, 2);
					double num13 = Math.Round(num7, 2);
					double num14 = Math.Round(num6 + num4, 2) - num12;
					double num15 = Math.Round(num7 + num5, 2) - num13;
					((RectangleD)(ref clientRectangle2))._002Ector(num12, num13, num14, num15);
					StiText stiText = (StiText)((StiService)renderedComponent).Clone();
					stiText.ClientRectangle = clientRectangle2;
					stiText.HorAlignment = (StiTextHorAlignment)1;
					stiText.VertAlignment = (StiVertAlignment)1;
					stiText.WordWrap = false;
					stiContainer.Components.Add(stiText);
					string textInternal = string.Empty;
					int num16 = (masterTextInCells.RightToLeft ? (size.Width - num11 - 1) : num11);
					if (num16 < text3.Length)
					{
						textInternal = new string(text3[num16], 1);
					}
					stiText.SetTextInternal(textInternal);
					num6 += num4 + num2;
				}
				num7 += num5 + num3;
			}
		}
		else
		{
			double num17 = ((RectangleD)(ref clientRectangle)).X + num;
			double num18 = ((RectangleD)(ref clientRectangle)).Y + num;
			bool flag = true;
			int num19 = 0;
			RectangleD clientRectangle3 = default(RectangleD);
			while (true)
			{
				double num20 = Math.Round(num17, 2);
				double num21 = Math.Round(num18, 2);
				double num22 = Math.Round(num17 + num4, 2) - num20;
				double num23 = Math.Round(num18 + num5, 2) - num21;
				((RectangleD)(ref clientRectangle3))._002Ector(num20, num21, num22, num23);
				if (((RectangleD)(ref clientRectangle3)).Right + num2 < ((RectangleD)(ref clientRectangle)).Right || flag)
				{
					StiText stiText2 = ((StiService)renderedComponent).Clone() as StiText;
					stiText2.ClientRectangle = clientRectangle3;
					stiText2.HorAlignment = (StiTextHorAlignment)1;
					stiText2.VertAlignment = (StiVertAlignment)1;
					stiText2.WordWrap = false;
					stiContainer.Components.Add(stiText2);
					string textInternal2 = string.Empty;
					if (textString != null && num19 < textString.Length)
					{
						textInternal2 = new string(textString[num19], 1);
					}
					stiText2.SetTextInternal(textInternal2);
					num17 += num4 + num2;
					num19++;
					flag = false;
					continue;
				}
				num18 += num5 + num3;
				num17 = (double)(float)((RectangleD)(ref clientRectangle)).X + num;
				flag = true;
				if (!masterTextInCells.WordWrap)
				{
					break;
				}
				if (measure && masterTextInCells.CanGrow)
				{
					if (num19 >= textString.Length)
					{
						break;
					}
				}
				else if (((RectangleD)(ref clientRectangle)).Bottom < num18 + num5 + num3)
				{
					break;
				}
			}
		}
		return stiContainer;
	}

	public static void ReplaceContainerWithContentCells(StiComponent comp, StiContainer cont)
	{
		int index = comp.Parent.Components.IndexOf(comp);
		comp.Parent.Components.RemoveAt(index);
		foreach (StiComponent component in cont.Components)
		{
			component.Left += cont.Left;
			component.Top += cont.Top;
			comp.Parent.Components.Insert(index++, component);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiTextInCells();
	}

	public override bool IsExportAsImage(StiExportFormat format)
	{
		if (format == StiExportFormat.ImageSvg || format == StiExportFormat.ImagePng)
		{
			return true;
		}
		return base.IsExportAsImage(format);
	}

	public StiTextInCells()
		: this(RectangleD.Empty, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiTextInCells(RectangleD rect)
		: this(rect, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiTextInCells(RectangleD rect, string text)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		TextBrush = (StiBrush)new StiSolidBrush(Color.Black);
		base.Brush = (StiBrush)new StiSolidBrush(Color.White);
		base.Border.Side = (StiBorderSides)15;
		base.Border.Color = Color.Black;
		base.Border.Size = 2.0;
		SetTextInternal(text);
		PlaceOnToolbox = false;
		Font = new Font("Arial", 14f, (FontStyle)1);
	}
}
