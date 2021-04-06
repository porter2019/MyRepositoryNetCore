if exists (select 1
            from  sysobjects
           where  id = object_id('BookInfoView')
            and   type = 'V')
   drop view BookInfoView
go

create view BookInfoView as 
select a.*,b.Title as CategoryName,b.FullId as CategoryFullId 
from BookInfo as a left join BookCategory as b on a.CategoryId = b.CategoryId

go