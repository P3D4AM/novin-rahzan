using System.Runtime.InteropServices;
using SMDOCX;

namespace AxSMDOCX;

[ClassInterface(ClassInterfaceType.None)]
public class AxMahakScaleOCXEventMulticaster : __MahakScaleOCX
{
	private AxMahakScaleOCX parent;

	public AxMahakScaleOCXEventMulticaster(AxMahakScaleOCX parent)
	{
		this.parent = parent;
	}
}
