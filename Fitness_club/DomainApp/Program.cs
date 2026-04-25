using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace DomainApp;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("Fitness System Demo\n");

        try
        {
            // Create Value Objects
            var clientUsername = new Username("john_doe");
            var trainerUsername = new Username("coach_anna");
            var title = new TrainingTitle("Morning Yoga");
            var desc = new Description("Light morning practice");
            var time = new TrainingTime(DateTime.Now.AddDays(1), 60);

            // Create entities
            var client = new Client(clientUsername);
            var trainer = new Trainer(trainerUsername);
            var training = trainer.CreateTraining(title, desc, time, 5, "Room 101");

            Console.WriteLine($"Client: {client.Username.Value}");
            Console.WriteLine($"Trainer: {trainer.Username.Value}");
            Console.WriteLine($"Training: {training.Title.Value}, Room: {training.Room}");
            Console.WriteLine($"Available places: {training.AvailablePlaces}");

            // Register for training
            var registration = client.RegisterForTraining(training);
            Console.WriteLine($"Registered: {registration.Status}, Places left: {training.AvailablePlaces}");

            // Cancel registration
            client.CancelRegistration(registration);
            Console.WriteLine($"After cancel - Status: {registration.Status}, Places: {training.AvailablePlaces}");

            // Complete past training
            var pastTime = new TrainingTime(DateTime.Now.AddDays(-1), 60);
            var pastTraining = trainer.CreateTraining(title, desc, pastTime, 3, "Room 202");
            pastTraining.Complete();
            Console.WriteLine($"Past training status: {pastTraining.Status}");

            // Mark attendance
            var attendClient = new Client(new Username("anna"));
            var attendReg = attendClient.RegisterForTraining(pastTraining);
            pastTraining.Complete();
            attendReg.MarkAttended();
            Console.WriteLine($"Attendance status: {attendReg.Status}");

            // Test validation
            try { var _ = new Username("ab"); }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }

            try
            {
                var fullTime = new TrainingTime(DateTime.Now.AddDays(2), 60);
                var fullTraining = trainer.CreateTraining(title, desc, fullTime, 1, "Room 303");
                var _ = new Client(new Username("client1")).RegisterForTraining(fullTraining);
                var _2 = new Client(new Username("client2")).RegisterForTraining(fullTraining);
            }
            catch (NoAvailablePlacesException ex) { Console.WriteLine($"Error: {ex.Message}"); }

            Console.WriteLine("\nDemo completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
    }
}