using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WEBAPIProject.AttributeRepository;
using WEBAPIProject.DBEntity_Layer;

namespace MVC_WEBAPI_Project.Controllers
{
    //RoutePrefix attribute to the controller.This attribute defines the initial URI segments for all methods on this controller.
    //[RoutePrefix("/api/customersapi")]
    public class CustomersAPIController : ApiController
    {
        private WEBAPIDataStoreEntities db = new WEBAPIDataStoreEntities();

        [Authorize] //For ASP.NET Identity Token based uthentication
        //[BasicAuthentication]// For Custom Basic authentication
        // GET: api/CustomersAPI
        [Route("GetAll")]//This without RoutePrefix will simplify the URI as http://localhost/WEBAPIproject/Getall
        [HttpGet]// To instruct Web API to map HTTP verb GET to FetchCustomers() method, decorate the method with attribute.
        //[EnableCorsAttribute("*", "*", "*")]
        //[RequireHttps]//Custom Implementation attribute
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET: api/CustomersAPI/5
        [ResponseType(typeof(Customer))]
        [BasicAuthentication]// For Custom Basic authentication
        [Route("GetbyID/{id:int}")]
        public async Task<IHttpActionResult> GetCustomer([FromUri]long id)//FromUri FromBody Attributes are implicit understanding or can alos be recognized explicitly
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/CustomersAPI/5
        [ResponseType(typeof(void))]
        //[Route("Updateone/{id:int}")]
        public async Task<IHttpActionResult> PutCustomer(long id, [FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustID)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CustomersAPI
        [ResponseType(typeof(Customer))]
        [ModelValidatorAttribute]//To validate Model using Custome Attribute

        //[Route("Addone")]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return CreatedAtRoute("DefaultApi", new { id = customer.CustID }, customer);

                //var message = Request.CreateResponse(HttpStatusCode.Created, customer);
                //message.Headers.Location = new Uri(Request.RequestUri +
                //    customer.ID.ToString());
                //return message;// The method return type should be changed to HttpResponseMessage

            }
            else
            {
                return BadRequest(ModelState);
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState); // an create error response whene we return Httpresponsemessage

            }

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //db.Customers.Add(customer);
            //await db.SaveChangesAsync();
            //return CreatedAtRoute("DefaultApi", new { id = customer.CustID }, customer);

        }

        // DELETE: api/CustomersAPI/5
        [ResponseType(typeof(Customer))]
        //[Route("Deleteone/{id}")]
        public async Task<IHttpActionResult> DeleteCustomer(long id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [CustomExceptionFilter]
        private bool CustomerExists(long id)
        {
            return db.Customers.Count(e => e.CustID == id) > 0;

            //=======Coded this snippet as part of exception handling in below ways======================================================================================
            //======= using HttpResponse Exceoption=======
            //var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            //{
            //    Content = new StringContent(string.Format("Customer not found for given Id {0}", id)),
            //    ReasonPhrase = "Customer Not Found"
            //};
            //throw new HttpResponseException(response);
            //=======using HttpError=========================
            //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Customer Not Found");
            //=======using ExceptionFilter Attribute========================================
            //see CustomeException filter attribute for the method
            //=======using Exception Handler Globally==============================================
            //see Exception Handler registered globally
            //==============
        }
    }
}