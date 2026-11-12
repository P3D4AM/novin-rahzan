using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.UI;

[ToolboxItem(false)]
internal class ColorPicker : UserControl
{
	private Color[] m_CustomColors = new Color[48];

	private Rectangle[] m_CustomColorsPos = new Rectangle[48];

	private object m_ColorScheme;

	private string m_TransText = "";

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Label label1;

	private ListBox listScheme;

	private Button btnOK;

	private Button btnCancel;

	private Panel colorPanel;

	private ListBox listSystem;

	private ListBox listWeb;

	private Color m_SelectedColor = Color.Empty;

	private string m_SelectedColorSchemeName = "";

	private Panel colorPreview;

	private TrackBar transparencyTrack;

	private bool m_Canceled;

	private IWindowsFormsEditorService m_EditorService;

	private Container components;

	internal IWindowsFormsEditorService EditorService
	{
		get
		{
			return m_EditorService;
		}
		set
		{
			m_EditorService = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public object ColorScheme
	{
		get
		{
			return m_ColorScheme;
		}
		set
		{
			m_ColorScheme = value;
			InitColorSchemeColors();
		}
	}

	public Color SelectedColor
	{
		get
		{
			return m_SelectedColor;
		}
		set
		{
			m_SelectedColor = value;
			OnSelectedColorChanged();
		}
	}

	public string SelectedColorSchemeName
	{
		get
		{
			return m_SelectedColorSchemeName;
		}
		set
		{
			m_SelectedColorSchemeName = value;
		}
	}

	public bool Canceled => m_Canceled;

	public ColorPicker()
	{
		InitializeComponent();
		InitCustomColors();
		InitOtherColors();
		m_TransText = ((Control)label1).Text;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Expected O, but got Unknown
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		tabControl1 = new TabControl();
		tabPage1 = new TabPage();
		listScheme = new ListBox();
		tabPage2 = new TabPage();
		listSystem = new ListBox();
		tabPage3 = new TabPage();
		listWeb = new ListBox();
		tabPage4 = new TabPage();
		colorPanel = new Panel();
		transparencyTrack = new TrackBar();
		label1 = new Label();
		colorPreview = new Panel();
		btnOK = new Button();
		btnCancel = new Button();
		((Control)tabControl1).SuspendLayout();
		((Control)tabPage1).SuspendLayout();
		((Control)tabPage2).SuspendLayout();
		((Control)tabPage3).SuspendLayout();
		((Control)tabPage4).SuspendLayout();
		((ISupportInitialize)transparencyTrack).BeginInit();
		((Control)this).SuspendLayout();
		((Control)tabControl1).Controls.Add((Control)(object)tabPage1);
		((Control)tabControl1).Controls.Add((Control)(object)tabPage2);
		((Control)tabControl1).Controls.Add((Control)(object)tabPage3);
		((Control)tabControl1).Controls.Add((Control)(object)tabPage4);
		((Control)tabControl1).Location = new Point(1, 1);
		((Control)tabControl1).Name = "tabControl1";
		tabControl1.SelectedIndex = 0;
		((Control)tabControl1).Size = new Size(208, 192);
		((Control)tabControl1).TabIndex = 0;
		tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
		((Control)tabPage1).Controls.Add((Control)(object)listScheme);
		tabPage1.Location = new Point(4, 22);
		((Control)tabPage1).Name = "tabPage1";
		((Control)tabPage1).Size = new Size(200, 166);
		tabPage1.TabIndex = 0;
		((Control)tabPage1).Text = "Scheme";
		((Control)listScheme).Dock = (DockStyle)5;
		listScheme.DrawMode = (DrawMode)1;
		listScheme.IntegralHeight = false;
		((Control)listScheme).Location = new Point(0, 0);
		((Control)listScheme).Name = "listScheme";
		((Control)listScheme).Size = new Size(200, 166);
		((Control)listScheme).TabIndex = 0;
		listScheme.DrawItem += new DrawItemEventHandler(DrawColorItem);
		listScheme.SelectedIndexChanged += ListSelectionChange;
		((Control)tabPage2).Controls.Add((Control)(object)listSystem);
		tabPage2.Location = new Point(4, 22);
		((Control)tabPage2).Name = "tabPage2";
		((Control)tabPage2).Size = new Size(200, 166);
		tabPage2.TabIndex = 1;
		((Control)tabPage2).Text = "System";
		((Control)listSystem).Dock = (DockStyle)5;
		listSystem.DrawMode = (DrawMode)1;
		listSystem.IntegralHeight = false;
		((Control)listSystem).Location = new Point(0, 0);
		((Control)listSystem).Name = "listSystem";
		((Control)listSystem).Size = new Size(200, 166);
		((Control)listSystem).TabIndex = 1;
		listSystem.DrawItem += new DrawItemEventHandler(DrawColorItem);
		listSystem.SelectedIndexChanged += ListSelectionChange;
		((Control)tabPage3).Controls.Add((Control)(object)listWeb);
		tabPage3.Location = new Point(4, 22);
		((Control)tabPage3).Name = "tabPage3";
		((Control)tabPage3).Size = new Size(200, 166);
		tabPage3.TabIndex = 2;
		((Control)tabPage3).Text = "Web";
		((Control)listWeb).Dock = (DockStyle)5;
		listWeb.DrawMode = (DrawMode)1;
		listWeb.IntegralHeight = false;
		((Control)listWeb).Location = new Point(0, 0);
		((Control)listWeb).Name = "listWeb";
		((Control)listWeb).Size = new Size(200, 166);
		((Control)listWeb).TabIndex = 1;
		listWeb.DrawItem += new DrawItemEventHandler(DrawColorItem);
		listWeb.SelectedIndexChanged += ListSelectionChange;
		((Control)tabPage4).Controls.Add((Control)(object)colorPanel);
		tabPage4.Location = new Point(4, 22);
		((Control)tabPage4).Name = "tabPage4";
		((Control)tabPage4).Size = new Size(200, 166);
		tabPage4.TabIndex = 3;
		((Control)tabPage4).Text = "Custom";
		((Control)colorPanel).Dock = (DockStyle)5;
		((Control)colorPanel).Location = new Point(0, 0);
		((Control)colorPanel).Name = "colorPanel";
		((Control)colorPanel).Size = new Size(200, 166);
		((Control)colorPanel).TabIndex = 0;
		((Control)colorPanel).MouseUp += new MouseEventHandler(CustomColorMouseUp);
		((Control)colorPanel).Paint += new PaintEventHandler(PaintCustomColors);
		((Control)transparencyTrack).Enabled = false;
		((Control)transparencyTrack).Location = new Point(1, 204);
		transparencyTrack.Maximum = 255;
		((Control)transparencyTrack).Name = "transparencyTrack";
		((Control)transparencyTrack).Size = new Size(200, 45);
		((Control)transparencyTrack).TabIndex = 1;
		transparencyTrack.TickFrequency = 16;
		transparencyTrack.Value = 255;
		transparencyTrack.ValueChanged += transparencyTrack_ValueChanged;
		((Control)label1).Location = new Point(1, 194);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(136, 16);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = "Transparency";
		((Control)colorPreview).BackColor = Color.White;
		colorPreview.BorderStyle = (BorderStyle)1;
		((Control)colorPreview).Location = new Point(8, 240);
		((Control)colorPreview).Name = "colorPreview";
		((Control)colorPreview).Size = new Size(40, 32);
		((Control)colorPreview).TabIndex = 3;
		((Control)colorPreview).Paint += new PaintEventHandler(colorPreview_Paint);
		((ButtonBase)btnOK).FlatStyle = (FlatStyle)3;
		((Control)btnOK).Location = new Point(72, 248);
		((Control)btnOK).Name = "btnOK";
		((Control)btnOK).Size = new Size(64, 24);
		((Control)btnOK).TabIndex = 4;
		((Control)btnOK).Text = "OK";
		((Control)btnOK).Click += btnOK_Click;
		((ButtonBase)btnCancel).FlatStyle = (FlatStyle)3;
		((Control)btnCancel).Location = new Point(142, 248);
		((Control)btnCancel).Name = "btnCancel";
		((Control)btnCancel).Size = new Size(64, 24);
		((Control)btnCancel).TabIndex = 5;
		((Control)btnCancel).Text = "Cancel";
		((Control)btnCancel).Click += btnCancel_Click;
		((Control)this).Controls.Add((Control)(object)btnCancel);
		((Control)this).Controls.Add((Control)(object)btnOK);
		((Control)this).Controls.Add((Control)(object)colorPreview);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)tabControl1);
		((Control)this).Controls.Add((Control)(object)transparencyTrack);
		((ScrollableControl)this).DockPadding.All = 1;
		((Control)this).Name = "ColorPicker";
		((Control)this).Size = new Size(211, 280);
		((Control)this).Paint += new PaintEventHandler(ColorPicker_Paint);
		((Control)tabControl1).ResumeLayout(false);
		((Control)tabPage1).ResumeLayout(false);
		((Control)tabPage2).ResumeLayout(false);
		((Control)tabPage3).ResumeLayout(false);
		((Control)tabPage4).ResumeLayout(false);
		((ISupportInitialize)transparencyTrack).EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void InitCustomColors()
	{
		m_CustomColors[0] = Color.FromArgb(255, 255, 255);
		m_CustomColors[1] = Color.FromArgb(255, 195, 198);
		m_CustomColors[2] = Color.FromArgb(255, 227, 198);
		m_CustomColors[3] = Color.FromArgb(255, 255, 198);
		m_CustomColors[4] = Color.FromArgb(198, 255, 198);
		m_CustomColors[5] = Color.FromArgb(198, 255, 255);
		m_CustomColors[6] = Color.FromArgb(198, 195, 255);
		m_CustomColors[7] = Color.FromArgb(255, 195, 255);
		m_CustomColors[8] = Color.FromArgb(231, 227, 231);
		m_CustomColors[9] = Color.FromArgb(255, 130, 132);
		m_CustomColors[10] = Color.FromArgb(255, 195, 132);
		m_CustomColors[11] = Color.FromArgb(255, 255, 132);
		m_CustomColors[12] = Color.FromArgb(132, 255, 132);
		m_CustomColors[13] = Color.FromArgb(132, 255, 255);
		m_CustomColors[14] = Color.FromArgb(132, 130, 255);
		m_CustomColors[15] = Color.FromArgb(255, 130, 255);
		m_CustomColors[16] = Color.FromArgb(198, 195, 198);
		m_CustomColors[17] = Color.FromArgb(255, 0, 0);
		m_CustomColors[18] = Color.FromArgb(255, 130, 0);
		m_CustomColors[19] = Color.FromArgb(255, 255, 0);
		m_CustomColors[20] = Color.FromArgb(0, 255, 0);
		m_CustomColors[21] = Color.FromArgb(0, 255, 255);
		m_CustomColors[22] = Color.FromArgb(0, 0, 255);
		m_CustomColors[23] = Color.FromArgb(255, 0, 255);
		m_CustomColors[24] = Color.FromArgb(132, 130, 132);
		m_CustomColors[25] = Color.FromArgb(198, 0, 0);
		m_CustomColors[26] = Color.FromArgb(198, 65, 0);
		m_CustomColors[27] = Color.FromArgb(198, 195, 0);
		m_CustomColors[28] = Color.FromArgb(0, 195, 0);
		m_CustomColors[29] = Color.FromArgb(0, 195, 198);
		m_CustomColors[30] = Color.FromArgb(0, 0, 198);
		m_CustomColors[31] = Color.FromArgb(198, 0, 198);
		m_CustomColors[32] = Color.FromArgb(66, 65, 66);
		m_CustomColors[33] = Color.FromArgb(132, 0, 0);
		m_CustomColors[34] = Color.FromArgb(132, 65, 0);
		m_CustomColors[35] = Color.FromArgb(132, 130, 0);
		m_CustomColors[36] = Color.FromArgb(0, 130, 0);
		m_CustomColors[37] = Color.FromArgb(0, 130, 132);
		m_CustomColors[38] = Color.FromArgb(0, 0, 132);
		m_CustomColors[39] = Color.FromArgb(132, 0, 132);
		m_CustomColors[40] = Color.FromArgb(0, 0, 0);
		m_CustomColors[41] = Color.FromArgb(66, 0, 0);
		m_CustomColors[42] = Color.FromArgb(132, 65, 66);
		m_CustomColors[43] = Color.FromArgb(66, 65, 0);
		m_CustomColors[44] = Color.FromArgb(0, 65, 0);
		m_CustomColors[45] = Color.FromArgb(0, 65, 66);
		m_CustomColors[46] = Color.FromArgb(0, 0, 66);
		m_CustomColors[47] = Color.FromArgb(66, 0, 66);
	}

	private void InitOtherColors()
	{
		listWeb.BeginUpdate();
		listWeb.Items.Clear();
		Type typeFromHandle = typeof(Color);
		PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Static | BindingFlags.Public);
		Color color = default(Color);
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			listWeb.Items.Add(propertyInfo.GetValue(color, null));
		}
		listWeb.EndUpdate();
		listSystem.BeginUpdate();
		listSystem.Items.Clear();
		typeFromHandle = typeof(SystemColors);
		properties = typeFromHandle.GetProperties(BindingFlags.Static | BindingFlags.Public);
		PropertyInfo[] array2 = properties;
		foreach (PropertyInfo propertyInfo2 in array2)
		{
			listSystem.Items.Add(propertyInfo2.GetValue(color, null));
		}
		listSystem.EndUpdate();
	}

	private void InitColorSchemeColors()
	{
		if (m_ColorScheme != null)
		{
			if (!tabControl1.TabPages.Contains(tabPage1))
			{
				tabControl1.TabPages.Add(tabPage1);
			}
			listScheme.BeginUpdate();
			listScheme.Items.Clear();
			Type type = m_ColorScheme.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (propertyInfo.PropertyType == typeof(Color))
				{
					listScheme.Items.Add((object)propertyInfo.Name);
				}
			}
			listScheme.EndUpdate();
		}
		else if (tabControl1.TabPages.Contains(tabPage1))
		{
			tabControl1.TabPages.Remove(tabPage1);
		}
	}

	private void PaintCustomColors(object sender, PaintEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		Rectangle empty = Rectangle.Empty;
		int num = 6;
		int num2 = 12;
		Graphics graphics = e.Graphics;
		Border3DSide val = (Border3DSide)15;
		int width = ((Control)colorPanel).ClientRectangle.Width;
		int num3 = 0;
		Color[] customColors = m_CustomColors;
		foreach (Color color in customColors)
		{
			empty = new Rectangle(num, num2, 21, 21);
			if (empty.Right > width)
			{
				num2 += 25;
				num = (empty.X = 6);
				empty.Y = num2;
			}
			ControlPaint.DrawBorder3D(graphics, num, num2, 21, 21, (Border3DStyle)10, val);
			empty.Inflate(-2, -2);
			graphics.FillRectangle((Brush)new SolidBrush(color), empty);
			m_CustomColorsPos[num3] = empty;
			num3++;
			num += 24;
		}
	}

	private void DrawColorItem(object sender, DrawItemEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		Rectangle rectangle = new Rectangle(bounds.X + 1, bounds.Y + 2, 24, bounds.Height - 4);
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		Color color = SystemColors.ControlText;
		if ((e.State & 1) != 0)
		{
			color = SystemColors.HighlightText;
			e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
		}
		else
		{
			e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
		}
		Color empty = Color.Empty;
		string text = "";
		if (val.Items[e.Index].GetType() == typeof(Color))
		{
			empty = (Color)val.Items[e.Index];
			text = empty.Name;
		}
		else
		{
			text = val.Items[e.Index].ToString();
			empty = (Color)m_ColorScheme.GetType().GetProperty(text).GetValue(m_ColorScheme, null);
		}
		e.Graphics.FillRectangle((Brush)new SolidBrush(empty), rectangle);
		e.Graphics.DrawRectangle(SystemPens.ControlText, rectangle);
		bounds.Offset(30, 0);
		bounds.Width -= 30;
		TextDrawing.DrawString(e.Graphics, text, ((Control)val).Font, color, bounds, eTextFormat.Default);
	}

	private void CustomColorMouseUp(object sender, MouseEventArgs e)
	{
		for (int i = 0; i < 48; i++)
		{
			if (m_CustomColorsPos[i].Contains(e.X, e.Y))
			{
				SelectedColor = m_CustomColors[i];
				m_SelectedColorSchemeName = "";
				break;
			}
		}
	}

	private void ListSelectionChange(object sender, EventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		if (val.SelectedItem != null)
		{
			if (val.SelectedItem is Color)
			{
				SelectedColor = (Color)val.SelectedItem;
				m_SelectedColorSchemeName = "";
			}
			else
			{
				m_SelectedColorSchemeName = val.SelectedItem.ToString();
				SelectedColor = (Color)m_ColorScheme.GetType().GetProperty(SelectedColorSchemeName).GetValue(m_ColorScheme, null);
			}
		}
		else
		{
			SelectedColor = Color.Empty;
			m_SelectedColorSchemeName = "";
		}
	}

	private void OnSelectedColorChanged()
	{
		((Control)colorPreview).BackColor = m_SelectedColor;
		transparencyTrack.Value = m_SelectedColor.A;
		UpdateTransparencyText();
	}

	public void UpdateUIWithSelection()
	{
		((ListControl)listSystem).SelectedIndex = -1;
		((ListControl)listWeb).SelectedIndex = -1;
		((ListControl)listScheme).SelectedIndex = -1;
		if (m_SelectedColor.IsSystemColor)
		{
			tabControl1.SelectedTab = tabPage2;
			SelectListBoxItem(listSystem, m_SelectedColor.Name);
		}
		else if (m_SelectedColor.IsNamedColor)
		{
			tabControl1.SelectedTab = tabPage3;
			SelectListBoxItem(listWeb, m_SelectedColor.Name);
		}
		else if (m_SelectedColorSchemeName != "")
		{
			tabControl1.SelectedTab = tabPage1;
			SelectListBoxItem(listScheme, m_SelectedColorSchemeName);
		}
		else
		{
			tabControl1.SelectedTab = tabPage4;
		}
	}

	private void SelectListBoxItem(ListBox listBox, string item)
	{
		foreach (object item2 in listBox.Items)
		{
			if (item2.ToString() == item)
			{
				listBox.SelectedItem = item2;
				break;
			}
		}
	}

	private void UpdateTransparencyText()
	{
		if (!SelectedColor.IsEmpty)
		{
			((Control)label1).Text = m_TransText + " (" + SelectedColor.A + ")";
		}
		else
		{
			((Control)label1).Text = m_TransText;
		}
	}

	private void transparencyTrack_ValueChanged(object sender, EventArgs e)
	{
		if (!SelectedColor.IsEmpty && SelectedColor.A != transparencyTrack.Value)
		{
			SelectedColor = Color.FromArgb(transparencyTrack.Value, SelectedColor);
			m_SelectedColorSchemeName = "";
		}
		UpdateTransparencyText();
	}

	private void colorPreview_Paint(object sender, PaintEventArgs e)
	{
		if (SelectedColor.IsEmpty)
		{
			Rectangle clientRectangle = ((Control)colorPreview).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			e.Graphics.DrawLine(SystemPens.ControlText, clientRectangle.X, clientRectangle.Y, clientRectangle.Right, clientRectangle.Bottom);
			e.Graphics.DrawLine(SystemPens.ControlText, clientRectangle.Right, clientRectangle.Y, clientRectangle.X, clientRectangle.Bottom);
		}
	}

	private void btnOK_Click(object sender, EventArgs e)
	{
		ClosePicker();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		m_Canceled = true;
		ClosePicker();
	}

	private void ClosePicker()
	{
		if (m_EditorService != null)
		{
			m_EditorService.CloseDropDown();
		}
		else if (((Control)this).Parent != null)
		{
			((Control)this).Parent.Hide();
		}
		else
		{
			((Control)this).Hide();
		}
	}

	private void ColorPicker_Paint(object sender, PaintEventArgs e)
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Width--;
		clientRectangle.Height--;
		e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, clientRectangle);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 0 && m_ColorScheme != null && m_SelectedColorSchemeName != "")
		{
			((Control)transparencyTrack).Enabled = false;
		}
		else
		{
			((Control)transparencyTrack).Enabled = true;
		}
	}
}
