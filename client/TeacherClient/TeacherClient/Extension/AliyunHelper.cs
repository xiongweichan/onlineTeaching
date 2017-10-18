using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherClient.Core;
using Telerik.Windows.Controls;

namespace TeacherClient
{
    public class AliyunHelper
    {
        public static readonly AliyunHelper Instance = new AliyunHelper();

        public string _path = AppDomain.CurrentDomain.BaseDirectory + "aliyun.12345";
        public static string GetPath(string dir, string name)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir + "\\" + name;
        }
        
        List<FileModel> _WaittingFiles = new List<FileModel>();

        public void Run()
        {
            try
            {
                _path = AppDomain.CurrentDomain.BaseDirectory + App.CurrentLogin.user.lecturer_code + "uplodingfiles.12345";
                if (File.Exists(_path))
                {
                    var str = File.ReadAllText(_path);
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        _WaittingFiles = str.FromJson<List<FileModel>>();
                        foreach (var item in _WaittingFiles)
                        {
                            item.Complated += Item_Complated;
                            item.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void Item_Complated(object sender, EventArgs e)
        {
            lock (_WaittingFiles)
            {
                _WaittingFiles.Remove(sender as FileModel);
                File.WriteAllText(_path, _WaittingFiles.ToJson());
            }
        }
        public List<FileModel> GetList(EnFileType type)
        {
            return _WaittingFiles.Where(T => T.Type == type).ToList();
        }

        private void Model_Started(object sender, EventArgs e)
        {
            File.WriteAllText(_path, _WaittingFiles.ToJson());
        }
        public FileModel Add(string path, string accessKeyId, string accessKeySecret, string bucketName
            , string endPoint, string key, EnFileType type)
        {
            try
            {

                FileModel model = new FileModel();
                FileInfo info = new FileInfo(path);

                model.AccessKeyId = accessKeyId;
                model.AccessKeySecret = accessKeySecret;
                model.BucketName = bucketName;
                model.EndPoint = endPoint;
                model.FileToUpload = path;
                model.Key = key;
                model.Type = type;
                model.Extension = info.Extension;
                model.FileName = info.Name;
                model.FileSize = info.Length;

                _WaittingFiles.Add(model);
                model.Started += Model_Started;
                model.Complated += Item_Complated;
                model.Start();
                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return null;
        }

        public class FileModel : ViewModelBase
        {
            public event EventHandler Started;
            public event EventHandler Complated;

            public string EndPoint { get; set; }
            public string AccessKeyId { get; set; }
            public string AccessKeySecret { get; set; }
            public string Key { get; set; }
            public string BucketName { get; set; }
            string _FileToUpload;
            public string FileToUpload
            {
                get { return _FileToUpload; }
                set
                {
                    _FileToUpload = value;
                    try
                    {
                        if (File.Exists(_FileToUpload))
                            FileSize = new FileInfo(_FileToUpload).Length;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            }
            public EnFileType Type { get; set; }
            public string Extension { get; set; }
            public string FileName { get; set; }
            public long FileSize { get; set; }
            long _UploadedBytes;
            public long UploadedBytes
            {
                get { return _UploadedBytes; }
                set
                {
                    _UploadedBytes = value;
                    this.OnPropertyChanged("UploadedBytes");
                }
            }
            double _Per;
            public double Per
            {
                get { return _Per; }
                set
                {
                    _Per = value;
                    this.OnPropertyChanged("Per");
                }
            }

            public void Start()
            {
                try
                {
                    OssClient client = new OssClient(EndPoint, AccessKeyId, AccessKeySecret);
                    string checkpointDir = Config.CacheFilePath;
                    var result = client.ResumableUploadObject(BucketName, Key, FileToUpload, null, checkpointDir, null, streamProgressCallback);
                }
                catch (OssException ex)
                {
                    Log.Error(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId), ex);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
            private void streamProgressCallback(object sender, StreamTransferProgressArgs args)
            {
                try
                {
                    Per = args.PercentDone;
                    UploadedBytes = args.TransferredBytes;
                    if (Per == 100 && Complated != null)
                    {
                        Complated(this, new EventArgs());
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

        }
        public enum EnFileType
        {
            Course,
            Courseware,
        }


    }


}
