using System;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

internal class GlyphServiceProvider : IServiceProvider
{
	private IServiceProvider source;

	public GlyphServiceProvider(IServiceProvider source)
	{
		this.source = source;
	}

	object IServiceProvider.GetService(Type serviceType)
	{
		if (serviceType.Equals(typeof(ISelectionService)))
		{
			return null;
		}
		return source.GetService(serviceType);
	}
}
