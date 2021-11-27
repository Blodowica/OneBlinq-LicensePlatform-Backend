using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IProductService
    {
        Task RefreshProduct();
        Task ToggleProduct(int productId);
        Task EditMaxUses(int productId, int maxUses);
    }
}
