namespace mCore;

public interface IvCard
{
	string Destination { get; set; }

	string FirstName { get; set; }

	string MiddleName { get; set; }

	string LastName { get; set; }

	string HomePhone { get; set; }

	string BusinessPhone { get; set; }

	string MobilePhone { get; set; }

	string Email { get; set; }

	string URL { get; set; }

	string JobTitle { get; set; }

	string Company { get; set; }

	string HomeAddressStreet { get; set; }

	string HomeAddressCity { get; set; }

	string HomeAddressState { get; set; }

	string HomeAddressZIP { get; set; }

	string HomeAddressCountry { get; set; }

	string BusinessAddressStreet { get; set; }

	string BusinessAddressCity { get; set; }

	string BusinessAddressState { get; set; }

	string BusinessAddressZIP { get; set; }

	string BusinessAddressCountry { get; set; }

	string Comment { get; set; }

	string Send();

	string SendFile(string vCardFile);

	string SendToQueue();

	string SendToQueue(QueuePriority Priority);

	string SendFileToQueue(string vCardFile);

	string SendFileToQueue(string vCardFile, QueuePriority Priority);
}
