using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.ExtendedControls;

namespace FormCustom.UC;

public class UcCheckBox : UserControl
{
	private Color _color;

	private Color _foreColor;

	private IContainer components;

	private CustomeToggleButton chb;

	private Label lblTitle;

	public string Title
	{
		get
		{
			return ((Control)lblTitle).Text;
		}
		set
		{
			((Control)lblTitle).Text = value;
		}
	}

	public bool Checked
	{
		get
		{
			return ((CheckBox)chb).Checked;
		}
		set
		{
			((CheckBox)chb).Checked = value;
		}
	}

	public Color Color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
			((Control)lblTitle).BackColor = _color;
			((Control)chb).BackColor = _color;
		}
	}

	public Font TitleFont
	{
		get
		{
			return ((Control)lblTitle).Font;
		}
		set
		{
			((Control)lblTitle).Font = value;
		}
	}

	public Color TitleColor
	{
		get
		{
			return ((Control)lblTitle).ForeColor;
		}
		set
		{
			((Control)lblTitle).ForeColor = (_foreColor = value);
		}
	}

	public event EventHandler OnCheckedChanged;

	public UcCheckBox()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void chb_CheckedChanged(object sender, EventArgs e)
	{
		this.OnCheckedChanged?.Invoke(this, e);
	}

	private void lblTitle_MouseEnter(object sender, EventArgs e)
	{
		((Control)lblTitle).ForeColor = Color.Red;
	}

	private void lblTitle_MouseLeave(object sender, EventArgs e)
	{
		((Control)lblTitle).ForeColor = _foreColor;
	}

	private void lblTitle_Click(object sender, EventArgs e)
	{
		((CheckBox)chb).Checked = !((CheckBox)chb).Checked;
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		chb = new CustomeToggleButton();
		lblTitle = new Label();
		((Control)this).SuspendLayout();
		((Control)chb).BackColor = Color.Transparent;
		((Control)chb).Cursor = Cursors.Hand;
		((Control)chb).Dock = (DockStyle)4;
		((Control)chb).Location = new Point(70, 0);
		((Control)chb).Margin = new Padding(4, 5, 4, 5);
		((Control)chb).Name = "chb";
		chb.OffBackColor = Color.Gray;
		chb.OfToggleColor = Color.DimGray;
		chb.OnBackColor = Color.FromArgb(65, 81, 220);
		chb.OnToggleColor = Color.FromArgb(228, 236, 255);
		((Control)chb).Size = new Size(52, 29);
		((Control)chb).TabIndex = 2;
		((Control)chb).Text = "chb";
		((ButtonBase)chb).UseVisualStyleBackColor = false;
		((CheckBox)chb).CheckedChanged += chb_CheckedChanged;
		((Control)lblTitle).BackColor = Color.Transparent;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Dock = (DockStyle)5;
		((Control)lblTitle).Font = new Font("PeydaWeb ExtBd", 9.749999f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)lblTitle).ForeColor = Color.FromArgb(106, 123, 132);
		((Control)lblTitle).Location = new Point(0, 0);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Padding = new Padding(0, 0, 6, 0);
		((Control)lblTitle).Size = new Size(70, 29);
		((Control)lblTitle).TabIndex = 3;
		((Control)lblTitle).Text = "---";
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)lblTitle).Click += lblTitle_Click;
		((Control)lblTitle).MouseEnter += lblTitle_MouseEnter;
		((Control)lblTitle).MouseLeave += lblTitle_MouseLeave;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)lblTitle);
		((Control)this).Controls.Add((Control)(object)chb);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcCheckBox";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(122, 29);
		((Control)this).ResumeLayout(false);
	}
}
