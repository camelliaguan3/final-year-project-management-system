/* COMP 3311: Task 3 - Final Year Project (FYP) Management System: Sample Database */

clear screen
set termout off
set feedback off

alter table ProjectGroup drop constraint FK_ProjectGroup_InterestedIn;
drop table Supervises;
drop table RequirementGrades;
drop table InterestedIn;
drop table CSEStudent;
drop table ProjectGroup;
drop table FYP;
drop table Faculty;

set heading off
select '*** Creating FYPMS database ***' from dual;
set heading on

create table Faculty(
	username    char(15) primary key,
	firstName   varchar2(15) not null,
    lastName    varchar2(25) not null,
    roomNo      char(5) not null,
    facultyCode char(2) unique not null,
	constraint CHK_Faculty_username check (regexp_like(rtrim(username), '^[a-z]{3,15}$')),
    constraint CHK_Faculty_roomNo check (regexp_like(rtrim(roomNo), '^[0-9]{4}[A-Z]{0,1}$')),
    constraint CHK_Faculty_facultyCode check (regexp_like(rtrim(facultyCode), '^[A-Z]{2}$')));

create table FYP(
	fypId               smallint primary key,
	title               varchar2(100) not null,
    description         varchar2(1200) not null,
    category            varchar2(30) not null,
	type                char(7) not null,
    otherRequirements   varchar2(200),
    minStudents         smallint default 1 not null,
    maxStudents         smallint default 1 not null,
    status              char(11) default 'available' not null,
    constraint CHK_FYP_type check(type in ('project', 'thesis')),
    constraint CHK_FYP_min_max_Students check ((minStudents between 1 and maxStudents) 
                    and (maxStudents between minStudents and 4)),
    constraint CHK_FYP_status check (status in ('available', 'unavailable')));

create table ProjectGroup(
	groupId         smallint primary key,
	groupCode		char(5) unique,
	assignedFypId   smallint references FYP(fypId) on delete set null,
    reader          char(15) references Faculty(username) on delete set null,
    constraint CHK_ProjectGroup_groupCode check (regexp_like(rtrim(groupCode), '^[A-Z]{2,4}[1-9]{1}$')));

create table CSEStudent(
	username    char(15) primary key,
	firstName   varchar2(15) not null,
    lastName    varchar2(25) not null,
	groupId     smallint references ProjectGroup(groupId) on delete set null,
    constraint CHK_CSEStudent_username check (regexp_like(rtrim(username), '^[a-z]{3,15}$')));

create table InterestedIn(
	fypId       smallint references FYP(fypId) on delete cascade,
	groupId     smallint references ProjectGroup(groupId) on delete cascade,
    priority    smallint not null,
    constraint CHK_InterestedIn_priority check (priority in (1, 2, 3, 4, 5)),
	primary key(fypId,groupId));

create table RequirementGrades(
	facultyUsername char(15) references Faculty(username) on delete cascade,
	studentUsername char(15) references CSEStudent(username) on delete cascade,
	proposalReport  number(4,1),
    progressReport  number(4,1),
    finalReport     number(4,1),
    presentation    number(4,1),
    constraint CHK_Req_proposalReport check (proposalReport between 0 and 100),
    constraint CHK_Req_progressReport check (progressReport between 0 and 100),
    constraint CHK_Req_finalReport check (finalReport between 0 and 100),
    constraint CHK_Req_presentation check (presentation between 0 and 100),
	primary key(facultyUsername,studentUsername));
	
create table Supervises(
	username        char(15) references Faculty(username) on delete cascade,
	fypId           smallint references FYP(fypId) on delete cascade,
    primary key (username,fypId));

alter table ProjectGroup add constraint FK_ProjectGroup_InterestedIn
    foreign key (assignedFypId,groupId) references InterestedIn(fypId,groupId) on delete set null;

set termout on
set heading off
select '*** Populating the FYPMS database ***' from dual;
set heading on

insert into Faculty values ('cafarella','Michelle','Cafarella','3702','MC');
insert into Faculty values ('fan','Jim','Fan','3372','JF');
insert into Faculty values ('garcia','Holly','Garcia','3068','HG');
insert into Faculty values ('hui','Nancy','Hui','3556','NH');
insert into Faculty values ('jag','Hector','Jag','3923','HJ');
insert into Faculty values ('larson','Pauline','Larson','3588','PL');
insert into Faculty values ('naughton','Jack','Naughton','2628','JN');
insert into Faculty values ('pantel','Patty','Pantel','2345','PP');
insert into Faculty values ('parames','Agnes','Parames','3776','AP');
insert into Faculty values ('ray','Nelson','Ray','4178','NR');
insert into Faculty values ('ruden','Elke','Ruden','3158','ER');
insert into Faculty values ('soliman','Mary','Soliman','4116','MS');
insert into Faculty values ('chang','Gerry','Chan','3522','GC');
insert into Faculty values ('thorn','Martin','Thorn','3538','MT');
insert into Faculty values ('chank','Karl','Chan','3532','KC');
insert into Faculty values ('chanv','Vincent','Chan','3408','VC');

insert into FYP values (1,'Learn Golf Using Kinect','Microsoft Kinect allows a person''s skeletal movement to be tracked and to recognize the person''s speech. In this FYP, you will write an app using a Kinect or two Kinects (which the department will provide to you) to teach golfing strokes. You are required to track the golfing movements of a learner, compare the learner''s movements with those from a master golfer and give feedback to the learner so that he/she can correct his/her movements.','artificial intelligence','project','C++/C#; algorithm design; creative mind',1,2,'available');
insert into FYP values (2,'MOOC Data Analytics: Social Network Analysis of Discussion Forum Data','Massive open online courses (MOOCs) on such online platforms as Coursera, edX, Khan Academy and Udacity are perceived by many people as reinventing education to a certain extent. A consequence of this recent trend is the availability of massive amounts of data from MOOCs for research in learning analytics and other areas. This FYP will make use of discussion forum data involving tens of thousands of students from several HKUST courses offered on Coursera. Some machine learning problems related to social network analysis will be studied.','artificial intelligence','thesis','strong background in programming and mathematics; good background in mathematics is essential for learning the machine learning models; experience in programming on Linux/Unix platforms is a plus.',1,1,'unavailable');
insert into FYP values (3,'Mobile Action RPG Game','In this FYP you will design and implement an adventure computer game. You should propose an interesting game scenario (i.e., the story). It can be anything and does not have to be an adventure game, but must be interesting/funny/surprising. Simple ideas are often the best — like StoneAge UST a few years ago. Also, you need to draw some pictures that show how your game will look (e.g., a few main characters, scenes — can be hand-drawn or computer-drawn). Someone in the group needs to have the artistic skills to make the game attractive. Designing a fun and interesting game scenario is an important part of the FYP, as is implementing it. It doesn''t matter what software package is used to implement the game (Java, Flash, Visual Basic), but it needs to be fun and interesting to play and watch. It can be a 3D RPG game if you choose. An interesting 2D game is also okay (e.g., dragons and monsters in the library maze of stacks).','computer games','project','creativity',1,4,'available');
insert into FYP values (4,'Side-Scrolling Computer Game','This category of game has been changing how people think about "gaming" at least by defining a new way to "play". Besides the classics, there are some recent side-scrolling games which are also popular and successful, such as LIMBO. As we found this kind of game fun, we would like to develop our own side-scrolling game. In addition, you should also think about adding RPG like attributes/growth to the game.','computer games','project','',4,4,'unavailable');
insert into FYP values (5,'Mobile Turn-Based Strategy Game','The aim of this FYP is to create a Turn-Based Strategy Game (examples include Fire Emblem, Advanced Wars) in a fantasy universe. It will be a series of levels that should be randomly generated and valid maps, with plans to extend this to include a genetic algorithm that helps create levels.','computer games','project','creativity',1,3,'available');
insert into FYP values (6,'A Spatial-temporal Data Analytical System for Microblogs','With the advances in GPS-equipped handheld devices, microblogs have entered a new era where time/location can be attached to each posted microblog. Consequently, it is possible to perform various kinds of analysis to find interesting pattern in the posts. Examples include:' || chr(10) || '(1) What is the spatial distribution of microblogs posted between 8am and 9am in Hong Kong?' || chr(10) || '(2) How does the above distribution changes if we set the time window to 8pm to 9pm?' || chr(10) || '(3) At HKUST, what were the most popular keywords in the microblogs posted last month?' || chr(10) || '(4) During last week, how many microblogs contain the keywords "FYP"? How are they distributed in space?' || chr(10) || 'In this FYP you will build a spatial-temporal data analytical system for microblogs.','database','project','programming; algorithm design; visualization',2,4,'available');
insert into FYP values (7,'A Study of Social Network Analysis','Data mining (or knowledge discovery) can find interesting patterns from past history. Websites like Facebook provide information about the relationships between individuals. With this information, we can select "better" customers for promotions. This is because, if we promote the new product to an individual and he is satisfied about this new product, it is more likely that he will promote this new product to his friends. With this reasoning, the salesman will not need to promote to some of his friends and thus a lot of effort can be saved. In this FYP, we will study how to select some potential customers for marketing with the use of these websites.','database','project','good programming skills',1,1,'available');
insert into FYP values (8,'Build a Personal Internet TVBox','Build a personal, customizable Internet TV box using Raspberry PI.','embedded systems and software','project','good programming skills; Linux',2,3,'available');
insert into FYP values (9,'Customizable Surround Sound with Raspberry Pi','Using a raspberry Pi, the aim is to build a smart speaker that will allow different surround sound settings for the audio playback and direct the channels to the speakers accordingly.','embedded systems and software','project','',1,1,'available');
insert into FYP values (10,'Radically New Intelligent Controllers / User Interfaces for Electronic Music','Much of electronic music has focused on the generation and synthesis of sound, but to a real musician, what is even more important is being able to control the sound in highly expressive ways. Hardware controllers have been built to mimic the user interfaces of traditional acoustic instruments like pianos, guitars, drums, wind instruments and string instruments. However, new types of controllers such as multitouch screens, accelerometers, Kinect, etc. offer a far richer set of possibilities for NON-TRADITIONAL expressive control over electronic instruments. In this FYP, you will develop new AI-assisted methods that exploit these new technologies to offer musicians radically new real-time user interfaces.','human language technology','project','strong skills in one or more of the following: programming, linguistics, and/or mathematics',3,4,'available');
insert into FYP values (11,'Machine Learning of Chinese and English','This FYT aims to build and experiment with models that use machine learning and pattern recognition techniques to automatically learn human languages, specifically Chinese and English. On the one hand, this FYP gives excellent international exposure to practical state-of-the-art engineering techniques for machine learning, data mining and intelligent language processing technologies. On the other hand, this FYP also provides a solid introduction to one of the grand challenges of science: how the human mind works.','human language technology','thesis','strong skills in one or more of the following: programming, linguistics, and/or mathematics',1,1,'available');
insert into FYP values (12,'Dynamic Road Networks','Road networks are represented by directed graphs where the nodes and the edges correspond to road intersections and road segments, respectively. Dynamic road networks (DRNs) capture accurately the traffic in a road network by assigning time-dependent weights on the edges, according to the time of traversal. For instance, a vehicle might take 20 minutes to traverse a road segment in Mongkok at 10:00am (peak hour), whereas at 11:00pm (low congestion) it could take only 5 minutes. Due to the severe effect of traffic, the fastest path between two points in a DRN depends heavily on the trip time. In this final year FYP, you will extend known shortest-path and related methods in order to efficiently compute fastest paths in DRNs.','miscellaneous','project','programming skills; C++; algorithm design',3,4,'available');
insert into FYP values (13,'Social Distance Computation','Nowadays, the analysis of social networks is essential for numerous marketing and advertisement purposes. A major analytic tool is the computation of the social distance between two users of the social network. The social distance measures how socially close two individuals are to one another. The goal of this final year FYP is to implement and experimentally evaluate the existing methods on social distance computation.','miscellaneous','project','familiar with the C++ programming language and the Linux operating system',1,4,'available');
insert into FYP values (14,'Music Emotion and Timbre','Music is one of the strongest triggers of emotions. Melody, rhythm and harmony are important triggers, but what about timbre? Though music emotion recognition has received a lot of attention, researchers have only recently begun considering the relationship between emotion and timbre. Our group''s research has shown that musical instruments have an emotional predisposition for sustaining instruments and decaying instruments (e.g., the melancholy sound of the English horn). We have tested eight emotions and found strong emotional predispositions for each instrument. The emotions Happy, Joyful, Heroic and Comic were strongly correlated with one another and the violin, trumpet and clarinet best evoked these emotions. Sad and Depressed were also strongly correlated, and were best evoked by the horn and flute. Scary was an emotional outlier and the oboe had an emotionally neutral disposition. For this FYT you will follow up the above work. You will consider either the emotional dispositions of other instruments or you might consider the effect of algorithms such as MP3 compression on musical emotion and timbre or you can propose you own idea for music emotion and timbre.','miscellaneous','thesis','COMP4441; deep desire to combine music and CS; good ear for music and musical timbres; reasonably strong statistics background',1,1,'available');
insert into FYP values (15,'Emotion Sensing Using Smartphones','Affective computing tries to assign computers the human-like capabilities of observation, interpretation and generation of affect features. Nowadays, thanks to powerful smart devices, abundant sensors are available in our daily life that make it possible to collect information for affect detection (e.g., sound, facial expression, touch gesture, human movement, etc.). Consequently, it is possible not only to track real emotions, but also to significantly improve the accuracy of affect detection. In this FYP, you will develop a framework on a smartphone that understands user emotions and produces positive responses for help.','mobile and wireless computing','project','solid programming skill in Java or C++; experience in Android development',1,1,'available');
insert into FYP values (16,'Price Sharing Application','Supermarkets or chain stores in Hong Kong are always changing the retail price of their items in unpredictable ways. Consumers may want to know if a certain promotion really brings tangible benefits to them. In this FYP you will construct a database of the price for all the goods in the market. First you will develop an app for consumers to share the price of the items they bought. They only need to use the camera to record the barcode and input the price manually. Through the same app, people can view the price history of a certain item when they are shopping.','mobile and wireless computing','project','Android programming; database; operating system',2,3,'available');
insert into FYP values (17,'Ride-sharing Mobile Application','Ride-sharing is a green initiative from the HKUST sustainability unit. The FYP involves creating mobile application support for ride sharing and server support for the program.','mobile applications','project','HTML5; Javascript; JQuery mobile',3,4,'available');
insert into FYP values (18,'Context-Aware War Game with Motion and Gesture Recognition','This FYP will develop a location-based, Android war game which allows players to interact with movements, gestures and NFC technologies. Players will be able to catch Pokémons and play games with each other. The scenes can be sensitive to time, weather and locations.','mobile gaming','project','Android; database; sensor programming; NFC; game design',1,4,'available');
insert into FYP values (19,'Puzzle and Action RPG Game for Mobile Devices','The most popular mobile game type nowadays is a puzzle game like Candy Crush Saga, targeted at people of all ages, and Puzzle and Dragons targeted at teenagers. We can see that the attractive point of these games is the puzzle solving system and the amazing combo moment. In addition, some types of games have already been popular for at least two decades. The representatives of this type of game are action RPG games. Combining these attractive elements, you will develop a Puzzle + Action RPG Game.','mobile gaming','project','programming; mobile computing',1,3,'unavailable');
insert into FYP values (20,'Time-travel on the Internet','In this FYP, students will develop an Internet archiving system to crawl the Internet similar to a search engine. Moreover, the system will store numerous copies of the web pages as the pages evolve over time. With this dynamically growing archive, users can "time travel" in the archive and search information that cannot be provided by existing search engines. After completing this FYP, the students will have good understanding of Internet technology and will have acquired skills for developing advanced user-oriented network systems.','networking','project','C/C++ programming; web programming; ability to use UNIX/Linux',1,4,'available');
insert into FYP values (21,'Wi-Fi Channel Optimizer for Android','This is a Wi-Fi FYP to implement an Android app which finds the optimal Wi-Fi channel when deploying a wireless router at home or office. An AP can usually automatically assign a least congested channel based on the startup scan result. Such a scan result reflects the environment in which the AP is located. However, this channel may not be the best channel for the users in different locations, such as in the bedroom with totally different interference characteristics. Therefore, it is better to also scan the interference at the user location. This app scans the wireless networks at multiple positions, analyzes the scan results with channel selection algorithms and suggests the best channel for the network.','networking','project','Java; general Wi-Fi knowledge; Android SDK',4,4,'available');
insert into FYP values (22,'Optimizing Compiler for Distributed Computing','Compiler optimization requires a tremendous amount of understanding in the high-level language and the underlying computer abstraction. It is more challenging to develop an optimizing compiler for a distributed platform. In this FYP, the student will design and implement optimization schemes for the C0 compiler, generate code for the i0 architecture and measure the improvement on the CCMR cloud computing test bed.','operating systems','project','',2,3,'available');
insert into FYP values (23,'A Chatbot for Learning Programming','This FYP aims to develop a simple chatbot for learning computer programming. It should be a Web-based environment with a collection of tools that allow students to easily and effectively self-learn computer programming skills.','software technology','project','AI skills, Internet programming',1,1,'available');
insert into FYP values (24,'HKUST Class Radar','From time to time, instructors would like to invite students to answer questions in class. The aim of this FYP is to develop a mobile app displaying a radar of students attending the class. This will facilitate the instructor to invite a particular student (especially those sitting at the back) to answer class questions. Instructors may preload their course enrollment and classroom configuration. The app also enables students to team up for class discussion in blended learning lectures.','software technology','project','COMP3111; Java; Android',3,4,'available');
insert into FYP values (25,'Predicting What People Will Talk About Tomorrow','This FYP aims to develop a system, either browser based or app based, to predict what people will talk about on social networks (e.g., Twitter). What the system should predict includes, but is not limit to: (i) what would they be talking about tomorrow, or the day after tomorrow, etc. and (ii) if they do talk about a topic, for how long and at what intensity (e.g., in terms of number of messages posted per hour). The method on hand is to extract the topics that people have talked about in the past, analyze their characteristics (e.g., are these just short-term interests that will die down very soon or they are hot topics that will last for a couple of weeks or they will recur periodically, like world soccer cup, every four years). Therefore, before you leave home, you can take a look at the screen and be "prepared" for what to talk about when you see your friends.','software technology','project','algorithm design; Java; interest in doing research',2,4,'available');
insert into FYP values (26,'Real-world Application Development','In this FYP your group will develop software (e.g., a website, an app, etc.) for a real company using software engineering and database management technology. You will have to find a suitable company that will allow you to carry out the FYP. Examples of possible applications might be managing customer records, managing inventory, tracking orders and sales, managing the membership of an organization, scheduling items for shipment or delivery, providing web access (either to the public or to other companies) to a database of the items that a company sells. This FYP will give you an opportunity to apply and integrate things you have learned in various courses to a real-world problem.','software technology','project','COMP3111 or COMP 3111H; COMP 3311; willing to work with users in a company',2,4,'available');
insert into FYP values (27,'Many-core Parallel Computing','Commodity processors have become parallel computing platforms involving hundreds of cores. This FYT will study the state of the art in many-core parallel computing and pick a smaller topic for further investigation.','software technology','thesis','fast learner',1,1,'available');
insert into FYP values (28,'Streaming Algorithms','For this FYT on streaming algorithms, the student should have strong skills in algorithm design and implementation, as well as mathematics.','theory','thesis','COMP3711 or COMP3711H; algorithm design; programming; mathematics',1,1,'unavailable');
insert into FYP values (29,'3D City Reconstruction from Images','This FYP will investigate a methodology for the large-scale 3D reconstruction of cities from ground-level images. The goal is to produce detailed geometry and appearance that is well-suited for displaying as "street views". The FYP will provide key components for platforms dedicated to emerging 3D maps and digital city applications, greatly improving the current representation based on 2D panoramas.','vision and graphics','project','C++ programming skills',4,4,'available');
insert into FYP values (30,'Photo Repairing','Inpainting is originally a technique commonly used by conservators to unify a painting that has suffered paint loss. This FYP aims at analyzing various inpainting techniques and developing a real-world application for photo repairing (e.g., fixing damaged photos).','vision and graphics','project','some background in mathematics and good programming skills.',2,3,'unavailable');
insert into FYP values (31,'When Augmented Reality Meets Big Data','With computing and sensing woven into the fabric of every day life, we are now awash in a flood of data from which we can gain rich insights. Augmented reality (AR) is able to collect and help analyze the growing torrent of data about user engagement metrics within our personal mobile and wearable devices. It enables us to blend information from our senses and digitalized world in myriad ways that were not possible before. It is becoming common to see AR integrated with big data to birth interesting new applications. In this project, you will explore the potential to capture value from the marriage between AR and big data technologies, encountering several challenges that must be addressed to fully realize this potential.','mobile and wireless computing','project','Java (Android); algorithm design; data mining',2,3,'unavailable');
insert into FYP values (32,'A Personal Media Library for iOS Devices','In this project you will build a personal media library for iOS devices.','mobile and wireless computing','project','hard work; motivation',2,3,'available');
insert into FYP values (33,'Moodle Multimedia Tools','Moodle is an open-source Learning Management System. In this project you need to create multimedia tools in Moodle using the HTML5 multimedia recording capability. For example, you can create a kind of online assignment which requires the students to record their voice. You can also create a multimedia plugin for a discussion forum so that teachers and students can communicate using video recordings.','software technology','project','JavaScript and PHP programming',2,2,'unavailable');

insert into ProjectGroup values (1,null,null,null);
insert into ProjectGroup values (2,null,null,null);
insert into ProjectGroup values (3,null,null,null);
insert into ProjectGroup values (4,null,null,null);
insert into ProjectGroup values (5,null,null,null);
insert into ProjectGroup values (6,null,null,null);
insert into ProjectGroup values (7,null,null,null);
insert into ProjectGroup values (8,null,null,null);
insert into ProjectGroup values (9,null,null,null);
insert into ProjectGroup values (10,null,null,null);
insert into ProjectGroup values (11,null,null,null);
insert into ProjectGroup values (12,null,null,null);
insert into ProjectGroup values (13,null,null,null);
insert into ProjectGroup values (14,null,null,null);
insert into ProjectGroup values (15,null,null,null);
insert into ProjectGroup values (16,null,null,null);
insert into ProjectGroup values (17,null,null,null);
insert into ProjectGroup values (18,null,null,null);
insert into ProjectGroup values (19,null,null,null);
insert into ProjectGroup values (20,null,null,null);
insert into ProjectGroup values (21,null,null,null);
insert into ProjectGroup values (22,null,null,null);
insert into ProjectGroup values (23,null,null,null);
insert into ProjectGroup values (24,null,null,null);

insert into CSEStudent values ('brunoho','Bruno','Ho',1);
insert into CSEStudent values ('daisyyeung','Daisy','Yeung',2);
insert into CSEStudent values ('adamau','Adam','Au',3);
insert into CSEStudent values ('lesterlo','Lester','Lo',4);
insert into CSEStudent values ('shirleysit','Shirley','Sit',5);
insert into CSEStudent values ('frankfung','Frank','Fung',6);
insert into CSEStudent values ('larrylai','Larry','Lai',6);
insert into CSEStudent values ('bradybond','Brady','Bond',7);
insert into CSEStudent values ('vivianso','Vivian','So',7);
insert into CSEStudent values ('fredfan','Fred','Fan',8);
insert into CSEStudent values ('timothytu','Timothy','Tu',8);
insert into CSEStudent values ('jennyjones','Jenny','Jones',8);
insert into CSEStudent values ('kathyma','Kathy','Ma',9);
insert into CSEStudent values ('monicama','Monica','Ma',9);
insert into CSEStudent values ('susansze','Susan','Sze',9);
insert into CSEStudent values ('brianma','Brian','Ma',9);
insert into CSEStudent values ('terrytam','Terry','Tam',10);
insert into CSEStudent values ('sharonsu','Sharon','Su',11);
insert into CSEStudent values ('wendywong','Wendy','Wong',12);
insert into CSEStudent values ('ireneip','Irene','Ip',13);
insert into CSEStudent values ('peterpoon','Peter','Poon',13);
insert into CSEStudent values ('tiffanytan','Tiffany','Tan',14);
insert into CSEStudent values ('dannydoan','Danny','Doan',14);
insert into CSEStudent values ('victoriayu','Victoria','Yu',14);
insert into CSEStudent values ('carolchen','Carol','Chen',15);
insert into CSEStudent values ('cindychan','Cindy','Chan',15);
insert into CSEStudent values ('yvonneyu','Yvonne','Yu',15);
insert into CSEStudent values ('tracytse','Tracy','Tse',15);
insert into CSEStudent values ('alanseto','Alan','Seto',16);
insert into CSEStudent values ('clintchu','Clint','Chu',17);
insert into CSEStudent values ('amandahui','Amanda','Hui',17);
insert into CSEStudent values ('stevielam','Stevie','Lam',18);
insert into CSEStudent values ('rezanlam','Rezan','Lam',18);
insert into CSEStudent values ('lucylam','Lucy','Lam',18);
insert into CSEStudent values ('henryho','Henry','Ho',19);
insert into CSEStudent values ('tonytong','Tony','Tong',19);
insert into CSEStudent values ('walterwu','Walter','Wu',19);
insert into CSEStudent values ('xavierxie','Xavier','Xie',19);
insert into CSEStudent values ('hughhaw','Hugh','Haw',20);
insert into CSEStudent values ('carlchan','Carl','Chan',21);
insert into CSEStudent values ('lucyliu','Lucy','Liu',21);
insert into CSEStudent values ('henryhe','Henry','He',22);
insert into CSEStudent values ('brianhe','Brian','He',22);
insert into CSEStudent values ('emilyhe','Emily','He',22);
insert into CSEStudent values ('owenor','Owen','Or',23);
insert into CSEStudent values ('dennisdong','Dennis','Dong',23);
insert into CSEStudent values ('sophiesuen','Sophie','Suen',23);
insert into CSEStudent values ('angusau','Angus','Au',23);
insert into CSEStudent values ('pattypower','Patty','Power',24);
insert into CSEStudent values ('arigrand','Ari','Grand',null);
insert into CSEStudent values ('maycaly','May','Caly',null);
insert into CSEStudent values ('alstein','Al','Stein',null);
insert into CSEStudent values ('larrymo','Larry','Mo',null);
insert into CSEStudent values ('brucewane','Bruce','Wane',null);
insert into CSEStudent values ('stevemo','Steve','Mo',null);
insert into CSEStudent values ('billgates','Bill','Gates',null);
insert into CSEStudent values ('elonmusk','Elon','Musk',null);
insert into CSEStudent values ('nikitesla','Niki','Tesla',null);
insert into CSEStudent values ('alturing','Al','Turing',null);
insert into CSEStudent values ('zoeymo','Zoey','Mo',null);

insert into InterestedIn values (1,6,1);
insert into InterestedIn values (1,12,1);
insert into InterestedIn values (1,16,4);
insert into InterestedIn values (2,1,2);
insert into InterestedIn values (2,2,1);
insert into InterestedIn values (2,3,1);
insert into InterestedIn values (3,3,5);
insert into InterestedIn values (3,11,2);
insert into InterestedIn values (3,12,1);
insert into InterestedIn values (3,15,3);
insert into InterestedIn values (3,16,2);
insert into InterestedIn values (4,9,1);
insert into InterestedIn values (4,15,5);
insert into InterestedIn values (5,3,4);
insert into InterestedIn values (5,4,1);
insert into InterestedIn values (5,5,1);
insert into InterestedIn values (5,13,2);
insert into InterestedIn values (5,16,3);
insert into InterestedIn values (5,17,2);
insert into InterestedIn values (5,18,5);
insert into InterestedIn values (5,24,1);
insert into InterestedIn values (6,7,3);
insert into InterestedIn values (6,18,2);
insert into InterestedIn values (7,2,2);
insert into InterestedIn values (7,10,3);
insert into InterestedIn values (7,11,1);
insert into InterestedIn values (8,8,3);
insert into InterestedIn values (8,13,1);
insert into InterestedIn values (8,17,1);
insert into InterestedIn values (9,16,1);
insert into InterestedIn values (10,9,3);
insert into InterestedIn values (10,15,3);
insert into InterestedIn values (11,1,1);
insert into InterestedIn values (11,2,2);
insert into InterestedIn values (11,3,3);
insert into InterestedIn values (12,8,2);
insert into InterestedIn values (13,10,5);
insert into InterestedIn values (16,13,2);
insert into InterestedIn values (16,14,1);
insert into InterestedIn values (16,17,2);
insert into InterestedIn values (16,18,1);
insert into InterestedIn values (15,16,3);
insert into InterestedIn values (17,15,4);
insert into InterestedIn values (18,3,3);
insert into InterestedIn values (18,5,2);
insert into InterestedIn values (18,8,4);
insert into InterestedIn values (18,11,1);
insert into InterestedIn values (19,13,3);
insert into InterestedIn values (19,14,1);
insert into InterestedIn values (19,17,4);
insert into InterestedIn values (19,18,3);
insert into InterestedIn values (20,11,3);
insert into InterestedIn values (20,13,4);
insert into InterestedIn values (20,14,2);
insert into InterestedIn values (20,18,2);
insert into InterestedIn values (21,15,2);
insert into InterestedIn values (23,10,1);
insert into InterestedIn values (24,9,2);
insert into InterestedIn values (25,13,5);
insert into InterestedIn values (25,17,5);
insert into InterestedIn values (25,19,2);
insert into InterestedIn values (26,6,1);
insert into InterestedIn values (26,15,1);
insert into InterestedIn values (26,18,1);
insert into InterestedIn values (26,19,3);
insert into InterestedIn values (28,12,2);
insert into InterestedIn values (29,9,4);
insert into InterestedIn values (29,19,1);
insert into InterestedIn values (30,6,2);
insert into InterestedIn values (30,8,1);
insert into InterestedIn values (33,7,1);
insert into InterestedIn values (33,13,3);
insert into InterestedIn values (33,17,3);

insert into RequirementGrades values ('ray','brunoho',60,70,80,90);
insert into RequirementGrades values ('naughton','lesterlo',66,72,75,80);
insert into RequirementGrades values ('cafarella','lesterlo',60,70,75,80);
insert into RequirementGrades values ('jag','bradybond',72,88,78,null);
insert into RequirementGrades values ('naughton','vivianso',75,77,null,null);
insert into RequirementGrades values ('naughton','brianma',77,84,81,null);
insert into RequirementGrades values ('naughton','kathyma',78,83,81,null);
insert into RequirementGrades values ('naughton','monicama',79,82,81,null);
insert into RequirementGrades values ('naughton','susansze',80,81,81,null);
insert into RequirementGrades values ('fan','brianma',72,83,79,null);
insert into RequirementGrades values ('fan','kathyma',71,84,80,null);
insert into RequirementGrades values ('fan','monicama',72,83,78,null);
insert into RequirementGrades values ('fan','susansze',71,85,77,null);
insert into RequirementGrades values ('hui','terrytam',75,67,72,77);
insert into RequirementGrades values ('naughton','terrytam',70,65,75,80);
insert into RequirementGrades values ('parames','tiffanytan',77,81,null,72);
insert into RequirementGrades values ('parames','victoriayu',77,81,null,76);
insert into RequirementGrades values ('parames','dannydoan',null,null,null,null);

insert into Supervises values ('cafarella',1);
insert into Supervises values ('cafarella',2);
insert into Supervises values ('cafarella',11);
insert into Supervises values ('cafarella',12);
insert into Supervises values ('fan',29);
insert into Supervises values ('fan',30);
insert into Supervises values ('garcia',17);
insert into Supervises values ('garcia',18);
insert into Supervises values ('garcia',23);
insert into Supervises values ('hui',13);
insert into Supervises values ('hui',22);
insert into Supervises values ('jag',5);
insert into Supervises values ('jag',6);
insert into Supervises values ('jag',8);
insert into Supervises values ('jag',9);
insert into Supervises values ('jag',24);
insert into Supervises values ('jag',26);
insert into Supervises values ('jag',33);
insert into Supervises values ('naughton',3);
insert into Supervises values ('naughton',4);
insert into Supervises values ('naughton',5);
insert into Supervises values ('naughton',7);
insert into Supervises values ('naughton',19);
insert into Supervises values ('naughton',31);
insert into Supervises values ('naughton',32);
insert into Supervises values ('naughton',33);
insert into Supervises values ('pantel',8);
insert into Supervises values ('pantel',9);
insert into Supervises values ('pantel',12);
insert into Supervises values ('pantel',21);
insert into Supervises values ('pantel',22);
insert into Supervises values ('parames',15);
insert into Supervises values ('parames',16);
insert into Supervises values ('parames',18);
insert into Supervises values ('parames',20);
insert into Supervises values ('ray',10);
insert into Supervises values ('ruden',14);
insert into Supervises values ('ruden',28);
insert into Supervises values ('soliman',21);
insert into Supervises values ('soliman',25);
insert into Supervises values ('soliman',27);

update ProjectGroup set groupCode='MC1',assignedFypId=2,reader='ray' where groupId=1;
update ProjectGroup set groupCode='JN1',assignedFypId=4,reader='fan' where groupId=9;
update ProjectGroup set groupCode='HJJN1',assignedFypId=5, reader='hui' where groupId=3;
update ProjectGroup set groupCode='HJJN2',assignedFypId=5,reader='cafarella' where groupId=4;
update ProjectGroup set groupCode='HJJN3',assignedFypId=5 where groupId=5;
update ProjectGroup set groupCode='JN2',assignedFypId=7,reader='larson' where groupId=11;
update ProjectGroup set groupCode='MC2',assignedFypId=11,reader='ray' where groupId=2;
update ProjectGroup set groupCode='NH1',assignedFypId=13,reader='naughton' where groupId=10;
update ProjectGroup set groupCode='AP1',assignedFypId=16 where groupId=14;
update ProjectGroup set groupCode='AP2',assignedFypId=20 where groupId=13;
update ProjectGroup set groupCode='HJ1',assignedFypId=26,reader='ruden' where groupId=6;
update ProjectGroup set groupCode='HJ2',assignedFypId=26,reader='naughton' where groupId=15;
update ProjectGroup set groupCode='ER1',assignedFypId=28,reader='naughton' where groupId=12;
update ProjectGroup set groupCode='JF1',assignedFypId=30,reader='naughton' where groupId=8;
update ProjectGroup set groupCode='HJJN4',assignedFypId=33 where groupId=7;

set feedback on
commit;