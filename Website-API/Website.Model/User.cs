using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Model
{
    [Table("User")]
    public class User
    {
        [Key, Required]
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string UserName { get; set; }
        [Required, MaxLength(1024)]
        public string PasswordHash { get; set; }
        [MaxLength(128)]
        public string FirstName { get; set; }
        [MaxLength(128)]
        public string LastName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
