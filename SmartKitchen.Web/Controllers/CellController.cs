using System.Web.Mvc;
using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.IServices;

namespace SmartKitchen.Web.Controllers
{
    [Authorize]
    public class CellController : BaseController
    {
        private readonly ICellService _cellService;

        public CellController(ICellService cellService)
        {
            _cellService = cellService;
        }

        [HttpPost]
        public ActionResult Create(CellCreationModel model)
        {
            var response = _cellService.AddOrUpdateCell(model, CurrentUser());
            if (response.Successful())
            {
                AddModelStateErrors(response);
                TempData["error"] = "This name is already taken";
            }
            return Redirect(Url.Action("View", "Storage", new { id = response.AddedGroupId }));
        }
    }
}