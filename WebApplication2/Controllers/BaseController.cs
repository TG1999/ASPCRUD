using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T> : ControllerBase where T : Product
    {
        protected string _filePath;
        protected static int _counter = 1;

        public BaseController(string filePath)
        {
            _filePath = filePath;
            if (!System.IO.File.Exists(_filePath))
            {
                var fileStream = System.IO.File.Create(_filePath);
                fileStream.Close();
            }
        }

        protected async Task<List<T>> GetEntitiesAsync()
        {
            using (StreamReader file = System.IO.File.OpenText(_filePath))
            {
                string json = await file.ReadToEndAsync();
                Console.WriteLine(json);
                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
        }

        protected async Task SaveEntitiesAsync(List<T> entities)
        {
            string json = JsonConvert.SerializeObject(entities, Formatting.Indented);
            using (StreamWriter writer = System.IO.File.CreateText(_filePath))
            {
                await writer.WriteAsync(json);
            }
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            var entities = await GetEntitiesAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> GetById(int id)
        {
            var entities = await GetEntitiesAsync();
            var entity = entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<ActionResult<T>> Create(T entity)
        { 
            entity.Id = _counter++; // Assign ID and increment the counter
            var entities = await GetEntitiesAsync();
            entities.Add(entity);
            await SaveEntitiesAsync(entities);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity); // Adjust based on your entity's primary key
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, T entity)
        {
            var entities = await GetEntitiesAsync();
            var existingEntity = entities.FirstOrDefault(e => e.Id == id);
            Console.WriteLine(existingEntity);
            Console.WriteLine(entities);
            if (existingEntity == null)
            {
                return NotFound();
            }
            // Update existing entity
            entities.Remove(existingEntity);
            entities.Add(entity);
            await SaveEntitiesAsync(entities);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entities = await GetEntitiesAsync();
            var entity = entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return NotFound();
            }
            entities.Remove(entity);
            await SaveEntitiesAsync(entities);
            return NoContent();
        }
    }
}
