# Модуль расчета справедливой цены опциона по модели Блека-Шоулза

__Модель ценообразования опционов Блэка–Шоулза__ — это модель, которая определяет теоретическую цену на европейские опционы, подразумевающая, что если базовый актив торгуется на рынке, то цена опциона на него неявным образом уже устанавливается самим рынком. Согласно модели Блэка–Шоулза ключевым элементом определения стоимости опциона является ожидаемая волатильность базового актива. В зависимости от колебания актива цена на него возрастает или понижается, что прямо пропорционально влияет на стоимость опциона.
<br>Разрабатываемый модуль рассчитывает справедливую цену опционов Call и Put, согласно модели Блэка-Шоулза.
<br>По модели Блэка-Шоулза считаются опционы для европейских и американских рынков. В данной реализации не используются дивиденды для европейского рынка, для американского - используются.

---

## Структура модуля
### Класс CalculatingFairPriceOfEuropeanOption

Данный класс предназначен для расчета справедливой цены опциона для европейского рынка. В классе могут рассчитываться как Call, так и Put опционы.
<br>Для использования класса и получения результата необходимо передать в конструктор класса необходимые параметры:
-	Рыночная цена базового актива – текущая цена акции, валютной пары или другого актива, лежащего в основе опционного контракта. - S
-	Страйк – цена, по которой владелец опциона может купить или продать базовый актив до даты экспирации. - X
-	Время до экспирации – период, оставшийся до истечения срока действия опционного контракта (обычно выражается в годах). - T
-	Показатель волатильности – мера ожидаемых будущих колебаний цены актива, показывающая его нестабильность, выражается в годовом измерении (на сколько сильно цена может колебаться в среднем за год). – o`
-	Безрисковая процентная ставка – теоретическая норма доходности по инвестициям с нулевым финансовым риском. - r
<br>Пример использования класса:

##### <center> C#
```csharp
 CalculatingFairPriceOfEuropeanOption newOption = new CalculatingFairPriceOfEuropeanOption(38.6459, 40, 0.1, 0.5, 0.3);

 Console.WriteLine("\nS = " + newOption.CurrentPriceOfUnderlyingAsset);
 Console.WriteLine("K = " + newOption.Strike);
 Console.WriteLine("r = " + newOption.RiskFreeInterestRate);
 Console.WriteLine("T = " + newOption.TimeToOptioneExpiration);
 Console.WriteLine("o` = " + newOption.Volatility);

 Console.WriteLine("\nC = " + newOption.PriceOptionCall);
 Console.WriteLine("P = " + newOption.PriceOptionPut);

 Console.WriteLine("\nDelta C = " + newOption.GreeksValue.DeltaOptionCall);
 Console.WriteLine("Delta P = " + newOption.GreeksValue.DeltaOptionPut);
 Console.WriteLine("Gamma = " + newOption.GreeksValue.Gamma);
 Console.WriteLine("Vega = " + newOption.GreeksValue.Vega);
 Console.WriteLine("Teta C = " + newOption.GreeksValue.TetaOptionCall);
 Console.WriteLine("Teta P = " + newOption.GreeksValue.TetaOptionPut);
 Console.WriteLine("Ro C = " + newOption.GreeksValue.RoOptionCall);
 Console.WriteLine("Ro P = " + newOption.GreeksValue.RoOptionPut);
```

### Класс CalculatingFairPriceOfAmericanOption

Данный класс предназначен для расчета справедливой цены опциона для американского рынка. В классе могут рассчитываться только Call цначения опционов.
<br>Для использования класса и получения результата необходимо передать в конструктор класса необходимые параметры:
-	Рыночная цена базового актива.
-	Страйк.
-	Время до экспирации.
-	Показатель волатильности.
-	Безрисковая процентная ставка.
-   Размеры дивидендов
-   Сроки до выплаты дивидендов

Размеры дивидендов и сроки до выплаты дивидендов - это массивы (обычно из 1-2 значений). Количество элементов массивов должны совпадать (не может быть, что в размерах дивидендов только 1 запись, а в сроках до выплаты 2). ЕСЛИ переданы пустые массивы, то опцион считается без дивидендным и считается, как европейский.
<br>Пример использования класса:

##### <center> C#
```csharp
 CalculatingFairPriceOfAmericanOption newOption = new CalculatingFairPriceOfAmericanOption(40, 40, 0.1, 0.5, 0.3, [0.7, 0.7], [(2.0 / 12), (5.0 / 12)]);

Console.WriteLine("\nS = " + newOption.CurrentPriceOfUnderlyingAsset);
Console.WriteLine("K = " + newOption.Strike);
Console.WriteLine("r = " + newOption.RiskFreeInterestRate);
Console.WriteLine("T = " + newOption.TimeToOptioneExpiration);
Console.WriteLine("o` = " + newOption.Volatility);

Console.WriteLine("\nРазмеры дивидендов = [" + string.Join(", ", newOption.Dividends) + "]");
Console.WriteLine("Сроки до выплаты = [" + string.Join(", ", newOption.DividendTimes) + "]");

Console.WriteLine("\nC = " + newOption.PriceOptionCall);

Console.WriteLine("\nDelta C = " + newOption.GreeksValue.DeltaOptionCall);
Console.WriteLine("Gamma = " + newOption.GreeksValue.Gamma);
Console.WriteLine("Vega = " + newOption.GreeksValue.Vega);
Console.WriteLine("Teta C = " + newOption.GreeksValue.TetaOptionCall);
Console.WriteLine("Ro C = " + newOption.GreeksValue.RoOptionCall);
```

При попытке обращения к значениям для Put опциона (стоимость Put опциона, греки) выводится сообщение о невозможности получения значения, т.к. Put опцион не рассчитывается для американского рынка.

### Класс CalculatingGreeks

Данный класс расчитан для подсчета коэффициентов чувствительности опционов (греоков).
<br>Для использования класса и получения результата необходимо передать в конструктор класса необходимые параметры:
-	Рыночная цена базового актива.
-	Страйк.
-	Время до экспирации.
-	Показатель волатильности.
-	Безрисковая процентная ставка.
-   Коэффициент d1
-   Коэффициент d2

Класс предназначен для использования внутри классов CalculatingFairPriceOfEuropeanOption и CalculatingFairPriceOfAmericanOption. 
Пример использования класса: 
##### <center> C#
```csharp
GreeksValue = new CalculatingGreeks(CurrentPriceOfUnderlyingAsset, Strike, RiskFreeInterestRate,
    TimeToOptioneExpiration, Volatility, _d1, _d2);
```
После передачи параметров в конструктор производится расчет всех греков.

---

## Система обновления данных

Система обновления данных создана для пересчета данных об опционах каждые несколько минут (обычно 5). Система может работать самостоятельно, а может быть внедрена в какое-либо другое приложение.

### Класс UpdatingData

Статический класс, предназначенный для запуска таймера с обновлением данных. 
Пример использования класса: 
##### <center> C#
```csharp
UpdatingData.MainUpdating();
Console.WriteLine("Сервис запущен. Нажмите любую клавишу для остановки...");
Console.ReadKey();
// Чтобы приложение не завершалось сразу
Thread.Sleep(Timeout.Infinite);
```

### Класс Logging

Класс для логирования необходим для отслеживания работы программы, определения ошибок, исключений, проблемных мест программы.