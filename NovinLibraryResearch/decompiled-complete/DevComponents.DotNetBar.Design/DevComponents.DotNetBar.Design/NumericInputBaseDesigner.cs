using System.Collections;
using System.Windows.Forms.Design;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design;

public class NumericInputBaseDesigner : VisualControlBaseDesigner
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

	public NumericInputBaseDesigner()
	{
		((ControlDesigner)this).AutoResizeHandles = true;
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		if (((ControlDesigner)this).Control is NumericInputBase numericInputBase)
		{
			numericInputBase.ShowUpDown = true;
			numericInputBase.AutoOverwrite = false;
			numericInputBase.ButtonFreeText.Shortcut = eShortcut.F2;
		}
		base.InitializeNewComponent(defaultValues);
	}
}
