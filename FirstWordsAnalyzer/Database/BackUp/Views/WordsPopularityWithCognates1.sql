USE [FirstWordsAnalyzer]
GO

/****** Object:  View [dbo].[WordsPopularityWithCognates1]    Script Date: 7/19/2025 1:05:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Script for SelectTopNRows command from SSMS  ******/
Create View [dbo].[WordsPopularityWithCognates1]

AS

SELECT WP.[WordId]
	  ,W.Text
	  ,W.FirstTranslationVariant
	  ,W.SecondTranslationVariant
	  ,W.ThirdTranslationVariant
      ,COALESCE(BWP.[Quantity],WP.[Quantity]) AS Quantity
	  ,BWP.[Quantity] - WP.[Quantity] AS Differance
  FROM [WordsPopularity] AS WP
  Left Join [BaseWordsPopularity] AS BWP
  On WP.[WordId] = BWP.[BasicWordId]
  Left Join [dbo].[Words] AS W
  On WP.Wordid = W.Id
  Where WordId not in(Select DerivedWordId From [FirstWordsAnalyzer].[dbo].[Cognates])
GO


