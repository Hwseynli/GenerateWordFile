using Microsoft.AspNetCore.Mvc;
using GenerateWordFile.Data;
using Microsoft.EntityFrameworkCore;
using GenerateWordFile.Models;
using GenerateWordFile.Helpers;
using System.ComponentModel.DataAnnotations;
using GenerateWordFile.ViewModels;

namespace GenerateWordFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPersonService _personService;

        public PeopleController(AppDbContext context,IPersonService personService)
        {
            _personService = personService;
            _context = context;
        }
        // Create a new person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(PersonVM personVM)
        {
            if (personVM == null) return BadRequest("Person cannot be null.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // IsValid metodu ilə Birthdayin doğruluğunu yoxlayaq.
            if (!personVM.Birthdate.IsValidBirthday())
            {
                return BadRequest("Invalid bithday!!!.");
            }
            //Servisdən istifadə edək (Person yaratmaq üçün).
            var createdPerson = await _personService.CreatePersonAsync(personVM);
            return CreatedAtAction(nameof(GetPerson), new { id = createdPerson.Id }, createdPerson);
        }

        // Read a person by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        // Update a person
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, PersonUpdateVM personVM)
        {
            if (id <= 0 || id ==null) return BadRequest();
            if (!id.PersonExists(_context)) return NotFound("ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            // Birthday in doğruluğunu yoxlayaq.
            if (!personVM.Birthdate.IsValidBirthday())
            {
                return BadRequest("Invalid data provided.");
            }
            //Service dən istifadə edək
            var result = await _personService.UpdatePersonAsync(id, personVM);
            if (!result)
            {
                return NotFound("Person not found.");
            }
            return NoContent();
        }

        // Delete a person
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            if(id<=0 || id ==null) return BadRequest();
            if(id.PersonExists(_context)) return NotFound("ID mismatch.");
            var person = await _context.People.FirstOrDefaultAsync(x=>x.Id==id);
            if (person == null) return NotFound();
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}