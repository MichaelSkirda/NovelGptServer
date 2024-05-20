namespace RenSharpServer.Services.Interfaces
{
	public interface IGptService
	{
		Task<string> MapAnswer(string input);
	}
}
