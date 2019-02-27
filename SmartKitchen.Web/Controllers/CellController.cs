using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IServices;
using System.Linq;
using System.Web.Mvc;

namespace SmartKitchen.Controllers
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
            var response = _cellService.AddCell(model, CurrentUser());
            if (response.Successful())
            {
                AddModelStateErrors(response);
                TempData["error"] = "This name is already taken";
            }
            return Redirect(Url.Action("View", "Storage", new { id = response.AddedGroupId }));
        }
    }
}