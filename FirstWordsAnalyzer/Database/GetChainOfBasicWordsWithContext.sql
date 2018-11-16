USE [FirstWordsAnalyzer]
GO

/****** Object:  StoredProcedure [dbo].[GetChainOfBasicWordsWithContext]    Script Date: 11/16/2018 7:22:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[GetChainOfBasicWordsWithContext]
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

  Select CCTE.Id, CCTE.BasicWordId, CCTE.DerivedWordId, WWC.WordText AS BasicWordText, WWC.FirstTranslationVariant AS BasicWordFirstTranslationVariant, WWC.SentenceId AS BasicWordSentenceId, WWC.SentenceText AS BasicWordSentenceText
  From CognatesCTE AS CCTE
  Join [dbo].[WordsWithContext] AS WWC
  On CCTE.BasicWordId = WWC.WordId
  Order By CCTE.distance, CCTE.BasicWordId

  END



GO


