namespace Microsoft.SqlServer.Management.XEvent;

internal interface IXEObjectInfoCollection<T> where T : IXEObjectInfo
{
	T this[string name] { get; }
}
