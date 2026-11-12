namespace Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

internal class MetroButtonItemPainter : Office2007ButtonItemPainter
{
	private static RoundRectangleShapeDescriptor _DefaultMetroShape = new RoundRectangleShapeDescriptor(0);

	protected override IShapeDescriptor GetButtonShape(ButtonItem button, ItemPaintArgs pa)
	{
		IShapeDescriptor buttonShape = GetButtonShape(button);
		if (pa.ContainerControl is ButtonX)
		{
			buttonShape = ((ButtonX)(object)pa.ContainerControl).GetButtonShape();
		}
		else if (pa.ContainerControl is NavigationBar)
		{
			buttonShape = ((NavigationBar)(object)pa.ContainerControl).ButtonShape;
		}
		return buttonShape;
	}

	private static IShapeDescriptor GetButtonShape(ButtonItem button)
	{
		if (button.Shape != null)
		{
			return button.Shape;
		}
		return _DefaultMetroShape;
	}
}
