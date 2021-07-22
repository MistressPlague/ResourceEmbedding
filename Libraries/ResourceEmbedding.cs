using System.IO;
using System.Linq;
using System.Reflection;

namespace ResourceEmbedding
{
    internal class ResourceEmbedding
    {
        internal static void WriteResources(string FolderName, params string[] FileExtensionsToWrite)
        {
            foreach (string resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (resourceName == FolderName)
                {
                    if (FileExtensionsToWrite.Any(o => resourceName.EndsWith(o)))
                    {
                        using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                        {
                            var FileName = resourceName;

                            FileName = FileName.Substring(FileName.IndexOf(".") + 1);
                            FileName = FileName.Substring(FileName.IndexOf(".") + 1);

                            if (!File.Exists(FileName))
                            {
                                using (var file = new FileStream(FileName, FileMode.Create, FileAccess.Write))
                                {
                                    CopyTo(resource, file);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static void CopyTo(Stream input, Stream output)
        {
            var buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }
}
