# FirstWordsAnalyzer - Повна схема бази даних

## Загальна інформація

**База даних**: FirstWordsAnalyzer  
**Сервер**: WIN-S76MUL2EO8J  
**Тип**: Microsoft SQL Server 2012+  
**Призначення**: Аналіз етимологічних зв'язків між словами в текстах

## Структура таблиць

### 1. Books (Книги)
**Призначення**: Зберігання інформації про книги та літературні твори

| Колонка | Тип | Розмір | Nullable | Ключ | Опис |
|---------|-----|--------|----------|------|------|
| Id | int | - | NOT NULL | PK, Identity | Унікальний ідентифікатор |
| Author | nvarchar | 50 | NULL | - | Автор книги |
| Name | nvarchar | 50 | NOT NULL | - | Назва книги |
| PublishingYear | date | - | NULL | - | Рік видання |

**Індекси:**
- `PK_Books` - Первинний ключ на Id

**Зв'язки:**
- Один до багатьох з `Sentences`

---

### 2. Sentences (Речення)
**Призначення**: Зберігання речень з книг

| Колонка | Тип | Розмір | Nullable | Ключ | Опис |
|---------|-----|--------|----------|------|------|
| Id | int | - | NOT NULL | PK, Identity | Унікальний ідентифікатор |
| BookId | int | - | NOT NULL | FK | Посилання на книгу |
| Text | nvarchar | 300 | NOT NULL | - | Текст речення |
| Length | numeric | 5,0 | NOT NULL | - | Довжина речення |

**Індекси:**
- `PK_Sentences` - Первинний ключ на Id
- `FK_Sentences_BookId` - Зовнішний ключ на Books.Id

**Зв'язки:**
- Багато до одного з `Books`
- Один до багатьох з `SentenceWords`

---

### 3. Words (Слова)
**Призначення**: Словник усіх слів з варіантами перекладу

| Колонка | Тип | Розмір | Nullable | Ключ | Опис |
|---------|-----|--------|----------|------|------|
| Id | int | - | NOT NULL | PK, Identity | Унікальний ідентифікатор |
| Text | nvarchar | 16 | NOT NULL | - | Текст слова |
| FirstTranslationVariant | nvarchar | 50 | NULL | - | Перший варіант перекладу |
| SecondTranslationVariant | nvarchar | 50 | NULL | - | Другий варіант перекладу |
| ThirdTranslationVariant | nvarchar | 50 | NULL | - | Третій варіант перекладу |
| NoSense | bit | - | NOT NULL | - | Чи немає сенсу |
| NotBasic | bit | - | NOT NULL | - | Чи не є базовим словом |

**Індекси:**
- `PK_Words` - Первинний ключ на Id

**Зв'язки:**
- Один до багатьох з `SentenceWords`
- Один до багатьох з `Cognates` (як BasicWord)
- Один до багатьох з `Cognates` (як DerivedWord)

---

### 4. SentenceWords (Слова в реченнях)
**Призначення**: Зв'язок між реченнями та словами з позицією

| Колонка | Тип | Розмір | Nullable | Ключ | Опис |
|---------|-----|--------|----------|------|------|
| SentenceId | int | - | NOT NULL | PK, FK | Посилання на речення |
| WordId | int | - | NOT NULL | PK, FK | Посилання на слово |
| Number | decimal | 3,0 | NOT NULL | PK | Номер слова в реченні |

**Індекси:**
- `PK_SentenceWords` - Композитний первинний ключ (SentenceId, WordId, Number)
- `FK_SentenceWords_SentenceId` - Зовнішний ключ на Sentences.Id
- `FK_SentenceWords_WordId` - Зовнішний ключ на Words.Id

**Зв'язки:**
- Багато до одного з `Sentences`
- Багато до одного з `Words`

---

### 5. Cognates (Спільнокореневі слова)
**Призначення**: Етимологічні зв'язки між словами

| Колонка | Тип | Розмір | Nullable | Ключ | Опис |
|---------|-----|--------|----------|------|------|
| Id | int | - | NOT NULL | PK, Identity | Унікальний ідентифікатор |
| BasicWordId | int | - | NOT NULL | FK | Базове слово |
| DerivedWordId | int | - | NOT NULL | FK | Похідне слово |
| WordPart | nvarchar | 10 | NOT NULL | - | Частина слова (корінь, суфікс) |
| WrongAssociation | bit | - | NOT NULL | - | Чи є хибною асоціацією |

**Індекси:**
- `PK_Cognates` - Первинний ключ на Id
- `FK_Cognates_BasicWordId` - Зовнішний ключ на Words.Id
- `FK_Cognates_DerivedWordId` - Зовнішний ключ на Words.Id
- `AK_BasicWordId_DerivedWordId` - Унікальний ключ (BasicWordId, DerivedWordId)

**Зв'язки:**
- Багато до одного з `Words` (як BasicWord)
- Багато до одного з `Words` (як DerivedWord)

---

## В'юшки (Views)

### 1. PotentialCognates
**Призначення**: Потенційні спільнокореневі слова для аналізу

**Структура:**
```sql
SELECT 
    [PotentialCognates].[BasicWordId] AS [BasicWordId], 
    [PotentialCognates].[DerivedWordId] AS [DerivedWordId], 
    [PotentialCognates].[BasicWordText] AS [BasicWordText], 
    [PotentialCognates].[DerivedWordText] AS [DerivedWordText], 
    [PotentialCognates].[BasicWordFirstTranslationVariant] AS [BasicWordFirstTranslationVariant], 
    [PotentialCognates].[DerivedWordFirstTranslationVariant] AS [DerivedWordFirstTranslationVariant]
FROM [dbo].[PotentialCognates]
```

**Колонки:**
- `BasicWordId` (int) - ID базового слова
- `DerivedWordId` (int) - ID похідного слова
- `BasicWordText` (nvarchar(16)) - Текст базового слова
- `DerivedWordText` (nvarchar(16)) - Текст похідного слова
- `BasicWordFirstTranslationVariant` (nvarchar(50)) - Переклад базового слова
- `DerivedWordFirstTranslationVariant` (nvarchar(50)) - Переклад похідного слова

---

### 2. WordsPopularityWithCognates2
**Призначення**: Статистика популярності слів з урахуванням спільнокореневих

**Структура:**
```sql
SELECT 
    [WordsPopularityWithCognates2].[WordId] AS [WordId], 
    [WordsPopularityWithCognates2].[Text] AS [Text], 
    [WordsPopularityWithCognates2].[FirstTranslationVariant] AS [FirstTranslationVariant], 
    [WordsPopularityWithCognates2].[SecondTranslationVariant] AS [SecondTranslationVariant], 
    [WordsPopularityWithCognates2].[ThirdTranslationVariant] AS [ThirdTranslationVariant], 
    [WordsPopularityWithCognates2].[Quantity] AS [Quantity], 
    [WordsPopularityWithCognates2].[Differance] AS [Differance]
FROM [dbo].[WordsPopularityWithCognates2]
```

**Колонки:**
- `WordId` (int) - ID слова
- `Text` (nvarchar(16)) - Текст слова
- `FirstTranslationVariant` (nvarchar(50)) - Перший варіант перекладу
- `SecondTranslationVariant` (nvarchar(50)) - Другий варіант перекладу
- `ThirdTranslationVariant` (nvarchar(50)) - Третій варіант перекладу
- `Quantity` (int) - Кількість використань
- `Differance` (int) - Різниця (можливо, з попереднім періодом)

---

## Збережені процедури (Stored Procedures)

### 1. GetChainOfDerivedWordsWithContext
**Призначення**: Отримання ланцюжка похідних слів з контекстом

**Параметри:**
- `@basicWordId` (int) - ID базового слова

**Логіка:**
```sql
WITH CognatesCTE AS (
    SELECT [BasicWordId] AS WordId,
           [BasicWordId],
           [DerivedWordId],
           0 AS distance,
           Id
    FROM [FirstWordsAnalyzer].[dbo].ActualCognates
    WHERE [BasicWordId] = @basicWordId
    
    UNION ALL
    
    SELECT B.WordId,
           D.BasicWordId,
           D.DerivedWordId,
           B.distance + 1,
           D.Id
    FROM CognatesCTE AS B
    JOIN [FirstWordsAnalyzer].[dbo].ActualCognates AS D
    ON B.DerivedWordId = D.BasicWordId
)
SELECT CCTE.Id, CCTE.BasicWordId, CCTE.DerivedWordId, CCTE.Distance,
       WP.Quantity, WWC.WordText, WWC.FirstTranslationVariant,
       WWC.SentenceId, WWC.SentenceText
FROM CognatesCTE AS CCTE
JOIN [dbo].[WordsPopularity] AS WP ON CCTE.DerivedWordId = WP.WordId
JOIN [dbo].[WordsWithContext] AS WWC ON CCTE.DerivedWordId = WWC.WordId
ORDER BY CCTE.distance, CCTE.BasicWordId, WP.Quantity DESC
```

**Повертає:**
- `Id` - ID зв'язку
- `BasicWordId` - ID базового слова
- `DerivedWordId` - ID похідного слова
- `Distance` - Відстань в ланцюжку
- `Quantity` - Популярність
- `WordText` - Текст слова
- `FirstTranslationVariant` - Переклад
- `SentenceId` - ID речення
- `SentenceText` - Текст речення

---

### 2. GetChainOfBasicWordsWithContext
**Призначення**: Отримання ланцюжка базових слів з контекстом

**Параметри:**
- `@derivedWordId` (int) - ID похідного слова

**Логіка:**
```sql
WITH CognatesCTE AS (
    SELECT [DerivedWordId] AS WordId,
           [BasicWordId],
           [DerivedWordId],
           0 AS distance,
           Id
    FROM [FirstWordsAnalyzer].[dbo].ActualCognates
    WHERE [DerivedWordId] = @derivedWordId
    
    UNION ALL
    
    SELECT D.WordId,
           B.BasicWordId,
           B.DerivedWordId,
           D.distance - 1,
           B.Id
    FROM CognatesCTE AS D
    JOIN [FirstWordsAnalyzer].[dbo].ActualCognates AS B
    ON D.BasicWordId = B.DerivedWordId
)
SELECT CCTE.Id, CCTE.BasicWordId, CCTE.DerivedWordId, CCTE.Distance,
       WP.Quantity, WWC.WordText, WWC.FirstTranslationVariant,
       WWC.SentenceId, WWC.SentenceText
FROM CognatesCTE AS CCTE
JOIN [dbo].[WordsPopularity] AS WP ON CCTE.BasicWordId = WP.WordId
JOIN [dbo].[WordsWithContext] AS WWC ON CCTE.BasicWordId = WWC.WordId
ORDER BY CCTE.distance, CCTE.BasicWordId
```

---

## Допоміжні об'єкти

### 1. Допоміжні таблиці/в'юшки (не в Entity Framework)
- `ActualCognates` - Актуальні спільнокореневі слова
- `WordsPopularity` - Популярність слів
- `WordsWithContext` - Слова з контекстом

### 2. Скрипти обслуговування

#### Видалення дублікатів з Cognates
```sql
WITH CTE AS (
    SELECT [BasicWordId], [DerivedWordId], [WordPart],
           MAX(Id) AS Id
    FROM [FirstWordsAnalyzer].[dbo].[Cognates]
    GROUP BY [BasicWordId], [DerivedWordId], [WordPart]
    HAVING COUNT(*) > 1
)
DELETE FROM [dbo].[Cognates]
WHERE Id IN (SELECT Id FROM CTE)
```

#### Унікальний constraint для Cognates
```sql
ALTER TABLE [dbo].[Cognates] 
ADD CONSTRAINT AK_BasicWordId_DerivedWordId 
UNIQUE (BasicWordId, DerivedWordId)
```

---

## Схема зв'язків

### ER-діаграма (текстовий формат)
```
Books (1) -----> (N) Sentences
                      |
                      | (1)
                      |
                      v
                 (N) SentenceWords (N)
                      |
                      | (1)
                      |
                      v
                 (1) Words (1)
                   |     |
                   |     |
            (N) ---+     +--- (N)
               |             |
               v             v
          Cognates.BasicWordId
                 |
                 |
          Cognates.DerivedWordId
```

### Детальні зв'язки
1. **Books → Sentences** (1:N)
   - `Books.Id` → `Sentences.BookId`

2. **Sentences → SentenceWords** (1:N)
   - `Sentences.Id` → `SentenceWords.SentenceId`

3. **Words → SentenceWords** (1:N)
   - `Words.Id` → `SentenceWords.WordId`

4. **Words → Cognates** (1:N) як BasicWord
   - `Words.Id` → `Cognates.BasicWordId`

5. **Words → Cognates** (1:N) як DerivedWord
   - `Words.Id` → `Cognates.DerivedWordId`

---

## Статистика та характеристики

### Розміри даних
- **Обмеження тексту слова**: 16 символів
- **Обмеження тексту речення**: 300 символів
- **Обмеження назви книги**: 50 символів
- **Обмеження автора**: 50 символів
- **Обмеження частини слова**: 10 символів

### Типи даних
- **Цілі числа**: int (4 байти)
- **Десяткові**: decimal/numeric з вказаною точністю
- **Текст**: nvarchar (Unicode)
- **Логічні**: bit
- **Дата**: date

### Індексація
- **Первинні ключі**: Кластеризовані індекси
- **Зовнішні ключі**: Некластеризовані індекси
- **Унікальні обмеження**: Для запобігання дублікатам

---

## Алгоритми та логіка

### 1. Рекурсивний пошук ланцюжків
Використовується **Common Table Expression (CTE)** для рекурсивного пошуку:
- Базовий випадок: знаходження прямих зв'язків
- Рекурсивний крок: пошук непрямих зв'язків через проміжні слова
- Обмеження глибини: через distance counter

### 2. Контекстний аналіз
- Поєднання слів з реченнями для контексту
- Врахування популярності слів
- Сортування за відстанню та популярністю

### 3. Управління дублікатами
- Унікальні constraints для запобігання дублікатам
- Скрипти очищення для видалення існуючих дублікатів

---

## Підключення

### Connection String
```xml
<add name="FirstWordsAnalyzerEntities" 
     connectionString="metadata=res://*/Models.EDM.csdl|res://*/Models.EDM.ssdl|res://*/Models.EDM.msl;
                      provider=System.Data.SqlClient;
                      provider connection string=&quot;data source=WIN-S76MUL2EO8J;
                                                   initial catalog=FirstWordsAnalyzer;
                                                   persist security info=True;
                                                   user id=sa;
                                                   password=conor;
                                                   MultipleActiveResultSets=True;
                                                   App=EntityFramework&quot;" 
     providerName="System.Data.EntityClient" />
```

### Налаштування Entity Framework
- **Версія**: 6.2.0
- **Lazy Loading**: Включено
- **Множинні активні результати**: Включено
- **Плюралізація**: Включено

---

## Безпека

### Аутентифікація
- **SQL Server Authentication**: sa/conor
- **Windows Authentication**: Не налаштовано

### Обмеження доступу
- Використання Entity Framework для запобігання SQL-ін'єкціям
- Параметризовані запити в stored procedures

---

## Рекомендації з оптимізації

### 1. Індексація
- Додати індекси на часто використовувані колонки для пошуку
- Індекс на `Words.Text` для швидкого пошуку слів
- Індекс на `Sentences.Text` для повнотекстового пошуку

### 2. Партиціювання
- Розглянути партиціювання `SentenceWords` за `SentenceId`
- Партиціювання `Cognates` за `BasicWordId`

### 3. Архівування
- Архівування старих даних за `Books.PublishingYear`
- Видалення неактуальних `PotentialCognates`

### 4. Моніторинг
- Додати статистику використання stored procedures
- Моніторинг продуктивності рекурсивних запитів

---

## Генерація DDL скриптів

### Створення таблиць
```sql
-- Таблиця Books
CREATE TABLE [dbo].[Books](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Author] [nvarchar](50) NULL,
    [PublishingYear] [date] NULL,
    [Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([Id])
);

-- Таблиця Words
CREATE TABLE [dbo].[Words](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Text] [nvarchar](16) NOT NULL,
    [FirstTranslationVariant] [nvarchar](50) NULL,
    [SecondTranslationVariant] [nvarchar](50) NULL,
    [ThirdTranslationVariant] [nvarchar](50) NULL,
    [NoSense] [bit] NOT NULL,
    [NotBasic] [bit] NOT NULL,
    CONSTRAINT [PK_Words] PRIMARY KEY CLUSTERED ([Id])
);

-- Таблиця Sentences
CREATE TABLE [dbo].[Sentences](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [BookId] [int] NOT NULL,
    [Text] [nvarchar](300) NOT NULL,
    [Length] [numeric](5, 0) NOT NULL,
    CONSTRAINT [PK_Sentences] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Sentences_Books] FOREIGN KEY([BookId]) REFERENCES [dbo].[Books] ([Id])
);

-- Таблиця SentenceWords
CREATE TABLE [dbo].[SentenceWords](
    [SentenceId] [int] NOT NULL,
    [WordId] [int] NOT NULL,
    [Number] [decimal](3, 0) NOT NULL,
    CONSTRAINT [PK_SentenceWords] PRIMARY KEY CLUSTERED ([SentenceId], [WordId], [Number]),
    CONSTRAINT [FK_SentenceWords_Sentences] FOREIGN KEY([SentenceId]) REFERENCES [dbo].[Sentences] ([Id]),
    CONSTRAINT [FK_SentenceWords_Words] FOREIGN KEY([WordId]) REFERENCES [dbo].[Words] ([Id])
);

-- Таблиця Cognates
CREATE TABLE [dbo].[Cognates](
    [BasicWordId] [int] NOT NULL,
    [DerivedWordId] [int] NOT NULL,
    [WordPart] [nvarchar](10) NOT NULL,
    [WrongAssociation] [bit] NOT NULL,
    [Id] [int] IDENTITY(1,1) NOT NULL,
    CONSTRAINT [PK_Cognates] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_Cognates_BasicWord] FOREIGN KEY([BasicWordId]) REFERENCES [dbo].[Words] ([Id]),
    CONSTRAINT [FK_Cognates_DerivedWord] FOREIGN KEY([DerivedWordId]) REFERENCES [dbo].[Words] ([Id]),
    CONSTRAINT [AK_BasicWordId_DerivedWordId] UNIQUE ([BasicWordId], [DerivedWordId])
);
```

---

## Висновок

Базу даних FirstWordsAnalyzer спроектовано для ефективного аналізу етимологічних зв'язків між словами. Вона використовує рекурсивні алгоритми для побудови ланцюжків спільнокореневих слів та забезпечує контекстний аналіз через зв'язки з реченнями та книгами. Структура підтримує як базові CRUD операції, так і складну аналітику через stored procedures та views.