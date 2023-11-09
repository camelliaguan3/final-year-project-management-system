/* Camellia Guan 21026514 */

/* COMP 3311: Task 2 - Final Year Project Management System: Tables */

clear screen
set feedback off


drop table RequirementGrades;
drop table CSEStudent;
drop table InterestedIn;
drop table ProjectGroup;
drop table Supervises;
drop table FYP;
drop table Faculty;

/* -------------------------------------------------------------------------- */
/* Creating Tables */
/* -------------------------------------------------------------------------- */

create table Faculty (
    username    char(15) primary key,
    firstName   varchar2(15) not null,
    lastName    varchar2(25) not null,
    roomNo      char(5) not null,
    facultyCode char(2) not null
);

create table FYP (
    fypId             smallint primary key,
    title             varchar2(100) not null,
    description       varchar2(1200) not null,
    category          varchar2(30) not null,
    type              char(7) not null,
    otherRequirements varchar2(200) not null,
    minStudents       smallint default 1 not null,
    maxStudents       smallint default 1 not null,
    status            char(11) default 'available' not null
);

create table Supervises (
    username char(15),
    fypId    smallint,
    primary key ( username, fypId )
);

create table ProjectGroup (
    groupId       smallint primary key,
    groupCode     char(5) not null,
    assignedFypId smallint,
    reader        char(15)
);

create table InterestedIn (
    fypId    smallint,
    groupId  smallint,
    priority smallint not null,
    primary key ( fypId, groupId ),
    foreign key ( fypId ) references FYP ( fypId )
        on delete cascade,
    foreign key ( groupId ) references ProjectGroup ( groupId )
        on delete cascade
);

create table CSEStudent (
    username  char(15) primary key,
    firstName varchar2(15) not null,
    lastName  varchar2(25) not null,
    groupId   smallint,
    foreign key ( groupId )
        references ProjectGroup ( groupId )
            on delete cascade
);

create table RequirementGrades (
    facultyUsername char(15),
    studentUsername char(15),
    proposalReport  number(4, 1) not null,
    progressReport  number(4, 1) not null,
    finalReport     number(4, 1) not null,
    presentation    number(4, 1) not null,
    primary key ( facultyUsername, studentUsername ),
    foreign key ( facultyUsername ) references Faculty ( username )
        on delete cascade,
    foreign key ( studentUsername ) references CSEStudent ( username )
        on delete cascade
);


/* -------------------------------------------------------------------------- */
/* Adding Check and Unique Constraints */
/* -------------------------------------------------------------------------- */

/* Faculty */
alter table Faculty
    add constraint FAC_facultyCode 
        check ( regexp_like ( facultyCode, '^[A-Z]{2}\s*$' ) );
    
alter table Faculty
    add constraint FAC_roomNo 
        check ( regexp_like ( roomNo, '^[0-9]{4}\s*$' ) 
             or regexp_like ( roomNo, '^[0-9]{4}[A-Z]\s*$' ) );
                                   
alter table Faculty
    add constraint FAC_username 
        check ( regexp_like ( username, '^[a-z]{3,15}\s*$' ) );
    
alter table Faculty add constraint UNIQUE_facultyCode unique ( facultyCode );

    
/* -------------------------------------------------------------------------- */ 
/* FYP */
alter table FYP
    add constraint FYP_status check ( status='available' or status='unavailable' );
    
alter table FYP
    add constraint FYP_minStudents check ( minStudents between 1 and maxStudents );
    
alter table FYP
    add constraint FYP_maxStudents check ( maxStudents between minStudents and 4 );
    
    
/* -------------------------------------------------------------------------- */

/* Supervises - looks like there are no additional constraints needed here */


/* -------------------------------------------------------------------------- */

/* ProjectGroup */
alter table ProjectGroup
    add constraint PRO_groupCode 
        check ( regexp_like ( groupCode, '^[A-Z]{2}[0-9]\s*$' )
             or regexp_like ( groupCode, '^[A-Z]{4}[0-9]\s*$' ) );

alter table ProjectGroup
    add constraint UNIQUE_groupCode unique ( groupCode );


/* -------------------------------------------------------------------------- */

/* InterestedIn */
alter table InterestedIn
    add constraint INT_priority check ( priority between 1 and 5 );

/* CSEStudent */
alter table CSEStudent
    add constraint CSE_username 
        check ( regexp_like ( username, '^[a-z]{3,15}\s*$' ) );
    
    
/* -------------------------------------------------------------------------- */

/* RequirementGrades */
alter table RequirementGrades 
    add constraint REQ_proposalReport check ( proposalReport between 0 and 100 );
    
alter table RequirementGrades 
    add constraint REQ_progressReport check ( progressReport between 0 and 100 );    
    
alter table RequirementGrades 
    add constraint REQ_finalReport check ( finalReport between 0 and 100 );
    
alter table RequirementGrades 
    add constraint REQ_presentation check ( presentation between 0 and 100 );
    
    
    
commit;