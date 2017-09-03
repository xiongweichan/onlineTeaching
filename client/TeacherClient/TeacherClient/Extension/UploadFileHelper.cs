﻿using System;
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

namespace TeacherClient
{
    public class UploadFileHelper
    {
        public static readonly UploadFileHelper Instance = new UploadFileHelper();
        public static readonly string _path = AppDomain.CurrentDomain.BaseDirectory + "uplodingfiles.12345";

        public void Run()
        {
            if(File.Exists(_path))
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

        private void Item_Complated(object sender, EventArgs e)
        {
            lock (_WaittingFiles)
            {
                _WaittingFiles.Remove(sender as FileModel);
                File.WriteAllText(_path, _WaittingFiles.ToJson());
            }
        }

        public FileModel Add(string path, EnFileType type)
        {
            if (_WaittingFiles.Any(T => T.LocalFile == path))
                return _WaittingFiles.FirstOrDefault(T=>T.LocalFile == path);

            FileModel model = new FileModel();
            FileInfo info = new FileInfo(path);
            model.Type = type;
            model.Extension = info.Extension;
            model.FileName = info.Name;
            model.FileSize = info.Length;
            model.LocalFile = path;
            model.RecordFile = string.Format("{0}\\{1}\\{2}.12345", CacheHelper.CacheFilePath, model.ID, info.Name);
            _WaittingFiles.Add(model);
            model.Started += Model_Started;
            model.Complated += Item_Complated;
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

            string _Link;
            public string Link
            {
                get { return _Link; }
                set
                {
                    _Link = value;
                    this.OnPropertyChanged("Link");
                }
            }

            string _Hash;
            public string Hash
            {
                get { return _Hash; }
                set
                {
                    _Hash = value;
                    this.OnPropertyChanged("Hash");
                }
            }

            public async void Start()
            {
                if (!string.IsNullOrWhiteSpace(ID) && !string.IsNullOrWhiteSpace(Token) && File.Exists(LocalFile))
                {
                    if (Started != null)
                        Started(this, new EventArgs());
                    // 包含两个参数，并且都有默认值
                    // 参数1(bool): uploadFromCDN是否从CDN加速上传，默认否
                    // 参数2(enum): chunkUnit上传分片大小，可选值128KB,256KB,512KB,1024KB,2048KB,4096KB
                    ResumableUploader ru = new ResumableUploader(false, ChunkUnit.U1024K);
                    //最大尝试次数(有效值1~20)，在上传过程中(如mkblk或者bput操作)如果发生错误，它将自动重试，如果没有错误则无需重试
                    int maxTry = 10;
                    // 使用默认进度处理，使用自定义上传控制            
                    UploadProgressHandler upph = new UploadProgressHandler(UploadProgress);
                    UploadController upctl = new UploadController(UploadControl);
                    HttpResult result = await ru.UploadFileAsync(LocalFile, SaveKey, Token, RecordFile, maxTry, upph, upctl);
                    if (result.Code == 200)
                    {
                        dynamic dy = result.Text.FromJson<dynamic>();
                        this.Hash = dy.hash;
                        this.Link = dy.key;
                    }
                    else
                    {
                        Log.Error(result);
                        MessageWindow.Alter("提示", "上传失败");
                    }
                }                
                if (Complated != null)
                    Complated(this, new EventArgs());
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
            string _Per;
            public string Per
            {
                get { return _Per; }
                set
                {
                    _Per = value;
                    this.OnPropertyChanged("Per");
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
                // 这个函数只是作为一个演示，实际当中请根据需要来设置
                // 这个演示的实际效果是“走走停停”，也就是停一下又继续，如此重复直至上传结束
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
                Per = (uploadedBytes / totalBytes).ToString("p0");
            }
        }
        public enum EnFileType
        {
            Course,
            Courseware,
        }
    }
}
