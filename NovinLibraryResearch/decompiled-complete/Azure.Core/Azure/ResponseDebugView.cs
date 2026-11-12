namespace Azure;

internal class ResponseDebugView<T>
{
	private readonly Response<T> _response;

	public Response GetRawResponse => _response.GetRawResponse();

	public T Value => _response.Value;

	public ResponseDebugView(Response<T> response)
	{
		_response = response;
	}
}
