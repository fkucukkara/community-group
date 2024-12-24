using CG.Core.Domain;
using CG.Core.Utilities;
using CG.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CG.API.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class PersonController(
    IRepository<Person> personRepository,
    IRepository<CommunityGroup> cgRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersons()
    {
        var result = await personRepository.GetQueryable().ToListAsync();
        return Ok(result);
    }

    [HttpGet("/pageNumber/pageSize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersonsByPaginated(int pageNumber, int pageSize)
    {
        var result = await PaginatedList<Person>.CreateAsync(personRepository.GetQueryable(), pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("/pageNumber/pageSize/search/sort")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedPersonList<Person>>> GetPersonsByPaginatedSearchSort(int pageNumber, int pageSize,
        string search, int sort)
    {
        var result = await PaginatedPersonList<Person>.CreatePersonAsync(personRepository.GetQueryable(), pageNumber, pageSize, search, sort);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPerson(int id)
    {
        return await personRepository.GetByIdAsync(id) is Person p
           ? Ok(p)
           : NotFound();
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
            await personRepository.UpdateAsync(person);

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
    public async Task<IActionResult> CreatePerson(Person person)
    {
        if (string.IsNullOrEmpty(person.Occupation) == false)
        {
            var existingCG = await cgRepository.GetQueryable().FirstOrDefaultAsync(f => f.Name == person.Occupation);
            if (existingCG != null)
            {
                person.CommunityGroupId = existingCG.Id;
                await personRepository.CreateAsync(person);
            }
            else
            {
                var newCG = new CommunityGroup()
                {
                    Name = person.Occupation
                };
                person.CommunityGroupId = (await cgRepository.CreateAsync(newCG))?.Id;
                await personRepository.CreateAsync(person);
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

        await personRepository.CreateRangeAsync(pList);

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePersonAsync(int id)
    {
        var person = await personRepository.GetByIdAsync(id);
        if (person == null)
        {
            return NotFound();
        }

        await personRepository.DeleteAsync(id);

        return NoContent();
    }

    [HttpDelete("/delete/range")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePersonRangeAsync(IEnumerable<Person> persons)
    {
        await personRepository.DeleteRangeAsync(persons);

        return NoContent();
    }

    private async Task<bool> PersonExists(int id)
    {
        return await personRepository.GetQueryable().AnyAsync(f => f.Id == id);
    }
}
