/****** Script for SelectTopNRows command from SSMS  ******/
With CTE AS
(
SELECT [BasicWordId]
      ,[DerivedWordId]
      ,[WordPart]
	  , Max(Id) AS Id
  FROM [FirstWordsAnalyzer].[dbo].[Cognates]
  Group By [BasicWordId], [DerivedWordId], [WordPart]
  Having Count(*) > 1)

 DELETE FROM [dbo].[Cognates]
 WHERE Id In(Select Id From CTE)