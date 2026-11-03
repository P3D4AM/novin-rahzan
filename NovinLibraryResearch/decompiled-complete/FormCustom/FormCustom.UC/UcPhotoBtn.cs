using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormCustom.UC;

public class UcPhotoBtn : UserControl
{
	private IContainer components;

	private Panel panel1;

	private Label labelbtn;

	private PictureBox picturebtn;

	public string DisplayText
	{
		get
		{
			return ((Control)labelbtn).Text;
		}
		set
		{
			((Control)labelbtn).Text = value;
		}
	}

	public Font DisplayFont
	{
		get
		{
			return ((Control)labelbtn).Font;
		}
		set
		{
			((Control)labelbtn).Font = value;
		}
	}

	public Color FontForeColor
	{
		get
		{
			return ((Control)labelbtn).ForeColor;
		}
		set
		{
			((Control)labelbtn).ForeColor = value;
		}
	}

	public Image Image
	{
		get
		{
			return picturebtn.Image;
		}
		set
		{
			picturebtn.Image = value;
		}
	}

	public Color ColorPic
	{
		get
		{
			return ((Control)picturebtn).BackColor;
		}
		set
		{
			((Control)picturebtn).BackColor = value;
		}
	}

	public event Action<object, EventArgs> OnClick;

	public UcPhotoBtn()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void UcPhotoBtn_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(this, EventArgs.Empty);
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
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		panel1 = new Panel();
		labelbtn = new Label();
		picturebtn = new PictureBox();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)picturebtn).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panel1).BackColor = Color.FromArgb(210, 221, 251);
		((Control)panel1).Controls.Add((Control)(object)labelbtn);
		((Control)panel1).Controls.Add((Control)(object)picturebtn);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Margin = new Padding(0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(75, 90);
		((Control)panel1).TabIndex = 0;
		((Control)labelbtn).BackColor = Color.FromArgb(255, 250, 249);
		((Control)labelbtn).Dock = (DockStyle)2;
		((Control)labelbtn).ForeColor = Color.DimGray;
		((Control)labelbtn).Location = new Point(0, 70);
		((Control)labelbtn).Margin = new Padding(0);
		((Control)labelbtn).Name = "labelbtn";
		((Control)labelbtn).Size = new Size(75, 20);
		((Control)labelbtn).TabIndex = 1;
		((Control)labelbtn).Text = "label1";
		labelbtn.TextAlign = (ContentAlignment)32;
		((Control)labelbtn).Click += UcPhotoBtn_Click;
		((Control)picturebtn).Anchor = (AnchorStyles)15;
		((Control)picturebtn).Location = new Point(0, 0);
		((Control)picturebtn).Margin = new Padding(0);
		((Control)picturebtn).Name = "picturebtn";
		((Control)picturebtn).Size = new Size(75, 67);
		picturebtn.SizeMode = (PictureBoxSizeMode)1;
		picturebtn.TabIndex = 0;
		picturebtn.TabStop = false;
		((Control)picturebtn).Click += UcPhotoBtn_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Margin = new Padding(0);
		((Control)this).Name = "UcPhotoBtn";
		((Control)this).Size = new Size(75, 90);
		((Control)this).Click += UcPhotoBtn_Click;
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)picturebtn).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
