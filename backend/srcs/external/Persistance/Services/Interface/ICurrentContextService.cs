namespace Persistance.Services.Interface;

public interface ICurrentContextService
{
	Dictionary<string, string> Claims   { get; }
	Dictionary<string, string> Headers  { get; }
}