using Aliyun.OSS;
using Aliyun.OSS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeacherClient.Core;
using Telerik.Windows.Controls;

namespace TeacherClient
{
    public class AliyunHelper2 : IDisposable
    {
        static AliyunHelper2 _Instance;

        public static AliyunHelper2 GetInstance()
        {
            if (_Instance == null)
                _Instance = new AliyunHelper2();
            return _Instance;
        }

        public List<UploadFileShowInfo> Files { get; private set; }
        BackgroundWorker _worker;
        string _path;
        AliyunHelper2()
        {
            if (App.CurrentLogin == null || App.CurrentLogin.user == null || string.IsNullOrWhiteSpace(App.CurrentLogin.user.lecturer_code))
            {
                MessageWindow.Alter("错误", "登录信息有误！请重新登陆。");
                return;
            }
            _path = AppDomain.CurrentDomain.BaseDirectory + App.CurrentLogin.user.lecturer_code + "uplodingfiles.12345";
            if (File.Exists(_path))
            {
                var str = File.ReadAllText(_path);
                if (!string.IsNullOrWhiteSpace(str))
                {
                    var list = str.FromJson<List<UploadFileInfo>>();
                    if (list != null)
                        Files = list.Select(T => new UploadFileShowInfo(T)).ToList();
                }
            }

            if (Files == null)
                Files = new List<UploadFileShowInfo>();
            _worker = new BackgroundWorker();
            _worker.DoWork += UploadFileExecute;
            _worker.RunWorkerAsync();
        }
        public UploadFileShowInfo CurrentFile { get; private set; }
        private void UploadFileExecute(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (Files.Any(T => T.Cancel))
                {
                    lock (Files)
                    {
                        Files.RemoveAll(T => T.Cancel);
                        File.WriteAllText(_path, Files.Select(T => T.UFile).ToJson());
                    }
                }

                bool need = true;
                try
                {
                    var file = Files.FirstOrDefault(T => !T.Pause);
                    if (file != null)
                    {
                        CurrentFile = file;
                        OssClient client = new OssClient(CurrentFile.UFile.EndPoint, CurrentFile.UFile.AccessKeyId, CurrentFile.UFile.AccessKeySecret);
                        string checkpointDir = CacheHelper.CacheFilePath;
                        var result = client.ResumableUploadObject(CurrentFile.UFile.BucketName, CurrentFile.UFile.Key, CurrentFile.UFile.FilePath, null, checkpointDir, null, streamProgressCallback);
                        UploadCompleted();
                        CurrentFile.IsCompleted = true;
                    }
                    else
                    {
                        need = false;
                        Thread.Sleep(100);
                    }
                }
                catch (OssException ex)
                {
                    Log.Error(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId), ex);
                    ViewModelBase.InvokeOnUIThread(() =>
                    {
                        MessageWindow.Alter("提示", "上传失败:" + ex.Message);
                    });
                }
                catch (Exception ex)
                {
                    if(CurrentFile != null)
                    {
                        //不是取消任务，是暂停任务
                        if (!CurrentFile.Cancel && CurrentFile.Pause)
                            need = false;
                        else if (!CurrentFile.Cancel)//不是取消任务
                        {
                            Log.Error(ex);
                            ViewModelBase.InvokeOnUIThread(() =>
                            {
                                MessageWindow.Alter("提示", "上传失败");
                            });
                        }
                    }                    
                }
                finally
                {
                    if (need && CurrentFile != null)
                    {
                        lock (Files)
                        {
                            Files.Remove(CurrentFile);
                            File.WriteAllText(_path, Files.Select(T => T.UFile).ToJson());
                        }
                    }
                    CurrentFile = null;
                }
            }
        }

        private void streamProgressCallback(object sender, StreamTransferProgressArgs args)
        {
            try
            {
                CurrentFile.Per = args.PercentDone;
                CurrentFile.UploadedBytes = args.TransferredBytes;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            if (CurrentFile.Cancel)
                throw new SystemException("取消");
            if (CurrentFile.Pause)
                throw new SystemException("暂停");
        }

        async void UploadCompleted()
        {
            switch (CurrentFile.UFile.Type)
            {
                case UploadFileInfo.EnFileType.Course_Video:
                    {
                        Contract.Request.AliyunUploadCompleted_Course r = new Contract.Request.AliyunUploadCompleted_Course();
                        r.token = App.CurrentLogin.token;
                        r.lec_id = App.CurrentLogin.lec_id;
                        r.id = CurrentFile.UFile.ID;
                        r.key = CurrentFile.UFile.Key;
                        r.type = "1";
                        var b = await WebHelper.doPost<Contract.Request.AliyunUploadCompleted_Course>(Config.Interface_courseLessonUploadCompleted, r);
                    }
                    break;
                case UploadFileInfo.EnFileType.Course_Document:
                    {
                        Contract.Request.AliyunUploadCompleted_Course r = new Contract.Request.AliyunUploadCompleted_Course();
                        r.token = App.CurrentLogin.token;
                        r.lec_id = App.CurrentLogin.lec_id;
                        r.id = CurrentFile.UFile.ID;
                        r.key = CurrentFile.UFile.Key;
                        r.type = "0";
                        var b = await WebHelper.doPost<Contract.Request.AliyunUploadCompleted_Course>(Config.Interface_courseLessonUploadCompleted, r);
                    }
                    break;
                case UploadFileInfo.EnFileType.Courseware:
                    {
                        Contract.Request.AliyunUploadCompleted r = new Contract.Request.AliyunUploadCompleted();
                        r.token = App.CurrentLogin.token;
                        r.lec_id = App.CurrentLogin.lec_id;
                        r.id = CurrentFile.UFile.ID;
                        r.key = CurrentFile.UFile.Key;
                        var b = await WebHelper.doPost<Contract.Request.AliyunUploadCompleted>(Config.Interface_coursewareUploadCompleted, r);
                    }
                    break;
                case UploadFileInfo.EnFileType.Live:
                    {
                        Contract.Request.AliyunUploadCompleted r = new Contract.Request.AliyunUploadCompleted();
                        r.token = App.CurrentLogin.token;
                        r.lec_id = App.CurrentLogin.lec_id;
                        r.id = CurrentFile.UFile.ID;
                        r.key = CurrentFile.UFile.Key;
                        var b = await WebHelper.doPost<Contract.Request.AliyunUploadCompleted>(Config.Interface_liveWareUploadCompleted, r);
                    }
                    break;
                case UploadFileInfo.EnFileType.Live_Video:
                    {
                        Contract.Request.AliyunUploadCompleted r = new Contract.Request.AliyunUploadCompleted();
                        r.token = App.CurrentLogin.token;
                        r.lec_id = App.CurrentLogin.lec_id;
                        r.id = CurrentFile.UFile.ID;
                        r.key = CurrentFile.UFile.Key;
                        var b = await WebHelper.doPost<Contract.Request.AliyunUploadCompleted>(Config.Interface_liveVideoUploadAliyunComplete, r);
                    }
                    break;
            }
        }

        public void Dispose()
        {
            _worker.Dispose();
        }

        public UploadFileShowInfo AddUploadTask(string path, string accessKeyId, string accessKeySecret, string bucketName
            , string endPoint, string key, UploadFileInfo.EnFileType type, string id)
        {
            UploadFileInfo info = new UploadFileInfo();
            info.FilePath = path;
            info.AccessKeyId = accessKeyId;
            info.AccessKeySecret = accessKeySecret;
            info.BucketName = bucketName;
            info.EndPoint = endPoint;
            info.Key = key;
            info.ID = id;
            info.Type = type;
            var s = new UploadFileShowInfo(info);
            lock (Files)
            {
                Files.Add(s);
                File.WriteAllText(_path, Files.Select(T => T.UFile).ToJson());
            }

            return s;
        }
    }

    public class UploadFileShowInfo : ViewModelBase
    {
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
        public bool Cancel { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }


        public UploadFileInfo UFile { get; set; }
        public UploadFileShowInfo(UploadFileInfo info)
        {
            UFile = info;
            if (File.Exists(UFile.FilePath))
            {
                var fi = new FileInfo(UFile.FilePath);
                FileSize = fi.Length;
                FileName = fi.Name;
                FileSize = fi.Length;
            }
        }
    }

    public class UploadFileInfo
    {
        public string ID { get; set; }
        public EnFileType Type { get; set; }
        public string EndPoint { get; set; }
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string Key { get; set; }
        public string BucketName { get; set; }
        public string FilePath { get; set; }

        public enum EnFileType
        {
            Course_Video,
            Course_Document,
            Courseware,
            Live,
            Live_Video,
        }
    }
}
