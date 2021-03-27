if exists (select 1
            from  sysobjects
           where  id = object_id('SysUserView')
            and   type = 'V')
   drop view SysUserView
go

create view SysUserView as 
select a.[UserId]
      ,a.[LoginName]
      ,a.[UserName]
      ,'@@**@@' as [Password]
      ,a.[Status]
      ,a.[CreatedUserId]
      ,a.[CreatedUserName]
      ,a.[CreatedDate]
      ,a.[IsDeleted]
      ,a.[Version]
,(
select ISNULL(
--STUFF(
(select ','+Cast(b.RoleId as varchar),';' + c.RoleName from SysRoleUser as b inner join SysRole as c on b.RoleId = c.RoleId and b.UserId = a.UserId for xml path(''))
--,1,1,'')
,'') as RoleInfo
) as RoleInfo from SysUser as a

go