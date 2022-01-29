using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class PreUserWithAppointmentService
    {
        private ApplicationDbContext _dbc;
        public PreUserWithAppointmentService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<PreUserWithAppointment> findById(int id)
        {
            return await _dbc.PreUserWithAppointments
                .Include(p => p.region)
                .Where(p => p.id == id).FirstOrDefaultAsync();
        }

        public async Task<PreUserWithAppointment> add(
            string username,
            string firstname,
            string secondname,
            string instagram,
            DateTime? date_of_birthday,
            string password,
            int is_need_curator,
            Region region,
            int experience,
            int expectations,
            int expected_time_for_lessons,
            string idols,
            string link1,
            string link2,
            string link3,
            string listOfTeachers)
        {
            PreUserWithAppointment preUserWithAppointment = new PreUserWithAppointment();
            preUserWithAppointment.username = username;
            preUserWithAppointment.firstname = firstname;
            preUserWithAppointment.secondname = secondname;
            preUserWithAppointment.instagram = instagram;
            preUserWithAppointment.date_of_birthday = date_of_birthday;
            preUserWithAppointment.password = password;
            preUserWithAppointment.is_need_curator = is_need_curator;
            preUserWithAppointment.region = region;
            preUserWithAppointment.experience = experience;
            preUserWithAppointment.expectations = expectations;
            preUserWithAppointment.expected_time_for_lessons = expected_time_for_lessons;
            preUserWithAppointment.idols = idols;
            preUserWithAppointment.link1 = link1;
            preUserWithAppointment.link2 = link2;
            preUserWithAppointment.link3 = link3;
            preUserWithAppointment.listOfTeachers = listOfTeachers;
            preUserWithAppointment.dateOfAdd = DateTime.Now;

            await _dbc.PreUserWithAppointments.AddAsync(preUserWithAppointment);
            await _dbc.SaveChangesAsync();

            return preUserWithAppointment;
        }



        public async Task<bool> remove(int id)
        {
            PreUserWithAppointment preUserWithAppointment = await findById(id);
            if (preUserWithAppointment == null) return false;

            _dbc.PreUserWithAppointments.Remove(preUserWithAppointment);
            await _dbc.SaveChangesAsync();
            return true;
        }

        public async Task<bool> setActive(PreUserWithAppointment preUserWithAppointment)
        {
            preUserWithAppointment.status = 1;
            preUserWithAppointment.dateOfRegistration = DateTime.Now;
            await _dbc.SaveChangesAsync();
            return true;
        }
    }
}
