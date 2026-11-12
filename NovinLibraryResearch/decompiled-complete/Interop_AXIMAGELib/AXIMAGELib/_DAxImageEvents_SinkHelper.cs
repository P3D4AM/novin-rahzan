using System.Runtime.InteropServices;

namespace AXIMAGELib;

[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class _DAxImageEvents_SinkHelper : _DAxImageEvents
{
	public _DAxImageEvents_ClickEventHandler m_ClickDelegate;

	public int m_dwCookie;

	public void Click()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ClickDelegate != null)
		{
			m_ClickDelegate();
		}
	}

	internal _DAxImageEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ClickDelegate = null;
	}
}
