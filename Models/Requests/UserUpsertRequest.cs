using System.ComponentModel.DataAnnotations;

namespace DotnetApi.Models.Requests
{
    public class UserUpsertRequest
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Email { get; set; } = default!;

        [Range(0, 2)]
        public int Role { get; set; }
    }
}