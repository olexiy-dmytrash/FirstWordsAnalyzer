USE [FirstWordsAnalyzer]
GO
/****** Object:  StoredProcedure [dbo].[GetChainOfBasicWordsWithContext]    Script Date: 11/16/2018 7:48:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[GetChainOfBasicWordsWithContext]
 @derivedWordId AS INT
 
AS
BEGIN
SET NOCOUNT ON;  

WITH CognatesCTE AS
(SELECT [DerivedWordId] AS WordId
	   ,[BasicWordId]
	   ,[DerivedWordId]
	   ,0 AS distance
	   ,Id
  FROM [FirstWordsAnalyzer].[dbo].ActualCognates AS UDW
  Where [DerivedWordId] = @derivedWordId 
  
  UNION ALL 

  Select D.WordId
	    ,B.BasicWordId
	    ,B.DerivedWordId
	    ,D.distance - 1
		,B.Id
  FROM CognatesCTE AS D 
  Join [FirstWordsAnalyzer].[dbo].ActualCognates AS B
  On D.BasicWordId = B.DerivedWordId)

  Select CCTE.Id, CCTE.BasicWordId, CCTE.DerivedWordId, CCTE.Distance, WP.Quantity, WWC.WordText, WWC.FirstTranslationVariant, WWC.SentenceId, WWC.SentenceText
  From CognatesCTE AS CCTE
  Join [dbo].[WordsPopularity] AS WP
  ON CCTE.BasicWordId = WP.WordId
  Join [dbo].[WordsWithContext] AS WWC
  On CCTE.BasicWordId = WWC.WordId
  Order By CCTE.distance, CCTE.BasicWordId

  END


