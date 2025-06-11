using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prj2Services.Models;
using Project2Database.Models;

namespace Prj2Services.Services
{
    public class BlogServices
    {
        private readonly AppDbContext _appDbContext;

        public BlogServices(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ResponseModel PostBlogHeader(TblBlogHeader header)
        {
            string str = File.ReadAllText("DreamDictionary.json");
            var models = JsonConvert.DeserializeObject<Rootobject>(str)!;
            foreach (var model in models.BlogHeader)
            {
                header.BlogId = model.BlogId;
                header.BlogTitle = model.BlogTitle;
                var res = _appDbContext.TblBlogHeaders.Add(header);
                _appDbContext.SaveChanges();
            }
            return new ResponseModel(true, "Post Success");
        }

        public ResponseModel PostBlogDetail(TblBlogDetail detail)
        {

            string str = File.ReadAllText("DreamDictionary.json");
            var res = JsonConvert.DeserializeObject<Rootobject>(str)!;
            foreach(var item in res.BlogDetail)
            {
                detail.BlogDetailId = item.BlogDetailId;
                detail.BlogId = item.BlogId;
                detail.BlogContent = item.BlogContent;
                var result = _appDbContext.TblBlogDetails.Add(detail);
                _appDbContext.SaveChanges();
            }
            return new ResponseModel(true, "Success");
       
        }


    }

    public class Rootobject
    {
        public Blogheader[] BlogHeader { get; set; }
        public Blogdetail[] BlogDetail { get; set; }
    }

    public class Blogheader
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
    }

    public class Blogdetail
    {
        public int BlogDetailId { get; set; }
        public int BlogId { get; set; }
        public string BlogContent { get; set; }
    }
}
