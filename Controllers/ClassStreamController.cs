using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ClassStreamController : Controller
    {
        private Models.ApplicationDbContext _context = new Models.ApplicationDbContext();
        // GET: ClassStream
        public ActionResult Index()
        {
            var classStreams = _context.ClassStreams.Include("Class").ToList();
            return View(classStreams);
        }

        public ActionResult Details(int? id)
        {
            // Fetch the ClassStream along with the related Class data
            var classStream = _context.ClassStreams
                .Include("Class") // Use the string-based Include method
                .SingleOrDefault(cs => cs.ClassStreamId == id);

            if (classStream == null)
            {
                return HttpNotFound();
            }

            return View(classStream);
        }

    }
}