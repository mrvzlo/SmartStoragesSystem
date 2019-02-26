using SmartKitchen.Domain.CreationModels;
using SmartKitchen.Domain.Enitities;
using SmartKitchen.Domain.IRepositories;
using SmartKitchen.Domain.IServices;

namespace SmartKitchen.DomainService.Services
{
    public class CellService : BaseService, ICellService
    {
        private readonly ICellRepository _cellRepository;
        private readonly IProductService _productService;

        public CellService(ICellRepository cellRepository, IProductService productService)
        {
            _cellRepository = cellRepository;
            _productService = productService;
        }

        public Cell GetOrCreateAndGet(CellCreationModel model)
        {
            var productId = _productService.GetOrCreateAndGet(model.Product).Id;
            var cell = GetCellByProductAndStorage(productId, model.Storage);
            if (cell != null) return cell;
            cell = new Cell
            {
                Storage = model.Storage,
                ProductId = productId,
                BestBefore = null
            };
            _cellRepository.AddCell(cell);
            return cell.Id <= 0 ? null : cell;
        }

        public Cell GetCellByProductAndStorage(int product, int storage) => 
            _cellRepository.GetCellByProductAndStorage(product, storage);
        
    }
}
