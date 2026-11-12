using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Controls;

public class StiColorBoxPopupForm : StiPopupForm
{
	private Panel[] panels;

	private Control colorBox;

	private TabPage tbSystem;

	private TabPage tbCustom;

	private TabPage tbWeb;

	private ListBox lbSystem;

	private ListBox lbWeb;

	private Button btOther;

	private bool isFromStyle;

	private IWindowsFormsEditorService edSrv;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color SelectedColor { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TabControl TabControl { get; set; }

	protected override bool UseCreateParams => edSrv == null;

	private void Localize()
	{
		((Control)tbWeb).Text = StiLocalization.Get("FormColorBoxPopup", "Web");
		((Control)tbSystem).Text = StiLocalization.Get("FormColorBoxPopup", "System");
		((Control)tbCustom).Text = StiLocalization.Get("FormColorBoxPopup", "Custom");
		((Control)btOther).Text = StiLocalization.Get("FormColorBoxPopup", "Others");
	}

	public override void SetLocation(int x, int y, int width, int height)
	{
		base.SetLocation(x, y, width, height);
		DockPaddingEdges dockPadding = ((ScrollableControl)this).DockPadding;
		dockPadding.Left += StiScaleUI.I2;
		DockPaddingEdges dockPadding2 = ((ScrollableControl)this).DockPadding;
		dockPadding2.Top += StiScaleUI.I2;
		DockPaddingEdges dockPadding3 = ((ScrollableControl)this).DockPadding;
		dockPadding3.Right += StiScaleUI.I2;
		DockPaddingEdges dockPadding4 = ((ScrollableControl)this).DockPadding;
		dockPadding4.Bottom += StiScaleUI.I2;
	}

	public override bool PreFilterMessage(ref Message m)
	{
		if (edSrv == null)
		{
			return base.PreFilterMessage(ref m);
		}
		return false;
	}

	public override void ClosePopupForm()
	{
		base.ClosePopupForm();
		if (edSrv != null)
		{
			edSrv.CloseDropDown();
		}
	}

	private void InitColors()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		panels = (Panel[])(object)new Panel[StiColors.CustomColors.Length];
		lbWeb.BeginUpdate();
		lbSystem.BeginUpdate();
		lbWeb.Items.Clear();
		Color[] colors = StiColors.Colors;
		foreach (Color color in colors)
		{
			lbWeb.Items.Add((object)color);
		}
		((Control)lbWeb).MouseUp += new MouseEventHandler(ListBoxWeb_MouseUp);
		lbSystem.Items.Clear();
		Color[] systemColors = StiColors.SystemColors;
		foreach (Color color2 in systemColors)
		{
			lbSystem.Items.Add((object)color2);
		}
		lbWeb.EndUpdate();
		lbSystem.EndUpdate();
		((Control)lbSystem).MouseUp += new MouseEventHandler(ListBoxSystem_MouseUp);
		int i2 = StiScaleUI.I2;
		int i3 = StiScaleUI.I6;
		int num = i2;
		int num2 = i3;
		int num3 = 0;
		for (int k = 0; k < StiColors.CustomColors.Length; k++)
		{
			panels[k] = new Panel();
			((Control)panels[k]).Left = num;
			((Control)panels[k]).Top = num2;
			((Control)panels[k]).Width = StiScaleUI.XXI(24.0);
			((Control)panels[k]).Height = StiScaleUI.YYI(24.0);
			((Control)panels[k]).BackColor = StiColors.CustomColors[k];
			((Control)panels[k]).MouseEnter += Panel_MouseEnter;
			((Control)panels[k]).MouseLeave += Panel_MouseLeave;
			((Control)panels[k]).MouseDown += new MouseEventHandler(Panel_MouseDown);
			((Control)panels[k]).MouseUp += new MouseEventHandler(Panel_MouseUp);
			((Control)panels[k]).Paint += new PaintEventHandler(Panel_Paint);
			((Control)tbCustom).Controls.Add((Control)(object)panels[k]);
			num += StiScaleUI.XXI(24.0);
			num3++;
			if (num3 == 8)
			{
				num = i2;
				num2 += StiScaleUI.YYI(26.0);
				num3 = 0;
			}
		}
	}

	private void DrawItem(bool web, object sender, DrawItemEventArgs e)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Invalid comparison between Unknown and I4
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index == -1)
		{
			return;
		}
		Color color;
		string text;
		if (web)
		{
			color = StiColors.Colors[e.Index];
			text = ((!(color == Color.Transparent) || !isFromStyle) ? StiLocalization.Get("PropertyColor", color.Name) : Loc.Get("FormStyleDesigner", "FromStyle"));
		}
		else
		{
			color = StiColors.SystemColors[e.Index];
			text = StiLocalization.Get("PropertySystemColors", color.Name);
		}
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		if ((e.State & 1) > 0)
		{
			bounds.Width--;
		}
		Rectangle rectangle = new Rectangle(bounds.X + StiScaleUI.I2, bounds.Y + StiScaleUI.I2, StiScaleUI.XXI(22.0), bounds.Height - StiScaleUI.I5);
		Rectangle rectangle2 = new Rectangle(rectangle.Right + StiScaleUI.I2, bounds.Y, bounds.Width - rectangle.X, bounds.Height);
		StiControlPaint.DrawItemBackground(graphics, bounds, e.State);
		SolidBrush val = new SolidBrush(color);
		try
		{
			graphics.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		graphics.DrawRectangle(Pens.Black, rectangle);
		StringFormat val2 = new StringFormat();
		try
		{
			if ((int)((Control)this).RightToLeft == 1)
			{
				val2.FormatFlags = (StringFormatFlags)(val2.FormatFlags | 1);
			}
			graphics.DrawString(text, ((Control)this).Font, Brushes.Black, (RectangleF)rectangle2, val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void DrawPanel(object sender, bool isMouseOver)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		Panel val = (Panel)((sender is Panel) ? sender : null);
		Graphics val2 = Graphics.FromHwnd(((Control)val).Handle);
		try
		{
			Rectangle clientRectangle = ((Control)val).ClientRectangle;
			if (isMouseOver)
			{
				val2.FillRectangle(StiBrushes.Focus, clientRectangle);
				val2.DrawRectangle(StiPens.SelectedText, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
			}
			else
			{
				val2.FillRectangle(Brushes.White, clientRectangle);
			}
			clientRectangle.X += StiScaleUI.I2;
			clientRectangle.Y += StiScaleUI.I2;
			clientRectangle.Width -= StiScaleUI.I5;
			clientRectangle.Height -= StiScaleUI.I5;
			SolidBrush val3 = new SolidBrush(((Control)val).BackColor);
			try
			{
				val2.FillRectangle((Brush)(object)val3, clientRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			val2.DrawRectangle(SystemPens.ControlDark, clientRectangle);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		e.Graphics.FillRectangle(Brushes.White, clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 27)
		{
			ClosePopupForm();
		}
	}

	private void ListBoxSystem_DrawItem(object sender, DrawItemEventArgs e)
	{
		DrawItem(web: false, sender, e);
	}

	private void ListBoxWeb_DrawItem(object sender, DrawItemEventArgs e)
	{
		DrawItem(web: true, sender, e);
	}

	private void ListBoxWeb_MouseMove(object sender, MouseEventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		Point pt = ((Control)val).PointToClient(Cursor.Position);
		for (int i = 0; i < val.Items.Count; i++)
		{
			if (val.GetItemRectangle(i).Contains(pt))
			{
				((ListControl)val).SelectedIndex = i;
				break;
			}
		}
	}

	private void ListBoxWeb_MouseUp(object sender, MouseEventArgs e)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if (e == null || (e.Button & 0x100000) > 0)
		{
			if (lbWeb.SelectedItem != null)
			{
				SelectedColor = (Color)lbWeb.SelectedItem;
			}
			ClosePopupForm();
		}
	}

	private void ListBoxSystem_MouseUp(object sender, MouseEventArgs e)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if (e == null || (e.Button & 0x100000) > 0)
		{
			if (lbSystem.SelectedItem != null)
			{
				SelectedColor = (Color)lbSystem.SelectedItem;
			}
			ClosePopupForm();
		}
	}

	private void TabControl_Paint(object sender, PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		if (TabControl.SelectedIndex == 1 && lbWeb.SelectedItem != null)
		{
			((Control)lbWeb).Focus();
		}
		else if (TabControl.SelectedIndex == 2 && lbSystem.SelectedItem != null)
		{
			((Control)lbSystem).Focus();
		}
	}

	private void ButtonOther_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		ColorDialog val = new ColorDialog();
		try
		{
			val.FullOpen = true;
			val.Color = SelectedColor;
			val.CustomColors = StiColorDialogHelper.Load();
			if ((int)((CommonDialog)val).ShowDialog() != 1)
			{
				return;
			}
			StiColorDialogHelper.Save(val.CustomColors);
			SelectedColor = val.Color;
			if (colorBox is StiColorBox)
			{
				((StiColorBox)(object)colorBox).SelectedColor = val.Color;
			}
			ClosePopupForm();
			Color[] colors = StiColors.Colors;
			for (int i = 0; i < colors.Length; i++)
			{
				Color selectedColor = colors[i];
				if (selectedColor.ToArgb() == SelectedColor.ToArgb())
				{
					SelectedColor = selectedColor;
					break;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void Panel_MouseEnter(object sender, EventArgs e)
	{
		DrawPanel(sender, isMouseOver: true);
	}

	private void Panel_MouseLeave(object sender, EventArgs e)
	{
		DrawPanel(sender, isMouseOver: false);
	}

	private void Panel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((e.Button & 0x100000) > 0)
		{
			DrawPanel(sender, isMouseOver: true);
		}
	}

	private void Panel_MouseUp(object sender, MouseEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if (e != null && (e.Button & 0x100000) <= 0)
		{
			return;
		}
		Panel val = (Panel)sender;
		SelectedColor = ((Control)val).BackColor;
		Color[] colors = StiColors.Colors;
		for (int i = 0; i < colors.Length; i++)
		{
			Color selectedColor = colors[i];
			if (selectedColor.ToArgb() == SelectedColor.ToArgb())
			{
				SelectedColor = selectedColor;
				break;
			}
		}
		ClosePopupForm();
	}

	private void Panel_Paint(object sender, PaintEventArgs e)
	{
		DrawPanel(sender, isMouseOver: false);
	}

	public StiColorBoxPopupForm(Color selectedColor, IWindowsFormsEditorService edSrv)
		: this(null, selectedColor, edSrv, isFromStyle: false)
	{
	}

	public StiColorBoxPopupForm(Color selectedColor, IWindowsFormsEditorService edSrv, bool isFromStyle)
		: this(null, selectedColor, edSrv, isFromStyle)
	{
		((Form)this).TopLevel = false;
		SetLocation(0, 0, StiScaleUI.XXI(210.0), StiScaleUI.YYI(276.0));
	}

	public StiColorBoxPopupForm()
		: this(null, Color.Transparent)
	{
	}

	public StiColorBoxPopupForm(Control colorBox, Color selectedColor)
		: this(colorBox, selectedColor, null)
	{
	}

	public StiColorBoxPopupForm(Control colorBox, Color selectedColor, IWindowsFormsEditorService edSrv)
		: this(colorBox, selectedColor, edSrv, isFromStyle: false)
	{
	}

	public StiColorBoxPopupForm(Control colorBox, Color selectedColor, IWindowsFormsEditorService edSrv, bool isFromStyle)
		: base(colorBox)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		try
		{
			((Control)this).SuspendLayout();
			this.isFromStyle = isFromStyle;
			((Control)this).BackColor = Color.White;
			TabControl = new TabControl();
			((Control)TabControl).SuspendLayout();
			((Control)TabControl).BackColor = Color.White;
			((Control)TabControl).Dock = (DockStyle)5;
			TabControl.Paint += new PaintEventHandler(TabControl_Paint);
			((Control)this).Controls.Add((Control)(object)TabControl);
			tbCustom = new TabPage();
			((Control)tbCustom).BackColor = Color.White;
			((Control)tbCustom).SuspendLayout();
			((Control)tbCustom).Text = "Custom";
			((ScrollableControl)tbCustom).DockPadding.All = StiScaleUI.I4;
			((Control)TabControl).Controls.Add((Control)(object)tbCustom);
			tbWeb = new TabPage();
			((Control)tbWeb).BackColor = Color.White;
			((Control)tbWeb).SuspendLayout();
			((Control)tbWeb).Text = "Web";
			((ScrollableControl)tbWeb).DockPadding.All = StiScaleUI.I4;
			((Control)TabControl).Controls.Add((Control)(object)tbWeb);
			tbSystem = new TabPage();
			((Control)tbSystem).BackColor = Color.White;
			((Control)tbSystem).SuspendLayout();
			((ScrollableControl)tbSystem).DockPadding.All = StiScaleUI.I4;
			((Control)tbSystem).Text = "System";
			((Control)TabControl).Controls.Add((Control)(object)tbSystem);
			lbWeb = new ListBox();
			((Control)lbWeb).BackColor = Color.White;
			lbWeb.BorderStyle = (BorderStyle)0;
			lbWeb.DrawMode = (DrawMode)2;
			((Control)lbWeb).Dock = (DockStyle)5;
			lbWeb.IntegralHeight = false;
			((Control)lbWeb).TabStop = false;
			lbWeb.ItemHeight = StiScaleUI.I13;
			((Control)lbWeb).MouseMove += new MouseEventHandler(ListBoxWeb_MouseMove);
			lbWeb.DrawItem += new DrawItemEventHandler(ListBoxWeb_DrawItem);
			((Control)tbWeb).Controls.Add((Control)(object)lbWeb);
			lbSystem = new ListBox();
			((Control)lbSystem).BackColor = Color.White;
			lbSystem.BorderStyle = (BorderStyle)0;
			lbSystem.DrawMode = (DrawMode)2;
			((Control)lbSystem).Dock = (DockStyle)5;
			lbSystem.IntegralHeight = false;
			((Control)lbSystem).TabStop = false;
			lbSystem.ItemHeight = StiScaleUI.I13;
			((Control)lbSystem).MouseMove += new MouseEventHandler(ListBoxWeb_MouseMove);
			lbSystem.DrawItem += new DrawItemEventHandler(ListBoxSystem_DrawItem);
			((Control)tbSystem).Controls.Add((Control)(object)lbSystem);
			btOther = new Button();
			((ButtonBase)btOther).UseVisualStyleBackColor = true;
			((Control)btOther).Location = new Point(StiScaleUI.XXI(58.0), StiScaleUI.YYI(216.0));
			((Control)btOther).Name = "Other...";
			((Control)btOther).Size = new Size(StiScaleUI.XXI(80.0), StiScaleUI.YYI(23.0));
			((Control)btOther).Click += ButtonOther_Click;
			((Control)tbCustom).Controls.Add((Control)(object)btOther);
			((Control)TabControl).ResumeLayout(false);
			((Control)tbCustom).ResumeLayout(false);
			((Control)tbWeb).ResumeLayout(false);
			((Control)tbSystem).ResumeLayout(false);
			((Control)this).ResumeLayout(false);
			this.edSrv = edSrv;
			this.colorBox = colorBox;
			SelectedColor = selectedColor;
			InitColors();
			((ListControl)lbSystem).SelectedIndex = -1;
			((ListControl)lbWeb).SelectedIndex = -1;
			bool flag = false;
			int num = 0;
			string text = selectedColor.ToString();
			Color[] systemColors = StiColors.SystemColors;
			for (int i = 0; i < systemColors.Length; i++)
			{
				Color color = systemColors[i];
				if (color.ToString() == text)
				{
					TabControl.SelectedIndex = 2;
					((ListControl)lbSystem).SelectedIndex = num;
					flag = true;
					break;
				}
				num++;
			}
			if (!flag)
			{
				num = 0;
				int num2 = selectedColor.ToArgb();
				Color[] colors = StiColors.Colors;
				foreach (Color color2 in colors)
				{
					if (color2.ToArgb() == num2)
					{
						TabControl.SelectedIndex = 1;
						((ListControl)lbWeb).SelectedIndex = num;
						flag = true;
						break;
					}
					num++;
				}
			}
			if (!flag)
			{
				TabControl.SelectedIndex = 0;
			}
			if (!((Component)(object)this).DesignMode)
			{
				Localize();
			}
		}
		catch
		{
		}
	}
}
