using System.IO;
using System.IO.Compression;

namespace SupaFabulus.Dev.Foundation.Utils
{
    public static class ZipUtils
    {
        public static readonly string MAIN_ENTRY_NAME = "main";
        
        
        public static byte[] Compress<TSource>(TSource[] source)
        {
            byte[] result = null;
            char[] chars = DataUtils.ConvertToChars<TSource>(source);
            
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    var entry = zip.CreateEntry(MAIN_ENTRY_NAME);
                    using (StreamWriter sw = new StreamWriter(entry.Open()))
                    {
                        sw.Write(chars);
                        sw.Flush();
                        sw.Close();
                    }
                    zip.Dispose();
                            
                }
                result = zipStream.GetBuffer();
            }

            return result;
        }

        
        public static byte[] Decompress(byte[] compressedData)
        {
            string entryData;
                    
            using (MemoryStream zipStream = new MemoryStream())
            {
                zipStream.Write(compressedData, 0, compressedData.Length);
                using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Read, false))
                {
                    var entry = zip.GetEntry(MAIN_ENTRY_NAME);
                    if (entry == null) return null;
                    
                    using (StreamReader sr = new StreamReader(entry.Open()))
                    {
                        entryData = sr.ReadToEnd();
                        sr.Close();
                    }
                    zip.Dispose();
                }
            }

            return DataUtils.StringToBytes(entryData);
        }
    }
}