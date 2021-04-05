if exists (select 1
            from  sysobjects
           where  id = object_id('fn_GetCategoryParentIds')
            and   type = 'FN')
   drop function fn_GetCategoryParentIds
go

--获取父ID合集，逗号分隔
Create FUNCTION [dbo].[fn_GetCategoryParentIds] (@Id int) RETURNS varchar(1000) 
AS
BEGIN
declare @a VARCHAR(1000);
set @a='';
WITH Record AS(
    SELECT
    BookId,
    ParentId
FROM
    BookCategory(NOLOCK)
    WHERE BookId=@Id
    UNION ALL
        SELECT
            a.BookId BookId,
            a.ParentId ParentId
        FROM
            BookCategory(NOLOCK) a JOIN Record b
            ON a.BookId=b.ParentId
        )
SELECT @a=isnull(@a+',','')+ltrim(BookId) FROM Record  --WHERE Status=1  
return SUBSTRING(@a, 2, len(@a))
END

go

if exists (select 1
            from  sysobjects
           where  id = object_id('fn_GetCategoryChildIds')
            and   type = 'FN')
   drop function fn_GetCategoryChildIds
go

--获取子ID合集，逗号分隔
Create FUNCTION [dbo].[fn_GetCategoryChildIds] (@Id int) RETURNS varchar(1000) 
AS
BEGIN
declare @a VARCHAR(1000);
set @a='';
WITH Record AS(
    SELECT
    BookId,
    ParentId
FROM
    BookCategory(NOLOCK)
    WHERE BookId=@Id
    UNION ALL
        SELECT
            a.BookId BookId,
            a.ParentId ParentId
        FROM
            BookCategory(NOLOCK) a JOIN Record b
            ON a.ParentId=b.BookId
        )
SELECT @a=isnull(@a+',','')+ltrim(BookId) FROM Record  --WHERE Status=1  
return SUBSTRING(@a, 2, len(@a))
END

go