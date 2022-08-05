namespace GroupProjectFrontEndV2.Models
{
    public class StudentProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User>? Students { get; set; }
    }
}
