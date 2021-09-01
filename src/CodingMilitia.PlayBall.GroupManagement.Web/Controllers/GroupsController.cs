﻿using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo.Filters;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    //[DemoActionFilterAttribute]
    //[ServiceFilter(typeof(DemoExceptionFilter))]
    [DemoExceptionFilterFactory]
    [Route("groups")]
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> IndexAsync(CancellationToken ct)
        {
            var groups = await _groupsService.GetAllAsync(ct);

            return View("Index", groups.ToViewModel());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> DetailsAsync(long id, string extra, CancellationToken ct)
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

            return View("Details", groupViewModel);
        }

        [HttpGet]
        [Route("exception")]
        public IActionResult GetException()
        {
            throw new ArgumentException("Some Argument Exception");
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(long id, GroupViewModel model, CancellationToken ct)
        {
            var group = await _groupsService.UpdateAsync(model.ToServiceModel(), ct);

            if (group == null)
            {
                return NotFound();
            }

            return RedirectToAction("IndexAsync");
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(GroupViewModel model, CancellationToken ct)
        {
            await _groupsService.AddAsync(model.ToServiceModel(), ct);

            return RedirectToAction("IndexAsync");
        }
    }
}
