using Caliburn.Micro;
using NBSchool.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSchool.WPFClient.ViewModels
{
    [Export()]
    public class TeacherIndexTeacherInfoViewModel : Conductor<IRendering>.Collection.OneActive, IRendering
    {
        public TeacherIndexTeacherInfoViewModel()
        {
            ShowUserInfo = true;
        }

        bool _ShowUserInfo;
        public bool ShowUserInfo
        {
            get { return _ShowUserInfo; }
            set { this.Set(ref _ShowUserInfo, value); }
        }
        bool _ShowAccountBinding;
        public bool ShowAccountBinding
        {
            get { return _ShowAccountBinding; }
            set { this.Set(ref _ShowAccountBinding, value); }
        }

        string _Account;
        public string Account
        {
            get { return _Account; }
            set { this.Set(ref _Account, value); }
        }

        string _TeacherID;
        public string TeacherID
        {
            get { return _TeacherID; }
            set { this.Set(ref _TeacherID, value); }
        }
        string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.Set(ref _Name, value); }
        }
        string _Sex;
        public string Sex
        {
            get { return _Sex; }
            set { this.Set(ref _Sex, value); }
        }
        int _Age;
        public int Age
        {
            get { return _Age; }
            set { this.Set(ref _Age, value); }
        }
        string _Month;
        public string Month
        {
            get { return _Month; }
            set { this.Set(ref _Month, value); }
        }
        string _Day;
        public string Day
        {
            get { return _Day; }
            set { this.Set(ref _Day, value); }
        }
        List<string> _Months;
        public List<string> Months
        {
            get { return _Months; }
            set { this.Set(ref _Months, value); }
        }
        List<string> _Days;
        public List<string> Days
        {
            get { return _Days; }
            set { this.Set(ref _Days, value); }
        }

        string _TeacherType;
        public string TeacherType
        {
            get { return _TeacherType; }
            set { this.Set(ref _TeacherType, value); }
        }
        List<string> _TeacherTypes;
        public List<string> TeacherTypes
        {
            get { return _TeacherTypes; }
            set { this.Set(ref _TeacherTypes, value); }
        }
        string _GoodDomain;
        public string GoodDomain
        {
            get { return _GoodDomain; }
            set { this.Set(ref _GoodDomain, value); }
        }
        List<string> _Domains;
        public List<string> Domains
        {
            get { return _Domains; }
            set { this.Set(ref _Domains, value); }
        }

        string _Province;
        public string Province
        {
            get { return _Province; }
            set { this.Set(ref _Province, value); }
        }
        string _City;
        public string City
        {
            get { return _City; }
            set { this.Set(ref _City, value); }
        }
        string _County;
        public string County
        {
            get { return _County; }
            set { this.Set(ref _County, value); }
        }
        List<string> _Provinces;
        public List<string> Provinces
        {
            get { return _Provinces; }
            set { this.Set(ref _Provinces, value); }
        }
        List<string> _Cities;
        public List<string> Cities
        {
            get { return _Cities; }
            set { this.Set(ref _Cities, value); }
        }
        List<string> _Counties;
        public List<string> Counties
        {
            get { return _Counties; }
            set { this.Set(ref _Counties, value); }
        }
        string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set { this.Set(ref _Phone, value); }
        }
        string _Weixin;
        public string Weixin
        {
            get { return _Weixin; }
            set { this.Set(ref _Weixin, value); }
        }
        string _Email;
        public string Email
        {
            get { return _Email; }
            set { this.Set(ref _Email, value); }
        }
        string _Job;
        public string Job
        {
            get { return _Job; }
            set { this.Set(ref _Job, value); }
        }


        bool _ShowPhoneFirst = true;
        public bool ShowPhoneFirst
        {
            get { return _ShowPhoneFirst; }
            set { this.Set(ref _ShowPhoneFirst, value); }
        }
        bool _ShowPhoneSecond;
        public bool ShowPhoneSecond
        {
            get { return _ShowPhoneSecond; }
            set { this.Set(ref _ShowPhoneSecond, value); }
        }
        bool _ShowPhoneThird;
        public bool ShowPhoneThird
        {
            get { return _ShowPhoneThird; }
            set { this.Set(ref _ShowPhoneThird, value); }
        }
        bool _ShowEmailFirst = true;
        public bool ShowEmailFirst
        {
            get { return _ShowEmailFirst; }
            set { this.Set(ref _ShowEmailFirst, value); }
        }
        bool _ShowEmailSecond;
        public bool ShowEmailSecond
        {
            get { return _ShowEmailSecond; }
            set { this.Set(ref _ShowEmailSecond, value); }
        }
        bool _ShowEmailThird;
        public bool ShowEmailThird
        {
            get { return _ShowEmailThird; }
            set { this.Set(ref _ShowEmailThird, value); }
        }
        string _OldPhone;
        public string OldPhone
        {
            get { return _OldPhone; }
            set { this.Set(ref _OldPhone, value); }
        }
        string _OldPhoneCode;
        public string OldPhoneCode
        {
            get { return _OldPhoneCode; }
            set { this.Set(ref _OldPhoneCode, value); }
        }
        string _NewPhone;
        public string NewPhone
        {
            get { return _NewPhone; }
            set { this.Set(ref _NewPhone, value); }
        }
        string _NewPhoneCode;
        public string NewPhoneCode
        {
            get { return _NewPhoneCode; }
            set { this.Set(ref _NewPhoneCode, value); }
        }
        string _OldEmail;
        public string OldEmail
        {
            get { return _OldEmail; }
            set { this.Set(ref _OldEmail, value); }
        }
        string _OldEmailCode;
        public string OldEmailCode
        {
            get { return _OldEmailCode; }
            set { this.Set(ref _OldEmailCode, value); }
        }
        string _NewEmail;
        public string NewEmail
        {
            get { return _NewEmail; }
            set { this.Set(ref _NewEmail, value); }
        }
        string _NewEmailCode;
        public string NewEmailCode
        {
            get { return _NewEmailCode; }
            set { this.Set(ref _NewEmailCode, value); }
        }


        public void PhoneToNext()
        {
            ShowPhoneFirst = false;
            ShowPhoneSecond = true;
            ShowPhoneThird = false;
        }
        public void PhoneSubmmit()
        {
            ShowPhoneFirst = false;
            ShowPhoneSecond = false;
            ShowPhoneThird = true;
        }
        public void PhoneOK()
        {
            ShowPhoneFirst = true;
            ShowPhoneSecond = false;
            ShowPhoneThird = false;
        }

        public void EmailToNext()
        {
            ShowEmailFirst = false;
            ShowEmailSecond = true;
            ShowEmailThird = false;
        }
        public void EmailSubmmit()
        {
            ShowEmailFirst = false;
            ShowEmailSecond = false;
            ShowEmailThird = true;
        }
        public void EmailOK()
        {
            ShowEmailFirst = true;
            ShowEmailSecond = false;
            ShowEmailThird = false;
        }
    }
}
