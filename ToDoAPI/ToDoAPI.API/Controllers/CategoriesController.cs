using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        //create an object that will connect to the database
        ToDoEntities db = new ToDoEntities();

        //below us our first ihttpactionresult. This will get a list of all categories

        //api/Categories
        public IHttpActionResult GetCategories()
        {
            //create a list to house the categories brought back from the database
            List<CategoryViewModel> cats = db.Categories.Select(c => new CategoryViewModel()
            {
                CategoryID = c.CategoryID,
                Name = c.Name,
                Description = c.Description
            }).ToList<CategoryViewModel>();

            if (cats.Count == 0)
            {
                return NotFound();
            }

            return Ok(cats);
        } //end getcategories

        //GetCategory() (like "details" in MVC)

        //api/Categories/id

        public IHttpActionResult GetCategory(int id)
        {
            CategoryViewModel cat = db.Categories.Where(c => c.CategoryID == id).Select(c => new CategoryViewModel()
            {
                CategoryID = c.CategoryID,
                Name = c.Name,
                Description = c.Description

            }).FirstOrDefault();

            //below is an example of a scopeless "if" statement
            if (cat == null)
                return NotFound();

            return Ok(cat);
        } //end GetCategory()

        //HttpPost is like "Create" in MVC

        //api/Categories/(HttpPost)

        public IHttpActionResult PostCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            db.Categories.Add(new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            });

            db.SaveChanges();
            return Ok();
        }//end PostCategory 

        //HttpPut is like "edit" in MVC

        //api/Categories/id (HttpPut)

        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            Category existingCategory = db.Categories.Where(c => c.CategoryID == cat.CategoryID).FirstOrDefault();

            if (existingCategory != null)
            {
                existingCategory.Name = cat.Name;
                existingCategory.Description = cat.Description;
                db.SaveChanges();
                return Ok();

            }
            else
            {
                return NotFound();
            }
        } //end PutCategory()

        //HttpDelete is like "delte" in MVC

        //api/Categories/id (HttpDelete)

        public IHttpActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Where(c => c.CategoryID == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        } //end DeleteCategory()

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
