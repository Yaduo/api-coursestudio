# API Documentation #
-------------------------------- Identity ##################################
1. Register user 
    ** POST     api/register  
    {
        "email":"yaduol@hotmail.com"
        "password":"Paragon123$"
        "confirmPassword":"Paragon123$"
    }
    ** NOTE: 密码大小写字母数字符号都要有

3. login (in JWT) 
    ** POST     api/token
    {
        "email": "yaduol@hotmail.com",
        "password": "Paragon123$"
    }

4. Manully request a registration confirmation email 
    ** GET      api/send-registration-confirm-email?userEmail="yaduol@hotmail.com"

5. Registration confirm by email 
    ** GET      api/registration-confirm?userId={xxxxxxx}&token={xxxxxxxx}

6. Request to reset password 
    ** GET      api/forget-password?userEmail={xxxxxxx}&token={xxxxxxxxxxxxx}

7. Reset password 
    ** POST     api/reset-password
    {
        "userId": xxxxxxxxxxxxxx,
        "token": xxxxxxxxxxxxxxxx,
        "newPassword": xxxxxxxxxxxxxxx,
    }

8. Change password 
    ** POST     api/change-password
    {
        "userId": xxxxxxxxxxxxxx,
        "currentPassword": xxxxxxxxxxxxxxxx,
        "newPassword": xxxxxxxxxxxxxxx,
    }

-------------------------------- Users ##################################
1. Get Users (by role) 
    ** GET      api/users?role=Tutor&pageNumber=1&pageSize=10

2. Get user detail
    ** GET      api/users/{id}

// Unfinished
3. Update user detail
    [Authrize]
    ** PATCH    api/users/{id}
    {
        unfinished
    }

// TODO: 似乎应该放到course api去 
4. Get User’s Teaching courses
    ** GET      api/users/{id}/teaching?pageNumber=1&pageSize=10

// TODO: 似乎应该放到course api去 
5. Get User‘s purchased course
    ** GET      api/users/{id}/purchases?pageNumber=1&pageSize=10

// TODO: 似乎应该放到tutor api去 
// Unfinished
6. Apply to become a tutor
    ** POST     api/users/{id}/tutor
 
// TODO: 似乎应该放到tutor api去 
// Unfinished
7. Deactive a tutor
    ** DELETE   api/users/{id}/tutor

// TODO: 似乎应该放到tutor api去
// Unfinished
8. Update a tutor detail
    ** PATCH    api/users/{id}/tutor

// not implement
9. Assign admin to a user
    ** POST     api/users/{id}/admin

-------------------------------- Course ##################################
1. Get courses by keywords, attributes, and paging
    ** GET      api/courses?keywords=cmpt 128&attributes=SFU&pageNumber=1&pageSize=10

2. Get courses by group of course Ids
    ** GET      api/courses/(1,2,3)

3. Get one course with curriculum detail
    ** GET      api/courses/5?needCurriculum=true

4. Create a course
    [Authrize for admin & tutor]
    ** POST     api/courses/
        ** please check PostMan for the request body

5. Update a course (partially)
    [Authrize for admin & tutor of the course]
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
    [Authrize for admin & tutor of the course]
    ** DELETE   api/courses/{id}

// TODO: 似乎应该放到course audit api去  
7 Submit a course to audit
    [Authrize for admin & tutor of the course]
    ** POST     api/courses/{id}/aduting
    {
        "courseId":{xxxxxxxxxxxxxxxxxx}
    }

* Sections
1. Get curriculum by course ID
    ** GET      api/courses/1/sections

2. Get one section 
    ** GET       api/courses/1/sections/5

3. Batch create sections
    [Authrize for admin & tutor of the course]
    ** POST     api/courses/1/sections
    [
        {sectionDto},
        {sectionDto},
        {sectionDto}
    ]

4. Batch delete sections
    [Authrize for admin & tutor of the course]
    ** DELETE api/courses/1/sections?sectionIds=5,8,9

5. Update one section (partially)
    [Authrize for admin & tutor of the course]
    ** PATCH    api/courses/1/sections/5
    [
        {
          "op": "replace",
          "path": "/title",
          "value": "kliuk"
        }
    ]

* Lectures
1. Get Lectures by courseId, and sectionId
    ** GET      api/courses/{courseId}/sections/{sectionId}/lectures

// 正在考虑要不要加入video信息
// 要不要权限
2. Get one lecture detail
    ** GET      api/courses/{courseId}/sections/{sectionId}/lectures/{lectureId}

3. Batch create lectures
    [Authrize for admin & tutor of the course]
    ** POST     api/courses/{courseId}/sections/{sectionId}/lectures
    [
        {LectureDto}, 
        {LectureDto}, 
        {LectureDto}
    ]

// TODO: 未完成
4. Batch delete lectures
    [Authrize for admin & tutor of the course]
    ** DELETE   api/courses/1/sections/{sectionId}/lectures?sectionIds=5,8,9

// TODO: 感觉没什么用，删了吧
5. Delete all lectures in one section
    [Authrize for admin & tutor of the course]
    ** DELETE   api/courses/1/sections/{sectionId}/lectures?sectionIds=5,8,9

6. Partially Update one lecture
    [Authrize for admin & tutor of the course]
    ** PATCH    api/courses/1/sections/{sectionId}/lectures/{lectureId}
    [
        {
          "op": "replace",
          "path": "/title",
          "value": "kliuk"
        }
    ]

* Videos

1. Get the video info from a lecture
    [Authorize: Admin, Tutor for this course, User purchased the course]
    ** GET      api/courses/{courseId}/sections/{sectionId}/lectures/{lectureId}/video

2. Stream a video from a lecture
    [Authorize: Admin, Tutor for this course, User purchased the course]
    ** GET      api/courses/{courseId}/sections/{sectionId}/lectures/{lectureId}/video/stream

3. Create a video
    [Authorize: Admin, Tutor for this course]
    ** FORM POST    api/courses/{courseId}/sections/{sectionId}/lectures/{lectureId}/video
    {
        file: {blob}
        title: "xxxxxxxxxxxxxx",
        description: "xxxxxxxxxxxxxx"
    }

4. Partially Update Video
    [Authorize: Admin, Tutor for this course]
    ** PATCH    api/courses/{courseId}/sections/{sectionId}/lectures/{lectureId}/video
    [
        {
          "op": "replace",
          "path": "/description",
          "value": "kkkkk22222222222222kkkkkkkkkkkk"
        }
    ]

//  需不需要？？
5. Delete a video

-------------------------------- Course bundle ##################################
// 课程组合, 相当于学习路径, 也可以用于套件销售


-------------------------------- Course Attributes ##################################

1. Get course attributs (the course attributes can be user for course search)
    ** GET api/attributes?parentAttributeIds=365846bf-7f7d-4710-9742-7f0fdfaf31a2

// 为系统添加课程属性，用于课程filer和search
// TODO: 需要fully test
2. Create a course attribute
    [Authorize: Admin only]
    ** POST     api/attributes
    {
        "value": {string}
        "entityAttributeTypeId": {int}
        "parentId": {Guid/Nullable}
        "isSearchable": {bool}
    }

-------------------------------- Playlists ##################################
1. Get all public playlists. 例如，课程推荐，热门课程, etc.
    ** GET api/playlist 

// 权限？？
2. Get playlist by id
    [Authorize] 
    ** GET      api/playlists/1

// 似乎可以放在user api里
2. Get user's playlists
    ** GET       api/playlists/user/6655492a-f217-4a1d-8015-32c8f17019e9

// TODO: unfinished
3. Create playlist
    ** POST     api/playlist 
    {   
       unfinished 
    }

4. Delete playlist by ID
    [Authorize] 
    ** DELETE   api/playlists/1

// 需要简化
5. Add course for a playlist
    [Authorize] 
    ** POST     api/playlists/{playlistId}/courses
    [   
        {
            "courseId": {int},
            "playlistId": {int}
        }
    ]

6. Batch remove courses from a playlist
    [Authorize] 
    ** DELETE  api/playlists/{playlistId}/courses/({courseId1}, {courseId2}, {courseId3})
  
// TODO: unfinished
7. Delete playlist

-------------------------------- Shopping Cart ##################################
1. Get current users shopping cart 
    [Authorize] 
    ** GET api/shoppingCart

// 感觉api长得有点怪，shippingcart和user的位置应该调换一下
2. Get a particular user‘s shopping cart by userId
    [Authorize: Admin] 
    ** GET      api/shoppingCart/user/{userId}

3. Batch add shopping cart items
    [Authorize] 
    ** POST     api/shoppingCart/items
    [
      {
        "UnitPrice": 99,
        "Quantity": 1,
        "Course":{ "Id": 1 }
      },
      {
        "UnitPrice": 90,
        "Quantity": 1,
        "Course":{ "Id": 2 }
      }
    ]
   
// 感觉api长得有点怪，shippingcart和user的位置应该调换一下
4. Batch add shopping cart items by admin user for a particular user
    [Authorize: Admin] 
    ** POST     api/shoppingCart/user/{userId}/items

5. Batch delete shoppingCart items
    [Authorize] 
    ** DELETE    api/shoppingCart/items/({itemId1}, {itemId2}, {itemId3}, {itemId4})

// 感觉api长得有点怪，shippingcart和user的位置应该调换一下
6. Batch delete shoppingCart items by admin user for a particular user
    [Authorize: Admin] 
    ** DELETE    api/shoppingCart/user/{userId}/items/({itemId1}, {itemId2}, {itemId3}, {itemId4})

7. Place Order
    [Authorize] 
    ** POST     api/shoppingCart/order
    [
        {shoppingCartItemId1}, 
        {shoppingCartItemId2}, 
        {shoppingCartItemId3}
    ]

// 感觉api长得有点怪，shippingcart和user的位置应该调换一下
8. Place Order by admin user for a particular user
    [Authorize: Admin] 
    ** POST     api/shoppingCart/user/{userId}/order
    [
        {shoppingCartItemId1}, 
        {shoppingCartItemId2}, 
        {shoppingCartItemId3}
    ]

-------------------------------- Sales Orders ##################################
1. Get sale orders for current user
    [Authorize] 
    ** GET      api/salesOrder

2. Get sale orders by admin user for a particular user
    [Authorize: Admin] 
    ** GET      api/salesOrder/user/{userId}
    
3. Get order detail
    [Authorize] 
    ** GET      api/salesOrder/{orderId}
    
4. Activate order
    ** POST     api/salesOrder/transaction
    {
        ActivateOrderRequestDto
    }

// TODO: Not implement
5. Activate order by admin user for a particular user
    ** POST     api/salesOrder/user/{userId}/transaction
    {
        ActivateOrderRequestDto
    }

-------------------------------- FakeThirdPartyPayment ##################################
1. Post order 
    pretent this is the the API from third Party Payment system to process peyment
    , and return a call back to our system (Activate Order API)

    ** POST     api/fakeThirdPartyPayment
    {
        PaymentProcessingRequestDto
    }

-------------------------------- Promotions ##################################
基于产品的促销：
✓ 1. 产品套装                 几个产品构成一个套装, 购买套装享受优惠
2. 买x送y                  当用户购买指定产品时，免费获得另外的产品    
3. 特惠价                  某产品以指定的特惠价出售
4. 步进式优惠               购买一定数量的产品，享受更低价格，购买越多，价格越低
5. 产品组合                 以优惠的价格购买指定的产品组合
✓ 6. 打折                   某产品价格以百分比优惠
7. 指定商品                 购买一个产品后，以优惠价购买指定的另一个产品

基于订单的促销
✓ 1. 赠品                    订单超过一定金额后，获得赠品   
✓ 2. 优惠券                  优惠券可以兑现/抵消部分金额
✓ 3. 折扣                    订单金额打折
4. 运费优惠                 N/A
5. 指定商品                 订单超过一定金额后，以优惠价格购买指定产品


1. Get promotions 

2. Create order promotion

3. Create course promotion

4. update promotion

5. active promotion

6. deactive promotion


-------------------------------- Course Auditings ##################################
// TODO: unfinished 权限
1. Get course auditings
    [Authorize: Admin] 
    ** GET      api/courseAuditings?pageNumber=1&pageSize=10&auditingState=1&needCourses=false

2. Get course auditing detail by id
    [Authorize: Admin] 
    ** GET      api/courseAuditings/{id}

3. Update course auditing
    Action types: Start, Stop, Reject, Approve, Reopen
    [Authorize: Admin] 
    ** PUT      api/courseAuditings/{id}
    {
        "action": {string}
        "comments": {string}
    }

 可能也需要取消审核功能，一个tutor可以随时取消审核。

-------------------------------- Tutor Auditings ##################################
// TODO: not implement
1. Get tutor auditings
    [Authorize: Admin] 
    ** GET      api/courseAuditings?pageNumber=1&pageSize=10&auditingState=1&needCourses=false

// TODO: not implement
2. Get tutor auditing detail by id
    [Authorize: Admin] 
    ** GET      api/courseAuditings/{id}

// TODO: not implement
3. Update tutor auditing
    Action types: Start, Stop, Reject, Approve, Reopen
    [Authorize: Admin] 
    ** PUT      api/courseAuditings/{id}
    {
        "action": {string}
        "comments": {string}
    }

 可能也需要取消审核功能，一个tutor可以随时取消审核。

-------------------------------- Promotion Auditings ##################################
// TODO: not implement
1. Get promotion auditings
    [Authorize: Admin] 
    ** GET      api/courseAuditings?pageNumber=1&pageSize=10&auditingState=1&needCourses=false

// TODO: not implement
2. Get promotion auditing detail by id
    [Authorize: Admin] 
    ** GET      api/courseAuditings/{id}

// TODO: not implement
3. Update promotion auditing
    Action types: Start, Stop, Reject, Approve, Reopen
    [Authorize: Admin] 
    ** PUT      api/courseAuditings/{id}
    {
        "action": {string}
        "comments": {string}
    }