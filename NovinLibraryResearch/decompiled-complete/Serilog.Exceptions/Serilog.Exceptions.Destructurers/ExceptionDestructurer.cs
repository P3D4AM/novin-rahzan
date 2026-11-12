using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Management.Instrumentation;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class ExceptionDestructurer : IExceptionDestructurer
{
	public virtual Type[] TargetTypes => new List<Type>
	{
		typeof(InvalidUdtException),
		typeof(AccessViolationException),
		typeof(AppDomainUnloadedException),
		typeof(ApplicationException),
		typeof(ArithmeticException),
		typeof(ArrayTypeMismatchException),
		typeof(CannotUnloadAppDomainException),
		typeof(KeyNotFoundException),
		typeof(CheckoutException),
		typeof(InvalidAsynchronousStateException),
		typeof(InvalidEnumArgumentException),
		typeof(SettingsPropertyIsReadOnlyException),
		typeof(SettingsPropertyNotFoundException),
		typeof(SettingsPropertyWrongTypeException),
		typeof(ContextMarshalException),
		typeof(ConstraintException),
		typeof(DataException),
		typeof(DeletedRowInaccessibleException),
		typeof(DuplicateNameException),
		typeof(EvaluateException),
		typeof(InRowChangingEventException),
		typeof(InvalidConstraintException),
		typeof(InvalidExpressionException),
		typeof(MissingPrimaryKeyException),
		typeof(NoNullAllowedException),
		typeof(OperationAbortedException),
		typeof(ReadOnlyException),
		typeof(RowNotInTableException),
		typeof(SqlAlreadyFilledException),
		typeof(SqlNotFilledException),
		typeof(StrongTypingException),
		typeof(SyntaxErrorException),
		typeof(VersionNotFoundException),
		typeof(EventLogInvalidDataException),
		typeof(EventLogNotFoundException),
		typeof(EventLogProviderDisabledException),
		typeof(EventLogReadingException),
		typeof(EventSourceException),
		typeof(DataMisalignedException),
		typeof(DivideByZeroException),
		typeof(DllNotFoundException),
		typeof(DuplicateWaitObjectException),
		typeof(EntryPointNotFoundException),
		typeof(Exception),
		typeof(FieldAccessException),
		typeof(FormatException),
		typeof(IndexOutOfRangeException),
		typeof(InsufficientExecutionStackException),
		typeof(InsufficientMemoryException),
		typeof(InvalidCastException),
		typeof(InvalidOperationException),
		typeof(InvalidProgramException),
		typeof(InvalidTimeZoneException),
		typeof(DirectoryNotFoundException),
		typeof(DriveNotFoundException),
		typeof(EndOfStreamException),
		typeof(InternalBufferOverflowException),
		typeof(InvalidDataException),
		typeof(IOException),
		typeof(IsolatedStorageException),
		typeof(PathTooLongException),
		typeof(InstanceNotFoundException),
		typeof(InstrumentationBaseException),
		typeof(InstrumentationException),
		typeof(MemberAccessException),
		typeof(MethodAccessException),
		typeof(MulticastNotSupportedException),
		typeof(CookieException),
		typeof(PingException),
		typeof(ProtocolViolationException),
		typeof(NotImplementedException),
		typeof(NotSupportedException),
		typeof(NullReferenceException),
		typeof(OutOfMemoryException),
		typeof(OverflowException),
		typeof(PlatformNotSupportedException),
		typeof(RankException),
		typeof(AmbiguousMatchException),
		typeof(CustomAttributeFormatException),
		typeof(InvalidFilterCriteriaException),
		typeof(TargetException),
		typeof(TargetInvocationException),
		typeof(TargetParameterCountException),
		typeof(MissingManifestResourceException),
		typeof(COMException),
		typeof(InvalidComObjectException),
		typeof(InvalidOleVariantTypeException),
		typeof(MarshalDirectiveException),
		typeof(SafeArrayRankMismatchException),
		typeof(SafeArrayTypeMismatchException),
		typeof(SEHException),
		typeof(RemotingException),
		typeof(RemotingTimeoutException),
		typeof(ServerException),
		typeof(SerializationException),
		typeof(AuthenticationException),
		typeof(InvalidCredentialException),
		typeof(CryptographicException),
		typeof(CryptographicUnexpectedOperationException),
		typeof(PolicyException),
		typeof(VerificationException),
		typeof(XmlSyntaxException),
		typeof(StackOverflowException),
		typeof(SystemException),
		typeof(BarrierPostPhaseException),
		typeof(LockRecursionException),
		typeof(SemaphoreFullException),
		typeof(SynchronizationLockException),
		typeof(TaskSchedulerException),
		typeof(ThreadInterruptedException),
		typeof(ThreadStartException),
		typeof(ThreadStateException),
		typeof(WaitHandleCannotBeOpenedException),
		typeof(TimeoutException),
		typeof(TimeZoneNotFoundException),
		typeof(TypeAccessException),
		typeof(TypeUnloadedException),
		typeof(UnauthorizedAccessException),
		typeof(UriFormatException)
	}.ToArray();

	public virtual void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		if (propertiesBag == null)
		{
			throw new ArgumentNullException("propertiesBag");
		}
		if (destructureException == null)
		{
			throw new ArgumentNullException("destructureException");
		}
		propertiesBag.AddProperty("Type", exception.GetType().FullName);
		DestructureCommonExceptionProperties(exception, propertiesBag, destructureException, (IDictionary data) => data.ToStringObjectDictionary());
	}

	internal static void DestructureCommonExceptionProperties(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> innerDestructure, Func<IDictionary, object> destructureDataProperty)
	{
		if (exception.Data.Count != 0)
		{
			propertiesBag.AddProperty("Data", destructureDataProperty(exception.Data));
		}
		if (!string.IsNullOrEmpty(exception.HelpLink))
		{
			propertiesBag.AddProperty("HelpLink", exception.HelpLink);
		}
		if (exception.HResult != 0)
		{
			propertiesBag.AddProperty("HResult", exception.HResult);
		}
		propertiesBag.AddProperty("Message", exception.Message);
		propertiesBag.AddProperty("Source", exception.Source);
		propertiesBag.AddProperty("StackTrace", exception.StackTrace);
		if ((object)exception.TargetSite != null)
		{
			propertiesBag.AddProperty("TargetSite", exception.TargetSite.ToString());
		}
		if (exception.InnerException != null)
		{
			propertiesBag.AddProperty("InnerException", innerDestructure(exception.InnerException));
		}
	}
}
