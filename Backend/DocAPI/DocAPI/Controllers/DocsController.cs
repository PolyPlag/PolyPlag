using DocAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

//Swagger API Implementation found here
//https://app.swaggerhub.com/apis/PolyPlag/poly-plag/1.0.0#/default/get_Docs__id_

namespace DocAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]

    public class DocsController : Controller
    {
        // Document Context verify 
        private readonly DocumentContext _context;
        public DocsController(DocumentContext context)
        {
            _context = context;

        }
        /// <summary>
        /// Shows the count of the current document in progress. 
        /// Can be used for statistics
        /// </summary>
        /// <returns></returns>
        // GET api/docs 
        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(_context.Documents.Count());
        }

        /// <summary>
        /// Returns the status of an Document with the specified id/
        /// </summary>
        /// <param name="id">the id of the document</param>
        /// <returns>Document</returns>
        // GET api/docs/5
        [HttpGet("{id}", Name = "GetDocId")]
        public IActionResult Get(int id)
        {
            if (!_context.Documents.ContainsKey(id))
            {
                return StatusCode(404);
            }

            else if (_context.Documents[id].Done == false)
            {
                return StatusCode(202);
            } else if (_context.Documents[id].Faulty == true)
            {
                var ignore = _context.Documents[id];
                _context.Documents.TryRemove(id, out ignore);
                return StatusCode(501);
            }
            
            return new ObjectResult(_context.Documents[id]);
        }

        
        /// <summary>
        /// POST api/docs. Creates an new Document item.
        /// </summary>
        /// <param name="item">the base64 encoded content of a word.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Document item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var docId = _context.GenerateId();
 
            _context.Documents.TryAdd(docId, item);

            //Async call so we dont block API 
           Task.Run(() => item.ParseContent());

            //Checks if we could parse  
            if (_context.Documents[docId].Faulty == true)
            {
                //Remove corrupted documents
                var ignore = _context.Documents[docId];
                _context.Documents.TryRemove(docId,out ignore);

                //Error code as defined. See swagger
                return StatusCode(501);
            }

            return Content(docId.ToString());
        }

        /// <summary>
        /// Deletes an Document with the specified ID
        /// </summary>
        /// <param name="id">The id of the param to delete</param>
        /// <returns>NoContentResult</returns>
        // DELETE api/docs/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_context.Documents.ContainsKey(id))
            {
                return BadRequest();
            }
            var ignore = _context.Documents[id];
            _context.Documents.TryRemove(id, out ignore);

            return new NoContentResult();
        }
    }
}