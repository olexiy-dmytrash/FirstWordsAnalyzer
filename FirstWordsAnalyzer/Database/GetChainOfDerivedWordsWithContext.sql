USE [FirstWordsAnalyzer]
GO
/****** Object:  StoredProcedure [dbo].[GetChainOfDerivedWordsWithContext]    Script Date: 11/16/2018 7:38:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROC [dbo].[GetChainOfDerivedWordsWithContext]
 @basicWordId AS INT
 
AS
BEGIN
SET NOCOUNT ON;  

WITH CognatesCTE AS
(SELECT [BasicWordId] AS WordId
	   ,[BasicWordId]
	   ,[DerivedWordId]
	   ,0 AS distance
	   ,Id
  FROM [FirstWordsAnalyzer].[dbo].ActualCognates AS UDW
  Where [BasicWordId] = @basicWordId 
  
  UNION ALL 

  Select B.WordId
	    ,D.BasicWordId
	    ,D.DerivedWordId
	    ,B.distance + 1
		,D.Id
  FROM CognatesCTE AS B 
  Join [FirstWordsAnalyzer].[dbo].ActualCognates AS D
  On B.DerivedWordId = D.BasicWordId)

  Select CCTE.Id, CCTE.BasicWordId, CCTE.DerivedWordId, CCTE.Distance, WP.Quantity, WWC.WordText, WWC.FirstTranslationVariant, WWC.SentenceId, WWC.SentenceText
  From CognatesCTE AS CCTE
  Join [dbo].[WordsPopularity] AS WP
  ON CCTE.DerivedWordId = WP.WordId
  Join [dbo].[WordsWithContext] AS WWC
  On CCTE.DerivedWordId = WWC.WordId
  Order By CCTE.distance, CCTE.BasicWordId, WP.Quantity DESC

  END

