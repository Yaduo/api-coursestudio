# TODO list #
Domain Even, ApplicationEvent
1. 需要做持久化，event记录。
2. 新的 根聚合entity 加入：EventRecords {Id, Name, CreateTime, Notes }
2. 需要 BaseDomainEventHandler类, 以及BaseApplicationEventHandler类。

Identity Module
1. identity
	✓ use Email & Password
	✓ email verification
	- user block and reactive by email

2. user infomation
	- Complete User infomation notification
	- update user info
	- delete/inactive user

3. Role/UserType management
	- seperate Tutor/Student type with ApplicationUser 
	- change user type
	- Tutor: playlist, teachinglist
	- student: playlist
	- role update: student can request to be a tutor, student is able to become a tutor, a tutor cannot become a student

4. Tutor Personal Page
	- display the tutor public personal infomation
	- display the tutor teaching list
	- display the tutor's rating

Course Module
1. Course Info:
	#- add IsApproved to COURSE TABLE
	#- add IsActive to COURSE TABLE
	#- add (blob)CoverPage to COURSE TABLE
	#- change shortDescription to Subtitle
	#- change longDescription to Description
	- create RefferenceMaterials Table, and add RefferenceMaterials to COURSE Model
	- create Prerequests Table, and add Prerequests to COURSE Model
	- create TargetStudent Table???? dont implement, Maybe no need
 
2. Course Attribute:
	#- Table Change:
		#* Courses Table: (id, Title ....)
		#* EntityAttributes Table: (id, Name)
		#* EntityAttributeValue Table: (id, Value, AttributeId, ParentId, IsSearchable)
		#* CourseAttribute Table: (id, CourseId, ValueId)
	#- Static Data:
		#* Attributes Table: 
		#	id		Name			
		#	1		Institution		
		#	2		Subject		
		#	3		CourseIdentity		
		#	4		Level		
		#	5		Region		
		#	6		Professor
		#	7		Objective
		#* AttributeValue Table: 
		#	id		Value				AttributeId		ParentId	IsSearchable	CourseTypeId
		#	1		SFU					1				null		true			1 (College)
		#	2 		UBC					1				null		false			1 (College)
		#	3		CPA					1				null		true			3 (Certification)
		#	4		Computer Science	2				1			true			1 (College)
		#	5		Economic			2				1			false			1 (College)	
		#	6		Computer Science	2				2			false			1 (College)
		#	7		Accounting			2				3			true			3 (Certification)
		#	8		CMPT 120			3				4			true			1 (College)
		#	9		Accounting 2		3				7			true			3 (Certification)
		#	10		100					4				1			true			1 (Colle
		#	11		Level 12			4				null		true			2 (HighSchoo
		#	12		BC					5				null		true			2 (HighSchool)
		#	13		Steven Pierce		6				8			true			1 (College)
	
3. Course Create: 
	- step 1: tutor/admin complete Course infomation:
		#	* Title
		#	* Subtitle
		#	* Course Type Relate Attributes in Multilevel dropdown list
		#	* Description
			* Course Image
	- step 2: tutor/admin complete Course Goals:
			* Prerequest List
			* Target Student List
			* Necessary Background List
	- step 3: tutor/admin create Curriculum:
		#	* Curriculum contains Section List
		#	* Section contain lecture List
		#	* Lecture contains one video 
			* lecture video need to be uploaded to server
	- step 4: tutor/admin create Automatic Message:
			* once a user enroll for this course, system will send the Welcome email to the user, the contain is Automatic Message.
4. Course Update:
	- same step as course Creation
5. # Curriculum Update:
	# - section, lecture, Video, CC
6. # Course Release: admin approve the course, then tutor/admin active the course, then user can search or see this course
7. # Course Unrelease: admin is able to unrelease the course
8. Course Delete
9. Get Curriculum:
	- if course owner or admin need to update Curriculum, they are able to get the full list of Curriculum with video 
	- otherwise, it will always return Curriculum without video
	- if a student want to watch video, they must call api to get vide
10. Get Video:
	- course owner or admin are able to get videos from course
	- student is able to get the videos after the purchase the course

		 
		
		
			
			
													    
					  


