namespace PlaySound.Model
{
    public class DialogResponseDto
    {
        public bool IsSuccess { get; set; }

        public AudioDto? Audio { get; set; }

        public string? Message { get; set; }
    }
}
