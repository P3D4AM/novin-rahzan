using System.Collections.Generic;
using System.Threading;

namespace FormCustom.UcBinding;

public class FooterBindingDataReadyEventArg
{
	public List<object> ListData { get; internal set; }

	public CancellationToken Token { get; internal set; }
}
