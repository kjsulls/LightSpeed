with results as
(
  select g.id as group_id,
         g.name as group_name,
         m.user_id as [user_id]
  from     memberships m, groups g
  where     m.group_id = g.id
  and     m.group_id in (select g1.id from groups g1 where g1.name = 'GroupA' or g1.name = 'GroupB')

  union

  select g.id as group_id,
         g.name as group_name,
         m.user_id as [user_id]
  from     memberships m, groups g
  where     m.group_id = g.id
  and     m.group_id in (select g1.id from groups g1 where g1.name = 'GroupA' and g1.name = 'GroupB')
 
)
select    r.group_id,
        r.group_name,
        u.id as [user_id],
        u.fName,
        u.lName
from    results r, users u
where    r.[user_id] = u.id
and        u.fName = 'User'
and        u.lName = 'A'