using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components.Table;

[StiToolbox(false)]
[StiContextTool(typeof(IStiCanGrow))]
[StiGdiPainter(typeof(StiTableCellRichTextGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiTableCellRichTextWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiTableCellRichTextQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfTableCellRichTextQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiTableCellRichText : StiRichText, IStiTableCell, IStiTableComponent
{
	private bool join;

	private int joinWidth;

	private int joinHeight;

	private StiTablceCellType cellType = StiTablceCellType.RichText;

	public override StiComponentId ComponentId => StiComponentId.StiTableCellRichText;

	[Browsable(false)]
	public override StiDockStyle DockStyle
	{
		get
		{
			return base.DockStyle;
		}
		set
		{
			base.DockStyle = value;
		}
	}

	[Browsable(false)]
	public override StiRestrictions Restrictions
	{
		get
		{
			return base.Restrictions;
		}
		set
		{
			base.Restrictions = value;
		}
	}

	[Browsable(false)]
	public override double Left
	{
		get
		{
			return base.Left;
		}
		set
		{
			base.Left = value;
		}
	}

	[Browsable(false)]
	public override double Top
	{
		get
		{
			return base.Top;
		}
		set
		{
			base.Top = value;
		}
	}

	[Browsable(false)]
	public override double Width
	{
		get
		{
			return base.Width;
		}
		set
		{
			base.Width = value;
		}
	}

	[Browsable(false)]
	public override double Height
	{
		get
		{
			return base.Height;
		}
		set
		{
			base.Height = value;
		}
	}

	[Browsable(false)]
	public override SizeD MinSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MinSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MinSize = value;
		}
	}

	[Browsable(false)]
	public override SizeD MaxSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MaxSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MaxSize = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool Locked
	{
		get
		{
			if (base.IsDesigning)
			{
				return !Report.IsPageDesigner;
			}
			return false;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool Linked
	{
		get
		{
			if (base.IsDesigning)
			{
				return !Report.IsPageDesigner;
			}
			return false;
		}
		set
		{
		}
	}

	public override bool CanShrink
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
			base.GrowToHeight = value;
		}
	}

	[Browsable(true)]
	[StiShowInContextMenu]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
			if (base.CanGrow != value)
			{
				base.CanGrow = value;
				if (Parent is StiTable)
				{
					((StiTable)Parent).ChangeGrowToHeightAtCell(this);
				}
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public int[] JoinCells { get; set; } = new int[0];


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(-1)]
	public int ParentJoin { get; set; } = -1;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool Join
	{
		get
		{
			return join;
		}
		set
		{
			if (join != value)
			{
				if (value)
				{
					CreateJoin();
				}
				else
				{
					DeleteJoin();
				}
				join = value;
			}
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(-1)]
	public int ID { get; set; } = -1;


	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(0)]
	public int JoinWidth
	{
		get
		{
			return joinWidth;
		}
		set
		{
			joinWidth = value;
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(0)]
	public int JoinHeight
	{
		get
		{
			return joinHeight;
		}
		set
		{
			joinHeight = value;
		}
	}

	[Browsable(false)]
	public bool Merged => ParentJoin != -1;

	[Browsable(false)]
	public bool ChangeTopPosition
	{
		get
		{
			if (!(Parent is StiTable))
			{
				return false;
			}
			int num = Parent.Components.IndexOf(this);
			return num >= ((StiTable)Parent).ColumnCount;
		}
	}

	[Browsable(false)]
	public bool ChangeLeftPosition
	{
		get
		{
			if (!(Parent is StiTable))
			{
				return false;
			}
			int num = Parent.Components.IndexOf(this);
			for (int i = 0; i < Parent.Components.Count; i += ((StiTable)Parent).ColumnCount)
			{
				if (i == num)
				{
					return false;
				}
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool ChangeRightPosition
	{
		get
		{
			if (!(Parent is StiTable))
			{
				return true;
			}
			int num = Parent.Components.IndexOf(this);
			for (int i = ((StiTable)Parent).ColumnCount - 1; i < Parent.Components.Count; i += ((StiTable)Parent).ColumnCount)
			{
				if (i == num)
				{
					return false;
				}
			}
			return true;
		}
	}

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(StiDockStyle.None)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Get or sets a type of the cell docking.")]
	[StiOrder(120)]
	public StiDockStyle CellDockStyle { get; set; } = StiDockStyle.None;


	[StiNonSerialized]
	[Browsable(false)]
	public StiComponent ParentJoinCell { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public object TableTag { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(StiTablceCellType.RichText)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Get or sets a type of the cell content.")]
	[StiOrder(100)]
	public StiTablceCellType CellType
	{
		get
		{
			return cellType;
		}
		set
		{
			cellType = value;
			if (Parent is StiTable stiTable)
			{
				switch (value)
				{
				case StiTablceCellType.Text:
					stiTable.ChangeTableCellContentInText(this);
					break;
				case StiTablceCellType.Image:
					stiTable.ChangeTableCellContentInImage(this);
					break;
				case StiTablceCellType.CheckBox:
					stiTable.ChangeTableCellContentInCheckBox(this);
					break;
				}
			}
		}
	}

	[StiSerializable]
	[Browsable(true)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a value which indicates that the cell have fixed width.")]
	[StiOrder(150)]
	public bool FixedWidth { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public int Column { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "Locked");
		JsonExtensions.RemoveProperty(val, "Linked");
		JsonExtensions.AddPropertyEnum(val, "CellDockStyle", (Enum)CellDockStyle, (Enum)StiDockStyle.None);
		JsonExtensions.AddPropertyIntArray(val, "JoinCells", JoinCells);
		JsonExtensions.AddPropertyInt(val, "ParentJoin", ParentJoin, -1);
		JsonExtensions.AddPropertyBool(val, "Join", Join, false, false);
		JsonExtensions.AddPropertyInt(val, "ID", ID, -1);
		JsonExtensions.AddPropertyInt(val, "JoinWidth", JoinWidth, 0);
		JsonExtensions.AddPropertyInt(val, "JoinHeight", JoinHeight, 0);
		JsonExtensions.AddPropertyEnum(val, "CellType", (Enum)CellType, (Enum)StiTablceCellType.RichText);
		JsonExtensions.AddPropertyBool(val, "FixedWidth", FixedWidth, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CellDockStyle":
				CellDockStyle = StiJsonReportObjectHelper.DeserializeEnum<StiDockStyle>(item);
				break;
			case "JoinCells":
				JoinCells = StiJsonReportObjectHelper.DeserializeIntArray(item);
				break;
			case "ParentJoin":
				ParentJoin = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Join":
				join = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ID":
				ID = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "JoinWidth":
				joinWidth = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "JoinHeight":
				joinHeight = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "CellType":
				cellType = StiJsonReportObjectHelper.DeserializeEnum<StiTablceCellType>(item);
				break;
			case "FixedWidth":
				FixedWidth = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Invalid comparison between Unknown and I4
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.RichTextDesign() });
		stiPropertyCollection.Add(StiPropertyCategories.Cell, new StiPropertyObject[3]
		{
			propertiesHelper.CellType(),
			propertiesHelper.CellDockStyle(),
			propertiesHelper.FixedWidth()
		});
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[1] { propertiesHelper.Text() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[2]
			{
				propertiesHelper.Text(),
				propertiesHelper.DataColumn()
			});
		}
		if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[6]
			{
				propertiesHelper.DetectUrls(),
				propertiesHelper.Editable(),
				propertiesHelper.Margins(),
				propertiesHelper.BackColor(),
				propertiesHelper.OnlyText(),
				propertiesHelper.WordWrap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[9]
			{
				propertiesHelper.DetectUrls(),
				propertiesHelper.Editable(),
				propertiesHelper.FullConvertExpression(),
				propertiesHelper.Margins(),
				propertiesHelper.BackColor(),
				propertiesHelper.OnlyText(),
				propertiesHelper.ProcessAt(),
				propertiesHelper.WordWrap(),
				propertiesHelper.Wysiwyg()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[3]
			{
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
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
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[9]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
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
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetExcelValueEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetValueEvent
		});
		return stiEventCollection;
	}

	public override object Clone(bool cloneProperties)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		StiTableCellRichText stiTableCellRichText = (StiTableCellRichText)base.Clone(cloneProperties);
		stiTableCellRichText.join = join;
		stiTableCellRichText.MinSize = new SizeD(0.0, 0.0);
		stiTableCellRichText.MaxSize = new SizeD(0.0, 0.0);
		stiTableCellRichText.joinWidth = joinWidth;
		stiTableCellRichText.CellDockStyle = CellDockStyle;
		stiTableCellRichText.joinHeight = joinHeight;
		stiTableCellRichText.cellType = cellType;
		stiTableCellRichText.Column = Column;
		stiTableCellRichText.FixedWidth = FixedWidth;
		stiTableCellRichText.ID = ID;
		stiTableCellRichText.JoinCells = (int[])JoinCells.Clone();
		stiTableCellRichText.ParentJoin = ((ParentJoin != -1) ? ParentJoin : (-1));
		return stiTableCellRichText;
	}

	public StiComponent GetJoinComponentByGuid(int id)
	{
		if (Parent == null && JoinCells.Length == 0)
		{
			return null;
		}
		foreach (StiComponent component in Parent.Components)
		{
			if (((IStiTableCell)component).ID == id)
			{
				return component;
			}
		}
		return null;
	}

	public StiComponent GetJoinComponentByIndex(int index)
	{
		if (index < 0 && Parent == null && JoinCells.Length == 0 && index >= JoinCells.Length)
		{
			return null;
		}
		foreach (StiComponent component in Parent.Components)
		{
			if (((IStiTableCell)component).ID == JoinCells[index])
			{
				return component;
			}
		}
		return null;
	}

	public bool ContainsGuid(int id)
	{
		if (JoinCells.Length != 0)
		{
			return JoinCells.Any((int t) => t == id);
		}
		return false;
	}

	private void CreateJoin()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		StiTable stiTable = Parent as StiTable;
		if (stiTable == null && Page == null)
		{
			return;
		}
		double sumWidth = 0.0;
		double sumHeight = 0.0;
		JoinCells = stiTable.CreateJoin(ref sumWidth, ref sumHeight, ref joinWidth, ref joinHeight);
		if (JoinCells.Length == 0)
		{
			return;
		}
		double num = 0.0;
		double num2 = 0.0;
		for (int i = 0; i < JoinCells.Length - 1; i++)
		{
			StiComponent joinComponentByIndex = GetJoinComponentByIndex(i);
			if (i == 0)
			{
				num = joinComponentByIndex.Left;
				num2 = joinComponentByIndex.Top;
			}
			joinComponentByIndex.Enabled = false;
			((IStiTableCell)joinComponentByIndex).ParentJoin = ID;
		}
		ParentJoin = ID;
		ClientRectangle = new RectangleD(num, num2, sumWidth, sumHeight);
	}

	private void DeleteJoin()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (Page != null || JoinCells.Length != 0)
		{
			for (int i = 0; i < JoinCells.Length - 1; i++)
			{
				StiComponent joinComponentByIndex = GetJoinComponentByIndex(i);
				joinComponentByIndex.Enabled = true;
				((IStiTableCell)joinComponentByIndex).ParentJoin = -1;
			}
			ClientRectangle = GetNewClientRectangle();
			JoinCells = new int[0];
			ParentJoin = -1;
			joinWidth = 0;
			joinHeight = 0;
		}
	}

	private RectangleD GetNewClientRectangle()
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		StiComponent joinComponentByIndex = GetJoinComponentByIndex(JoinCells.Length - 2);
		int num = Parent.Components.IndexOf(joinComponentByIndex);
		int num2 = Parent.Components.IndexOf(this);
		if (num2 - num == 1)
		{
			return new RectangleD(joinComponentByIndex.Right, joinComponentByIndex.Top, Right - joinComponentByIndex.Right, joinComponentByIndex.Height);
		}
		return new RectangleD(joinComponentByIndex.Left, joinComponentByIndex.Bottom, joinComponentByIndex.Width, Bottom - joinComponentByIndex.Bottom);
	}

	public void SetJoinSize()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (join)
		{
			StiComponent joinComponentByIndex = GetJoinComponentByIndex(0);
			double num = joinComponentByIndex.Left;
			double num2 = joinComponentByIndex.Top;
			ClientRectangle = new RectangleD(num, num2, Right - num, Bottom - num2);
		}
	}

	public double GetRealHeightAfterInsertRows()
	{
		if (!join)
		{
			return base.Height;
		}
		if (joinHeight == 1)
		{
			return base.Height;
		}
		double num = 0.0;
		int num2 = Parent.Components.IndexOf(GetJoinComponentByIndex(0)) / ((StiTable)Parent).ColumnCount;
		int num3 = Parent.Components.IndexOf(this) / ((StiTable)Parent).ColumnCount;
		for (int i = num2; i < num3; i++)
		{
			int index = i * ((StiTable)Parent).ColumnCount;
			num += Parent.Components[index].Height;
		}
		return base.Height - num;
	}

	public double GetRealHeight()
	{
		if (!join)
		{
			return base.Height;
		}
		StiComponent joinComponentByIndex = GetJoinComponentByIndex(JoinCells.Length - 2);
		int num = Parent.Components.IndexOf(this);
		int num2 = Parent.Components.IndexOf(joinComponentByIndex);
		if (((StiTable)Parent).ColumnCount <= 1)
		{
			return Bottom - joinComponentByIndex.Bottom;
		}
		if (num - num2 != 1)
		{
			return Bottom - joinComponentByIndex.Bottom;
		}
		return joinComponentByIndex.Height;
	}

	public double GetRealTop()
	{
		if (!join)
		{
			return base.Top;
		}
		StiComponent joinComponentByIndex = GetJoinComponentByIndex(JoinCells.Length - 2);
		int num = Parent.Components.IndexOf(this);
		int num2 = Parent.Components.IndexOf(joinComponentByIndex);
		if (((StiTable)Parent).ColumnCount <= 1)
		{
			return joinComponentByIndex.Bottom;
		}
		if (num - num2 != 1)
		{
			return joinComponentByIndex.Bottom;
		}
		return joinComponentByIndex.Top;
	}

	public double GetRealWidth()
	{
		if (!join)
		{
			return base.Width;
		}
		StiComponent joinComponentByIndex = GetJoinComponentByIndex(JoinCells.Length - 2);
		int num = Parent.Components.IndexOf(this);
		int num2 = Parent.Components.IndexOf(joinComponentByIndex);
		if (((StiTable)Parent).RowCount <= 1)
		{
			return Right - joinComponentByIndex.Right;
		}
		if (num - num2 != 1)
		{
			return joinComponentByIndex.Width;
		}
		return Right - joinComponentByIndex.Right;
	}

	public double GetRealLeft()
	{
		if (!join)
		{
			return Left;
		}
		StiComponent joinComponentByIndex = GetJoinComponentByIndex(JoinCells.Length - 2);
		int num = Parent.Components.IndexOf(this);
		int num2 = Parent.Components.IndexOf(joinComponentByIndex);
		if (((StiTable)Parent).RowCount <= 1)
		{
			return joinComponentByIndex.Right;
		}
		if (num - num2 != 1)
		{
			return joinComponentByIndex.Left;
		}
		return joinComponentByIndex.Right;
	}

	internal void SetJoin(bool isJoin)
	{
		join = isJoin;
	}

	public override StiComponent CreateNew()
	{
		return new StiTableCellRichText();
	}

	public StiTableCellRichText()
	{
		base.BackColor = Color.White;
	}
}
