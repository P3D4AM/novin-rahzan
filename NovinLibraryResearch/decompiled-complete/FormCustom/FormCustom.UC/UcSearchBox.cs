using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.Properties;
using PacketParser;

namespace FormCustom.UC;

public class UcSearchBox : UserControl
{
	public delegate void EventHandler(object sender, EventArgs e);

	public delegate void KeyEventHandler(object sender, KeyEventArgs e);

	private bool _isSearchBox = true;

	private IContainer components;

	private TextBoxX textBox1;

	private PictureBox pictureBox1;

	public override string Text
	{
		get
		{
			return ((Control)textBox1).Text;
		}
		set
		{
			((Control)textBox1).Text = value;
		}
	}

	public int SelectionStart
	{
		get
		{
			return ((TextBoxBase)textBox1).SelectionStart;
		}
		set
		{
			((TextBoxBase)textBox1).SelectionStart = value;
		}
	}

	public int SelectionLength
	{
		get
		{
			return ((TextBoxBase)textBox1).SelectionLength;
		}
		set
		{
			((TextBoxBase)textBox1).SelectionLength = value;
		}
	}

	public override bool Focused
	{
		get
		{
			if (!((Control)this).Focused)
			{
				return ((Control)textBox1).Focused;
			}
			return true;
		}
	}

	public bool IsSaerchBox
	{
		get
		{
			return _isSearchBox;
		}
		set
		{
			_isSearchBox = value;
			if (_isSearchBox)
			{
				((Control)pictureBox1).Visible = true;
				textBox1.WatermarkText = "[مورد جستجو را وارد نمایید] ...".GetTranslate();
			}
			else
			{
				((Control)pictureBox1).Visible = false;
				textBox1.WatermarkText = "";
			}
		}
	}

	public event EventHandler OnTextChanged;

	public event KeyEventHandler OnKeyDown;

	public UcSearchBox()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		this.OnTextChanged?.Invoke(sender, e);
	}

	private void UcSearchBox_Enter(object sender, EventArgs e)
	{
		((Control)textBox1).Focus();
	}

	private void textBox1_KeyDown(object sender, KeyEventArgs e)
	{
		this.OnKeyDown?.Invoke(sender, e);
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		textBox1 = new TextBoxX();
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)textBox1).BackColor = Color.White;
		textBox1.Border.BackColor = Color.White;
		textBox1.Border.BackColor2 = Color.White;
		textBox1.Border.BorderBottom = eStyleBorderType.Solid;
		textBox1.Border.BorderLeft = eStyleBorderType.Solid;
		textBox1.Border.BorderRight = eStyleBorderType.Solid;
		textBox1.Border.BorderTop = eStyleBorderType.Solid;
		textBox1.Border.Class = "TextBoxBorder";
		textBox1.Border.CornerDiameter = 15;
		textBox1.Border.CornerType = eCornerType.Rounded;
		textBox1.Border.PaddingLeft = 20;
		textBox1.Border.PaddingRight = 20;
		((Control)textBox1).Dock = (DockStyle)5;
		textBox1.FocusHighlightEnabled = true;
		((Control)textBox1).Font = new Font("B Titr", 8f, (FontStyle)1);
		((Control)textBox1).Location = new Point(0, 0);
		((Control)textBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)textBox1).Name = "textBox1";
		textBox1.PreventEnterBeep = true;
		((Control)textBox1).Size = new Size(380, 40);
		((Control)textBox1).TabIndex = 5;
		textBox1.WatermarkColor = Color.FromArgb(65, 81, 220);
		textBox1.WatermarkText = "[مورد جستجو را وارد نمایید]...";
		((Control)textBox1).TextChanged += textBox1_TextChanged;
		((Control)textBox1).KeyDown += new KeyEventHandler(textBox1_KeyDown);
		((Control)pictureBox1).BackColor = Color.White;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources._34324324;
		((Control)pictureBox1).Location = new Point(11, 6);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(18, 28);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 6;
		pictureBox1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcSearchBox";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(380, 40);
		((Control)this).Enter += UcSearchBox_Enter;
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
