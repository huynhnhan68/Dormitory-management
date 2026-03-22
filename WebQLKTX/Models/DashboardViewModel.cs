using System.Collections.Generic;

namespace WebQLKTX.Models
{

    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalRooms { get; set; }
        public int TotalInvoices { get; set; }
        public int TotalRequests { get; set; }
        public int TotalAnnouncements { get; set; }
        public int TotalHealthDeclarations { get; set; }
        public int TotalMedicalRegistrations { get; set; }
        public List<StudentStat> StudentStats { get; set; } = new List<StudentStat>();

        public int TotalMaleStudents { get; set; } // Số lượng sinh viên nam
        public int TotalFemaleStudents { get; set; } // Số lượng sinh viên nữ

    }

    public class StudentStat
    {
        public string Gender { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
