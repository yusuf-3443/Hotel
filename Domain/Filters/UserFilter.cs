namespace Domain.Filters;

public class UserFilter:PaginationFilter
{
    public string? Username { get; set; }
}