using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.ReportProducer.UC;

public class UCPanel : UserControl
{
	public delegate void OnExpand_UserControlEventHandler();

	public delegate void OnCollapse_UserControlEventHandler();

	public delegate void ControlResized_UserControlEventHandler();

	private bool _onResizing;

	private string _headerText = "";

	private string _headerText2 = "";

	private int _hRem;

	private int _initPanelH;

	private bool _expand;

	private IContainer components;

	private LinkLabel LLTitle;

	private PictureBox PIC;

	internal Label LLTitle2;

	internal Timer Timer1;

	internal Panel panelKol;

	private PanelEx MainPanel;

	internal Panel PanelTop_Header;

	internal Panel panelMain;

	public string HeaderText
	{
		get
		{
			return _headerText;
		}
		set
		{
			_headerText = value;
			((Control)LLTitle).Text = _headerText;
		}
	}

	public string HeaderText2
	{
		get
		{
			return _headerText2;
		}
		set
		{
			_headerText2 = value;
			((Control)LLTitle2).Text = _headerText2;
		}
	}

	public bool Expand
	{
		get
		{
			return _expand;
		}
		set
		{
			if (_expand != value)
			{
				_expand = value;
				((Control)MainPanel).Enabled = Expand;
				if (!Expand)
				{
					_initPanelH = ((Control)this).Height - ((Control)PanelTop_Header).Height - 2;
				}
				if (Expand)
				{
					_hRem = _initPanelH;
					Timer1.Enabled = true;
					PIC.Image = (Image)(object)Resources.CollapseUp;
					this.OnExpand?.Invoke();
				}
				else
				{
					_hRem = -_initPanelH;
					Timer1.Enabled = true;
					PIC.Image = (Image)(object)Resources.expandPic;
					this.OnCollapse?.Invoke();
				}
			}
		}
	}

	public event OnExpand_UserControlEventHandler OnExpand;

	public event OnCollapse_UserControlEventHandler OnCollapse;

	public event ControlResized_UserControlEventHandler ControlResized;

	public UCPanel()
	{
		InitializeComponent();
		((Control)this).Height = ((Control)PanelTop_Header).Height;
		_expand = true;
		_initPanelH = ((Control)MainPanel).Height;
		PIC.Image = (Image)(object)Resources.CollapseUp;
	}

	private void PIC_Click(object sender, EventArgs e)
	{
		Expand = !Expand;
	}

	private void LLTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Expand = !Expand;
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		_onResizing = true;
		Application.DoEvents();
		if (_hRem > 0)
		{
			_hRem -= _initPanelH;
			((Control)this).Height = ((Control)this).Height + _initPanelH;
		}
		else
		{
			_hRem += _initPanelH;
			((Control)this).Height = ((Control)this).Height - _initPanelH;
		}
		if (_hRem == 0)
		{
			Timer1.Enabled = false;
			_onResizing = false;
			if (this.ControlResized != null)
			{
				this.ControlResized();
			}
		}
		else if (Math.Abs(_hRem) < 20)
		{
			((Control)this).Height = ((Control)this).Height + _hRem;
			_hRem = 0;
		}
		else if (_hRem > 0)
		{
			_hRem -= 20;
			((Control)this).Height = ((Control)this).Height + 20;
		}
		else
		{
			_hRem += 20;
			((Control)this).Height = ((Control)this).Height - 20;
		}
	}

	private void panelMain1_Resize(object sender, EventArgs e)
	{
		if (!_onResizing)
		{
			_initPanelH = 1 + ((Control)this).Height - ((Control)PanelTop_Header).Height;
		}
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		components = new Container();
		Timer1 = new Timer(components);
		panelKol = new Panel();
		panelMain = new Panel();
		PanelTop_Header = new Panel();
		LLTitle = new LinkLabel();
		PIC = new PictureBox();
		LLTitle2 = new Label();
		MainPanel = new PanelEx();
		((Control)panelKol).SuspendLayout();
		((Control)PanelTop_Header).SuspendLayout();
		((ISupportInitialize)PIC).BeginInit();
		((Control)this).SuspendLayout();
		Timer1.Interval = 50;
		Timer1.Tick += Timer1_Tick;
		((Control)panelKol).Anchor = (AnchorStyles)15;
		((Control)panelKol).BackColor = SystemColors.Control;
		panelKol.BorderStyle = (BorderStyle)1;
		((Control)panelKol).Controls.Add((Control)(object)panelMain);
		((Control)panelKol).Controls.Add((Control)(object)PanelTop_Header);
		((Control)panelKol).Location = new Point(0, 0);
		((Control)panelKol).Name = "panelKol";
		((Control)panelKol).Size = new Size(600, 400);
		((Control)panelKol).TabIndex = 6;
		((Control)panelMain).Anchor = (AnchorStyles)15;
		((Control)panelMain).Location = new Point(-1, 29);
		((Control)panelMain).Name = "panelMain";
		((Control)panelMain).Size = new Size(600, 370);
		((Control)panelMain).TabIndex = 5;
		((Control)PanelTop_Header).Anchor = (AnchorStyles)13;
		((Control)PanelTop_Header).BackColor = SystemColors.Highlight;
		((Control)PanelTop_Header).BackgroundImage = (Image)(object)Resources.a;
		((Control)PanelTop_Header).Controls.Add((Control)(object)LLTitle);
		((Control)PanelTop_Header).Controls.Add((Control)(object)PIC);
		((Control)PanelTop_Header).Controls.Add((Control)(object)LLTitle2);
		((Control)PanelTop_Header).Location = new Point(-1, -1);
		((Control)PanelTop_Header).Name = "PanelTop_Header";
		((Control)PanelTop_Header).Size = new Size(600, 31);
		((Control)PanelTop_Header).TabIndex = 4;
		((Control)LLTitle).Anchor = (AnchorStyles)13;
		((Control)LLTitle).BackColor = Color.Transparent;
		((Control)LLTitle).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		LLTitle.LinkBehavior = (LinkBehavior)3;
		LLTitle.LinkColor = Color.FromArgb(64, 64, 64);
		((Control)LLTitle).Location = new Point(465, 1);
		((Control)LLTitle).Name = "LLTitle";
		((Control)LLTitle).Size = new Size(128, 26);
		((Control)LLTitle).TabIndex = 3;
		LLTitle.TabStop = true;
		((Control)LLTitle).Text = "[تاریخ]";
		((Label)LLTitle).TextAlign = (ContentAlignment)16;
		LLTitle.LinkClicked += new LinkLabelLinkClickedEventHandler(LLTitle_LinkClicked);
		((Control)PIC).BackColor = Color.Transparent;
		((Control)PIC).Location = new Point(-1, 0);
		((Control)PIC).Name = "PIC";
		((Control)PIC).Size = new Size(37, 29);
		PIC.SizeMode = (PictureBoxSizeMode)1;
		PIC.TabIndex = 1;
		PIC.TabStop = false;
		((Control)PIC).Click += PIC_Click;
		((Control)LLTitle2).BackColor = Color.Transparent;
		((Control)LLTitle2).Font = new Font("Tahoma", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)LLTitle2).Location = new Point(95, 4);
		((Control)LLTitle2).Name = "LLTitle2";
		((Control)LLTitle2).Size = new Size(341, 23);
		((Control)LLTitle2).TabIndex = 4;
		((Control)LLTitle2).Text = "[غیرفعال]";
		LLTitle2.TextAlign = (ContentAlignment)32;
		((Control)MainPanel).Anchor = (AnchorStyles)15;
		MainPanel.CanvasColor = SystemColors.Control;
		MainPanel.ColorSchemeStyle = (eDotNetBarStyle)9;
		MainPanel.DisabledBackColor = Color.Empty;
		((Control)MainPanel).Location = new Point(111, 111);
		((Control)MainPanel).Name = "MainPanel";
		((Control)MainPanel).Size = new Size(537, 255);
		MainPanel.Style.Alignment = (StringAlignment)1;
		MainPanel.Style.BackColor1.ColorSchemePart = (eColorSchemePart)51;
		MainPanel.Style.BackColor2.ColorSchemePart = (eColorSchemePart)52;
		MainPanel.Style.Border = (eBorderType)1;
		MainPanel.Style.BorderColor.ColorSchemePart = (eColorSchemePart)53;
		MainPanel.Style.ForeColor.ColorSchemePart = (eColorSchemePart)54;
		MainPanel.Style.GradientAngle = 90;
		((Control)MainPanel).TabIndex = 0;
		((Control)MainPanel).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)panelKol);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)this).MaximumSize = new Size(600, 400);
		((Control)this).MinimumSize = new Size(600, 10);
		((Control)this).Name = "UCPanel";
		((Control)this).Size = new Size(600, 400);
		((Control)panelKol).ResumeLayout(false);
		((Control)PanelTop_Header).ResumeLayout(false);
		((ISupportInitialize)PIC).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
