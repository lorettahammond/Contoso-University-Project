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
using System.Data.Entity.Infrastructure;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Course
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.Department);
            return View(courses.ToList());    //automatic scaffolding has specified eager loading for the Department
                                              //navigation property by using the Include method
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();   //method gets a list of all departments sorted by name, creates a SelectList collection 
                                                //for a drop-down list, and passes the collection to the view in a ViewBag property. 
                                                //The method accepts the optional selectedDepartment parameter that allows the calling 
                                                //code to specify the item that will be selected when the drop-down list is rendered. 
                                                //The view will pass the name DepartmentID to the DropDownList helper, and the helper 
                                                //then knows to look in the ViewBag object for a SelectList named DepartmentID.
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            try 
            {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        catch (RetryLimitExceededException) /* dex */
        {
        
        //Log the error (uncomment dex variable name and add a line here to write a log.) 
        ModelState.AddModelError ("", "Unable to save changes. Try again, and if the problem persists, see your system administrator."); 
        } 
        PopulateDepartmentsDropDownList (course.DepartmentID);
        return View(course); 
    }   
        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);  //The HttpGet Edit method sets the selected item, based on the ID of the department that is already assigned to the course being edited
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits,DepartmentID")] Course course)
        {
            try 
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
        } 
        catch (RetryLimitExceededException /* dex */) 
        { 
            //Log the error (uncomment dex variable name and add a line here to write a log.) 
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator."); 
        }
            PopulateDepartmentsDropDownList(course.DepartmentID);                //  The HttpPost methods for both Create and Edit also include code that sets 
                                                                                 //the selected item when they redisplay the page after an error
                                                                                //This code ensures that when the page is redisplayed to show the error message, 
                                                                                //whatever department was selected stays selected.
         return View(course);
        }
        
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null) 
        { 
            var departmentsQuery = from d in db.Departments 
                orderby d.Name 
                select d; 
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment); 
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
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

//pg. 164, Contoso University
//pgs. 184-= 185, Contoso University
