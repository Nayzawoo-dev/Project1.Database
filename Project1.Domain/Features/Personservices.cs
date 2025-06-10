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
            try
            {
                var list = _dbcontext.TblWindows.ToList();
                var result = new ResponseModels(true, "All Data", list);
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseModels(false, ex.ToString());
            }
        }

        public ResponseModels GetPersons(int id)
        {
            try
            {
                var list = _dbcontext.TblWindows.Where(x => x.Id == id).FirstOrDefault();
                if (list == null)
                {
                    var result = new ResponseModels(false, "Person Not Found");
                }
                var res = new ResponseModels(true, "Person Found", list);
                return res;
            }
            catch (Exception ex)
            {
                return new ResponseModels(false, ex.ToString());


            }
        }

        public ResponseModels GetPersons(int pageNo, int pageSize)
        {
            try
            {
                var list = _dbcontext.TblWindows.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                return new ResponseModels(true, "Person List", list);
            }
            catch (Exception ex)
            {
                return new ResponseModels(false, ex.ToString());

            }
        }

        public ResponseModels PostPerson(TblWindow window)
        {
            try
            {
                var res = _dbcontext.TblWindows.Add(window);
                _dbcontext.SaveChanges();
                return new ResponseModels(true, "Post Successful");
            }
            catch (Exception ex)
            {
                return new ResponseModels(false, ex.ToString());
            }
        }

        public ResponseModels UpdateCreatePerson(TblWindow window, int id)
        {
            try
            {
                var list = _dbcontext.TblWindows.Where(x => x.Id == id).FirstOrDefault();
                if (list == null)
                {
                    if (window.UserName.isNull() && window.Password.isNull())
                    {
                        return new ResponseModels(false, "No Field To Post");
                    }
                    _dbcontext.TblWindows.Add(window);
                    _dbcontext.SaveChanges();
                    return new ResponseModels(true, "Post Complete", list);
                }
                if (window.UserName.isNull() && window.Password.isNull())
                {
                    return new ResponseModels(true, "No Field To Update");
                }
                list.UserName = window.UserName;
                list.Password = window.Password;
                _dbcontext.SaveChanges();
                return new ResponseModels(true, "Update Complete", list);

            }
            catch (Exception ex)
            {
                return new ResponseModels(false, ex.ToString());
            }

        }

        public ResponseModels UpdatePerson(TblWindow window, int id)
        {
            try
            {
                var list = _dbcontext.TblWindows.Where(x => x.Id == id).FirstOrDefault();
                if (list == null)
                {
                    return new ResponseModels(false, "Person Not Found");
                }
                list.UserName = window.UserName;
                list.Password = window.Password;
                _dbcontext.SaveChanges();
                return new ResponseModels(true, "Update Successful", list);

            }
            catch (Exception ex)
            {
                return new ResponseModels(false, ex.ToString());
            }

        }

        public ResponseModels DeletePerson(int id)
        {
            try
            {
                var list = _dbcontext.TblWindows.Where(x => x.Id == id).FirstOrDefault();
                if (list == null)
                {
                    return new ResponseModels(false, "Person Not Found");
                }
                var result = _dbcontext.TblWindows.Remove(list);
                _dbcontext.SaveChanges();
                return new ResponseModels(true, "Delete Successful");

            }
            catch (Exception ex)
            {
                return new ResponseModels(false, ex.ToString());
            }
        }
    }
}
