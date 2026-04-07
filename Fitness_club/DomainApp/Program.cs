using Domain.FitnessClub.Entities;
using Domain.FitnessClub.Exceptions;
using Domain.ValueObjects;

namespace DomainApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы Фитнес-системы ===\n");

            try
            {
                Console.WriteLine("--- 1. Создание Value Objects ---");

                var clientUsername = new Username("john_doe");
                var trainerUsername = new Username("coach_anna");
                var trainingTitle = new TrainingTitle("Morning Yoga Flow");
                var description = new Description("Легкая утренняя практика для пробуждения тела и ума");
                var trainingTime = new TrainingTime(DateTime.Now.AddDays(1), 60);

                Console.WriteLine($"Создан Username: {clientUsername.Value}");
                Console.WriteLine($"Создан Title тренировки: {trainingTitle.Value}");
                Console.WriteLine($"Создано время тренировки: {trainingTime.StartTime:dd.MM.yyyy HH:mm}, длительность: {trainingTime.DurationMinutes} мин.\n");

                Console.WriteLine("--- 2. Создание Client и Trainer ---");

                var client = new Client(clientUsername);
                var trainer = new Trainer(trainerUsername);

                Console.WriteLine($"Создан клиент: {client.Username.Value} (Id: {client.Id})");
                Console.WriteLine($"Создан тренер: {trainer.Username.Value} (Id: {trainer.Id})\n");

                Console.WriteLine("--- 3. Изменение Username ---");

                var newUsername = new Username("john_smith");
                bool usernameChanged = client.ChangeUsername(newUsername);
                Console.WriteLine($"Username изменен: {usernameChanged} -> {client.Username.Value}\n");

                Console.WriteLine("--- 4. Создание тренировки ---");

                var training = trainer.CreateTraining(trainingTitle, description, trainingTime, 5, "Room 101");
                Console.WriteLine($"Создана тренировка: {training.Title.Value}");
                Console.WriteLine($"  Тренер: {training.Trainer.Username.Value}");
                Console.WriteLine($"  Время: {training.Time.StartTime:dd.MM.yyyy HH:mm} (длит. {training.Time.DurationMinutes} мин.)");
                Console.WriteLine($"  Комната: {training.Room}");
                Console.WriteLine($"  Макс. участников: {training.MaxParticipants}");
                Console.WriteLine($"  Доступно мест: {training.AvailablePlaces}");
                Console.WriteLine($"  Статус: {training.Status}\n");

                Console.WriteLine("--- 5. Регистрация на тренировку ---");

                var registration = client.RegisterForTraining(training);
                Console.WriteLine($"Клиент {client.Username.Value} зарегистрирован на тренировку");
                Console.WriteLine($"  Id регистрации: {registration.Id}");
                Console.WriteLine($"  Статус регистрации: {registration.Status}");
                Console.WriteLine($"  Доступно мест после регистрации: {training.AvailablePlaces}\n");

                Console.WriteLine("--- 6. Проверка регистрации ---");

                bool isRegistered = client.IsRegisteredForTraining(training.Id);
                Console.WriteLine($"Клиент зарегистрирован на тренировку: {isRegistered}\n");

                Console.WriteLine("--- 7. Редактирование тренировки ---");

                var newTitle = new TrainingTitle("Advanced Yoga Flow");
                var newDescription = new Description("Интенсивная практика для опытных");
                bool edited = trainer.EditTraining(training, newTitle, newDescription, "Room 202");
                Console.WriteLine($"Тренировка отредактирована: {edited}");
                Console.WriteLine($"  Новое название: {training.Title.Value}");
                Console.WriteLine($"  Новая комната: {training.Room}");
                Console.WriteLine($"  Дата изменения: {training.LastModifiedAt}\n");

                Console.WriteLine("--- 8. Перенос тренировки ---");

                var newTime = new TrainingTime(DateTime.Now.AddDays(2), 90);
                bool rescheduled = trainer.RescheduleTraining(training, newTime);
                Console.WriteLine($"Тренировка перенесена: {rescheduled}");
                Console.WriteLine($"  Новое время: {training.Time.StartTime:dd.MM.yyyy HH:mm} (длит. {training.Time.DurationMinutes} мин.)\n");

                Console.WriteLine("--- 9. Отмена регистрации ---");

                bool cancelled = client.CancelRegistration(registration);
                Console.WriteLine($"Регистрация отменена: {cancelled}");
                Console.WriteLine($"  Статус регистрации: {registration.Status}");
                Console.WriteLine($"  Доступно мест после отмены: {training.AvailablePlaces}\n");

                Console.WriteLine("--- 10. Отмена тренировки ---");

                var anotherTime = new TrainingTime(DateTime.Now.AddDays(3), 45);
                var trainingToCancel = trainer.CreateTraining(trainingTitle, description, anotherTime, 3, "Room 303");
                Console.WriteLine($"Создана тренировка для отмены: {trainingToCancel.Title.Value}");

                bool trainingCancelled = trainer.CancelTraining(trainingToCancel);
                Console.WriteLine($"Тренировка отменена: {trainingCancelled}");
                Console.WriteLine($"  Статус тренировки: {trainingToCancel.Status}\n");

                Console.WriteLine("--- 11. Complete тренировки ---");

                var completeTime = new TrainingTime(DateTime.Now.AddDays(-1), 60);
                var completedTraining = trainer.CreateTraining(trainingTitle, description, completeTime, 5, "Room 999");
                Console.WriteLine($"Создана тренировка с прошедшей датой: {completedTraining.Time.StartTime:dd.MM.yyyy HH:mm}");
                bool completed = completedTraining.Complete();
                Console.WriteLine($"Тренировка завершена: {completed}, статус: {completedTraining.Status}\n");

                Console.WriteLine("--- 12. MarkAttended регистрации ---");

                var attendClient = new Client(new Username("attending_client"));
                var pastTrainingTime = new TrainingTime(DateTime.Now.AddDays(-2), 60);
                var pastTraining = trainer.CreateTraining(trainingTitle, description, pastTrainingTime, 5, "Room 888");
                Console.WriteLine($"Создана тренировка: {pastTraining.Title.Value}, время: {pastTraining.Time.StartTime:dd.MM.yyyy HH:mm}");

                var attendReg = attendClient.RegisterForTraining(pastTraining);
                Console.WriteLine($"Клиент {attendClient.Username.Value} зарегистрирован, статус: {attendReg.Status}");

                pastTraining.Complete();
                Console.WriteLine($"Тренировка завершена, статус тренировки: {pastTraining.Status}");

                bool marked = attendReg.MarkAttended();
                Console.WriteLine($"Отметка о посещении: {marked}, статус регистрации: {attendReg.Status}\n");

                Console.WriteLine("--- 13. Свойства тренировки ---");

                Console.WriteLine($"Есть свободные места: {training.HasAvailablePlaces}");
                Console.WriteLine($"Подтвержденных регистраций: {training.ConfirmedRegistrationsCount}\n");

                Console.WriteLine("--- 14. Проверка пересечения времени ---");

                var conflictingTime = new TrainingTime(DateTime.Now.AddDays(1).AddMinutes(30), 60);
                try
                {
                    var conflictingTraining = trainer.CreateTraining(trainingTitle, description, conflictingTime, 3, "Room 777");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Ошибка пересечения времени: {ex.Message}\n");
                }

                Console.WriteLine("--- 15. Демонстрация ошибок ---");

                try
                {
                    var invalidUsername = new Username("ab");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания Username: {ex.Message}");
                }

                try
                {
                    var invalidTime = new TrainingTime(DateTime.Now.AddDays(1), -30);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка создания TrainingTime: {ex.Message}");
                }

                try
                {
                    var anotherClient = new Client(new Username("jane_doe"));
                    var fullTrainingTime = new TrainingTime(DateTime.Now.AddDays(4), 60);
                    var fullTraining = trainer.CreateTraining(trainingTitle, description, fullTrainingTime, 1, "Room 404");
                    var reg1 = anotherClient.RegisterForTraining(fullTraining);
                    Console.WriteLine($"Первый клиент зарегистрирован");

                    var thirdClient = new Client(new Username("bob_wilson"));
                    var reg2 = thirdClient.RegisterForTraining(fullTraining);
                }
                catch (NoAvailablePlacesException ex)
                {
                    Console.WriteLine($"Ошибка регистрации (нет мест): {ex.Message}");
                }

                try
                {
                    var anotherTrainer = new Trainer(new Username("other_coach"));
                    var anotherTrainerTime = new TrainingTime(DateTime.Now.AddDays(5), 60);
                    var anotherTraining = anotherTrainer.CreateTraining(trainingTitle, description, anotherTrainerTime, 3, "Room 505");
                    trainer.EditTraining(anotherTraining, newTitle, newDescription, "Room 606");
                }
                catch (AnotherTrainerEditTrainingException ex)
                {
                    Console.WriteLine($"Ошибка редактирования (чужой тренер): {ex.Message}");
                }

                Console.WriteLine("\n=== Демонстрация завершена ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}