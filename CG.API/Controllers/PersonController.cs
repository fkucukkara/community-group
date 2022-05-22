using CG.Core.Domain;
using CG.Core.Utilities;
using CG.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CG.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        #region Fields

        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<CommunityGroup> _cgRepository;

        #endregion

        #region Ctor
        public PersonController(IRepository<Person> personRepository,
            IRepository<CommunityGroup> cgRepository)
        {
            _personRepository = personRepository;
            _cgRepository = cgRepository;
        }

        #endregion

        #region Methods

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _personRepository.GetQueryable().ToListAsync();
        }

        [HttpGet("/pageNumber/pageSize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<Person>>> GetPersonsByPaginated(int pageNumber, int pageSize)
        {
            return await PaginatedList<Person>.CreateAsync(_personRepository.GetQueryable(), pageNumber, pageSize);
        }

        [HttpGet("/pageNumber/pageSize/search/sort")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedPersonList<Person>>> GetPersonsByPaginatedSearchSort(int pageNumber, int pageSize,
            string search, int sort)
        {
            return await PaginatedPersonList<Person>.CreatePersonAsync(_personRepository.GetQueryable(), pageNumber, pageSize, search, sort);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            try
            {
                await _personRepository.UpdateAsync(person);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (await PersonExists(id) == false)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            if (string.IsNullOrEmpty(person.Occupation) == false)
            {
                var existingCG = await _cgRepository.GetQueryable().FirstOrDefaultAsync(f => f.Name == person.Occupation);
                if (existingCG != null)
                {
                    person.CommunityGroupId = existingCG.Id;
                    await _personRepository.CreateAsync(person);
                }
                else
                {
                    var newCG = new CommunityGroup()
                    {
                        Name = person.Occupation
                    };
                    person.CommunityGroupId = (await _cgRepository.CreateAsync(newCG))?.Id;
                    await _personRepository.CreateAsync(person);
                }
            }

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        [HttpPost("/csv/path")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostPersonByCSVAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return BadRequest();
            }

            string[] lines = System.IO.File.ReadAllLines(path);
            var pList = new List<Person>();
            foreach (var line in lines)
            {
                var values = line.Split(',');
                if (values == null)
                {
                    return BadRequest();
                }

                var p = new Person()
                {
                    CreatedOn = System.DateTime.UtcNow,
                    IsDeleted = false,
                    FirstName = values[0],
                    LastName = values[1],
                    Email = values[2],
                    Occupation = values[3]
                };
                pList.Add(p);
            }

            await _personRepository.CreateRangeAsync(pList);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePersonAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            await _personRepository.DeleteAsync(id);

            return NoContent();
        }

        [HttpDelete("/delete/range")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePersonRangeAsync(IEnumerable<Person> persons)
        {
            await _personRepository.DeleteRangeAsync(persons);

            return NoContent();
        }

        private async Task<bool> PersonExists(int id)
        {
            return await _personRepository.GetQueryable().AnyAsync(f => f.Id == id);
        }

        #endregion
    }
}
