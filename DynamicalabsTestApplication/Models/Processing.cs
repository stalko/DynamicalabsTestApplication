using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicalabsTestApplication.Models
{
    public class Processing
    {
        public async Task MethodA(string pathToZIP)
        {
            var tasks = new List<Task>();
            var tmpFilesPath = new List<string>();

            try
            {
                var zipName = Path.GetFileNameWithoutExtension(pathToZIP);
                using (var zip = ZipFile.Open(pathToZIP, ZipArchiveMode.Read))
                {
                    foreach (var item in zip.Entries)
                    {
                        if (Path.GetExtension(item.FullName) != ".csv")
                            continue;

                        //Extract csv file to temporary folder
                        var tmpFilePath = Path.GetTempFileName();
                        item.ExtractToFile(tmpFilePath, true);
                        tasks.Add(MethodB(tmpFilePath, item.Name));
                        tmpFilesPath.Add(tmpFilePath);
                    }
                    //Wait all task
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                //Remove temporary files
                foreach (var file in tmpFilesPath)
                {
                    var fi = new FileInfo(file);
                    if (fi.Exists) fi.Delete();
                }
            }
            
        }

        private async Task MethodB(string filePath, string fileName)
        {
            //Sleep 1 second
            System.Threading.Thread.Sleep(1000);
        }
    }
}
