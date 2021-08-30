using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Demo;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")]
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;
        private readonly SomeRootConfiguration _config;
        private readonly SecretsConfiguration _secretsConfig;

        public GroupsController(
            IGroupsService groupsService, 
            SomeRootConfiguration config,
            SecretsConfiguration secretsConfig)
        {
            _groupsService = groupsService;
            _config = config;
            _secretsConfig = secretsConfig;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var groups = _groupsService.GetAll();

            return View(groups.ToViewModel());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Details(long id, string extra)
        {
            var group = _groupsService.GetById(id);

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

            return View(groupViewModel);
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, GroupViewModel model)
        {
            var group = _groupsService.Update(model.ToServiceModel());

            if (group == null)
            {
                return NotFound();
            }

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
            _groupsService.Add(model.ToServiceModel());

            return RedirectToAction("Index");
        }
    }
}
