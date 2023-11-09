/* Camellia Guan 21026514 */

/* COMP 3311: Task 2 - Final Year Project Management System: Queries */

clear screen


/* QUERY 1: ----------------------------------------------------------------- */
/* 
For each project group that has been assigned a reader, find the group code, 
the title of the FYP to which the project group has been assigned and the name 
of the assigned reader. Order the result by group code ascending. Note: The first
and last names of a reader should be returned as a single value labelled READER.
*/

select groupCode, title, firstName || ' ' || lastName as "READER"
from ProjectGroup, FYP, Faculty
where (ProjectGroup.assignedFypId = FYP.fypId) 
    and reader is not null 
    and (ProjectGroup.reader = Faculty.username)
order by groupCode asc;


/* QUERY 2: ----------------------------------------------------------------- */
/*
For FYPs in the category "software technology" that have only one supervisor, 
find the title of the FYP and the username of the supervisor. Order the result 
by title ascending.
*/

with CountSup as
    (select fypId, count(*) as numSupervisor
    from Supervises
    group by fypId)
select title, username
from FYP natural join Supervises natural join CountSup
where category = 'software technology' and numSupervisor = 1
order by title asc;


/* QUERY 3: ----------------------------------------------------------------- */
/*
For each FYP project category, find the category name, the number of FYPs in 
that category and the number of project groups that have been assigned to FYPs 
in that category. If no project group has been assigned to an FYP in a project 
category, then 0 should be returned as the number of groups assigned. Order the
result first by the number of project groups in descending order and then by 
category in ascending order.
*/

select category, count(distinct fypId) as "NUMBER OF FYPS", 
    count(groupId) as "NUMBER OF GROUPS"
from FYP left outer join ProjectGroup on (FYP.fypId = ProjectGroup.assignedFypId)
group by category
order by count(groupId) desc, category asc;


/* QUERY 4: ----------------------------------------------------------------- */
/*
Find the FYP title, supervisor first and last names and first and last names of 
the students in the project group for those project groups that have exactly two
members and that have been assigned to an FYP for which they specified a priority
of 1. Order the result by title ascending. Note: For each supervisor, the first
and last names should be returned as a single value named SUPERVISORS; for each
project group member, the first and last names for each group member should be 
returned as a single value labelled GROUP MEMBERS.
*/

with CountStu as
    (select groupId, count(*) as numStudent
    from ProjectGroup natural join CSEStudent
    group by groupId)
select title, Faculty.firstName || ' ' || Faculty.lastName as "SUPERVISORS",
    CSEStudent.firstName || ' ' || CSEStudent.lastName as "GROUP MEMBERS"
from Faculty, Supervises, FYP, ProjectGroup, CSEStudent, InterestedIn, CountStu
where Faculty.username = Supervises.username 
    and Supervises.fypId = FYP.fypId
    and FYP.fypId = ProjectGroup.assignedFypId 
    and ProjectGroup.groupId = CSEStudent.groupId
    and ProjectGroup.groupId = InterestedIn.groupId
    and ProjectGroup.groupId = CountStu.groupId
    and priority = 1
    and numStudent = 2
order by title asc;


/* QUERY 5: ----------------------------------------------------------------- */
/*
For each faculty, find their first and last names, the number of different 
categories in which they have posted FYPs and the number of FYPs they have posted.
If a faculty has not posted any FYPs, then the number of FYPS and categories 
should be shown as 0. Order the result by faculty last name ascending. Note: The
first and last names of a faculty should be returned as a single value labelled 
FACULTY.
*/

select firstName || ' ' || lastName as "FACULTY", count(distinct category), count(distinct fypId)
from Faculty natural left outer join Supervises natural left outer join FYP
group by username, firstName, lastName
order by lastName asc;