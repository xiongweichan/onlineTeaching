using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;
using TeacherClient.Core;
using Qiniu.IO;
using Qiniu.IO.Model;
using Qiniu.Http;
using System.Windows.Threading;

namespace TeacherClient
{
    public class UploadFileHelper
    {
        public static readonly UploadFileHelper Instance = new UploadFileHelper();
        public string _path = AppDomain.CurrentDomain.BaseDirectory + "uplodingfiles.12345";

        public static string GetPath(string dir, string name)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir + "\\" + name;
        }

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

        public FileModel Add(string path, string token, string domain, string key, EnFileType type)
        {
            if (_WaittingFiles.Any(T => T.LocalFile == path))
                return _WaittingFiles.FirstOrDefault(T => T.LocalFile == path);

            FileModel model = new FileModel();
            FileInfo info = new FileInfo(path);
            model.ID = Guid.NewGuid().ToString();
            model.SaveKey = key;
            model.Token = token;
            model.Type = type;
            model.Extension = info.Extension;
            model.FileName = info.Name;
            model.FileSize = info.Length;
            model.LocalFile = path;
            model.RecordFile = GetPath(string.Format("{0}\\{1}", CacheHelper.CacheFilePath, model.ID), info.Name + ".12345");
            _WaittingFiles.Add(model);
            model.Started += Model_Started;
            model.Complated += Item_Complated;
            model.Start();
            return model;
        }

        public List<FileModel> GetList(EnFileType type)
        {
            return _WaittingFiles.Where(T => T.Type == type).ToList();
        }

        private void Model_Started(object sender, EventArgs e)
        {
            File.WriteAllText(_path, _WaittingFiles.ToJson());
        }

        List<FileModel> _WaittingFiles = new List<FileModel>();

        public class FileModel : ViewModelBase
        {
            public event EventHandler Started;
            public event EventHandler Complated;

            public string ID { get; set; }
            public string Token { get; set; }
            public EnFileType Type { get; set; }
            public string Extension { get; set; }
            public string FileName { get; set; }
            public long FileSize { get; set; }
            public string LocalFile { get; set; }
            public string RecordFile { get; set; }
            public string SaveKey { get; set; }

            public void Start()
            {
                Task.Factory.StartNew(StartReal);
            }

            async void StartReal()
            {

                if (!string.IsNullOrWhiteSpace(ID) && !string.IsNullOrWhiteSpace(Token) && File.Exists(LocalFile))
                {
                    if (Started != null)
                        Started(this, new EventArgs());
                    try
                    {
                        // 包含两个参数，并且都有默认值
                        // 参数1(bool): uploadFromCDN是否从CDN加速上传，默认否
                        // 参数2(enum): chunkUnit上传分片大小，可选值128KB,256KB,512KB,1024KB,2048KB,4096KB
                        ResumableUploader ru = new ResumableUploader(false, ChunkUnit.U128K);
                        //最大尝试次数(有效值1~20)，在上传过程中(如mkblk或者bput操作)如果发生错误，它将自动重试，如果没有错误则无需重试
                        int maxTry = 10;
                        // 使用默认进度处理，使用自定义上传控制            
                        UploadProgressHandler upph = new UploadProgressHandler(UploadProgress);
                        UploadController upctl = new UploadController(UploadControl);
                        HttpResult result = await ru.UploadFileAsync(LocalFile, SaveKey, Token, RecordFile, maxTry, upph, upctl);
                        try
                        {
                            if (result != null && result.Code == 200)
                            {
                                IsCompleted = true;
                            }
                            else
                            {
                                Log.Error(result);
                                ViewModelBase.InvokeOnUIThread(new Action(() =>
                                {
                                    MessageWindow.Alter("提示", "上传失败");

                                }));
                            }
                            if (Complated != null)
                                Complated(this, new EventArgs());
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            }

            public void Cancel()
            {
                _cancel = true;

                if (Complated != null)
                    Complated(this, new EventArgs());
            }

            bool _cancel = false;
            bool _Pause;
            public bool Pause
            {
                get { return _Pause; }
                set
                {
                    _Pause = value;
                    this.OnPropertyChanged("Pause");
                }
            }
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
            bool _IsCompleted;
            public bool IsCompleted
            {
                get { return _IsCompleted; }
                set
                {
                    _IsCompleted = value;
                    this.OnPropertyChanged("IsCompleted");
                }
            }
            /// <summary>
            /// 上传控制
            /// </summary>
            /// <returns></returns>
            private UPTS UploadControl()
            {
                if (_cancel)
                    return UPTS.Aborted;
                if (Pause)
                {
                    return UPTS.Suspended;
                }
                else
                {
                    return UPTS.Activated;
                }
            }

            void UploadProgress(long uploadedBytes, long totalBytes)
            {
                UploadedBytes = uploadedBytes;
                Per = (double)uploadedBytes * 100 / totalBytes;
            }
        }
        public enum EnFileType
        {
            Course,
            Courseware,
        }
    }
}
