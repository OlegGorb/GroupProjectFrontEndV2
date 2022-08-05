using System.ComponentModel.DataAnnotations;

namespace GroupProjectFrontEndV2.Models
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public StudentProgram? Program { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        public bool Admin { get; set; } = false;

        public ICollection<TimeSpend>? TimeSpends { get; set; }

        public string GetTotalTime()
        {
            TimeSpan total = new TimeSpan();

            if (TimeSpends != null || TimeSpends.Count > 0)
            {
                foreach (var timeSpend in TimeSpends)
                {
                    total += timeSpend.EndDateTime - timeSpend.StartDateTime;
                }
            };

            string totalStr = total.ToString("hh\\:mm\\:ss");
            return totalStr;
        }
    }
}
