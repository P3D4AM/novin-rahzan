using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ColorSchemeEditor : Form
{
	private PropertyGrid propertyGrid1;

	private GroupBox groupBox1;

	private Container components;

	private ColorScheme m_ColorScheme;

	private Button button1;

	private Button button2;

	private MenuPanel m_Menu;

	private Bar m_SimpleBar;

	private Bar m_FloatBar;

	private Button button3;

	private Bar m_PopupBar;

	private Button btnSaveScheme;

	private Button btnLoadScheme;

	private OpenFileDialog openFileDialog1;

	private SaveFileDialog saveFileDialog1;

	public bool ColorSchemeChanged;

	public ColorScheme ColorScheme
	{
		get
		{
			return m_ColorScheme;
		}
		set
		{
			if (m_Menu == null)
			{
				AddDotNetBarControls();
			}
			m_ColorScheme = value;
			propertyGrid1.SelectedObject = m_ColorScheme;
			m_Menu.ColorScheme = m_ColorScheme;
			m_SimpleBar.ColorScheme = m_ColorScheme;
			m_FloatBar.ColorScheme = m_ColorScheme;
			m_PopupBar.ColorScheme = m_ColorScheme;
			ColorSchemeChanged = false;
		}
	}

	public ColorSchemeEditor()
	{
		InitializeComponent();
		((Form)this).StartPosition = (FormStartPosition)1;
	}

	private void AddDotNetBarControls()
	{
		if (m_Menu != null)
		{
			m_Menu.RecalcSize();
			m_SimpleBar.Size = new Size(((Control)groupBox1).ClientRectangle.Width - 12, 16);
			m_SimpleBar.RecalcSize();
			m_FloatBar.Size = new Size(((Control)groupBox1).ClientRectangle.Width, 16);
			m_FloatBar.RecalcSize();
			m_PopupBar.Size = new Size(((Control)groupBox1).ClientRectangle.Width, 24);
			m_PopupBar.RecalcSize();
			return;
		}
		m_Menu = new MenuPanel();
		m_Menu.PopupMenu = false;
		((Control)m_Menu).Location = new Point(8, 135);
		m_SimpleBar = new Bar();
		m_SimpleBar.PassiveBar = true;
		m_SimpleBar.Location = new Point(4, 16);
		m_SimpleBar.ThemeAware = false;
		m_FloatBar = new Bar();
		m_FloatBar.PassiveBar = true;
		m_FloatBar.Location = new Point(4, 46);
		m_FloatBar.ThemeAware = false;
		m_PopupBar = new Bar();
		m_PopupBar.PassiveBar = true;
		m_PopupBar.Location = new Point(4, 96);
		m_PopupBar.ThemeAware = false;
		ButtonItem buttonItem = new ButtonItem();
		Bitmap val = null;
		ButtonItem buttonItem2 = new ButtonItem("new", "&New...");
		val = BarFunctions.LoadBitmap("BarEditorImages.FileNew.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		m_SimpleBar.Items.Add(buttonItem2.Copy());
		m_FloatBar.Items.Add(buttonItem2.Copy());
		m_PopupBar.Items.Add(buttonItem2.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "&Open");
		val = BarFunctions.LoadBitmap("BarEditorImages.FileOpen.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		ButtonItem buttonItem3 = (ButtonItem)buttonItem2.Copy();
		buttonItem3.ButtonStyle = eButtonStyle.ImageAndText;
		m_SimpleBar.Items.Add(buttonItem3);
		m_FloatBar.Items.Add(buttonItem3.Copy());
		m_PopupBar.Items.Add(buttonItem3.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("close", "&Close");
		val = BarFunctions.LoadBitmap("BarEditorImages.FileClose.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		buttonItem3 = (ButtonItem)buttonItem2.Copy();
		buttonItem3.Checked = true;
		buttonItem3.ButtonStyle = eButtonStyle.ImageAndText;
		m_SimpleBar.Items.Add(buttonItem3);
		m_PopupBar.Items.Add(buttonItem3.Copy());
		buttonItem3 = (ButtonItem)buttonItem3.Copy();
		buttonItem3.Enabled = false;
		m_FloatBar.Items.Add(buttonItem3.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "Add Ne&w Item...");
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "Add Existin&g Item...");
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("opensol", "Open Solution...");
		buttonItem2.BeginGroup = true;
		val = BarFunctions.LoadBitmap("BarEditorImages.FileOpenSol.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		buttonItem3 = (ButtonItem)buttonItem2.Copy();
		buttonItem3.Enabled = false;
		m_SimpleBar.Items.Add(buttonItem3);
		m_FloatBar.Items.Add(buttonItem3.Copy());
		m_PopupBar.Items.Add(buttonItem3.Copy());
		buttonItem.SubItems.Add(buttonItem2);
		buttonItem2 = new ButtonItem("open", "Close Solution");
		val = BarFunctions.LoadBitmap("BarEditorImages.FileCloseSol.bmp");
		val.MakeTransparent(Color.Magenta);
		buttonItem2.Image = (Image)(object)val;
		buttonItem2.Enabled = false;
		buttonItem.SubItems.Add(buttonItem2);
		m_Menu.ParentItem = buttonItem;
		((Control)groupBox1).Controls.Add((Control)(object)m_Menu);
		m_Menu.RecalcSize();
		m_Menu.Show();
		m_SimpleBar.Size = new Size(((Control)groupBox1).ClientRectangle.Width, 16);
		m_SimpleBar.GrabHandleStyle = eGrabHandleStyle.StripeFlat;
		((Control)groupBox1).Controls.Add((Control)(object)m_SimpleBar);
		m_SimpleBar.RecalcSize();
		m_SimpleBar.Show();
		m_FloatBar.SetBarState(eBarState.Floating);
		m_FloatBar.Size = new Size(((Control)groupBox1).ClientRectangle.Width, 16);
		((Control)groupBox1).Controls.Add((Control)(object)m_FloatBar);
		((Control)m_FloatBar).Text = "Bar Caption";
		m_FloatBar.RecalcSize();
		m_FloatBar.Show();
		m_PopupBar.SetBarState(eBarState.Popup);
		m_PopupBar.Size = new Size(((Control)groupBox1).ClientRectangle.Width, 24);
		m_PopupBar.PopupWidth = ((Control)groupBox1).ClientRectangle.Width;
		((Control)groupBox1).Controls.Add((Control)(object)m_PopupBar);
		m_PopupBar.RecalcSize();
		m_PopupBar.Show();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
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
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		propertyGrid1 = new PropertyGrid();
		groupBox1 = new GroupBox();
		button1 = new Button();
		button2 = new Button();
		button3 = new Button();
		btnSaveScheme = new Button();
		btnLoadScheme = new Button();
		openFileDialog1 = new OpenFileDialog();
		saveFileDialog1 = new SaveFileDialog();
		((Control)this).SuspendLayout();
		propertyGrid1.CommandsVisibleIfAvailable = true;
		propertyGrid1.LargeButtons = false;
		propertyGrid1.LineColor = SystemColors.ScrollBar;
		((Control)propertyGrid1).Location = new Point(208, 4);
		((Control)propertyGrid1).Name = "propertyGrid1";
		((Control)propertyGrid1).Size = new Size(352, 299);
		((Control)propertyGrid1).TabIndex = 0;
		((Control)propertyGrid1).Text = "propertyGrid1";
		propertyGrid1.ViewBackColor = SystemColors.Window;
		propertyGrid1.ViewForeColor = SystemColors.WindowText;
		propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);
		((Control)groupBox1).Location = new Point(10, 0);
		((Control)groupBox1).Name = "groupBox1";
		((Control)groupBox1).Size = new Size(190, 303);
		((Control)groupBox1).TabIndex = 1;
		groupBox1.TabStop = false;
		((Control)groupBox1).Text = "Color Scheme Preview:";
		((ButtonBase)button1).FlatStyle = (FlatStyle)3;
		((Control)button1).Location = new Point(404, 307);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(76, 24);
		((Control)button1).TabIndex = 2;
		((Control)button1).Text = "OK";
		((Control)button1).Click += button1_Click;
		((ButtonBase)button2).FlatStyle = (FlatStyle)3;
		((Control)button2).Location = new Point(316, 307);
		((Control)button2).Name = "button2";
		((Control)button2).Size = new Size(76, 24);
		((Control)button2).TabIndex = 3;
		((Control)button2).Text = "&Reset";
		((Control)button2).Click += button2_Click;
		((ButtonBase)button3).FlatStyle = (FlatStyle)3;
		((Control)button3).Location = new Point(484, 307);
		((Control)button3).Name = "button3";
		((Control)button3).Size = new Size(76, 24);
		((Control)button3).TabIndex = 4;
		((Control)button3).Text = "Cancel";
		((Control)button3).Click += button3_Click;
		((ButtonBase)btnSaveScheme).FlatStyle = (FlatStyle)3;
		((Control)btnSaveScheme).Location = new Point(10, 308);
		((Control)btnSaveScheme).Name = "btnSaveScheme";
		((Control)btnSaveScheme).Size = new Size(92, 24);
		((Control)btnSaveScheme).TabIndex = 5;
		((Control)btnSaveScheme).Text = "Save Scheme...";
		((Control)btnSaveScheme).Click += btnSaveScheme_Click;
		((ButtonBase)btnLoadScheme).FlatStyle = (FlatStyle)3;
		((Control)btnLoadScheme).Location = new Point(107, 308);
		((Control)btnLoadScheme).Name = "btnLoadScheme";
		((Control)btnLoadScheme).Size = new Size(92, 24);
		((Control)btnLoadScheme).TabIndex = 6;
		((Control)btnLoadScheme).Text = "Load Scheme...";
		((Control)btnLoadScheme).Click += btnLoadScheme_Click;
		((FileDialog)openFileDialog1).DefaultExt = "*.dcs";
		((FileDialog)openFileDialog1).Filter = "DotNetBar Color Scheme files|*.dcs|All files|*.*";
		((FileDialog)openFileDialog1).Title = "Open DotNetBar Color Scheme File";
		((FileDialog)saveFileDialog1).DefaultExt = "dcs";
		((FileDialog)saveFileDialog1).FileName = "colorscheme1";
		((FileDialog)saveFileDialog1).Filter = "DotNetBar Color Scheme files|*.dcs|All files|*.*";
		((FileDialog)saveFileDialog1).Title = "Save DotNetBar Color Scheme File";
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(567, 338);
		((Control)this).Controls.AddRange((Control[])(object)new Control[7]
		{
			(Control)btnLoadScheme,
			(Control)btnSaveScheme,
			(Control)button3,
			(Control)button2,
			(Control)button1,
			(Control)groupBox1,
			(Control)propertyGrid1
		});
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "ColorSchemeEditor";
		((Control)this).Text = "ColorScheme Editor";
		((Form)this).Load += ColorSchemeEditorLoad;
		((Control)this).ResumeLayout(false);
	}

	private void ColorSchemeEditorLoad(object sender, EventArgs e)
	{
		AddDotNetBarControls();
	}

	private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		((Control)m_Menu).Refresh();
		((Control)m_SimpleBar).Refresh();
		((Control)m_FloatBar).Refresh();
		((Control)m_PopupBar).Refresh();
		ColorSchemeChanged = true;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		((Control)this).Hide();
	}

	private void button2_Click(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		if ((int)MessageBox.Show("Reseting the Color Scheme will destroy any changes you made. Are you sure you want to reset the Color Scheme?", "Color Scheme Editor", (MessageBoxButtons)4) == 6)
		{
			ColorScheme = new ColorScheme(ColorScheme.Style);
			ColorSchemeChanged = true;
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		ColorSchemeChanged = false;
		((Control)this).Hide();
	}

	private void btnSaveScheme_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((CommonDialog)saveFileDialog1).ShowDialog() == 1)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("colorschemes");
			xmlDocument.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlDocument.CreateElement("colorscheme");
			xmlElement.AppendChild(xmlElement2);
			m_ColorScheme.Serialize(xmlElement2);
			xmlDocument.Save(((FileDialog)saveFileDialog1).FileName);
			MessageBox.Show("Color Scheme has been saved.", "DotNetBar Color Scheme Editor", (MessageBoxButtons)0, (MessageBoxIcon)64);
		}
	}

	private void btnLoadScheme_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)((CommonDialog)openFileDialog1).ShowDialog() == 1 && File.Exists(((FileDialog)openFileDialog1).FileName))
		{
			ColorScheme colorScheme = new ColorScheme();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(((FileDialog)openFileDialog1).FileName);
			colorScheme.Deserialize(xmlDocument.FirstChild.FirstChild as XmlElement);
			ColorScheme = colorScheme;
			ColorSchemeChanged = true;
		}
	}
}
