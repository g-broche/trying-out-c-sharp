using TestApi.Models;

namespace TestApi.DTOs.Responses;

#pragma warning restore format

public record UserDetailDto(
    string FirstName,
    string LastName,
    string Email,
    string CreatedAt,
    string UpdatedAt
    )
{
    public UserDetailDto(User user)
    : this(
        user.FirstName,
        user.LastName,
        user.Email,
        user.CreatedAt.ToString("O"),
        user.UpdatedAt.ToString("O")
    )
    {
    }
}
