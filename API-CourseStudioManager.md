# API Documentation #
-------------------------------- Identity 
1. Login (in JWT) 
    ** POST     api/token
    {
        "email": "yaduol@hotmail.com",
        "password": "Paragon123$"
    }



-------------------------------- Users 
1. Manully register a user (Create User) 
    ** POST     api/users
    {
        "email":"yaduol@hotmail.com"
        "password":"Paragon123$"
        "confirmPassword":"Paragon123$",
        "...other user infomation"
    }
    ** NOTE: 密码大小写字母数字符号都要有

2. Change user's password (Update)
    ** PUT     api/users/{userId}/password
    {
        "assword": xxxxxxxxxxxxxxx,
        "confirmPassword":"Paragon123$"
    }

3. Manully send Reg Confirm Email to a user
    ** POST     api/users/{userId}/reg-confirm-request
    empty body

3. Get users
    ** GET      api/users?pageNumber=1&pageSize=10

4. Get user detail
    ** GET      api/users/{userId}

5. Update user detail (everything)
    ** PUT    api/users/{userId}
    body: userDto

8. Get User‘s purchased course
    ** GET      api/users/{id}/purchases?pageNumber=1&pageSize=10

9. Get User‘s shoppingCart
    ** GET      api/users/{id}/shoppingCart

10. Get User‘s orders
    ** GET      api/users/{id}/orders

11. Get User‘s order detail
    ** GET      api/users/{id}/orders/{orderId}

-------------------------------- Tutor 
1. Get Tutors
    ** GET      api/tutors?pageNumber=1&pageSize=10

2. Get tutor detail
    ** GET      api/tutors/{id}

3. Deactive a tutor
    ** DELETE   api/tutors/{id}

3. Get tutor’s Teaching courses
    ** GET      api/tutor/{id}/teachings?pageNumber=1&pageSize=10

3. Update tutor’s resume
    ** PUT      api/tutor/{id}/resume

-------------------------------- Auditings 
//////// course auditings
1. Get course auditings
    ** GET      api/courseAuditings?pageNumber=1&pageSize=10&auditingState=1&needCourses=false

2. Get course auditing detail by id
    ** GET      api/courseAuditings/{id}

3. Update course auditing
    Action types: Start, Stop, Reject, Approve, Reopen
    ** PUT      api/courseAuditings/{id}
    {
        "action": {string}
        "comments": {string}
    }

//////// tutor auditings
4. Get tutor auditings
    ** GET      api/tutorAuditings?pageNumber=1&pageSize=10&auditingState=1&needCourses=false

2. Get course auditing detail by id
    ** GET      api/tutorAuditings/{id}

    3. Update course auditing
    Action types: Start, Stop, Reject, Approve, Reopen
    ** PUT      api/tutorAuditings/{id}
    {
        "action": {string}
        "comments": {string}
    }
    
-------------------------------- Course 
1. Get courses by keywords, attributes, tutor and paging
    ** GET      api/courses?keywords=cmpt 128&attributes=SFU&pageNumber=1&pageSize=10

3. Get one course with curriculum detail and video detail
    ** GET      api/courses/5

4. Create a course
    ** POST     api/courses/
        ** please check PostMan for the request body

5. Update a course (partially)
    ** PATCH    api/courses/{id}
    [
        {
          "op": "replace",
          "path": "/title",
          "value": "newdadsga til++++++++artially update"
        },
        {
          "op": "replace",
          "path": "/description",
          "value": "new description"
        }
    ]

6. DELETE a course 
    ** DELETE   api/courses/{id}
  
7 Submit a course to audit
    ** POST     api/courses/{id}/aduting


8 release a course
    ** POST     api/courses/{id}/release

// TODO: 未完成！！！
-------------------------------- Course bundle 
// 课程组合, 相当于学习路径, 也可以用于套件销售
1. Get Bundle
    ** GET      api/bundles?keywords=cmpt 128&attributes=SFU&pageNumber=1&pageSize=10 

2. Create a bundle
    ** POST     api/bundles/
    [ courseIds ]

5. Update a course (partially)
    ** PUT    api/bundles/{id}
    [ courseIds ]

6. DELETE a course 
    ** DELETE   api/bundles/{id}



-------------------------------- Course Attributes ##################################

1. Get Course Attributes (same as CS API)
    ** GET api/courseAttributes

2. Get Entity Attribute Types
    ** GET api/courseAttributes/entityAttributeTypes

2. Create Entity Attribute Type
    ** POST     api/courseAttributes/entityAttributeTypes
    {
        "name":"jajaja"
    }

2. Update Entity Attribute Type
    ** PUT     api/courseAttributes/entityAttributeTypes/{entityAttributeTypeId}
    {
        "name":"jaja+++ja"
    }

2. Get Entity Attribute
    ** GET     api/courseAttributes/entityAttributeTypes/{entityAttributeTypeId}/entityAttributes

2. Create Entity Attribute
    ** POST     api/courseAttributes/entityAttributeTypes/{entityAttributeTypeId}/entityAttributes
    {
        "entityAttributeTypeId":3,
        "Value": "heihedasghihei2",
        "IsSearchable": false
    }

2. Update Entity Attribute
    ** PUT     api/courseAttributes/entityAttributeTypes/{entityAttributeTypeId}/entityAttributes/{entityAttributeId}
    {
        "entityAttributeTypeId":3,
        "Value": "heihedasghihei2",
        "IsSearchable": false
    }

2. Delete Entity Attribute
    ** DELETE     api/courseAttributes/entityAttributeTypes/{entityAttributeTypeId}/entityAttributes/{entityAttributeId}


-------------------------------- Playlists ##################################
1. Get all public playlists. 例如，课程推荐，热门课程, etc.
    ** GET api/playlists 

2. Get playlist by id
    ** GET      api/playlists/{id}

3. Create playlist (public only)
    ** POST     api/playlist 
    [ courseIds ]

4. Delete playlist (public only)
    ** DELETE   api/playlists/1

5. Add course for a playlist
    ** POST     api/playlists/{playlistId}/courses
    [ courseIds ]

6. Batch remove courses from a playlist
    ** DELETE  api/playlists/{playlistId}/courses/({courseId1}, {courseId2}, {courseId3})
  
7. Delete playlist
    ** DELETE  api/playlists/{playlistId}

-------------------------------- Promotions ##################################
1. Get promotions 
    ** GET      api/coupons

2. Create order promotion
    ** GET      api/orders/coupons

3. Create course promotion
    ** GET      api/courses/coupons

4. Get coupon detatil
    ** GET      api/coupons/{couponId}
        
4. update promotion
    ** PATCH      api/coupons/{couponId}

5. active promotion
    ** put      api/coupons/{couponId}/activation
    
6. deactive promotion
    ** put      api/coupons/{couponId}/deactivation


-------------------------------- Orders ##################################
1. Get sale orders
    ** GET      api/orders

3. Get order detail
    ** GET      api/orders/{orderNumber}
    
4. Activate and approve order manully
    ** POST     api/orders/{orderNumber}/activation

5. Get all silent post order
    ** GET      api/orders/silent


// TODO: 以后在做，可以用专门的report system(project)
--------------------------------  Sales
1. 总体销售 by datetime between
2. 某课程销售
3. 某老师销量


