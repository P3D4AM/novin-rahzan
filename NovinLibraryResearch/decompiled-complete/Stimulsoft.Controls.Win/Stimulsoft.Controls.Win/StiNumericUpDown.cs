using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiNumericUpDown), "Toolbox.StiNumericUpDown.bmp")]
public class StiNumericUpDown : StiNumericUpDownBase, ISupportInitialize
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FixedHeight { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public decimal Value
	{
		get
		{
			return ((NumericUpDown)base.NumericBox).Value;
		}
		set
		{
			((NumericUpDown)base.NumericBox).Value = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public decimal Maximum
	{
		get
		{
			return ((NumericUpDown)base.NumericBox).Maximum;
		}
		set
		{
			((NumericUpDown)base.NumericBox).Maximum = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public decimal Minimum
	{
		get
		{
			return ((NumericUpDown)base.NumericBox).Minimum;
		}
		set
		{
			((NumericUpDown)base.NumericBox).Minimum = value;
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			((NumericUpDown)base.NumericBox).ValueChanged += value;
		}
		remove
		{
			((NumericUpDown)base.NumericBox).ValueChanged -= value;
		}
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (FixedHeight)
		{
			height = StiScale2.YYI(21.0);
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	void ISupportInitialize.BeginInit()
	{
	}

	void ISupportInitialize.EndInit()
	{
	}

	public StiNumericUpDown()
	{
		Minimum = decimal.MinValue;
		Maximum = decimal.MaxValue;
	}
}
