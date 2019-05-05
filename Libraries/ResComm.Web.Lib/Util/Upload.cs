using Microsoft.WindowsAzure;
using Microsoft.Azure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ResComm.Web.Lib.Util
{
    public class Upload
    {
        internal static bool IsValidImage(string fileName)
        {
            Regex regex = new Regex(@"(.*?)\.(jpg|JPG|jpeg|JPEG|png|PNG|gif|GIF|bmp|BMP)$");
            return regex.IsMatch(fileName);
        }
        public string uploadFileToBlob(HttpPostedFileBase FileModel)
        {
            string response = null;

            if (!IsValidImage(FileModel.FileName))
            {
                throw new Exception("Invalid image file");
            }

            string uploadedPath = "";
            if (FileModel.ContentLength > 0)
            {
                try
                {
                    var fileName = Path.GetFileName(FileModel.FileName);

                    //access container
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                    // Create the blob client
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Retrieve reference to a previously created container
                    CloudBlobContainer container = blobClient.GetContainerReference("upload");

                    // Create the container if it doesn't already exist
                    container.CreateIfNotExist();
                    container.SetPermissions(
                    new BlobContainerPermissions
                    {
                        PublicAccess =
                            BlobContainerPublicAccessType.Blob
                    });

                    // Retrieve reference to a blob named "myblob"
                    CloudBlob blob = container.GetBlobReference(fileName);
                    blob.Properties.ContentType = "image\\jpeg";    //make sure saved as jpeg

                    // Create or overwrite the "myblob" blob with contents from a local file


                    MemoryStream target = new MemoryStream();
                    FileModel.InputStream.CopyTo(target);
                    //StreamReader sourceStream = new StreamReader(FileModel.InputStream);
                    //byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    byte[] fileContents = target.ToArray();
                    target.Close();

                    blob.UploadByteArray(fileContents);
                    System.Configuration.AppSettingsReader settingsReader =new AppSettingsReader();
                    string fileurl = (string)settingsReader.GetValue("fileurl", typeof(String));
                    uploadedPath = fileurl + fileName;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            response = uploadedPath;
            return response;
        }
    }
}
