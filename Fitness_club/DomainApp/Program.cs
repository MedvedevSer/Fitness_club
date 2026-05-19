using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace DomainApp;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== Fitness System Demo ===\n");

        try
        {
            // 1. Создание клиента и тренера
            Console.WriteLine("--- 1. Create Client and Trainer ---");
            var client = new Client(new Username("john_doe"));
            var trainer = new Trainer(new Username("coach_anna"));
            Console.WriteLine($"Client: {client.Username.Value}, Id: {client.Id}");
            Console.WriteLine($"Trainer: {trainer.Username.Value}, Id: {trainer.Id}\n");

            // 2. Изменение имени пользователя
            Console.WriteLine("--- 2. Change Username ---");
            client.ChangeUsername(new Username("john_smith"));
            Console.WriteLine($"New client username: {client.Username.Value}\n");

            // 3. Создание тренировки тренером
            Console.WriteLine("--- 3. Create Training ---");
            var training = trainer.CreateTraining(
                new TrainingTitle("Morning Yoga"),
                new Description("Light morning practice for beginners"),
                new TrainingTime(DateTime.Now.AddDays(2), 60),
                5,
                "Room 101");
            Console.WriteLine($"Training: {training.Title.Value}");
            Console.WriteLine($"  Trainer: {training.Trainer.Username.Value}");
            Console.WriteLine($"  Time: {training.Time.StartTime:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"  Duration: {training.Time.DurationMinutes} min");
            Console.WriteLine($"  Room: {training.Room}");
            Console.WriteLine($"  Max participants: {training.MaxParticipants}");
            Console.WriteLine($"  Available places: {training.AvailablePlaces}");
            Console.WriteLine($"  Status: {training.Status}\n");

            // 4. Регистрация клиента на тренировку
            Console.WriteLine("--- 4. Register for Training ---");
            var registration = client.RegisterForTraining(training);
            Console.WriteLine($"Registration created:");
            Console.WriteLine($"  Client: {registration.Client.Username.Value}");
            Console.WriteLine($"  Training: {registration.Training.Title.Value}");
            Console.WriteLine($"  Status: {registration.Status}");
            Console.WriteLine($"  Available places after: {training.AvailablePlaces}\n");

            // 5. Попытка повторной регистрации (должна выбросить исключение)
            Console.WriteLine("--- 5. Try duplicate registration (should fail) ---");
            try
            {
                var duplicateRegistration = client.RegisterForTraining(training);
            }
            catch (ClientAlreadyRegisteredException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }

            // 6. Проверка статуса регистрации
            Console.WriteLine("--- 6. Check Registration Status ---");
            bool isRegistered = client.IsRegisteredForTraining(training);
            Console.WriteLine($"Is client registered for training? {isRegistered}\n");

            // 7. Заполнение всех мест
            Console.WriteLine("--- 7. Fill all available places ---");
            var client2 = new Client(new Username("jane_doe"));
            var client3 = new Client(new Username("bob_wilson"));
            var client4 = new Client(new Username("alice_joy"));
            var client5 = new Client(new Username("charlie_brown"));

            var reg2 = client2.RegisterForTraining(training);
            var reg3 = client3.RegisterForTraining(training);
            var reg4 = client4.RegisterForTraining(training);
            var reg5 = client5.RegisterForTraining(training);
            Console.WriteLine($"5 clients registered, available places: {training.AvailablePlaces}\n");

            // 8. Попытка регистрации на полную тренировку (должна выбросить исключение)
            Console.WriteLine("--- 8. Try register on full training (should fail) ---");
            try
            {
                var client6 = new Client(new Username("extra_client"));
                var reg6 = client6.RegisterForTraining(training);
            }
            catch (NoAvailablePlacesException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }

            // 9. Создание второй тренировки в то же время (должна выбросить исключение)
            Console.WriteLine("--- 9. Try create training at same time (should fail) ---");
            try
            {
                var sameTimeTraining = trainer.CreateTraining(
                    new TrainingTitle("Evening Yoga"),
                    new Description("Evening practice"),
                    new TrainingTime(DateTime.Now.AddDays(2), 60),
                    3,
                    "Room 102");
            }
            catch (TrainerAlreadyHasTrainingAtTimeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }

            // 10. Отмена регистрации клиентом
            Console.WriteLine("--- 10. Cancel Registration ---");
            bool canceled = client.CancelRegistration(registration, trainer);
            Console.WriteLine($"Registration canceled: {canceled}");
            Console.WriteLine($"Registration status: {registration.Status}");
            Console.WriteLine($"Available places after cancel: {training.AvailablePlaces}\n");

            // 11. Отметка посещения (после завершения тренировки)
            Console.WriteLine("--- 11. Mark Attendance (after training completed) ---");
            var pastTraining = trainer.CreateTraining(
                new TrainingTitle("Past Yoga"),
                new Description("Yesterday's practice"),
                new TrainingTime(DateTime.Now.AddDays(-1), 60),
                3,
                "Room 103");

            var pastRegistration = client.RegisterForTraining(pastTraining);
            Console.WriteLine($"Past training date: {pastTraining.Time.StartTime:dd.MM.yyyy HH:mm}");

            bool completed = trainer.CompleteTraining(pastTraining);
            Console.WriteLine($"Training completed: {completed}");

            bool marked = pastRegistration.MarkAttended(trainer);
            Console.WriteLine($"Attendance marked: {marked}");
            Console.WriteLine($"Registration status: {pastRegistration.Status}\n");

            // 12. Отмена тренировки тренером
            Console.WriteLine("--- 12. Cancel Training by Trainer ---");
            var trainingToCancel = trainer.CreateTraining(
                new TrainingTitle("Cancelable Yoga"),
                new Description("Future training"),
                new TrainingTime(DateTime.Now.AddDays(5), 60),
                4,
                "Room 104");

            var cancelReg = client2.RegisterForTraining(trainingToCancel);
            Console.WriteLine($"Training created, available places: {trainingToCancel.AvailablePlaces}");

            bool trainingCanceled = trainer.CancelTraining(trainingToCancel);
            Console.WriteLine($"Training canceled: {trainingCanceled}");
            Console.WriteLine($"Training status: {trainingToCancel.Status}");
            Console.WriteLine($"Available places after cancel: {trainingToCancel.AvailablePlaces}\n");

            // 13. Редактирование тренировки
            Console.WriteLine("--- 13. Edit Training ---");
            var edited = trainer.EditTraining(
                training,
                new TrainingTitle("Advanced Yoga Flow"),
                new Description("Intensive practice for experienced"),
                "Room 201");
            Console.WriteLine($"Training edited: {edited}");
            Console.WriteLine($"New title: {training.Title.Value}");
            Console.WriteLine($"New description: {training.Description.Value}");
            Console.WriteLine($"New room: {training.Room}");
            Console.WriteLine($"Last modified: {training.LastModifiedAt}\n");

            // 14. Перенос тренировки
            Console.WriteLine("--- 14. Reschedule Training ---");
            var newTime = new TrainingTime(DateTime.Now.AddDays(3), 90);
            bool rescheduled = trainer.RescheduleTraining(training, newTime);
            Console.WriteLine($"Training rescheduled: {rescheduled}");
            Console.WriteLine($"New time: {training.Time.StartTime:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"New duration: {training.Time.DurationMinutes} min");
            Console.WriteLine($"Last modified: {training.LastModifiedAt}\n");

            // 15. Демонстрация ошибок при работе с чужой тренировкой
            Console.WriteLine("--- 15. Try edit another trainer's training (should fail) ---");
            var anotherTrainer = new Trainer(new Username("evil_trainer"));
            try
            {
                var editedByAnother = anotherTrainer.EditTraining(
                    training,
                    new TrainingTitle("Hacked Yoga"),
                    new Description("Hacked description"),
                    "Room 999");
            }
            catch (AnotherTrainerEditTrainingException ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }

            Console.WriteLine("=== All tests completed successfully ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}