using SystemOfUpdatingDataOnOptions.Classes.ModelsSystem;

UpdatingData.MainUpdating();

Console.WriteLine("Сервис запущен. Нажмите любую клавишу для остановки...");
Console.ReadKey();
// Чтобы приложение не завершалось сразу
Thread.Sleep(Timeout.Infinite);