-- List of groups ‘User A’ has a member in
select	distinct(g.id),
		g.name
from	groups g, users u, memberships m
where	g.id = m.group_id
and		u.id = m.[user_id]
and		u.fName = 'User'
and		u.lName = 'A'

-- Users with memberships in either GroupA or GroupB
select  distinct(u.id),
		u.fName,
		u.lName
from	memberships m, groups g, users u
where	m.group_id = g.id
and		u.id = m.[user_id]
and		(g.name = 'GroupA' or g.name = 'GroupB')


-- Users with memberships in both GroupA and GroupB
with results as
(
	select  u.id,
			u.fName,
			u.lName
	from	memberships m, groups g, users u
	where	m.group_id = g.id
	and		m.user_id = u.id
	and		g.name = 'GroupA'

union
	select  u.id,
			u.fName,
			u.lName
	from	memberships m, groups g, users u
	where	m.group_id = g.id
	and		m.user_id = u.id
	and		g.name = 'GroupB'
 
)
select * from results