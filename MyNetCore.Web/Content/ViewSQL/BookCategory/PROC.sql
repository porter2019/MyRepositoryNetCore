if exists (select 1
            from  sysobjects
           where  id = object_id('sp_update_book_category_layer')
            and   type = 'P')
   drop view sp_update_book_category_layer
go

--更新树级层级关系
create proc sp_update_book_category_layer
as

with p as (
	select a.CategoryId, a.Title, a.OrderNo, 
	    cast('' as varchar(8000)) as ParentTitle, 						
	    cast('|' + cast(a.CategoryId as varchar) + '|' as varchar(8000)) as FullId, 
	    cast(a.Title as varchar(8000)) as FullTitle,						
	    1 as LevelNo,
	    cast(a.OrderNo as varchar(8000)) as FullOrderNo
	from BookCategory a where a.ParentId = 0
	union all
	select a.CategoryId, a.Title, a.OrderNo,
	    cast(p.Title as varchar(8000)) as ParentTitle,						
	    cast(p.FullId + cast(a.CategoryId as varchar) + '|' as varchar(8000)) as FullId, 
	    cast(p.FullTitle + '|' + a.Title as varchar(8000)) as FullTitle,
	    (p.LevelNo + 1) as LevelNo,
	    cast(p.FullOrderNo + '|' + a.OrderNo as varchar(8000)) as FullOrderNo
	from BookCategory a 
	inner join p on a.ParentId = p.CategoryId
	where a.CategoryId > 0
)

update q set 
    q.ParentTitle = p.ParentTitle, 					
    q.FullId = p.FullId, 
    q.FullTitle = p.FullTitle, 					
    q.LevelNo = p.LevelNo, 
    q.FullOrderNo = p.FullOrderNo
from BookCategory q 
inner join p on q.CategoryId = p.CategoryId

go