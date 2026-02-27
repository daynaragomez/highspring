namespace Highspring.Application.Common;

public class ConcurrencyConflictException(string message) : Exception(message)
{
}
