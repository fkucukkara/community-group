using CG.Core.Domain;
using CG.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CG.API.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class CommunityGroupController(
    IRepository<CommunityGroup> cgRepository,
    IRepository<Person> personRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCommunityGroups()
    {
        var result = await cgRepository.GetQueryable().ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCommunityGroup(int id)
    {
        return await cgRepository.GetByIdAsync(id) is CommunityGroup cg
           ? Ok(cg)
           : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CommunityGroup>> CreateCommunityGroup(CommunityGroup cg)
    {
        await cgRepository.CreateAsync(cg);
        return CreatedAtAction(nameof(GetCommunityGroup), new { id = cg.Id }, cg);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCommunityGroup(int id, CommunityGroup cg)
    {
        if (id != cg.Id)
        {
            return BadRequest();
        }
        try
        {
            await cgRepository.UpdateAsync(cg);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await CommunityGroupExists(id) == false)
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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCommunityGroup(int id)
    {
        var cg = await cgRepository.GetByIdAsync(id);
        if (cg == null)
        {
            return NotFound();
        }

        await cgRepository.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("community/persons/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCommunityGroupPersons(int id)
    {
        var cg = await cgRepository.GetQueryable().Include(c => c.Persons).FirstOrDefaultAsync(c => c.Id == id);
        if (cg == null)
        {
            return NotFound();
        }

        return Ok(cg);
    }

    [HttpPut("assign/communityId/personId/")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AssignPerson(int communityId, int personId)
    {
        if (await CommunityGroupExists(communityId) == false || await PersonExists(personId) == false)
        {
            return NotFound();
        }

        var person = await personRepository.GetByIdAsync(personId);
        person.CommunityGroupId = communityId;
        await personRepository.UpdateAsync(person);

        return NoContent();
    }

    [HttpPut("remove/communityId/personId/")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemovePerson(int communityId, int personId)
    {
        if (await CommunityGroupExists(communityId) == false || await PersonExists(personId) == false)
        {
            return NotFound();
        }

        var person = await personRepository.GetByIdAsync(personId);
        person.CommunityGroupId = null;
        await personRepository.UpdateAsync(person);

        return NoContent();
    }

    private async Task<bool> CommunityGroupExists(int id)
    {
        return await cgRepository.GetQueryable().AnyAsync(f => f.Id == id);
    }
    private async Task<bool> PersonExists(int id)
    {
        return await personRepository.GetQueryable().AnyAsync(f => f.Id == id);
    }
}
