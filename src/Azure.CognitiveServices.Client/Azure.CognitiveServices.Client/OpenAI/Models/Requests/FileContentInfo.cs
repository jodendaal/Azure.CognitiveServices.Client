using Azure.CognitiveServices.Client.OpenAI.Models.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Azure.CognitiveServices.Client.OpenAI.Models.Requests
{
    public class FileContentInfo
    {
        public FileContentInfo(byte[] fileContent, string fileName)
        {
            if (fileContent == null || fileContent.Length == 0)
            {
                throw new OpenAIValidationException("FileContent is required");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new OpenAIValidationException("FileName is required");
            }

            FileContent = fileContent;
            FileName = fileName;
        }
        [Required]
        public byte[] FileContent { get; set; }
        [Required]
        public string FileName { get; set; }

        public static FileContentInfo Load(string file)
        {
            var bytes = File.ReadAllBytes(file);
            var name = new FileInfo(file).Name;
            return new FileContentInfo(bytes, name);
        }

        public void Save(string path)
        {
            File.WriteAllBytes(path, FileContent);
        }
    }
}
