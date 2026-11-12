using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class MaskedTextBoxAdvDesignerActionList : DesignerActionList
{
	private ITypeDiscoveryService discoverySvc;

	private IHelpService helpService;

	private MaskedTextBoxAdv maskedTextBox;

	private IUIService uiSvc;

	public MaskedTextBoxAdvDesignerActionList(MaskedTextBoxAdvDesigner designer)
		: base(((ComponentDesigner)designer).Component)
	{
		maskedTextBox = (MaskedTextBoxAdv)((ComponentDesigner)designer).Component;
		discoverySvc = ((DesignerActionList)this).GetService(typeof(ITypeDiscoveryService)) as ITypeDiscoveryService;
		ref IUIService reference = ref uiSvc;
		object service = ((DesignerActionList)this).GetService(typeof(IUIService));
		reference = (IUIService)((service is IUIService) ? service : null);
		helpService = ((DesignerActionList)this).GetService(typeof(IHelpService)) as IHelpService;
		_ = discoverySvc;
	}

	public override DesignerActionItemCollection GetSortedActionItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "SetMask", "Set Mask..."));
		return val;
	}

	public void SetMask()
	{
		string text = MaskAdvPropertyEditor.EditMask(discoverySvc, uiSvc, maskedTextBox, helpService);
		if (text != null)
		{
			TypeDescriptor.GetProperties(maskedTextBox)["Mask"]?.SetValue(maskedTextBox, text);
		}
	}
}
