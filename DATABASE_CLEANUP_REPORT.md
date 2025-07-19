# FirstWordsAnalyzer - Звіт з очищення бази даних

## Аналіз використання об'єктів бази даних

На основі аналізу коду додатку та Entity Framework моделі, визначено які views та stored procedures використовуються та які можна видалити.

---

## 🟢 Використовувані об'єкти (НЕ видаляти)

### Views:
1. **PotentialCognates** 
   - Використовується в: `PotentialCognatesController`
   - Код: `db.PotentialCognates.OrderBy(c => c.BasicWordText).ThenBy(c => c.DerivedWordText).ToListAsync()`

2. **WordsPopularityWithCognates2**
   - Використовується в: `WordsPopularityWithCognates2Controller`
   - Код: `db.WordsPopularityWithCognates2.Find(id)`, `repository.GetAll()`

### Stored Procedures:
1. **GetChainOfDerivedWordsWithContext**
   - Використовується в: `WordsPopularityWithCognates2Controller.Details()`
   - Код: `db.Database.SqlQuery<DerivedWordChainCellWithContext>("GetChainOfDerivedWordsWithContext @wordId", param)`

2. **GetChainOfBasicWordsWithContext**
   - Використовується в: `WordsPopularityWithCognates2Controller.Details()`
   - Код: `db.Database.SqlQuery<DerivedWordChainCellWithContext>("GetChainOfBasicWordsWithContext @wordId", param1)`

### Entity Framework Function Imports:
1. **GetChainOfDerivedWords** (без контексту)
   - Імпортується в Entity Framework моделі
   - Метод: `db.GetChainOfDerivedWords(basicWordId)`
   - **Статус**: НЕ використовується в коді, але доступний через EF

2. **GetChainOfDerivedWordsWithContext** 
   - Імпортується в Entity Framework моделі
   - Метод: `db.GetChainOfDerivedWordsWithContext(basicWordId)`
   - **Статус**: Використовується через SqlQuery, не через EF метод

---

## 🔴 Невикористовувані об'єкти (на видалення)

### Views для видалення:

**Згідно з інформацією з REFACTORING_REPORT.md:**

1. **BaseWordsPopularity**
   - Не знайдено використання в коді додатку
   - Не використовується в Entity Framework моделі

2. **ChainsOfDerivedWords**
   - Не знайдено використання в коді додатку
   - Не використовується в Entity Framework моделі

3. **ExistingWords**
   - Не знайдено використання в коді додатку
   - Не використовується в Entity Framework моделі

4. **UniqueDerivedWords**
   - Не знайдено використання в коді додатку
   - Не використовується в Entity Framework моделі

### Stored Procedures для видалення:

**Згідно з інформацією з REFACTORING_REPORT.md:**

1. **GetBasicWords**
   - Не знайдено використання в коді додатку
   - Не імпортується в Entity Framework

2. **GetChainOfDerivedWords** (без контексту)
   - Хоча імпортується в EF, НЕ використовується в коді додатку
   - Замінено на `GetChainOfDerivedWordsWithContext`

3. **GetWordsPopularity**
   - Не знайдено використання в коді додатку
   - Не імпортується в Entity Framework

4. **MyProcedure**
   - Не знайдено використання в коді додатку
   - Схоже на тестову процедуру

5. **SetWrongAssociation**
   - Не знайдено використання в коді додатку
   - Не імпортується в Entity Framework

---

## 🔍 Додатковий аналіз невикористовуваних views

З аналізу stored procedures видно, що деякі views можуть використовуватися всередині процедур:

### Views, які можуть використовуватися в stored procedures:
1. **WordsPopularity** - згадується в `GetChainOfBasicWordsWithContext.sql` та `GetChainOfDerivedWordsWithContext.sql`
2. **WordsWithContext** - згадується в `GetChainOfBasicWordsWithContext.sql` та `GetChainOfDerivedWordsWithContext.sql`
3. **ActualCognates** - згадується в `GetChainOfBasicWordsWithContext.sql` та `GetChainOfDerivedWordsWithContext.sql`

**Рекомендація**: Ці views НЕ видаляти, оскільки вони використовуються в активних stored procedures.

---

## 📋 Фінальний список для видалення

### SQL команди для видалення невикористовуваних об'єктів:

```sql
USE FirstWordsAnalyzer;
GO

-- Видалення невикористовуваних Views
DROP VIEW IF EXISTS [dbo].[BaseWordsPopularity];
DROP VIEW IF EXISTS [dbo].[ChainsOfDerivedWords];
DROP VIEW IF EXISTS [dbo].[ExistingWords];
DROP VIEW IF EXISTS [dbo].[UniqueDerivedWords];

-- Видалення невикористовуваних Stored Procedures
DROP PROCEDURE IF EXISTS [dbo].[GetBasicWords];
DROP PROCEDURE IF EXISTS [dbo].[GetChainOfDerivedWords]; -- Без контексту
DROP PROCEDURE IF EXISTS [dbo].[GetWordsPopularity];
DROP PROCEDURE IF EXISTS [dbo].[MyProcedure];
DROP PROCEDURE IF EXISTS [dbo].[SetWrongAssociation];

PRINT 'Невикористовувані об''єкти бази даних видалено успішно';
```

---

## ⚠️ Застереження перед видаленням

### 1. Створити backup
```sql
-- Створити backup перед видаленням
BACKUP DATABASE FirstWordsAnalyzer 
TO DISK = 'C:\Backup\FirstWordsAnalyzer_BeforeCleanup.bak'
```

### 2. Перевірити залежності
Перед видаленням запустити наступний скрипт для перевірки залежностей:

```sql
-- Перевірка залежностей для views
SELECT 
    o.name AS dependent_object,
    o.type_desc AS dependent_type,
    d.referenced_entity_name AS referenced_object
FROM sys.sql_dependencies d
INNER JOIN sys.objects o ON d.object_id = o.object_id
WHERE d.referenced_entity_name IN (
    'BaseWordsPopularity', 
    'ChainsOfDerivedWords', 
    'ExistingWords', 
    'UniqueDerivedWords'
);

-- Перевірка залежностей для procedures
SELECT 
    o.name AS dependent_object,
    o.type_desc AS dependent_type,
    d.referenced_entity_name AS referenced_object
FROM sys.sql_dependencies d
INNER JOIN sys.objects o ON d.object_id = o.object_id
WHERE d.referenced_entity_name IN (
    'GetBasicWords',
    'GetChainOfDerivedWords',
    'GetWordsPopularity',
    'MyProcedure',
    'SetWrongAssociation'
);
```

### 3. Оновити Entity Framework модель
Після видалення об'єктів з бази даних:

1. **Видалити з EDMX моделі**:
   - Видалити `GetChainOfDerivedWords` Function Import з EDM.edmx
   - Видалити `GetChainOfDerivedWords_Result.cs`

2. **Очистити code-behind файли**:
   - Видалити методи з `EDM.Context.cs`
   - Перегенерувати модель якщо потрібно

---

## 📊 Очікувані результати

### Кількісні покращення:
- **Видалення 4 views** - зменшення складності схеми БД
- **Видалення 5 stored procedures** - спрощення логіки БД
- **Очищення EF моделі** - видалення невикористовуваних импортів

### Якісні покращення:
- **Спрощення схеми БД** - легше розуміти структуру
- **Покращення продуктивності** - менше об'єктів для кешування
- **Зменшення плутанини** - відсутність мертвого коду в БД

---

## 🔄 Рекомендації з виконання

### Етапи виконання:
1. **Створити backup** бази даних
2. **Перевірити залежності** за допомогою SQL скриптів
3. **Видалити об'єкти БД** у правильному порядку (спочатку procedures, потім views)
4. **Оновити Entity Framework модель**
5. **Протестувати функціональність** додатку

### Порядок видалення:
1. Спочатку stored procedures (можуть залежати від views)
2. Потім views (базові об'єкти)
3. Нарешті оновити EF модель

---

## 🎯 Висновок

Виявлено **9 невикористовуваних об'єктів БД** (4 views + 5 stored procedures), які можна безпечно видалити. Це спростить архітектуру бази даних та зменшить плутанину для розробників.

**Важливо**: Обов'язково створити backup та перевірити залежності перед видаленням!