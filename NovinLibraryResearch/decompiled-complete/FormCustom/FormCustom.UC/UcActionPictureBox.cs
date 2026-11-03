using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcActionPictureBox : UserControl
{
	public delegate void OnClickEventHandler(object sender, EventArgs e);

	private Image _enableImage;

	private Image _disableImage;

	private Image _hoverImage;

	private IContainer components;

	private ToolTip toolTip1;

	private SplitContainer splitContainer1;

	private PictureBox pictureBox1;

	private Label lblTitle;

	public Color EnableColor { get; set; }

	public Color DisableColor { get; set; }

	public Color HoverColor { get; set; }

	public bool Enabled
	{
		get
		{
			return ((Control)pictureBox1).Enabled;
		}
		set
		{
			((Control)this).Enabled = value;
			((Control)pictureBox1).Enabled = value;
			((Control)lblTitle).Enabled = value;
			if (!((Control)pictureBox1).Enabled)
			{
				pictureBox1.Image = _disableImage;
				((Control)lblTitle).ForeColor = DisableColor;
			}
			else
			{
				pictureBox1.Image = _enableImage;
				((Control)lblTitle).ForeColor = EnableColor;
			}
		}
	}

	public Image EnableImage
	{
		get
		{
			return _enableImage;
		}
		set
		{
			_enableImage = value;
			pictureBox1.Image = _enableImage;
		}
	}

	public Image DisableImage
	{
		get
		{
			return _disableImage;
		}
		set
		{
			_disableImage = value;
			pictureBox1.Image = _disableImage;
		}
	}

	public Image HoverImage
	{
		get
		{
			return _hoverImage;
		}
		set
		{
			_hoverImage = value;
			pictureBox1.Image = _hoverImage;
		}
	}

	public event OnClickEventHandler OnClick;

	public void Init(string toolTipMessage, Font font = null)
	{
		try
		{
			if (((Control)this).InvokeRequired)
			{
				((Control)((Control)this).FindForm()).Invoke((Delegate)(Action)delegate
				{
					Init(toolTipMessage);
				});
				return;
			}
			toolTip1.SetToolTip((Control)(object)pictureBox1, toolTipMessage);
			((Control)lblTitle).Text = toolTipMessage;
			if (font != null)
			{
				((Control)lblTitle).Font = font;
			}
			if (!Enabled)
			{
				pictureBox1.Image = _disableImage;
				((Control)lblTitle).ForeColor = DisableColor;
			}
			else
			{
				pictureBox1.Image = _enableImage;
				((Control)lblTitle).ForeColor = EnableColor;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcActionPictureBox.cs", 90);
		}
	}

	public void PerformClick()
	{
		try
		{
			if (Enabled)
			{
				this.OnClick?.Invoke(this, new EventArgs());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PerformClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcActionPictureBox.cs", 99);
		}
	}

	public UcActionPictureBox()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		try
		{
			if (Enabled)
			{
				this.OnClick?.Invoke(this, new EventArgs());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBox1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcActionPictureBox.cs", 115);
		}
	}

	private void pictureBox1_MouseEnter(object sender, EventArgs e)
	{
		try
		{
			if (Enabled)
			{
				pictureBox1.Image = _hoverImage;
				((Control)lblTitle).ForeColor = HoverColor;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBox1_MouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcActionPictureBox.cs", 125);
		}
	}

	private void pictureBox1_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			if (Enabled)
			{
				pictureBox1.Image = _enableImage;
				((Control)lblTitle).ForeColor = EnableColor;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "pictureBox1_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcActionPictureBox.cs", 135);
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
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		toolTip1 = new ToolTip(components);
		splitContainer1 = new SplitContainer();
		pictureBox1 = new PictureBox();
		lblTitle = new Label();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(0, 0);
		((Control)splitContainer1).Name = "splitContainer1";
		splitContainer1.Orientation = (Orientation)0;
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)pictureBox1);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)lblTitle);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(63, 77);
		splitContainer1.SplitterDistance = 29;
		((Control)splitContainer1).TabIndex = 20;
		((Control)pictureBox1).Cursor = Cursors.Hand;
		((Control)pictureBox1).Dock = (DockStyle)5;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(63, 29);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += pictureBox1_Click;
		((Control)pictureBox1).MouseEnter += pictureBox1_MouseEnter;
		((Control)pictureBox1).MouseLeave += pictureBox1_MouseLeave;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Dock = (DockStyle)5;
		((Control)lblTitle).Font = new Font("PeydaWeb Black", 8f, (FontStyle)1);
		((Control)lblTitle).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)lblTitle).Location = new Point(0, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(63, 44);
		((Control)lblTitle).TabIndex = 3;
		lblTitle.TextAlign = (ContentAlignment)32;
		((Control)lblTitle).Click += pictureBox1_Click;
		((Control)lblTitle).MouseEnter += pictureBox1_MouseEnter;
		((Control)lblTitle).MouseLeave += pictureBox1_MouseLeave;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 21f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Font = new Font("Anjoman Regular", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcActionPictureBox";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(63, 77);
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
