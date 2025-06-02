
using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Application.DTOs.VideoRequest
{
    public class VideoRequestDTO
    {
        public int VideoRequestId { get; set; }
        
        public int UserId { get; set; }
        
        public string? UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Topic { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string SubTopic { get; set; } = null!;
        
        [Required]
        [StringLength(50)]
        public string RequestStatus { get; set; } = null!;
        
        [Required]
        [StringLength(200)]
        public string ShortTitle { get; set; } = null!;

        [Required]
        [StringLength(4000)]
        public string RequestDescription { get; set; } = null!;
        
        [StringLength(4000)]
        public string? Response { get; set; }
        
        [StringLength(2000)]
        public string? VideoUrls { get; set; }
    }
}
