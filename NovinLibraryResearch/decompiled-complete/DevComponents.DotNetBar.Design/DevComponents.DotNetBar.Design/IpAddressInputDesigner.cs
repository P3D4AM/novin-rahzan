using System.Collections;
using System.Windows.Forms.Design;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design;

public class IpAddressInputDesigner : VisualControlBaseDesigner
{
	public override SelectionRules SelectionRules
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			SelectionRules val = ((ControlDesigner)this).SelectionRules;
			if (!((ControlDesigner)this).Control.AutoSize)
			{
				val = (SelectionRules)(val & -4);
			}
			return val;
		}
	}

	public IpAddressInputDesigner()
	{
		((ControlDesigner)this).AutoResizeHandles = true;
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is IpAddressInput ipAddressInput)
		{
			ipAddressInput.AutoOverwrite = true;
			ipAddressInput.ButtonFreeText.Visible = true;
			ipAddressInput.ButtonFreeText.Shortcut = eShortcut.F2;
			ipAddressInput.Style = eDotNetBarStyle.StyleManagerControlled;
		}
		base.InitializeNewComponent(defaultValues);
	}
}
