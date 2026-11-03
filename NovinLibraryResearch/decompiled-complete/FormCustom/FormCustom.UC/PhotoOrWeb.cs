using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EntityCache.Business.Global;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.UC;

public class PhotoOrWeb : UserControl
{
	public int radius = 30;

	private IContainer components;

	private PictureBox mainPictureBox;

	private WebBrowser mainWebBrowser;

	public PostBusiness Post { get; private set; }

	[DefaultValue(20)]
	public int Radius
	{
		get
		{
			return radius;
		}
		set
		{
			radius = value;
			RecreateRegion();
		}
	}

	public event Action<object, EventArgs> OnClick;

	public void Init(PostBusiness post, bool IsMain, bool IsClicked = false)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Post = post;
			((Control)mainWebBrowser).Visible = false;
			((Control)mainPictureBox).Visible = false;
			if (IsMain)
			{
				if (!string.IsNullOrEmpty(Post.Link))
				{
					((Control)mainWebBrowser).Visible = true;
					WebBrowser obj = mainWebBrowser;
					if (obj != null)
					{
						obj.Navigate(Post.Link);
					}
				}
				else
				{
					((Control)mainPictureBox).Visible = true;
					mainPictureBox.Image = byteArrayToImage(Post.Image);
				}
			}
			else
			{
				((Control)mainPictureBox).Visible = true;
				if (IsClicked)
				{
					mainPictureBox.Image = (Image)(object)FormCustom.Properties.Resources.smallCircleDark;
				}
				else
				{
					mainPictureBox.Image = (Image)(object)FormCustom.Properties.Resources.LightCircle;
				}
				new ToolTip().SetToolTip((Control)(object)mainPictureBox, Post.Title);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\PhotoOrWeb.cs", 50);
		}
	}

	public Image byteArrayToImage(byte[] byteArrayIn)
	{
		try
		{
			return Image.FromStream((Stream)new MemoryStream(byteArrayIn));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "byteArrayToImage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\Slider\\PhotoOrWeb.cs", 60);
		}
		return null;
	}

	public PhotoOrWeb()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(this, e);
	}

	public void ChangeBackColor()
	{
		mainPictureBox.Image = (Image)(object)FormCustom.Properties.Resources.LightCircle;
	}

	[DllImport("gdi32.dll")]
	private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

	private GraphicsPath GetRoundRectagle(Rectangle bounds, int radius)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		float num = radius;
		GraphicsPath val = new GraphicsPath();
		val.StartFigure();
		val.AddArc((float)bounds.Left, (float)bounds.Top, num, num, 180f, 90f);
		val.AddArc((float)bounds.Right - num, (float)bounds.Top, num, num, 270f, 90f);
		val.AddArc((float)bounds.Right - num, (float)bounds.Bottom - num, num, num, 0f, 90f);
		val.AddArc((float)bounds.Left, (float)bounds.Bottom - num, num, num, 90f, 90f);
		val.CloseFigure();
		return val;
	}

	private void RecreateRegion()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		((Control)this).Region = Region.FromHrgn(CreateRoundRectRgn(clientRectangle.Left, clientRectangle.Top, clientRectangle.Right, clientRectangle.Bottom, Radius, radius));
		((Control)this).Invalidate();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		RecreateRegion();
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
		mainPictureBox = new PictureBox();
		mainWebBrowser = new WebBrowser();
		((ISupportInitialize)mainPictureBox).BeginInit();
		((Control)this).SuspendLayout();
		((Control)mainPictureBox).Anchor = (AnchorStyles)15;
		((Control)mainPictureBox).Location = new Point(0, 0);
		((Control)mainPictureBox).Name = "mainPictureBox";
		((Control)mainPictureBox).Size = new Size(361, 409);
		mainPictureBox.SizeMode = (PictureBoxSizeMode)1;
		mainPictureBox.TabIndex = 0;
		mainPictureBox.TabStop = false;
		((Control)mainPictureBox).Visible = false;
		((Control)mainPictureBox).Click += pictureBox1_Click;
		((Control)mainWebBrowser).Anchor = (AnchorStyles)15;
		((Control)mainWebBrowser).Location = new Point(0, 0);
		((Control)mainWebBrowser).Name = "mainWebBrowser";
		((Control)mainWebBrowser).Size = new Size(358, 406);
		((Control)mainWebBrowser).TabIndex = 1;
		((Control)mainWebBrowser).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)mainWebBrowser);
		((Control)this).Controls.Add((Control)(object)mainPictureBox);
		((Control)this).Name = "PhotoOrWeb";
		((Control)this).Size = new Size(361, 409);
		((ISupportInitialize)mainPictureBox).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
