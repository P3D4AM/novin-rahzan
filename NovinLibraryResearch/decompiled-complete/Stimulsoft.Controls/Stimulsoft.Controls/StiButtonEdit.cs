using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiTabControl), "Toolbox.StiButtonEdit.bmp")]
public class StiButtonEdit : StiButtonEditBase
{
	private Icon buttonIcon;

	private Image image;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool FixedHeight { get; set; } = true;


	[Category("Behavior")]
	[DefaultValue(null)]
	public Icon ButtonIcon
	{
		get
		{
			return buttonIcon;
		}
		set
		{
			buttonIcon = value;
			if (value == null)
			{
				base.ButtonBitmap = null;
			}
			else
			{
				base.ButtonBitmap = (Image)(object)buttonIcon.ToBitmap();
			}
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	public Image Image
	{
		get
		{
			return image;
		}
		set
		{
			if (Image != value)
			{
				image = value;
				if (image != null && image is Bitmap)
				{
					Image obj = image;
					StiImageUtils.MakeImageBackgroundAlphaZero((Bitmap)(object)((obj is Bitmap) ? obj : null));
				}
				base.ButtonBitmap = image;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	public bool AllowEscape { get; set; }

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if ((keyData & 0xFFFF) == 27 && AllowEscape && !string.IsNullOrEmpty(((Control)this).Text))
		{
			((Control)this).Text = string.Empty;
			return true;
		}
		return ((Control)this).ProcessDialogKey(keyData);
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (FixedHeight)
		{
			height = StiScaleUI.I(21);
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}
}
