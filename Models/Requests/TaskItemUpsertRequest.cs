using System.ComponentModel.DataAnnotations;

namespace DotnetApi.Models.Requests
{
    public class TaskItemUpsertRequest
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        public bool? IsCompleted { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}