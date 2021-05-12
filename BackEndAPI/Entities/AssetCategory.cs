using System.Collections.Generic;

namespace BackEndAPI.Models
{
    public class AssetCategory : IEntity
    {
        public int Id { get; set; }

        public string CategoryCode { get; set; }
        
        public string CategoryName { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
        
    }
}