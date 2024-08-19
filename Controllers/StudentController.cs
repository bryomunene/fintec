using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        private Models.ApplicationDbContext _context = new Models.ApplicationDbContext();
        public ActionResult Index()
        {
            // Retrieve all students from the database
            var students = _context.Students.Include("ClassStream").ToList();
            return View(students);
        }

        public ActionResult Create()
        {
            ViewBag.ClassStreamList = new SelectList(_context.ClassStreams, "ClassStreamId", "StreamName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassStreamList = new SelectList(_context.ClassStreams, "ClassStreamId", "StreamName", student.ClassStreamId);
            return View(student);
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            var student = _context.Students.Include("ClassStream").FirstOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Edit/5
        // GET: Student/Edit/5
        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = _context.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            // Populate ClassStream dropdown
            ViewBag.ClassStreamList = new SelectList(_context.ClassStreams, "ClassStreamId", "StreamName", student.ClassStreamId);
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingStudent = _context.Students.AsNoTracking().FirstOrDefault(s => s.StudentId == student.StudentId);
                    if (existingStudent == null)
                    {
                        return HttpNotFound();
                    }

                    _context.Entry(student).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (ex.Entries != null && ex.Entries.Any())
                    {
                        var entry = ex.Entries.SingleOrDefault();
                        if (entry != null)
                        {
                            var clientValues = (Student)entry.Entity;
                            var databaseValues = entry.GetDatabaseValues()?.ToObject() as Student;

                            ModelState.AddModelError("", "The record you attempted to edit has been changed by another user. The following changes were detected:");
                            ModelState.AddModelError("", $"Original: {clientValues.FirstName} {clientValues.LastName}");
                            ModelState.AddModelError("", databaseValues != null
                                ? $"Current: {databaseValues.FirstName} {databaseValues.LastName}"
                                : "The record was deleted by another user.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "The record you attempted to edit was not found.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Concurrency error occurred but no entries were found.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes. Please try again later.");
                }
            }

            // Repopulate ClassStream dropdown if the model is invalid
            ViewBag.ClassStreamList = new SelectList(_context.ClassStreams, "ClassStreamId", "StreamName", student.ClassStreamId);
            return View(student);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Student student = _context.Students.Include(s => s.ClassStream).SingleOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = _context.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ByClassStream(int id)
        {
            // Fetch students belonging to the specified ClassStreamId
            var students = _context.Students
                .Include(s => s.ClassStream) // Include the ClassStream to access StreamName
                .Where(s => s.ClassStreamId == id)
                .ToList();

            // Fetch the class stream for display purposes
            var classStream = _context.ClassStreams
                .SingleOrDefault(cs => cs.ClassStreamId == id);

            if (classStream == null)
            {
                return HttpNotFound();
            }

            // Prepare ViewModel
            var viewModel = new StudentByClassStreamViewModel
            {
                ClassStream = classStream,
                Students = students
            };

            return View(viewModel);
        }

    }

}