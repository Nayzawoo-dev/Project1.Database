using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Project1.Database.Models;

namespace Project1.Domain.Features
{
    public class Personservices
    {
        private readonly AppDbContext _dbcontext;

        public Personservices(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public ResponseModels GetPersons()
        {
            var list = _dbcontext.TblWindows.ToList();
            var result = new ResponseModels(true, "All Data", list);
            return result;
        }

        public ResponseModels GetPersons(int id)
        {
            var list = _dbcontext.TblWindows.Where(x => x.Id == id).FirstOrDefault();
            if (list == null)
            {
                var result = new ResponseModels(false, "Person Not Found");
            }
            var res = new ResponseModels(true, "Person Found", list);
            return res;
        }

        public ResponseModels CreatePerson(TblWindow window)
        {
            var res = _dbcontext.TblWindows.Add(window);
            _dbcontext.SaveChanges();
            var result = new ResponseModels(true, "Create Successful", res);
            return result;
        }

        public ResponseModels UpdateCreatePerson(TblWindow window, int id)
        {
            var list = _dbcontext.TblWindows.Where(x => x.Id == id).FirstOrDefault();
            if (list == null)
            {
                if(window.UserName.isNull() && window.Password.isNull())
                {
                    return new ResponseModels(false, "No Field To Post");
                }
                _dbcontext.TblWindows.Add(window);
                _dbcontext.SaveChanges();
                return new ResponseModels(true, "Post Complete",list);
            }
            if (window.UserName.isNull() && window.Password.isNull())
            {
                return new ResponseModels(true, "No Field To Update");
            }
            list.UserName = window.UserName;
            list.Password = window.Password;
            _dbcontext.TblWindows.Add(window);
            _dbcontext.SaveChanges();
            return new ResponseModels(false, "Update Complete",list);

        }
    }
}
