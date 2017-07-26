using Caliburn.Micro;
using NBSchool.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSchool.WPFClient.ViewModels
{
    [Export()]
    public class TeacherIndexSummaryInfoViewModel : Conductor<IRendering>.Collection.OneActive, IRendering
    {
        public TeacherIndexSummaryInfoViewModel()
        {
            this.UserName = "aaa";
            this.Sex = "男";
            this.Age = "24";
            this.TeacherID = "1234";
            this.TeacherType = "高级";
            this.GoodDomain = "C#";

            this.WaittingCheckCount = 5;
            this.WaittingLiveCount = 2;
            this.WaittingRewardCount = 10;

            this.Messages = new ObservableCollection<MessageInfo>(new List<MessageInfo> {
                new MessageInfo { Title = "11111111111111111", Date="[2017-07-23]" },
                new MessageInfo { Title = "222222222222222", Date="[2017-07-22]" },
                new MessageInfo { Title = "333333333333333333333333", Date="[2017-07-21]" },
                new MessageInfo { Title = "444444444444444444", Date="[2017-07-20]" },
            });
        }

        string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { this.Set(ref _UserName, value); }
        }
        string _Sex;
        public string Sex
        {
            get { return _Sex; }
            set { this.Set(ref _Sex, value); }
        }
        string _Age;
        public string Age
        {
            get { return _Age; }
            set { this.Set(ref _Age, value); }
        }
        string _TeacherID;
        public string TeacherID
        {
            get { return _TeacherID; }
            set { this.Set(ref _TeacherID, value); }
        }
        string _TeacherType;
        public string TeacherType
        {
            get { return _TeacherType; }
            set { this.Set(ref _TeacherType, value); }
        }
        string _GoodDomain;
        public string GoodDomain
        {
            get { return _GoodDomain; }
            set { this.Set(ref _GoodDomain, value); }
        }

        int _WaittingCheckCount;
        public int WaittingCheckCount
        {
            get { return _WaittingCheckCount; }
            set { this.Set(ref _WaittingCheckCount, value); }
        }
        int _WaittingLiveCount;
        public int WaittingLiveCount
        {
            get { return _WaittingLiveCount; }
            set { this.Set(ref _WaittingLiveCount, value); }
        }
        int _WaittingRewardCount;
        public int WaittingRewardCount
        {
            get { return _WaittingRewardCount; }
            set { this.Set(ref _WaittingRewardCount, value); }
        }
        ObservableCollection<MessageInfo> _Messages;
        public ObservableCollection<MessageInfo> Messages
        {
            get { return _Messages; }
            set { this.Set(ref _Messages, value); }
        }
    }
    public class MessageInfo : PropertyChangedBase
    {
        string _Title;
        public string Title
        {
            get { return _Title; }
            set { this.Set(ref _Title, value); }
        }
        string _Date;
        public string Date
        {
            get { return _Date; }
            set { this.Set(ref _Date, value); }
        }
    }
}
