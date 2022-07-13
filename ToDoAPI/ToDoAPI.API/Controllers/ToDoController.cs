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
    //api/resources
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoController : ApiController
    {
        //create db connection
        ToDoEntities db = new ToDoEntities();

        //GET - api/resources
        public IHttpActionResult GetToDos()
        {
            //Create a list of ResourceViewModel objects, call to the db to get data, and place each resource in the db in the list below
            List<ToDoViewModel> toDo = db.ToDoItems.Include("Category").Select(t => new ToDoViewModel()
            {
                //Assign parameters of the Resources coming from the db to the dtos (ResourceViewModel)
                ToDoID = t.ToDoID,
                Action = t.Action,
                Done = t.Done,
                CategoryID = t.CategoryID

            }).ToList<ToDoViewModel>();

            //Check to see if there are any objects
            if (toDo.Count == 0)
            {
                return NotFound();
            }
            //Return the objects
            return Ok(toDo);
        } //end GetResources

        //api/resources/id

        public IHttpActionResult GetToDo(int id)
        {
            //Go find the resource
            ToDoViewModel toDo = db.ToDoItems.Include("Category").Where(t => t.ToDoID == id).Select(t => new ToDoViewModel()
            {
                //copy/paste the assignments from the first Get
                ToDoID = t.ToDoID,
                Action = t.Action,
                Done = t.Done,
                CategoryID = t.CategoryID,
            }).FirstOrDefault();
            //If we cannot find, pass a 404 error
            if (toDo == null)
                return NotFound(); //One of the few scopeless If Statements
                                   //Return the resource
            return Ok(toDo);
        } //End GetResource()

        //In an API, POST = Create
        //MethodType = POST
        //api/resources
        public IHttpActionResult PostToDo(ToDoViewModel toDo)
        {
            if (!ModelState.IsValid) //if the data they sent is not put together properly
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem newToDo = new ToDoItem()
            {
                //Translating the DTO to an EF representation of the object so we can pass this info to the db
                ToDoID = toDo.ToDoID,
                Action = toDo.Action,
                Done = toDo.Done,
                CategoryID = toDo.CategoryID
            };

            db.ToDoItems.Add(newToDo);
            db.SaveChanges();
            return Ok(newToDo);

        } //End PostResource()

        //PUT = Edit
        //api/resource (HttpPut)
        public IHttpActionResult PutToDo(ToDoViewModel toDo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            ToDoItem existingToDo = db.ToDoItems.Where(t => t.ToDoID == toDo.ToDoID).FirstOrDefault();

            //If we find the resource, we will edit that resource
            if (existingToDo != null)
            {
                existingToDo.ToDoID = toDo.ToDoID;
                existingToDo.Action = toDo.Action;
                existingToDo.Done = toDo.Done;
                existingToDo.CategoryID = toDo.CategoryID;
                db.SaveChanges();
                return Ok();

            }
            else
            {
                return NotFound();
            }
        } //End PutResource()

        //Delete
        //api/resources/id (Delete)
        public IHttpActionResult DeleteToDo(int id)
        {
            ToDoItem toDo = db.ToDoItems.Where(t => t.ToDoID == id).FirstOrDefault();

            if (toDo != null)
            {
                //we've found the resource in the db...let's delet it
                db.ToDoItems.Remove(toDo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        } //End DeleteResource()

        //We use the Dispose() to dispose of any connections to the datatbase after we are done with it - best practice to handle performance - dispose of connections that are not used

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

