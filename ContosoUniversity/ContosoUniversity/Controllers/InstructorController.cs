using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using System.Data.Entity.Infrastructure;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Instructor
        
        public ActionResult Index(int? id, int? courseID)           //The method accepts optional route data (id) and a query string parameter (courseID) 
                                                                    //that provide the ID values of the selected instructor and selected course, and passes 
                                                                    //all of the required data to the view. The parameters are provided by the Select hyperlinks on the page.
        {
            var viewModel = new InstructorIndexData();  //creates an instance of the view model and puts it in the list of instructors
            viewModel.Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)                   //This line ^ next specifies eager loading for Instructor.OfficeAssisgment
                                                                    //and Instructor.Courses navigation property
                .Include(i => i.Courses.Select(c => c.Department))  //Loads Courses & for each Course loaded, it does eager loading for the 
                                                                    //Course.Department navigation property
                .OrderBy(i => i.LastName);
            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(
                i => i.ID == id.Value).Single().Courses;
            }
            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                
                // Lazy loading 
                //viewModel.Enrollments = viewModel.Courses.Where( 
                // x => x.CourseID == courseID).Single().Enrollments; 
                // Explicit loading 
                
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single(); 
                
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                
                foreach (Enrollment enrollment in selectedCourse.Enrollments) 
                { 
                    db.Entry(enrollment).Reference(x => x.Student).Load(); 
                }
                viewModel.Enrollments = selectedCourse.Enrollments;
            }
            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.
                Include(i => i.OfficeAssignment)
                .Where(i => i.ID == id).
                Single();

            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location");
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Where(i => i.ID == id)
                .Single();
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

           
        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Instructors
            .Include(i => i.OfficeAssignment)
            .Where(i => i.ID == id)
            .Single();
            if (TryUpdateModel(instructorToUpdate, "",
            new string[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }
                    db.Entry(instructorToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(instructorToUpdate);
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);     //code drops the ViewBag statement and adds eager loading for the
                                                                 //associated OfficeAssignment entity. You can't perform eager loading 
                                                                 //with the Find method, so the Where and Single methods are used instead 
                                                                 //to select the instructor
            db.Instructors.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

//pg. 170, Contoso University
//pg. 193, Contoso University
//pg. 194, Contoso University