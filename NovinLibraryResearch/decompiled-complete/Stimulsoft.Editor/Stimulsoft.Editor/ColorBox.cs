using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class ColorBox : ComboBox
{
	private Container components;

	private const int colorWidth = 28;

	private Brush fontBrush;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ObjectCollection Items => ((ComboBox)this).Items;

	[Description("Represents text associated with this control.")]
	public string Text => ((ComboBox)this).Text;

	[Category("Appearence")]
	[Description("Gets or sets curently selected color in the \"ColorBox\".")]
	public Color SelectedColor
	{
		get
		{
			if (((ComboBox)this).SelectedItem == null)
			{
				return Color.Empty;
			}
			return GetColor(((ListControl)this).SelectedIndex);
		}
		set
		{
			if (value == Color.Empty)
			{
				((ListControl)this).SelectedIndex = 0;
				return;
			}
			for (int i = 0; i < Items.Count; i++)
			{
				if (string.Compare(value.Name, Items[i].ToString(), ignoreCase: true) == 0)
				{
					((ListControl)this).SelectedIndex = i;
					break;
				}
			}
		}
	}

	public ColorBox(IContainer container)
	{
		container.Add((IComponent?)this);
		InternalCreate();
		InitializeComponent();
	}

	public ColorBox()
	{
		InternalCreate();
		InitializeComponent();
	}

	private void InternalCreate()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		fontBrush = (Brush)new SolidBrush(((Control)this).ForeColor);
		((ComboBox)this).DropDownStyle = (ComboBoxStyle)1;
		((ComboBox)this).DrawMode = (DrawMode)1;
		UpdateColors();
		((ListControl)this).SelectedIndex = 0;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ComboBox)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
	}

	private void UpdateColors()
	{
		Items.Add((object)StringConsts.EmptyColor);
		Type typeFromHandle = typeof(Color);
		PropertyInfo[] properties = typeFromHandle.GetProperties();
		for (int i = 0; i < properties.Length; i++)
		{
			if (properties[i].PropertyType == typeof(Color))
			{
				Items.Add((object)properties[i].Name);
			}
		}
		typeFromHandle = typeof(SystemColors);
		properties = typeFromHandle.GetProperties();
		for (int j = 0; j < properties.Length; j++)
		{
			if (properties[j].PropertyType == typeof(Color))
			{
				Items.Add((object)properties[j].Name);
			}
		}
	}

	private Color GetColor(int index)
	{
		if (index > 0 && index < Items.Count)
		{
			return Color.FromName(Items[index].ToString());
		}
		return Color.Empty;
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		((ComboBox)this).OnForeColorChanged(e);
		fontBrush = (Brush)new SolidBrush(((Control)this).ForeColor);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Invalid comparison between Unknown and I4
		if (e.Index < 0 || e.Index >= Items.Count)
		{
			return;
		}
		e.DrawBackground();
		bool flag = (e.State & 4) > 0;
		bool flag2 = (e.State & 0x10) != 0 && (e.State & 0x200) == 0;
		bool flag3 = (e.State & 1) > 0;
		if (flag2)
		{
			e.DrawFocusRectangle();
		}
		Brush val = (Brush)new SolidBrush(GetColor(e.Index));
		try
		{
			string text = ((e.Index == 0) ? StringConsts.EmptyColor : GetColor(e.Index).Name);
			Rectangle rectangle = new Rectangle(e.Bounds.Left, e.Bounds.Top, Math.Min(e.Bounds.Width, 28), e.Bounds.Height);
			rectangle.Inflate(-2, -2);
			Rectangle rectangle2 = new Rectangle(rectangle.Right + 1, e.Bounds.Top, e.Bounds.Width - rectangle.Right - 1, e.Bounds.Height);
			if ((int)e.State == 64)
			{
				e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
			}
			e.Graphics.FillRectangle(val, rectangle);
			e.Graphics.DrawRectangle(Pens.Black, rectangle);
			Brush val2 = fontBrush;
			if (flag)
			{
				val2 = Brushes.Gray;
			}
			else if (flag3)
			{
				val2 = SystemBrushes.HighlightText;
			}
			e.Graphics.DrawString(text, ((Control)this).Font, val2, (RectangleF)rectangle2);
		}
		finally
		{
			val.Dispose();
		}
	}
}
