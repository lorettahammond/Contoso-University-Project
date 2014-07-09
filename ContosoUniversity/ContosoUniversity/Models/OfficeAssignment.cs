using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContosoUniversity.Models
{
    public class OfficeAssignment
    {
        [Key]           //Key attribute: used to identify InstructorID as the primary key, 
                        //because the ID naming convention is not used
                        //Also used if the entity does have its own primary key but you want to name the 
                        //property something different than classnameID or ID.
        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
//pg. 136, Contoso University