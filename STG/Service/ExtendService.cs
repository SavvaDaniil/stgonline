using Microsoft.EntityFrameworkCore;
using STG.Data;
using STG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG.Service
{
    public class ExtendService
    {
        private ApplicationDbContext _dbc;
        public ExtendService(ApplicationDbContext dbc)
        {
            this._dbc = dbc;
        }

        public async Task<Extend> findById(int id)
        {
            return await _dbc.Extends
                .Where(p => p.id == id)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Extend>> listAllNotFinished()
        {
            return await _dbc.Extends
                .Where(p => p.status == 0)
                .Include(p => p.user)
                .OrderBy(p => p.id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Extend>> listAllNotFinishedByUser(User user)
        {
            return await _dbc.Extends
                .Where(p => p.status == 0 && p.user == user)
                .Include(p => p.user)
                .OrderBy(p => p.id)
                .ToListAsync();
        }

        public async Task<Extend> findByIdNotFinished(int id)
        {
            return await _dbc.Extends
                .Where(p => p.id == id && p.status == 0)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<Extend> find(User user, int id_of_purchase_subscription)
        {
            return await _dbc.Extends
                .Where(p => p.id_of_purchase_subscription == id_of_purchase_subscription && p.user == user)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<Extend> findNotFinished(User user, int id_of_purchase_subscription)
        {
            return await _dbc.Extends
                .Where(p => p.id_of_purchase_subscription == id_of_purchase_subscription && p.user == user && p.status == 0)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefaultAsync();
        }

        public async Task<Extend> add(User user, int id_of_purchase_subscription)
        {
            Extend extend = new Extend();
            extend.user = user;
            extend.status = 0;
            extend.success = 0;
            extend.id_of_purchase_subscription = id_of_purchase_subscription;

            extend.date_of_add = DateTime.Now;

            await _dbc.Extends.AddAsync(extend);
            await _dbc.SaveChangesAsync();

            return extend;
        }

        public async Task<bool> updateStartThread(int id, DateTime start, DateTime must_be_finish)
        {
            Extend extend = await findById(id);
            if (extend == null) return false;

            extend.date_of_thread_start = start;
            extend.date_of_thread_must_be_finished = must_be_finish;

            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> updateStartThread(Extend extend, DateTime start, DateTime must_be_finish)
        {
            extend.date_of_thread_start = start;
            extend.date_of_thread_must_be_finished = must_be_finish;

            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> updateFinishThread(Extend extend, DateTime finish)
        {
            extend.status = 1;
            extend.date_of_thread_finished = finish;

            await _dbc.SaveChangesAsync();

            return true;
        }

        public async Task<bool> updateResultSuccessfull(Extend extend)
        {
            extend.success = 1;
            await _dbc.SaveChangesAsync();
            return true;
        }


        public async Task<bool> canselExtendsByUser(User user)
        {
            IEnumerable<Extend> extends = await listAllNotFinishedByUser(user);
            foreach (Extend extend in extends)
            {
                _dbc.Extends.Remove(extend);
            }
            return true;
        }


        public async Task<bool> delete(int id)
        {
            Extend extend = await findById(id);
            if (extend == null) return false;
            _dbc.Extends.Remove(extend);
            return true;
        }
    }
}
