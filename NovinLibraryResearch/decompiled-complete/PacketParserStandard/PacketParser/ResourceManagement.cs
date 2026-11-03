using System;

namespace PacketParser;

public static class ResourceManagement
{
	private static TextBrandResourceManagement _textBrandResourceManagement;

	public static TextBrandResourceManagement TextBrandResourceManagement
	{
		get
		{
			return _textBrandResourceManagement;
		}
		set
		{
			if (_textBrandResourceManagement == null)
			{
				_textBrandResourceManagement = value ?? throw new NullReferenceException("value of TextBrandResourceManagement is null");
			}
		}
	}
}
