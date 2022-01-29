using STG.DTO.Payment;
using STG.DTO.UserDTO;
using STG.Entities;
using STG.ViewModels.Admin;
using STG.ViewModels.Lesson;
using STG.ViewModels.Package;
using STG.ViewModels.PackageChat;
using STG.ViewModels.Payment;
using STG.ViewModels.Statement;
using STG.ViewModels.TeacherViewModel;
using STG.ViewModels.User;
using STG.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.ViewModels
{
    public class JsonAnswerStatus
    {
        public string status { get; }
        public string errors { get; }
        public STG.Entities.User user { get; set; }
        public int forget_id { get; set; }

        public Teacher teacher { get; }
        public List<TeacherPreviewViewModel> teacherLites { get; }
        public ListTeacherCuratorPreviewsViewModel curatorPreviews { get; }
        public List<TeacherCuratorChooseViewModel> curatorChooseViewModels { get; }

        public StyleLiteViewModel style { get; }
        public LessonTypeLiteViewModel lessonType { get; }
        public List<VideoSectionViewModel> videosectionList { get; set; }

        public List<PackageDayViewModel> package_day_list { get; set; }
        public List<LessonMicroViewModel> lessonList { get; set; }

        public PaymentLiteViewModel payment { get; set; }
        public TinkoffGetStateDTO paymentGetState { get; set; }

        public UserSearchViewModel result { get; set; }
        public AdminSearchViewModel result_admins { get; set; }

        public AdminEditViewModel admin { get; set; }
        public UserEditViewModel userdata { get; set; }
        public UserProfileViewModel userProfile { get; set; }

        public List<PackageChatMessageViewModel> packageChatMessages { get; set; }
        public UserPassingPackageViewModel user_passing_package { get; set; }

        public StatementsSearchViewModel statementsSearchViewModel { get; set; }
        public StatementEditViewModel statement_edit { get; set; }

        public List<LessonLiteViewModel> lessons { get; }
        public LessonVideoViewModel lesson { get; }
        public TeacherIndexModalViewModel teacherModal { get; }
        public LessonHomeworkViewModel lessonHomework { get; }
        public LessonTeaserViewModel lessonTeaserViewModel { get; }
        public LessonBuyViewModel lessonBuyViewModel { get; }

        public List<PackagePreviewViewModel> packagePreviews { get; }
        public PackageInfoViewModel packageInfo { get; }
        public PackageBuyViewModel packageBuy { get; }


        public JsonAnswerStatus(string status, string errors)
        {
            this.status = status;
            this.errors = errors;
        }
        public JsonAnswerStatus(string status, string errors, STG.Entities.User user)
        {
            this.status = status;
            this.errors = errors;
            this.user = user;
        }

        public JsonAnswerStatus(string status, string errors, int forget_id) : this(status, errors)
        {
            this.forget_id = forget_id;
        }

        public JsonAnswerStatus(string status, string errors, Teacher teacher)
        {
            this.status = status;
            this.errors = errors;
            this.teacher = teacher;
        }

        public JsonAnswerStatus(string status, string errors, StyleLiteViewModel style)
        {
            this.status = status;
            this.errors = errors;
            this.style = style;
        }

        public JsonAnswerStatus(string status, string errors, LessonTypeLiteViewModel lessonType)
        {
            this.status = status;
            this.errors = errors;
            this.lessonType = lessonType;
        }

        public JsonAnswerStatus(string status, string errors, List<VideoSectionViewModel> videosectionList)
        {
            this.status = status;
            this.errors = errors;
            this.videosectionList = videosectionList;
        }

        public JsonAnswerStatus(string status, string errors, List<PackageDayViewModel> package_day_list)
        {
            this.status = status;
            this.errors = errors;
            this.package_day_list = package_day_list;
        }

        public JsonAnswerStatus(string status, string errors, PaymentLiteViewModel payment) : this(status, errors)
        {
            this.payment = payment;
        }

        public JsonAnswerStatus(string status, string errors, TinkoffGetStateDTO paymentGetState) : this(status, errors)
        {
            this.paymentGetState = paymentGetState;
        }

        public JsonAnswerStatus(string status, string errors, UserSearchViewModel result) : this(status, errors)
        {
            this.result = result;
        }

        public JsonAnswerStatus(string status, string errors, AdminSearchViewModel result_admins) : this(status, errors)
        {
            this.result_admins = result_admins;
        }

        public JsonAnswerStatus(string status, string errors, AdminEditViewModel admin) : this(status, errors)
        {
            this.admin = admin;
        }

        public JsonAnswerStatus(string status, string errors, UserEditViewModel userdata) : this(status, errors)
        {
            this.userdata = userdata;
        }

        public JsonAnswerStatus(string status, string errors, UserProfileViewModel userProfile) : this(status, errors)
        {
            this.userProfile = userProfile;
        }

        public JsonAnswerStatus(string status, string errors, List<PackageChatMessageViewModel> packageChatMessages) : this(status, errors)
        {
            this.packageChatMessages = packageChatMessages;
        }

        public JsonAnswerStatus(string status, string errors, UserPassingPackageViewModel user_passing_package) : this(status, errors)
        {
            this.user_passing_package = user_passing_package;
        }

        public JsonAnswerStatus(string status, string errors, StatementsSearchViewModel statementsSearchViewModel) : this(status, errors)
        {
            this.statementsSearchViewModel = statementsSearchViewModel;
        }

        public JsonAnswerStatus(string status, string errors, StatementEditViewModel statement_edit) : this(status, errors)
        {
            this.statement_edit = statement_edit;
        }

        public JsonAnswerStatus(string status, string errors, List<LessonLiteViewModel> lessons) : this(status, errors)
        {
            this.lessons = lessons;
        }

        public JsonAnswerStatus(string status, string errors, LessonVideoViewModel lesson) : this(status, errors)
        {
            this.lesson = lesson;
        }

        public JsonAnswerStatus(string status, string errors, TeacherIndexModalViewModel teacherModal) : this(status, errors)
        {
            this.teacherModal = teacherModal;
        }

        public JsonAnswerStatus(string status, string errors, LessonHomeworkViewModel lessonHomework) : this(status, errors)
        {
            this.lessonHomework = lessonHomework;
        }

        public JsonAnswerStatus(string status, string errors, LessonTeaserViewModel lessonTeaserViewModel) : this(status, errors)
        {
            this.lessonTeaserViewModel = lessonTeaserViewModel;
        }

        public JsonAnswerStatus(string status, string errors, LessonBuyViewModel lessonBuyViewModel) : this(status, errors)
        {
            this.lessonBuyViewModel = lessonBuyViewModel;
        }

        public JsonAnswerStatus(string status, string errors, List<TeacherPreviewViewModel> teacherLites) : this(status, errors)
        {
            this.teacherLites = teacherLites;
        }

        public JsonAnswerStatus(string status, string errors, ListTeacherCuratorPreviewsViewModel curatorPreviews) : this(status, errors)
        {
            this.curatorPreviews = curatorPreviews;
        }

        public JsonAnswerStatus(string status, string errors, List<TeacherCuratorChooseViewModel> curatorChooseViewModels) : this(status, errors)
        {
            this.curatorChooseViewModels = curatorChooseViewModels;
        }

        public JsonAnswerStatus(string status, string errors, List<PackagePreviewViewModel> packagePreviews) : this(status, errors)
        {
            this.packagePreviews = packagePreviews;
        }

        public JsonAnswerStatus(string status, string errors, PackageInfoViewModel packageInfo) : this(status, errors)
        {
            this.packageInfo = packageInfo;
        }

        public JsonAnswerStatus(string status, string errors, PackageBuyViewModel packageBuy) : this(status, errors)
        {
            this.packageBuy = packageBuy;
        }
    }
}
