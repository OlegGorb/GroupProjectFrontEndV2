namespace GroupProjectFrontEndV2.Models
{
    public class TimeSpend
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public User User { get; set; }

        public string GetLength()
        {
            return (StartDateTime - EndDateTime).ToString("hh\\:mm\\:ss");
        }
    }
}
