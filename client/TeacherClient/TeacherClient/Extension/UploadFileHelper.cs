using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace TeacherClient
{
    public class UploadFileHelper
    {

    }
    public class UploadFiles : ViewModelBase
    {
        public UploadFiles()
        {
            //BackgroundWorker
        }

        int _Per;
        public int Per
        {
            get { return _Per; }
            set
            {
                _Per = value;
                this.OnPropertyChanged("Per");
            }
        }
        string _Speed;
        public string Speed
        {
            get { return _Speed; }
            set
            {
                _Speed = value;
                this.OnPropertyChanged("Speed");
            }
        }
        ObservableCollection<UploadFile> _Files;
        public ObservableCollection<UploadFile> Files
        {
            get { return _Files; }
            set
            {
                _Files = value;
                this.OnPropertyChanged("Files");
            }
        }
    }
    public class UploadFile : ViewModelBase
    {
        string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                this.OnPropertyChanged("Name");
            }
        }
        string _Extension;
        public string Extension
        {
            get { return _Extension; }
            set
            {
                _Extension = value;
                this.OnPropertyChanged("Extension");
            }
        }
        public int FileSize { get; set; }
        int _isUpload = 0;
        public void SetPer(int add)
        {
            _isUpload += add;
            Per1 = string.Format("{0}MB/{1}MB", _isUpload / 1024 / 1024, FileSize / 1024 / 1024);
            Per2 = _isUpload / FileSize;
            this.OnPropertyChanged("Per1");
            this.OnPropertyChanged("Per2");
        }

        string _Per1;
        public string Per1
        {
            get { return _Per1; }
            set
            {
                _Per1 = value;
                this.OnPropertyChanged("Per1");
            }
        }
        int _Per2;
        public int Per2
        {
            get { return _Per2; }
            set
            {
                _Per2 = value;
                this.OnPropertyChanged("Per2");
            }
        }
        bool _IsUping;
        public bool IsUping
        {
            get { return _IsUping; }
            set
            {
                _IsUping = value;
                this.OnPropertyChanged("IsUping");
            }
        }
    }
}
