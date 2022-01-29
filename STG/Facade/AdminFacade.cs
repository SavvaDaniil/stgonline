using Microsoft.AspNetCore.Http;
using STG.Data;
using STG.DTO.Admin;
using STG.Entities;
using STG.Service;
using STG.ViewModels;
using STG.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STG.Facade
{
    public class AdminFacade
    {
        private ApplicationDbContext _dbc;
        public AdminFacade(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        private string[] listOfPanels = new string[] {
            "panel_users",
            "panel_statements",
            "panel_mentoring",
            "panel_homeworks",
            "panel_lessons",
            "panel_packages",
            "panel_videos",
            "panel_teachers",
            "panel_styles",
            "panel_lessontypes",
            "panel_admins"
        };

        public async Task<JsonAnswerStatus> add(AdminNewDTO adminNewDTO)
        {
            AdminService adminService = new AdminService(_dbc);
            Admin admin = await adminService.add(adminNewDTO.username);
            if (admin == null) return new JsonAnswerStatus("error", "unknown");

            return new JsonAnswerStatus("success", null);
        }
        public async Task<JsonAnswerStatus> delete(AdminSearchIdDTO adminSearchIdDTO)
        {
            AdminService adminService = new AdminService(_dbc);

            return (await adminService.delete(adminSearchIdDTO.id)
                ? new JsonAnswerStatus("success", null)
                : new JsonAnswerStatus("error", "unknown")
            );
        }

        public async Task<JsonAnswerViewModel> update(HttpContext httpContext, AdminDTO adminDTO)
        {
            Admin admin = await this.getCurrentOrNull(httpContext);
            if (admin == null) return new JsonAnswerViewModel("error","not_found");

            AdminService adminService = new AdminService(_dbc);

            return await adminService.update(admin, adminDTO);
        }


        public async Task<JsonAnswerStatus> edit(AdminEditDTO adminEditDTO)
        {
            AdminService adminService = new AdminService(_dbc);
            return await adminService.update(adminEditDTO);
        }

        public async Task<Admin> login(AdminLoginDTO adminLoginDTO)
        {
            AdminService adminService = new AdminService(_dbc);
            Admin admin = await adminService.findByUsername(adminLoginDTO.username);
            if (admin != null && BCrypt.Net.BCrypt.Verify(adminLoginDTO.password, admin.Password))
            {
                return admin;
            }
            return null;
        }

        public async Task<AdminProfileViewModel> getCurrentProfile(HttpContext httpContext)
        {
            AdminService adminService = new AdminService(_dbc);

            Admin admin = await this.getCurrentOrNull(httpContext);
            if (admin == null) return null;

            return new AdminProfileViewModel(
                admin.Id,
                admin.Username,
                admin.position
            );
        }

        public async Task<AdminAuthorizeViewModel> getAuthorizeViewModel(HttpContext httpContext, string menuActive = null)
        {
            Admin admin = await getCurrentOrNull(httpContext);
            if (admin == null) return new AdminAuthorizeViewModel("", 0, null, 0, 0);
            List<string> listOfAvailablePanels = new List<string>();
            if (admin.panel_users >= 1) listOfAvailablePanels.Add("panel_users");
            if (admin.panel_statements >= 1) listOfAvailablePanels.Add("panel_statements");
            if (admin.panel_homeworks >= 1) listOfAvailablePanels.Add("panel_mentoring");
            if (admin.panel_homeworks >= 1) listOfAvailablePanels.Add("panel_homeworks");
            if (admin.panel_lessons >= 1) listOfAvailablePanels.Add("panel_lessons");
            if (admin.panel_packages >= 1) listOfAvailablePanels.Add("panel_packages");
            if (admin.panel_videos >= 1) listOfAvailablePanels.Add("panel_videos");
            if (admin.panel_teachers >= 1) listOfAvailablePanels.Add("panel_teachers");
            if (admin.panel_styles >= 1) listOfAvailablePanels.Add("panel_styles");
            if (admin.panel_lessontypes >= 1) listOfAvailablePanels.Add("panel_lessontypes");

            HomeworkService homeworkService = new HomeworkService(_dbc);
            PackageChatService packageChatService = new PackageChatService(_dbc);
            StatementService statementService = new StatementService(_dbc);
            int countUnreadPackeChatsAndHomeworks = await homeworkService.getCountAllNotReadedByAnyTeacher() + await packageChatService.getCountNotReadedByAnyTeacher();
            int countInactivatedStatements = await statementService.getCountAllInactived();

            return new AdminAuthorizeViewModel(menuActive, admin.level, listOfAvailablePanels, countUnreadPackeChatsAndHomeworks, countInactivatedStatements);
        }

        public async Task<Admin> getCurrentOrNull(HttpContext httpContext)
        {
            AdminService adminService = new AdminService(_dbc);
            if (httpContext.User.FindFirstValue(ClaimTypes.Role) != "Admin") return null;
            return await adminService.findById(int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }


        public async Task<AdminEditViewModel> get(int id)
        {
            AdminService adminService = new AdminService(_dbc);
            Admin admin = await adminService.findById(id);
            if (admin == null) return null;

            List<(string, int, string)> accesses_to_panels = new List<(string, int, string)>();
            accesses_to_panels.Add(("panel_users", admin.panel_users, "Пользователи"));
            accesses_to_panels.Add(("panel_statements", admin.panel_statements, "Заявки на наставничество"));
            accesses_to_panels.Add(("panel_mentoring", admin.panel_mentoring, "Наставники"));
            accesses_to_panels.Add(("panel_homeworks", admin.panel_homeworks, "Домашние задания"));
            accesses_to_panels.Add(("panel_lessons", admin.panel_lessons, "Уроки"));
            accesses_to_panels.Add(("panel_packages", admin.panel_packages, "Программы"));
            accesses_to_panels.Add(("panel_videos", admin.panel_videos, "Видео"));
            accesses_to_panels.Add(("panel_teachers", admin.panel_teachers, "Преподаватели"));
            accesses_to_panels.Add(("panel_styles", admin.panel_styles, "Стили"));
            accesses_to_panels.Add(("panel_lessontypes", admin.panel_lessontypes, "Типы уроков"));
            accesses_to_panels.Add(("panel_admins", admin.panel_admins, "Администраторы"));

            return new AdminEditViewModel(
                admin.Id,
                admin.Username,
                admin.active,
                admin.position,
                accesses_to_panels
            );
        }

        public async Task<JsonAnswerStatus> search(AdminSearchDTO adminSearchDTO)
        {
            AdminService adminService = new AdminService(_dbc);
            List<Admin> adminsSearchResult = await adminService.searchAdmins(adminSearchDTO);
            int searchCount = adminService.searchCount(adminSearchDTO);

            List<AdminSearchPreviewViewModel> adminSearchPreviewViewModels = new List<AdminSearchPreviewViewModel>();
            foreach (Admin admin in adminsSearchResult)
            {
                adminSearchPreviewViewModels.Add(new AdminSearchPreviewViewModel(
                    admin.Id,
                    admin.Username,
                    admin.position,
                    admin.active
                    )
                );
            }

            return new JsonAnswerStatus("success", null, new AdminSearchViewModel(adminSearchPreviewViewModels, searchCount));
        }


    }
}
