clear screen;

with numStudents as (select count(*) as totalStudents
                     from ProjectGroup natural join CSEStudent 
                     where groupId=5)
select distinct fypId, title, category, type, minStudents, maxStudents
from FYP natural left outer join InterestedIn, numStudents
where groupId!=5
      and totalStudents between minStudents and maxStudents 
      and status='available'
order by title asc;

select groupId, count(*) as totalStudents
from ProjectGroup natural join CSEStudent
group by groupId
order by groupId;