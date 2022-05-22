using CG.Core.Domain;
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
    public class CommunityGroupController : ControllerBase
    {
        #region Fields

        private readonly IRepository<CommunityGroup> _cgRepository;
        private readonly IRepository<Person> _personRepository;

        #endregion

        #region Ctor
        public CommunityGroupController(IRepository<CommunityGroup> cgRepository,
            IRepository<Person> personRepository)
        {
            _cgRepository = cgRepository;
            _personRepository = personRepository;
        }

        #endregion

        #region Methods

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CommunityGroup>>> GetCommunityGroups()
        {
            return await _cgRepository.GetQueryable().ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CommunityGroup>> GetCommunityGroup(int id)
        {
            var cg = await _cgRepository.GetByIdAsync(id);

            if (cg == null)
            {
                return NotFound();
            }

            return cg;
        }

        [HttpGet("/community/persons/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommunityGroupWithPersons(int id)
        {
            var cg = await _cgRepository.GetQueryable().Include(c => c.Persons).FirstOrDefaultAsync(c => c.Id == id);
            if (cg == null)
            {
                return NotFound();
            }

            return Ok(cg);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutCommunityGroup(int id, CommunityGroup cg)
        {
            if (id != cg.Id)
            {
                return BadRequest();
            }
            try
            {
                await _cgRepository.UpdateAsync(cg);

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

        [HttpPut("/assign/communityId/personId/")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AssignPerson(int communityId, int personId)
        {
            if (await CommunityGroupExists(communityId) == false || await PersonExists(personId) == false)
            {
                return NotFound();
            }

            var person = await _personRepository.GetByIdAsync(personId);
            person.CommunityGroupId = communityId;
            await _personRepository.UpdateAsync(person);

            return NoContent();
        }

        [HttpPut("/remove/communityId/personId/")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemovePerson(int communityId, int personId)
        {
            if (await CommunityGroupExists(communityId) == false || await PersonExists(personId) == false)
            {
                return NotFound();
            }

            var person = await _personRepository.GetByIdAsync(personId);
            person.CommunityGroupId = null;
            await _personRepository.UpdateAsync(person);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CommunityGroup>> PostCommunityGroup(CommunityGroup cg)
        {
            await _cgRepository.CreateAsync(cg);

            return CreatedAtAction("GetCommunityGroup", new { id = cg.Id }, cg);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCommunityGroup(int id)
        {
            var cg = await _cgRepository.GetByIdAsync(id);
            if (cg == null)
            {
                return NotFound();
            }

            await _cgRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CommunityGroupExists(int id)
        {
            return await _cgRepository.GetQueryable().AnyAsync(f => f.Id == id);
        }
        private async Task<bool> PersonExists(int id)
        {
            return await _personRepository.GetQueryable().AnyAsync(f => f.Id == id);
        }

        #endregion
    }
}
