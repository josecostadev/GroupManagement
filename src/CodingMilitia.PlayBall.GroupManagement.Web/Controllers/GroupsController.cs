using CodingMilitia.PlayBall.GroupManagement.Web.Demo;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")]
    public class GroupsController : Controller
    {
        private static IList<GroupViewModel> groups = new List<GroupViewModel>
        {
            //new GroupViewModel
            //{
            //    Id = currentGroupId++,
            //    Name = "Group 1 Name"
            //}
        };

        private readonly IGroupIdGenerator _groupIdGenerator;

        public GroupsController(IGroupIdGenerator groupIdGenerator)
        {
            _groupIdGenerator = groupIdGenerator;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View(groups);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Details(long id, string extra)
        {
            var group = groups.SingleOrDefault(o => o.Id == id);

            if(group == null)
            {
                return NotFound();
            }

            if(!string.IsNullOrEmpty(extra))
            {
                var newGroup = new GroupViewModel
                {
                    Id = group.Id,
                    Name = $"{group.Name}. Extra: {extra}"
                };

                group = newGroup;
            }

            return View(group);
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, GroupViewModel model)
        {
            var group = groups.SingleOrDefault(o => o.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            group.Name = model.Name;

            return RedirectToAction("Index");
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
        public IActionResult Create(GroupViewModel model)
        {
            var group = new GroupViewModel
            {
                Id = _groupIdGenerator.Next(),
                Name = model.Name
            };

            groups.Add(group);

            return RedirectToAction("Index");
        }
    }
}
