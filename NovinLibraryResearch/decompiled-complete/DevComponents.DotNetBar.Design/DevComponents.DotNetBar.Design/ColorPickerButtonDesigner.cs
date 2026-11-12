using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class ColorPickerButtonDesigner : ButtonXDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		base.InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		ColorPickerButton component = ((ControlDesigner)this).Control as ColorPickerButton;
		TypeDescriptor.GetProperties(component)["Image"].SetValue(component, Helpers.LoadBitmap("SystemImages.ColorPickerButtonImage.png"));
		TypeDescriptor.GetProperties(component)["SelectedColorImageRectangle"].SetValue(component, new Rectangle(2, 2, 12, 12));
		TypeDescriptor.GetProperties(component)["Text"].SetValue(component, "");
		TypeDescriptor.GetProperties(component)["Size"].SetValue(component, new Size(37, 23));
		TypeDescriptor.GetProperties(component)["Style"].SetValue(component, eDotNetBarStyle.StyleManagerControlled);
	}
}
