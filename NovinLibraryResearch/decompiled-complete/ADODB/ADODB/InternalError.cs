using System.Reflection;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
[DefaultMember("Description")]
public class InternalError : Error
{
	internal InternalErrors m_Errors;

	internal Error m_Error;

	public string Description => m_Error.Description;

	public int NativeError => m_Error.NativeError;

	public string SQLState => m_Error.SQLState;

	public string Source => m_Error.Source;

	public int Number => m_Error.Number;

	public string HelpFile => m_Error.HelpFile;

	public int HelpContext => m_Error.HelpContext;

	internal InternalError(Error error)
	{
		m_Error = error;
	}
}
