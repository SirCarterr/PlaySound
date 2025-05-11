namespace PlaySound.Constants
{
    public static class DialogCaptions
    {
        public const string Error = "Error";
        public const string Attention = "Attention";
        public const string Information = "Information";
    }

    public static class DialogMessages
    {
        public const string DeleteQuestion = "Are you sure you want to delete this audio?";
        public const string InvalidFileExtension = "Invalid file extension. Please select an MP3 file.";
        public const string FileSizeExceed = "File size exceeds the maximum allowed size (3MB).";
    }

    public static class AudioConstants
    {
        public const string VirtualCableDevice = "CABLE Input";
        public const int DefaultSampleRate = 48000;
        public const int DefaultChannelCount = 2;
    }
} 