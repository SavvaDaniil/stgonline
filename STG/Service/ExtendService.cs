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

        public Extend findById(int id)
        {
            return _dbc.Extends
                .Where(p => p.id == id)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }

        public IEnumerable<Extend> listAllNotFinished()
        {
            return _dbc.Extends
                .Where(p => p.status == 0)
                .Include(p => p.user)
                .OrderBy(p => p.id)
                .ToList();
        }

        public IEnumerable<Extend> listAllNotFinishedByUser(User user)
        {
            return _dbc.Extends
                .Where(p => p.status == 0 && p.user == user)
                .Include(p => p.user)
                .OrderBy(p => p.id)
                .ToList();
        }

        public Extend findByIdNotFinished(int id)
        {
            return _dbc.Extends
                .Where(p => p.id == id && p.status == 0)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }

        public Extend find(User user, int id_of_purchase_subscription)
        {
            return _dbc.Extends
                .Where(p => p.id_of_purchase_subscription == id_of_purchase_subscription && p.user == user)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }

        public Extend findNotFinished(User user, int id_of_purchase_subscription)
        {
            return _dbc.Extends
                .Where(p => p.id_of_purchase_subscription == id_of_purchase_subscription && p.user == user && p.status == 0)
                .Include(p => p.user)
                .OrderByDescending(p => p.id)
                .FirstOrDefault();
        }

        public Extend add(User user, int id_of_purchase_subscription)
        {
            Extend extend = new Extend();
            extend.user = user;
            extend.status = 0;
            extend.success = 0;
            extend.id_of_purchase_subscription = id_of_purchase_subscription;

            extend.date_of_add = DateTime.Now;

            _dbc.Extends.Add(extend);
            _dbc.SaveChanges();

            return extend;
        }

        public bool updateStartThread(int id, DateTime start, DateTime must_be_finish)
        {
            Extend extend = findById(id);
            if (extend == null) return false;

            extend.date_of_thread_start = start;
            extend.date_of_thread_must_be_finished = must_be_finish;

            _dbc.SaveChanges();

            return true;
        }

        public bool updateStartThread(Extend extend, DateTime start, DateTime must_be_finish)
        {
            extend.date_of_thread_start = start;
            extend.date_of_thread_must_be_finished = must_be_finish;

            _dbc.SaveChanges();

            return true;
        }

        public bool updateFinishThread(Extend extend, DateTime finish)
        {
            extend.status = 1;
            extend.date_of_thread_finished = finish;

            _dbc.SaveChanges();

            return true;
        }

        public bool updateResultSuccessfull(Extend extend)
        {
            extend.success = 1;
            _dbc.SaveChanges();
            return true;
        }


        public bool canselExtendsByUser(User user)
        {
            IEnumerable<Extend> extends = listAllNotFinishedByUser(user);
            foreach (Extend extend in extends)
            {
                _dbc.Extends.Remove(extend);
            }
            return true;
        }


        public bool delete(int id)
        {
            Extend extend = findById(id);
            if (extend == null) return false;
            _dbc.Extends.Remove(extend);
            return true;
        }
    }
}
