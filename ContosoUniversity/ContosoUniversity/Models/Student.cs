using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]                          //Required attribute: makes the name properties required fields
        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "Last Name")]       //Display attribute: specifies that the caption for the text boxes
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Full Name")]
        public string FullName      //FullName is a calculated property that returns a value that's created by concatenating two other properties.
        { 
            get 
            { 
                return LastName + ", " + FirstMidName; 
        }   }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}

//pg. 127, Contoso University

//pg. 133, Contoso University (Note: Replaces code from pg. 127
