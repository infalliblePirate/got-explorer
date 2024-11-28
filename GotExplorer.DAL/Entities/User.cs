using Microsoft.AspNetCore.Identity;
using GotExplorer.DAL.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace GotExplorer.DAL.Entities
{
    public class User : IdentityUser<int>, IEntity<int>
    {   
        public int ImageId { get; set; }

        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}
