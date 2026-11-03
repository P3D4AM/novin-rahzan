using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcCascadingMenu : UserControl
{
	private bool _isExpand;

	private bool _isMaster = true;

	private bool _isCollaps = true;

	private IContainer components;

	private PanelEx panel1;

	private Label label1;

	private PictureBox pictureBoxDropDone;

	private PictureBox pictureBoxIcon;

	private Line button1;

	public string DisplayText
	{
		get
		{
			return ((Control)label1).Text;
		}
		set
		{
			((Control)label1).Text = value;
		}
	}

	public Color DisplayColor
	{
		get
		{
			return panel1.Style.BackColor1.Color;
		}
		set
		{
			try
			{
				panel1.Style.BackColor1.Color = value;
				panel1.Style.BackColor2.Color = value;
				((Control)label1).BackColor = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DisplayColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 27);
			}
		}
	}

	public Font DisplayFont
	{
		get
		{
			return ((Control)label1).Font;
		}
		set
		{
			((Control)label1).Font = value;
		}
	}

	public Color FontForeColor
	{
		get
		{
			return ((Control)label1).ForeColor;
		}
		set
		{
			((Control)label1).ForeColor = value;
		}
	}

	public Image ImageIcon { get; set; }

	public Image ImageIcon1 { get; set; }

	public Image ImageDropDone
	{
		get
		{
			return pictureBoxDropDone.Image;
		}
		set
		{
			pictureBoxDropDone.Image = value;
		}
	}

	public bool IsMaster
	{
		get
		{
			return _isMaster;
		}
		set
		{
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			try
			{
				_isMaster = value;
				if (_isMaster)
				{
					panel1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
					panel1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
					((Control)label1).ForeColor = Color.DimGray;
					((Control)pictureBoxIcon).Visible = true;
					((Control)pictureBoxDropDone).Visible = true;
					((Control)button1).Visible = false;
					pictureBoxIcon.Image = ImageIcon;
				}
				else
				{
					panel1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
					panel1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
					((Control)label1).ForeColor = Color.DimGray;
					((Control)label1).Font = new Font("PeydaWeb", 12.749999f, (FontStyle)1, (GraphicsUnit)3);
					((Control)pictureBoxIcon).Visible = false;
					((Control)pictureBoxDropDone).Visible = false;
					((Control)button1).Visible = false;
					pictureBoxIcon.Image = ImageIcon;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "IsMaster", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 65);
			}
		}
	}

	public event Action<object, EventArgs> OnClick;

	public void Expand()
	{
		try
		{
			if (!_isExpand)
			{
				pictureBoxDropDone.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_2;
				panel1.Style.BackColor1.Color = Color.FromArgb(65, 81, 220);
				panel1.Style.BackColor2.Color = Color.FromArgb(65, 81, 220);
				((Control)label1).BackColor = Color.FromArgb(65, 81, 220);
				((Control)label1).ForeColor = Color.White;
				((Control)button1).Visible = true;
				pictureBoxIcon.Image = ImageIcon1;
				_isExpand = true;
			}
			else
			{
				pictureBoxDropDone.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_1;
				panel1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
				panel1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
				((Control)label1).BackColor = Color.FromArgb(210, 221, 251);
				((Control)label1).ForeColor = Color.DimGray;
				((Control)button1).Visible = false;
				pictureBoxIcon.Image = ImageIcon;
				_isExpand = false;
			}
			this.OnClick?.Invoke(this, EventArgs.Empty);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Expand", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 97);
		}
	}

	private void MouseEnter()
	{
		try
		{
			if (!_isMaster)
			{
				((Control)label1).ForeColor = Color.Red;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 106);
		}
	}

	public UcCascadingMenu()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void label1_Click(object sender, EventArgs e)
	{
		Expand();
	}

	private void label1_MouseClick(object sender, MouseEventArgs e)
	{
		try
		{
			if (pictureBoxIcon.Image == null)
			{
				panel1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
				panel1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
				((Control)label1).BackColor = Color.FromArgb(210, 221, 251);
				((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
				((Control)button1).Visible = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "label1_MouseClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 129);
		}
	}

	private void label1_MouseEnter(object sender, EventArgs e)
	{
		MouseEnter();
	}

	private void label1_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			if (!_isMaster)
			{
				((Control)label1).ForeColor = Color.DimGray;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "label1_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 139);
		}
	}

	private void pictureBoxIcon_MouseEnter(object sender, EventArgs e)
	{
		MouseEnter();
	}

	private void pictureBoxIcon_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			if (!_isMaster)
			{
				((Control)label1).ForeColor = Color.DimGray;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBoxIcon_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 149);
		}
	}

	private void pictureBoxDropDone_MouseEnter(object sender, EventArgs e)
	{
		MouseEnter();
	}

	private void pictureBoxDropDone_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			if (!_isMaster)
			{
				((Control)label1).ForeColor = Color.DimGray;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBoxDropDone_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 159);
		}
	}

	private void pictureBoxIcon_MouseClick(object sender, MouseEventArgs e)
	{
		try
		{
			if (pictureBoxIcon.Image == null)
			{
				panel1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
				panel1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
				((Control)label1).BackColor = Color.FromArgb(210, 221, 251);
				((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
				((Control)button1).Visible = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBoxIcon_MouseClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 172);
		}
	}

	private void pictureBoxDropDone_MouseClick(object sender, MouseEventArgs e)
	{
		try
		{
			if (pictureBoxIcon.Image == null)
			{
				panel1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
				panel1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
				((Control)label1).BackColor = Color.FromArgb(210, 221, 251);
				((Control)label1).ForeColor = Color.FromArgb(65, 81, 220);
				((Control)button1).Visible = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBoxDropDone_MouseClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcCascadingMenu.cs", 185);
		}
	}

	private void InitializeComponent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		panel1 = new PanelEx();
		button1 = new Line();
		pictureBoxDropDone = new PictureBox();
		pictureBoxIcon = new PictureBox();
		label1 = new Label();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)pictureBoxDropDone).BeginInit();
		((ISupportInitialize)pictureBoxIcon).BeginInit();
		((Control)this).SuspendLayout();
		panel1.CanvasColor = Color.Empty;
		((Control)panel1).Controls.Add((Control)(object)button1);
		((Control)panel1).Controls.Add((Control)(object)pictureBoxDropDone);
		((Control)panel1).Controls.Add((Control)(object)pictureBoxIcon);
		((Control)panel1).Controls.Add((Control)(object)label1);
		panel1.DisabledBackColor = Color.Empty;
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Font = new Font("PeydaWeb", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Margin = new Padding(0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(149, 46);
		panel1.Style.BackColor1.Color = Color.FromArgb(210, 221, 251);
		panel1.Style.BackColor2.Color = Color.FromArgb(210, 221, 251);
		panel1.Style.CornerDiameter = 13;
		((Control)panel1).TabIndex = 0;
		((Control)button1).Anchor = (AnchorStyles)11;
		((Control)button1).ForeColor = Color.White;
		button1.LineAlignment = eItemAlignment.Near;
		((Control)button1).Location = new Point(140, 13);
		((Control)button1).Margin = new Padding(0);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(7, 19);
		((Control)button1).TabIndex = 3;
		((Control)button1).Text = "button1";
		button1.Thickness = 5;
		button1.VerticalLine = true;
		((Control)pictureBoxDropDone).BackgroundImageLayout = (ImageLayout)0;
		pictureBoxDropDone.Image = (Image)(object)FormCustom.Properties.Resources.Untitled_1;
		((Control)pictureBoxDropDone).Location = new Point(7, 15);
		((Control)pictureBoxDropDone).Margin = new Padding(15);
		((Control)pictureBoxDropDone).Name = "pictureBoxDropDone";
		((Control)pictureBoxDropDone).Size = new Size(17, 17);
		pictureBoxDropDone.SizeMode = (PictureBoxSizeMode)1;
		pictureBoxDropDone.TabIndex = 2;
		pictureBoxDropDone.TabStop = false;
		((Control)pictureBoxDropDone).Click += label1_Click;
		((Control)pictureBoxDropDone).MouseClick += new MouseEventHandler(pictureBoxDropDone_MouseClick);
		((Control)pictureBoxDropDone).MouseEnter += pictureBoxDropDone_MouseEnter;
		((Control)pictureBoxDropDone).MouseLeave += pictureBoxDropDone_MouseLeave;
		((Control)pictureBoxIcon).Anchor = (AnchorStyles)11;
		((Control)pictureBoxIcon).BackgroundImageLayout = (ImageLayout)0;
		((Control)pictureBoxIcon).Location = new Point(105, 7);
		((Control)pictureBoxIcon).Margin = new Padding(0);
		((Control)pictureBoxIcon).Name = "pictureBoxIcon";
		((Control)pictureBoxIcon).Size = new Size(35, 31);
		pictureBoxIcon.SizeMode = (PictureBoxSizeMode)1;
		pictureBoxIcon.TabIndex = 1;
		pictureBoxIcon.TabStop = false;
		((Control)pictureBoxIcon).Click += label1_Click;
		((Control)pictureBoxIcon).MouseClick += new MouseEventHandler(pictureBoxIcon_MouseClick);
		((Control)pictureBoxIcon).MouseEnter += pictureBoxIcon_MouseEnter;
		((Control)pictureBoxIcon).MouseLeave += pictureBoxIcon_MouseLeave;
		((Control)label1).Anchor = (AnchorStyles)15;
		((Control)label1).BackColor = Color.FromArgb(210, 221, 251);
		((Control)label1).Cursor = Cursors.Hand;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 15.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(32, 0);
		((Control)label1).Margin = new Padding(0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(65, 43);
		((Control)label1).TabIndex = 0;
		label1.TextAlign = (ContentAlignment)32;
		((Control)label1).Click += label1_Click;
		((Control)label1).MouseClick += new MouseEventHandler(label1_MouseClick);
		((Control)label1).MouseEnter += label1_MouseEnter;
		((Control)label1).MouseLeave += label1_MouseLeave;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Margin = new Padding(0);
		((Control)this).Name = "UcCascadingMenu";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(149, 46);
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)pictureBoxDropDone).EndInit();
		((ISupportInitialize)pictureBoxIcon).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
