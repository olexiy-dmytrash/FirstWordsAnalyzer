# Інструкція з очищення бази даних FirstWordsAnalyzer

## Огляд

Цей набір скриптів призначений для безпечного видалення 9 невикористовуваних об'єктів з бази даних FirstWordsAnalyzer:
- **4 Views**: BaseWordsPopularity, ChainsOfDerivedWords, ExistingWords, UniqueDerivedWords
- **5 Stored Procedures**: GetBasicWords, GetChainOfDerivedWords, GetWordsPopularity, MyProcedure, SetWrongAssociation

---

## 📋 Порядок виконання

### Крок 1: Отримання визначень об'єктів
```sql
-- Запустіть цей скрипт для отримання DDL всіх об'єктів
sqlcmd -S WIN-S76MUL2EO8J -U sa -P conor -d FirstWordsAnalyzer -i "01_GetObjectDefinitions.sql" -o "definitions_output.txt"
```

**Мета**: Зберегти визначення всіх об'єктів для можливого відновлення.

### Крок 2: Створення backup скрипту
1. Відкрийте файл `02_BackupObjectsBeforeDelete.sql`
2. Скопіюйте результати з `definitions_output.txt`
3. Замініть коментарі у файлі на реальні DDL скрипти
4. Збережіть файл як backup

### Крок 3: Перевірка залежностей
```sql
-- ОБОВ'ЯЗКОВО! Перевірте залежності перед видаленням
sqlcmd -S WIN-S76MUL2EO8J -U sa -P conor -d FirstWordsAnalyzer -i "03_CheckDependencies.sql"
```

**Важливо**: Якщо скрипт знайде залежності - НЕ виконуйте видалення!

### Крок 4: Видалення об'єктів
```sql
-- Тільки після успішної перевірки залежностей!
sqlcmd -S WIN-S76MUL2EO8J -U sa -P conor -d FirstWordsAnalyzer -i "04_DeleteObjects.sql"
```

---

## 🔧 Альтернативний спосіб (через SQL Server Management Studio)

### 1. Підключення до SSMS
- **Server**: WIN-S76MUL2EO8J
- **Authentication**: SQL Server Authentication
- **Login**: sa
- **Password**: conor
- **Database**: FirstWordsAnalyzer

### 2. Виконання скриптів
1. Відкрийте кожен скрипт у SSMS
2. Виконайте по черзі: 01 → 03 → 04
3. Перевіряйте результати кожного кроку

---

## ⚠️ Важливі застереження

### Перед виконанням:
- [ ] Переконайтеся, що у вас є права на видалення об'єктів
- [ ] Створіть повний backup бази даних
- [ ] Попередьте інших користувачів про технічні роботи
- [ ] Запустіть перевірку залежностей

### Під час виконання:
- [ ] Виконуйте скрипти по черзі
- [ ] Перевіряйте результати кожного кроку
- [ ] НЕ ігноруйте попередження про залежності

### Після виконання:
- [ ] Протестуйте веб-додаток
- [ ] Перевірте всі контролери
- [ ] Оновіть Entity Framework модель

---

## 🛠️ Що робити після видалення

### 1. Тестування додатку
Перевірте роботу всіх функцій:
- Перегляд популярності слів
- Аналіз спільнокореневих слів
- Детальна інформація про слова
- Ланцюжки похідних/базових слів

### 2. Оновлення Entity Framework моделі

#### Видалити з проекту:
```
FirstWordsAnalyzer/Models/GetChainOfDerivedWords_Result.cs
```

#### Оновити EDM.edmx:
1. Відкрийте Models/EDM.edmx у Visual Studio
2. Перейдіть в Model Browser
3. Знайдіть Function Imports
4. Видаліть `GetChainOfDerivedWords`
5. Збережіть модель

#### Оновити EDM.Context.cs:
Видаліть метод:
```csharp
public virtual ObjectResult<GetChainOfDerivedWords_Result> GetChainOfDerivedWords(Nullable<int> basicWordId)
```

### 3. Перевірка кодової бази
Переконайтеся, що код не посилається на видалені об'єкти:
```bash
# Пошук посилань у коді
grep -r "GetBasicWords\|GetWordsPopularity\|MyProcedure\|SetWrongAssociation" .
grep -r "BaseWordsPopularity\|ChainsOfDerivedWords\|ExistingWords\|UniqueDerivedWords" .
```

---

## 🔄 Відновлення у разі проблем

### Якщо щось пішло не так:
1. Використайте файл `02_BackupObjectsBeforeDelete.sql` з заповненими DDL
2. Відновіть backup бази даних
3. Запустіть backup скрипт для відновлення об'єктів

### Команда для відновлення з backup:
```sql
USE master;
GO

-- Відключення з'єднань
ALTER DATABASE FirstWordsAnalyzer SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- Відновлення з backup
RESTORE DATABASE FirstWordsAnalyzer 
FROM DISK = 'шлях_до_backup_файлу.bak'
WITH REPLACE;
GO

-- Повернення в режим multi-user
ALTER DATABASE FirstWordsAnalyzer SET MULTI_USER;
GO
```

---

## 📊 Очікувані результати

### Після успішного виконання:
- ✅ 9 невикористовуваних об'єктів видалено
- ✅ База даних стала простішою та зрозумілішою
- ✅ Зменшено плутанину для розробників
- ✅ Покращено продуктивність (менше об'єктів для кешування)

### Розмір очищення:
- **Views**: 4 об'єкти
- **Stored Procedures**: 5 об'єктів
- **Загалом**: 9 об'єктів БД

---

## 📞 Контакти та підтримка

У разі виникнення проблем:
1. Перевірте логи виконання скриптів
2. Переконайтеся у правильності підключення до БД
3. Використайте backup для відновлення
4. Перевірте права доступу користувача sa

**Важливо**: Всі зміни оборотні за допомогою backup файлів!