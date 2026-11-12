using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.HadrModel;

public interface IValidatorProvider
{
	List<Validator> Validators();
}
