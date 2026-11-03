namespace PacketParser.DepartmentApiModel;

public class GenericApiResponse<T>
{
	public string Message { get; set; }

	public T Result { get; set; }
}
