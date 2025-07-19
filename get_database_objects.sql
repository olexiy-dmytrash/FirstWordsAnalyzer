-- SQL скрипт для отримання всіх views та stored procedures з бази FirstWordsAnalyzer

USE FirstWordsAnalyzer;
GO

-- Отримання всіх views
PRINT '=== ALL VIEWS ==='
SELECT 
    'VIEW' as ObjectType,
    SCHEMA_NAME(schema_id) as SchemaName,
    name as ObjectName,
    create_date,
    modify_date
FROM sys.views
ORDER BY name;

PRINT ''
PRINT '=== ALL STORED PROCEDURES ==='
-- Отримання всіх stored procedures (виключаючи системні)
SELECT 
    'PROCEDURE' as ObjectType,
    SCHEMA_NAME(schema_id) as SchemaName,
    name as ObjectName,
    create_date,
    modify_date
FROM sys.procedures
WHERE is_ms_shipped = 0
ORDER BY name;

PRINT ''
PRINT '=== ALL FUNCTIONS ==='
-- Отримання всіх user-defined functions
SELECT 
    'FUNCTION' as ObjectType,
    SCHEMA_NAME(schema_id) as SchemaName,
    name as ObjectName,
    create_date,
    modify_date
FROM sys.objects
WHERE type IN ('FN', 'IF', 'TF') -- Scalar function, Inline table function, Table function
ORDER BY name;

PRINT ''
PRINT '=== VIEW DEFINITIONS ==='
-- Отримання визначень всіх views
SELECT 
    v.name as ViewName,
    m.definition as ViewDefinition
FROM sys.views v
INNER JOIN sys.sql_modules m ON v.object_id = m.object_id
ORDER BY v.name;

PRINT ''
PRINT '=== PROCEDURE DEFINITIONS ==='
-- Отримання визначень всіх stored procedures
SELECT 
    p.name as ProcedureName,
    m.definition as ProcedureDefinition
FROM sys.procedures p
INNER JOIN sys.sql_modules m ON p.object_id = m.object_id
WHERE p.is_ms_shipped = 0
ORDER BY p.name;