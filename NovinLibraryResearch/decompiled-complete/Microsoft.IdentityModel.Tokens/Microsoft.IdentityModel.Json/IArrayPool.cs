namespace Microsoft.IdentityModel.Json;

internal interface IArrayPool<T>
{
	T[] Rent(int minimumLength);

	void Return(T[] array);
}
