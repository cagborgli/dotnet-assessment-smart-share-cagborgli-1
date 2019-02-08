using System;

namespace Core
{
    [Serializable]
    public class FileRequest
    {
        public Action Action { get; set; }
        public string Filename { get; set; }
        public byte[] TheFile { get; set; }
        public string Password { get; set; }
        public DateTime TimeCreated { get; set; }
        public int MaxDownload { get; set; }
        public DateTime Expiration { get; set; }
        public int TotalDownload { get; set; }
    }

    public enum Action {upload, download, view}
}
