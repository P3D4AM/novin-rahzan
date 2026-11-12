using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(CircularProgress), "Controls.CircularProgress.ico")]
[ToolboxItem(false)]
[DefaultEvent("ValueChanged")]
public class CircularProgress : BaseItemControl
{
	private CircularProgressItem _Item;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Image BackgroundImageExt { get; set; }

	[DefaultValue(eCircularProgressType.Line)]
	[Category("Appearance")]
	[Description("Indicates circular progress bar type.")]
	public eCircularProgressType ProgressBarType
	{
		get
		{
			return _Item.ProgressBarType;
		}
		set
		{
			_Item.ProgressBarType = value;
		}
	}

	[Description("Indicates maximum value of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int Maximum
	{
		get
		{
			return _Item.Maximum;
		}
		set
		{
			_Item.Maximum = value;
		}
	}

	[Description("Indicates minimum value of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return _Item.Minimum;
		}
		set
		{
			_Item.Minimum = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of progress percentage text")]
	public Color ProgressTextColor
	{
		get
		{
			return _Item.ProgressTextColor;
		}
		set
		{
			_Item.ProgressTextColor = value;
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether text that displays the progress bar completion percentage text is visible")]
	public bool ProgressTextVisible
	{
		get
		{
			return _Item.ProgressTextVisible;
		}
		set
		{
			_Item.ProgressTextVisible = value;
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates text displayed on top of the circular progress bar.")]
	public string ProgressText
	{
		get
		{
			return _Item.ProgressText;
		}
		set
		{
			_Item.ProgressText = value;
		}
	}

	[Description("Indicates current value of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Value
	{
		get
		{
			return _Item.Value;
		}
		set
		{
			_Item.Value = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IsRunning
	{
		get
		{
			return _Item.IsRunning;
		}
		set
		{
			_Item.IsRunning = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of progress indicator.")]
	public Color ProgressColor
	{
		get
		{
			return _Item.ProgressColor;
		}
		set
		{
			_Item.ProgressColor = value;
		}
	}

	[Category("Pie")]
	[Description("Indicates color of pie progress bar dark border.")]
	public Color PieBorderDark
	{
		get
		{
			return _Item.PieBorderDark;
		}
		set
		{
			_Item.PieBorderDark = value;
		}
	}

	[Category("Pie")]
	[Description("Indicates color of pie progress bar light border. ")]
	public Color PieBorderLight
	{
		get
		{
			return _Item.PieBorderLight;
		}
		set
		{
			_Item.PieBorderLight = value;
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar dark border.")]
	public Color SpokeBorderDark
	{
		get
		{
			return _Item.SpokeBorderDark;
		}
		set
		{
			_Item.SpokeBorderDark = value;
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar light border..")]
	public Color SpokeBorderLight
	{
		get
		{
			return _Item.SpokeBorderLight;
		}
		set
		{
			_Item.SpokeBorderLight = value;
		}
	}

	[DefaultValue("{0}%")]
	[Category("Appearance")]
	[Description("Indicates format string for progress value.")]
	public string ProgressTextFormat
	{
		get
		{
			return _Item.ProgressTextFormat;
		}
		set
		{
			_Item.ProgressTextFormat = value;
		}
	}

	[DefaultValue(100)]
	[Description("Indicates the animation speed for endless running progress. Lower number means faster running.")]
	[Category("Behavior")]
	public int AnimationSpeed
	{
		get
		{
			return _Item.AnimationSpeed;
		}
		set
		{
			_Item.AnimationSpeed = value;
		}
	}

	[Browsable(false)]
	public override eDotNetBarStyle Style
	{
		get
		{
			return base.Style;
		}
		set
		{
			base.Style = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).RightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RightToLeft = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).BackgroundImageLayout = value;
		}
	}

	public event EventHandler ValueChanged;

	public CircularProgress()
	{
		_Item = new CircularProgressItem();
		_Item.ValueChanged += ItemValueChanged;
		_Item.TextVisible = false;
		HostItem = _Item;
	}

	protected override void Dispose(bool disposing)
	{
		_Item.Dispose();
		((Control)this).Dispose(disposing);
	}

	protected override void RecalcSize()
	{
		_Item.Diameter = Math.Min(((Control)this).Width, ((Control)this).Height);
		base.RecalcSize();
	}

	private void ItemValueChanged(object sender, EventArgs e)
	{
		OnValueChanged(e);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		this.ValueChanged?.Invoke(this, e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressTextColor()
	{
		return _Item.ShouldSerializeProgressTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressTextColor()
	{
		_Item.ResetProgressTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressColor()
	{
		return _Item.ShouldSerializeProgressColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressColor()
	{
		_Item.ResetProgressColor();
	}

	protected override void InternalPaintBackground(PaintEventArgs e)
	{
		if (BackgroundImageExt != null)
		{
			e.Graphics.DrawImage(BackgroundImageExt, 0, 0, ((Control)this).Width, ((Control)this).Height);
		}
		else
		{
			base.InternalPaintBackground(e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderDark()
	{
		return _Item.ShouldSerializePieBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderDark()
	{
		_Item.ResetPieBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderLight()
	{
		return _Item.ShouldSerializePieBorderLight();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderLight()
	{
		_Item.ResetPieBorderLight();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderDark()
	{
		return _Item.ShouldSerializeSpokeBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderDark()
	{
		_Item.ResetSpokeBorderDark();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderLight()
	{
		return _Item.ShouldSerializeSpokeBorderLight();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderLight()
	{
		_Item.ResetSpokeBorderLight();
	}
}
