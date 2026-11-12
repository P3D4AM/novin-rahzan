using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient.Server;

[Serializable]
public sealed class InvalidUdtException : SystemException
{
	private const int InvalidUdtHResult = -2146232009;

	internal InvalidUdtException()
	{
		base.HResult = -2146232009;
	}

	internal InvalidUdtException(string message)
		: base(message)
	{
		base.HResult = -2146232009;
	}

	internal InvalidUdtException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146232009;
	}

	private InvalidUdtException(SerializationInfo si, StreamingContext sc)
		: base(si, sc)
	{
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
	public override void GetObjectData(SerializationInfo si, StreamingContext context)
	{
		base.GetObjectData(si, context);
	}

	internal static InvalidUdtException Create(Type udtType, string resourceReason)
	{
		string @string = StringsHelper.GetString(resourceReason);
		string string2 = StringsHelper.GetString(Strings.SqlUdt_InvalidUdtMessage, udtType.FullName, @string);
		InvalidUdtException ex = new InvalidUdtException(string2);
		ADP.TraceExceptionAsReturnValue(ex);
		return ex;
	}
}
