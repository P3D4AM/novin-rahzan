using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base;

public class StiExceptionPreviewForm : Form
{
	[ToolboxItem(false)]
	public class ExceptionButton : Button
	{
		private bool isMouseOver;

		private bool isPressed;

		public bool IsContinueButton { get; set; }

		protected override void OnMouseEnter(EventArgs e)
		{
			((Button)this).OnMouseEnter(e);
			isMouseOver = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Button)this).OnMouseLeave(e);
			isMouseOver = false;
			((Control)this).Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			((ButtonBase)this).OnMouseDown(mevent);
			isPressed = true;
			((Control)this).Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			((Button)this).OnMouseUp(mevent);
			isPressed = false;
			((Control)this).Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Expected O, but got Unknown
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Expected O, but got Unknown
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Expected O, but got Unknown
			((ButtonBase)this).OnPaint(pevent);
			Graphics graphics = pevent.Graphics;
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - StiScale.I(1), ((Control)this).Height - StiScale.I(1));
			if (isPressed)
			{
				SolidBrush val = new SolidBrush(Color.FromArgb(163, 189, 227));
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				Pen val2 = new Pen(Color.FromArgb(63, 110, 181));
				try
				{
					graphics.DrawRectangle(val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else if (isMouseOver)
			{
				SolidBrush val3 = new SolidBrush(Color.FromArgb(213, 225, 242));
				try
				{
					graphics.FillRectangle((Brush)(object)val3, rectangle);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Pen val4 = new Pen(Color.FromArgb(163, 189, 227));
				try
				{
					graphics.DrawRectangle(val4, rectangle);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			else if (IsContinueButton)
			{
				SolidBrush val5 = new SolidBrush(Color.FromArgb(0, 121, 219));
				try
				{
					graphics.FillRectangle((Brush)(object)val5, rectangle);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				Pen val6 = new Pen(Color.FromArgb(43, 87, 154));
				try
				{
					graphics.DrawRectangle(val6, rectangle);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			else
			{
				graphics.FillRectangle(Brushes.White, rectangle);
				Pen val7 = new Pen(Color.FromArgb(163, 189, 227));
				try
				{
					graphics.DrawRectangle(val7, rectangle);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			SolidBrush val8 = new SolidBrush(((Control)this).ForeColor);
			try
			{
				StringFormat val9 = new StringFormat();
				try
				{
					val9.LineAlignment = (StringAlignment)1;
					val9.Alignment = (StringAlignment)1;
					graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)(object)val8, new RectangleF(new PointF(0f, 0f), new SizeF(((Control)this).Width, ((Control)this).Height)), val9);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
	}

	private Exception ex;

	private IContainer components;

	private Panel panel1;

	private PictureBox imageMessage;

	private Panel panel2;

	private Label textBlockTitle;

	private Panel panel3;

	private ExceptionButton buttonReport;

	private ExceptionButton buttonContinue;

	public StiExceptionPreviewForm(Exception ex)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		((Form)this).ShowIcon = false;
		this.ex = ex;
		if (StiScale.Id == SystemScaleID.x2)
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiExceptionPreviewForm));
			imageMessage.Image = (Image)componentResourceManager.GetObject("imageMessage_x2.Image");
		}
		Localize();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("ExceptionProvider", "ExceptionReport");
		((Control)textBlockTitle).Text = ex?.Message;
		((Control)buttonContinue).Text = Loc.Get("DesignerFx", "Continue");
		((Control)buttonReport).Text = Loc.Get("Components", "StiReport");
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void ButtonReport_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void ButtonContinue_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiExceptionPreviewForm));
		panel1 = new Panel();
		imageMessage = new PictureBox();
		panel2 = new Panel();
		textBlockTitle = new Label();
		panel3 = new Panel();
		buttonContinue = new ExceptionButton();
		buttonReport = new ExceptionButton();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)imageMessage).BeginInit();
		((Control)panel2).SuspendLayout();
		((Control)panel3).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).Controls.Add((Control)(object)imageMessage);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(687, 190);
		((Control)panel1).TabIndex = 0;
		imageMessage.Image = (Image)componentResourceManager.GetObject("imageMessage.Image");
		((Control)imageMessage).Location = new Point(244, 50);
		((Control)imageMessage).Name = "imageMessage";
		((Control)imageMessage).Size = new Size(112, 112);
		imageMessage.SizeMode = (PictureBoxSizeMode)1;
		imageMessage.TabIndex = 0;
		imageMessage.TabStop = false;
		((Control)panel2).Controls.Add((Control)(object)textBlockTitle);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(0, 200);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(687, 80);
		((Control)panel2).TabIndex = 1;
		((Control)textBlockTitle).Dock = (DockStyle)5;
		((Control)textBlockTitle).Font = new Font("Arial", 13.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)textBlockTitle).ForeColor = Color.FromArgb(3, 109, 185);
		((Control)textBlockTitle).Location = new Point(0, 0);
		((Control)textBlockTitle).Name = "textBlockTitle";
		((Control)textBlockTitle).Size = new Size(687, 35);
		((Control)textBlockTitle).TabIndex = 0;
		((Control)textBlockTitle).Text = "message";
		textBlockTitle.TextAlign = (ContentAlignment)2;
		((Control)textBlockTitle).Padding = new Padding(6, 0, 6, 0);
		((Control)panel3).Controls.Add((Control)(object)buttonContinue);
		((Control)panel3).Controls.Add((Control)(object)buttonReport);
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(0, 235);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(687, 65);
		((Control)panel3).TabIndex = 2;
		((Control)buttonContinue).Font = new Font("Microsoft Sans Serif", 10.125f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)buttonContinue).ForeColor = Color.White;
		buttonContinue.IsContinueButton = true;
		((Control)buttonContinue).Location = new Point(306, 15);
		((Control)buttonContinue).Name = "buttonContinue";
		((Control)buttonContinue).Size = new Size(140, 30);
		((Control)buttonContinue).TabIndex = 1;
		((Control)buttonContinue).Text = "Continue";
		((ButtonBase)buttonContinue).UseVisualStyleBackColor = true;
		((Control)buttonContinue).Click += ButtonContinue_Click;
		((Control)buttonReport).Font = new Font("Microsoft Sans Serif", 10.125f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		buttonReport.IsContinueButton = false;
		((Control)buttonReport).Location = new Point(154, 15);
		((Control)buttonReport).Name = "buttonReport";
		((Control)buttonReport).Size = new Size(140, 30);
		((Control)buttonReport).TabIndex = 0;
		((Control)buttonReport).Text = "Report";
		((ButtonBase)buttonReport).UseVisualStyleBackColor = true;
		((Control)buttonReport).Click += ButtonReport_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonContinue;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(600, 370);
		((Control)this).Controls.Add((Control)(object)panel3);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).KeyPreview = true;
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiExceptionPreviewForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Exception Report";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)imageMessage).EndInit();
		((Control)panel2).ResumeLayout(false);
		((Control)panel3).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
