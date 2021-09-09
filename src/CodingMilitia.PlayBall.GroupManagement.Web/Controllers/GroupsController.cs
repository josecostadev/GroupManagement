using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    //[DemoExceptionFilterFactory]
    [Route("groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupsService;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IGroupsService groupsService, ILogger<GroupsController> logger)
        {
            _groupsService = groupsService;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            var groups = await _groupsService.GetAllAsync(ct);

            return Ok(groups.ToViewModel());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id, string extra, CancellationToken ct)
        {
            try
            {
                var group = await _groupsService.GetByIdAsync(id, ct);

                if (group == null)
                {
                    return NotFound();
                }

                var groupViewModel = group.ToViewModel();

                if (!string.IsNullOrEmpty(extra))
                {
                    var newGroup = new GroupViewModel
                    {
                        Id = groupViewModel.Id,
                        Name = $"{group.Name}. Extra: {extra}"
                    };

                    groupViewModel = newGroup;
                }

                return Ok(groupViewModel);
            }
            catch (TaskCanceledException cancellationException)
            {
                _logger.LogError("Task was canceled on {actionName}. Ex: {exception}", nameof(UpdateAsync), cancellationException);
                return Content("Task was canceled");
            }
        }

        [HttpGet]
        [Route("exception")]
        public IActionResult GetException()
        {
            throw new ArgumentException("Some Argument Exception");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, GroupViewModel model, CancellationToken ct)
        {
            model.Id = id; // not needed when uwe move to MediatR
            var updatedGroup = await _groupsService.UpdateAsync(model.ToServiceModel(), ct);

            if (updatedGroup == null)
            {
                return NotFound();
            }

            return Ok(updatedGroup.ToViewModel());
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync(GroupViewModel model, CancellationToken ct)
        {
            model.Id = 0; // not needed when uwe move to MediatR
            var createdGroup = await _groupsService.AddAsync(model.ToServiceModel(), ct);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdGroup.Id }, createdGroup.ToViewModel());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken ct)
        {
            var deletedGroup = await _groupsService.DeleteAsync(id, ct);
            
            if (deletedGroup == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
