using System;

namespace Stimulsoft.Base;

public interface IStiAppDataColumn : IStiAppDataCell, IStiAppCell
{
	string GetNameInSource();

	Type GetDataType();
}
