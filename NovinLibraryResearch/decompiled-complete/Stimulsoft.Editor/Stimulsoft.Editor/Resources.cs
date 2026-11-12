using System.ComponentModel;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class Resources : Component
{
	private Container components;

	public Resources(IContainer container)
	{
		container.Add(this);
		InitializeComponent();
	}

	public Resources()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		components = new Container();
	}
}
