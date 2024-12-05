//using System;
//using System.Collections.Generic;
//using System.IO;
//using Newtonsoft.Json;

//public static class DataStorage
//{
//    public static void SaveToFile<T>(string filePath, T data)
//    {
//        File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));
//    }

//    public static T LoadFromFile<T>(string filePath)
//    {
//        if (!File.Exists(filePath))
//        {
//            Console.WriteLine($"File {filePath} not found. Creating a new one.");
//            File.WriteAllText(filePath, JsonConvert.SerializeObject(new List<T>()));
//            return default;
//        }
//        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
//    }
//}

//class Program
//{
//    static void Main()
//    {
//        // Load data
//        var students = DataStorage.LoadFromFile<List<Student>>("students.json") ?? new List<Student>();
//        var supervisors = DataStorage.LoadFromFile<List<PersonalSupervisor>>("supervisors.json") ?? new List<PersonalSupervisor>();
//        var tutors = DataStorage.LoadFromFile<List<SeniorTutor>>("tutors.json") ?? new List<SeniorTutor>();
//        var meetings = DataStorage.LoadFromFile<List<Meeting>>("meetings.json") ?? new List<Meeting>();

//        Console.WriteLine("Data loaded successfully!");

//        // Example: Display all students
//        foreach (var student in students)
//        {
//            Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Status: {student.Status}");
//        }

//        // Save data on exit
//        Console.WriteLine("Exiting...");
//        DataStorage.SaveToFile("students.json", students);
//        DataStorage.SaveToFile("supervisors.json", supervisors);
//        DataStorage.SaveToFile("tutors.json", tutors);
//        DataStorage.SaveToFile("meetings.json", meetings);
//    }


//    static void StudentMenu(List<Student> students, List<Meeting> meetings)
//    {
//        Console.Write("Enter your Student ID: ");
//        var studentId = Console.ReadLine();

//        var student = students.Find(s => s.Id == studentId);
//        if (student == null)
//        {
//            Console.WriteLine("Student not found.");
//            return;
//        }

//        Console.WriteLine("1. Report Progress  2. View Meeting History  3. Book a Meeting");
//        var choice = Console.ReadLine();

//        switch (choice)
//        {
//            case "1":
//                Console.Write("Enter your progress/well-being status: ");
//                student.Status = Console.ReadLine();
//                Console.WriteLine("Status updated.");
//                break;
//            case "2":
//                Console.WriteLine("Meeting History:");
//                foreach (var history in student.MeetingHistory)
//                    Console.WriteLine(history);
//                break;
//            case "3":
//                Console.Write("Enter the Supervisor ID to book a meeting with: ");
//                var supervisorId = Console.ReadLine();
//                Console.Write("Enter the meeting date (YYYY-MM-DD): ");
//                var date = DateTime.Parse(Console.ReadLine());

//                meetings.Add(new Meeting
//                {
//                    MeetingId = Guid.NewGuid().ToString(),
//                    StudentId = student.Id,
//                    SupervisorId = supervisorId,
//                    ScheduledTime = date
//                });

//                Console.WriteLine("Meeting booked successfully.");
//                break;
//            default:
//                Console.WriteLine("Invalid choice.");
//                break;
//        }
//    }

//    static void SupervisorMenu(List<PersonalSupervisor> supervisors, List<Student> students, List<Meeting> meetings)
//    {
//        Console.Write("Enter your Supervisor ID: ");
//        var supervisorId = Console.ReadLine();

//        var supervisor = supervisors.Find(s => s.Id == supervisorId);
//        if (supervisor == null)
//        {
//            Console.WriteLine("Supervisor not found.");
//            return;
//        }

//        Console.WriteLine("1. View Students  2. Book a Meeting with Student  3. View Meetings");
//        var choice = Console.ReadLine();

//        switch (choice)
//        {
//            case "1":
//                Console.WriteLine("Assigned Students:");
//                foreach (var student in supervisor.Students)
//                    Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Status: {student.Status}");
//                break;
//            case "2":
//                Console.Write("Enter Student ID: ");
//                var studentId = Console.ReadLine();
//                Console.Write("Enter the meeting date (YYYY-MM-DD): ");
//                var date = DateTime.Parse(Console.ReadLine());

//                meetings.Add(new Meeting
//                {
//                    MeetingId = Guid.NewGuid().ToString(),
//                    StudentId = studentId,
//                    SupervisorId = supervisor.Id,
//                    ScheduledTime = date
//                });

//                Console.WriteLine("Meeting booked successfully.");
//                break;
//            case "3":
//                Console.WriteLine("Scheduled Meetings:");
//                foreach (var meeting in meetings.FindAll(m => m.SupervisorId == supervisor.Id))
//                    Console.WriteLine($"Meeting with Student ID: {meeting.StudentId}, Date: {meeting.ScheduledTime}");
//                break;
//            default:
//                Console.WriteLine("Invalid choice.");
//                break;
//        }
//    }

//    static void TutorMenu(List<PersonalSupervisor> supervisors)
//    {
//        Console.WriteLine("Supervisors Overview:");
//        foreach (var supervisor in supervisors)
//        {
//            Console.WriteLine($"Supervisor ID: {supervisor.Id}, Name: {supervisor.Name}");
//            foreach (var student in supervisor.Students)
//                Console.WriteLine($"  Student ID: {student.Id}, Name: {student.Name}, Status: {student.Status}");
//        }
//    }
//}

//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class DataStorage
{
    public static void SaveToFile<T>(string filePath, T data)
    {
        File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));
    }

    public static T LoadFromFile<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return default;

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
    }
}

public class Student
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; } // Progress/Well-being status
    public List<string> MeetingHistory { get; set; } = new List<string>();
}

public class PersonalSupervisor
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();
}

public class SeniorTutor
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<PersonalSupervisor> Supervisors { get; set; } = new List<PersonalSupervisor>();
}

public class Meeting
{
    public string MeetingId { get; set; }
    public string StudentId { get; set; }
    public string SupervisorId { get; set; }
    public DateTime ScheduledTime { get; set; }
}


class Program
{
    static void Main()
    {
        // Load data
        var students = DataStorage.LoadFromFile<List<Student>>("students.json") ?? new List<Student>();
        Console.WriteLine($"Loaded {students.Count} students from file.");
        var supervisors = DataStorage.LoadFromFile<List<PersonalSupervisor>>("supervisors.json") ?? new List<PersonalSupervisor>();
        Console.WriteLine($"Loaded {supervisors.Count} supervisors from file.");
        var tutors = DataStorage.LoadFromFile<List<SeniorTutor>>("tutors.json") ?? new List<SeniorTutor>();
        Console.WriteLine($"Loaded {tutors.Count} tutors from file.");
        var meetings = DataStorage.LoadFromFile<List<Meeting>>("meetings.json") ?? new List<Meeting>();

        // Debugging output
        Console.WriteLine("\nLoaded Students:");
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Name: {student.Name}");
        }
        Console.WriteLine("\nLoaded Supervisors:");
        foreach (var PersonalSupervisor in supervisors)
        {
            Console.WriteLine($"ID: {PersonalSupervisor.Id}, Name: {PersonalSupervisor.Name}");
        }
        Console.WriteLine("\nLoaded Tutors:");
        foreach (var SeniorTutor in tutors)
        {
            Console.WriteLine($"ID: {SeniorTutor.Id}, Name: {SeniorTutor.Name}");
        }
        Console.WriteLine("\n\nWelcome to the Engagement Monitoring System");
        while (true)
        {
            Console.WriteLine("Select Role: 1. Student  2. Personal Supervisor  3. Senior Tutor  4. Exit");
            var roleChoice = Console.ReadLine();

            switch (roleChoice)
            {
                case "1":
                    StudentMenu(students, meetings);
                    break;
                case "2":
                    SupervisorMenu(supervisors, students, meetings);
                    break;
                case "3":
                    TutorMenu(supervisors, students, meetings);
                    break;
                case "4":
                    // Save data
                    DataStorage.SaveToFile("students.json", students);
                    DataStorage.SaveToFile("supervisors.json", supervisors);
                    DataStorage.SaveToFile("tutors.json", tutors);
                    DataStorage.SaveToFile("meetings.json", meetings);
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    static void StudentMenu(List<Student> students, List<Meeting> meetings)
    {
        Console.Write("Enter your Student ID: ");
        var studentId = Console.ReadLine();

        var student = students.Find(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.WriteLine("1. Report Progress  2. Book a Meeting");
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Enter your progress/well-being status: ");
                student.Status = Console.ReadLine();
                Console.WriteLine("Status updated.");
                break;
            case "2":
                Console.Write("Enter the Supervisor ID to book a meeting with: ");
                var supervisorId = Console.ReadLine();
                Console.Write("Enter the meeting date (YYYY-MM-DD): ");
                var date = DateTime.Parse(Console.ReadLine());

                meetings.Add(new Meeting
                {
                    MeetingId = Guid.NewGuid().ToString(),
                    StudentId = student.Id,
                    SupervisorId = supervisorId,
                    ScheduledTime = date
                });

                Console.WriteLine("Meeting booked successfully.");
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    static void SupervisorMenu(List<PersonalSupervisor> supervisors, List<Student> students, List<Meeting> meetings)
    {
        Console.Write("Enter your Supervisor ID: ");
        var supervisorId = Console.ReadLine();

        var supervisor = supervisors.Find(s => s.Id == supervisorId);
        if (supervisor == null)
        {
            Console.WriteLine("Supervisor not found.");
            return;
        }

        Console.WriteLine("1. Check a Student's Status  2. Book a Meeting with Student  3. View Meetings");
        var choice = Console.ReadLine();

        switch (choice)
        {

            case "1":
                Console.Write("Enter Student ID to check their status: ");
                var studentId = Console.ReadLine();
                var student = students.Find(s => s.Id == studentId);

                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    break;
                }

                Console.WriteLine($"Student Name: {student.Name}");
                Console.WriteLine($"Progress/Well-being Status: {student.Status}");
                break;

            case "2":
                Console.Write("Enter Student ID: ");
                studentId = Console.ReadLine();
                Console.Write("Enter the meeting date (YYYY-MM-DD): ");
                var date = DateTime.Parse(Console.ReadLine());

                meetings.Add(new Meeting
                {
                    MeetingId = Guid.NewGuid().ToString(),
                    StudentId = studentId,
                    SupervisorId = supervisor.Id,
                    ScheduledTime = date
                });

                Console.WriteLine("Meeting booked successfully.");
                break;

            case "3":
                Console.WriteLine("Scheduled Meetings:");
                foreach (var meeting in meetings.FindAll(m => m.SupervisorId == supervisor.Id))
                    Console.WriteLine($"Meeting with Student ID: {meeting.StudentId}, Date: {meeting.ScheduledTime}");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    static void TutorMenu(List<PersonalSupervisor> supervisors, List<Student> students, List<Meeting> meetings)
    {
        Console.WriteLine("\nSupervisors Overview:");
        foreach (var supervisor in supervisors)
        {
            Console.WriteLine($"\nSupervisor ID: {supervisor.Id}, Name: {supervisor.Name}");

            // List all meetings involving this supervisor
            var supervisorMeetings = meetings.FindAll(m => m.SupervisorId == supervisor.Id);

            if (supervisorMeetings.Count > 0)
            {
                Console.WriteLine("Meetings:");
                foreach (var meeting in supervisorMeetings)
                {
                    // Find the student associated with the meeting
                    var student = students.Find(s => s.Id == meeting.StudentId);
                    if (student != null)
                    {
                        Console.WriteLine($"  - Meeting ID: {meeting.MeetingId}, Student: {student.Name}, Date: {meeting.ScheduledTime}");
                    }
                    else
                    {
                        Console.WriteLine($"  - Meeting ID: {meeting.MeetingId}, Student: [Unknown], Date: {meeting.ScheduledTime}");
                    }
                }
            }
            else
            {
                Console.WriteLine("  No meetings scheduled yet.");
            }
        }
    }

}
