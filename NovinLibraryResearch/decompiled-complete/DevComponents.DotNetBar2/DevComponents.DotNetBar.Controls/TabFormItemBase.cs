using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.TabFormItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class TabFormItemBase : ButtonItem
{
	private bool _ᎈ = true;

	private GraphicsPath _ᎉ;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool ᎇ
	{
		get
		{
			return _ᎈ;
		}
		set
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			_ᎈ = value;
			if (ContainerControl is Control)
			{
				((Control)ContainerControl).Invalidate();
			}
			else
			{
				Refresh();
			}
		}
	}

	[Browsable(false)]
	public GraphicsPath TabPath
	{
		get
		{
			return _ᎉ;
		}
		internal set
		{
			if (_ᎉ != null)
			{
				_ᎉ.Dispose();
			}
			_ᎉ = value;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (_ᎉ != null)
		{
			_ᎉ.Dispose();
			_ᎉ = null;
		}
		base.Dispose(disposing);
	}
}
